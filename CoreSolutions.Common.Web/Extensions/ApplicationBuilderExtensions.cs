using CoreSolutions.Common.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CoreSolutions.Common.Web.Extensions
{
    /// <summary>
    /// Extensions to ApplicationBuilder.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the error logging to the application's request pipeline.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>
        /// The IApplicationBuilder instance.
        /// </returns>
        public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }

        /// <summary>
        /// Adds the request logging to the application's request pipeline.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>
        /// The IApplicationBuilder instance.
        /// </returns>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
