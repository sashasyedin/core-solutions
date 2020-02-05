using CoreSolutions.Common.Web.Resolvers.Abstractions;
using CoreSolutions.Utils;
using Microsoft.AspNetCore.Http;

namespace CoreSolutions.Common.Web.Resolvers
{
    public class IpResolver : IIpResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IpResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor.ThrowIfNull(nameof(httpContextAccessor));
        }

        string IIpResolver.Resolve() => _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
    }
}
