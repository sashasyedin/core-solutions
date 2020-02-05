using System;

namespace CoreSolutions.Common.Web.RestEase
{
    public class RestEaseServiceNotFoundException : Exception
    {
        public RestEaseServiceNotFoundException(string serviceName)
            : this(string.Empty, serviceName)
        {
        }

        public RestEaseServiceNotFoundException(string message, string serviceName)
            : base(message)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; set; }
    }
}
