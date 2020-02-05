using System.Collections.Generic;

namespace CoreSolutions.Common.Web.RestEase
{
    public class RestEaseOptions
    {
        public IEnumerable<Service> Services { get; set; }

        public class Service
        {
            public string Host { get; set; }
            public string Name { get; set; }
            public int Port { get; set; }
            public string Scheme { get; set; }
        }
    }
}
