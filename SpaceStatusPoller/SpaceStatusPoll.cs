using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.Web.Http;

namespace SpaceStatusPoller
{
    public sealed class SpaceStatusPoll : IBackgroundTask
    {
        private TileUpdater _tileUpdater;

        public SpaceStatusPoll()
        {
            _tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            if (taskInstance != null)
                taskInstance.Canceled += taskInstance_Canceled;

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            var response = await this.requestSpaceStatus();
            eSpaceStatus status = this.filterMainLibs(response.Content.ToString());
            this.sendTileUpdateNotification(status);
            deferral.Complete();
        }

        void taskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
        }


        private void sendTileUpdateNotification(eSpaceStatus status)
        {            
            string image = "";
            string altText = "";
            switch (status)
            {
                case eSpaceStatus.Unknown:
                    image = "\\Assets\\spaceStatusClosed_170x170.png";
                    altText = "Closed";
                    break;
                case eSpaceStatus.Open:
                    image = "\\Assets\\spaceStatusOpen_170x170.png";
                    altText = "Open";
                    break;
                case eSpaceStatus.Closed:
                    image = "\\Assets\\spaceStatusClosed_170x170.png";
                    altText = "Closed";
                    break;
            }

            StringBuilder xmlString = new StringBuilder();
            xmlString.Append("<tile>");
            xmlString.Append("<visual version=\"3\">");
            xmlString.Append("<binding template=\"TileSquare71x71Image\" fallback=\"TileSquare71x71Image\">");
            xmlString.Append(String.Format("<image id=\"1\" src=\"{0}\" alt=\"{1}\" />", image, altText));
            xmlString.Append("</binding></visual></tile>");

            var tileXml = new XmlDocument(); ;
            tileXml.LoadXml(xmlString.ToString());

            var tileNotification = new TileNotification(tileXml);
            _tileUpdater.Update(tileNotification);
        }

        private async Task<HttpResponseMessage> requestSpaceStatus()
        {
            Uri itSyndikatUri = new Uri("http://it-syndikat.org/status-s.php");
            var HttpClientGetLibrary = new HttpClient();

            HttpResponseMessage libsResponse = null;
            try
            {
                libsResponse = await HttpClientGetLibrary.GetAsync(itSyndikatUri);
                HttpClientGetLibrary.Dispose();
            }
            catch (Exception ex)
            {
            }
            return libsResponse;
        }

        private eSpaceStatus filterMainLibs(string httpResponse)
        {
            //HtmlDocument dom = new HtmlDocument();
            //dom.LoadHtml(httpResponse);
            //var inner = dom.GetElementbyId("itsstatus").InnerText;

            if (httpResponse.Equals("false"))
                return eSpaceStatus.Closed;
            else if (httpResponse.Equals("true"))
                return eSpaceStatus.Open;
            else
                return eSpaceStatus.Unknown;
        }

    }
}
