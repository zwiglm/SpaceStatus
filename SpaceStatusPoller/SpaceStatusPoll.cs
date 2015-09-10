using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Windows.ApplicationModel.Background;
using Windows.Web.Http;

namespace SpaceStatusPoller
{
    public sealed class SpaceStatusPoll : IBackgroundTask
    {
        public SpaceStatusPoll()
        {
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            if (taskInstance != null)
                taskInstance.Canceled += taskInstance_Canceled;

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            var response = await this.requestSpaceStatus();
            eSpaceStatus status = this.filterMainLibs(response.Content.ToString());
            deferral.Complete();
        }

        void taskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
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
