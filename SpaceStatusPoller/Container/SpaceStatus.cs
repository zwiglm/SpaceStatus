using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceStatusPoller.Container
{
    public sealed class SpaceStatus
    {
        public string api { get; set; }
        public string space { get; set; }
        public Uri url { get; set; }
        public Icon icon { get; set; }

        public string address { get; set; }
        public Contact contact { get; set; }

        public bool open { get; set; }
        public int lastchange { get; set; }
        public Uri logo { get; set; }

        public double lat { get; set; }
        public double lon { get; set; }
    }
}
