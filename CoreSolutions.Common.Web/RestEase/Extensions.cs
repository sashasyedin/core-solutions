using CoreSolutions.Common.Web.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase;
using System;
using System.Linq;
using System.Net.Http;

namespace CoreSolutions.Common.Web.RestEase
{
    public static class Extensions
    {
        public static void RegisterServiceForwarder<T>(this IServiceCollection services, string serviceName, string sectionName)
            where T : class
        {
            var clientName = typeof(T).ToString();
            var options = ConfigureOptions(services, sectionName);

            ConfigureClient(services, clientName, serviceName, options);
            ConfigureForwarder<T>(services, clientName);
        }

        private static RestEaseOptions ConfigureOptions(IServiceCollection services, string sectionName)
        {
            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.Configure<RestEaseOptions>(configuration.GetSection(sectionName));
            return configuration.GetOptions<RestEaseOptions>(sectionName);
        }

        private static void ConfigureClient(
            IServiceCollection services,
            string clientName,
            string serviceName,
            RestEaseOptions options)
        {
            services.AddHttpClient(clientName, client =>
            {
                var service = options.Services.SingleOrDefault(
                    s => s.Name.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase));

                if (service == null)
                {
                    throw new RestEaseServiceNotFoundException(
                        $"RestEase service: '{serviceName}' was not found.",
                        serviceName);
                }

                var uriBuilder = new UriBuilder
                {
                    Scheme = service.Scheme,
                    Host = service.Host,
                    Port = service.Port,
                };

                client.BaseAddress = uriBuilder.Uri;
            });
        }

        private static void ConfigureForwarder<T>(IServiceCollection services, string clientName)
            where T : class
        {
            services.AddTransient<T>(c => new RestClient(c.GetService<IHttpClientFactory>().CreateClient(clientName))
            {
                RequestQueryParamSerializer = new QueryParamSerializer(),
            }
            .For<T>());
        }
    }
}
