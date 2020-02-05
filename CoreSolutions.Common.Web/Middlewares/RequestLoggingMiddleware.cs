using CoreSolutions.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoreSolutions.Common.Web.Middlewares
{
    /// <summary>
    /// The request logging middleware.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">The logger.</param>
        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next.ThrowIfNull(nameof(next));
            _logger = logger.ThrowIfNull(nameof(logger));
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The task result.
        /// </returns>
        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation(await FormatRequest(context.Request));

            // Copy a pointer to the original response:
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // Continue down the middleware pipeline:
                await _next(context);

                _logger.LogInformation(await FormatResponse(context.Response));
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        /// <summary>
        /// Formats the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The task result.
        /// </returns>
        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableRewind();

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"Request: {request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        /// <summary>
        /// Formats the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>
        /// The task result.
        /// </returns>
        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"Response: {text}";
        }
    }
}
