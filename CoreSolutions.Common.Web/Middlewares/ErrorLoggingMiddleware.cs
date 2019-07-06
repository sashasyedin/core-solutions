﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CoreSolutions.Common.Web.Middlewares
{
    /// <summary>
    /// The error logging middleware.
    /// </summary>
    public class ErrorLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorLoggingMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">The logger.</param>
        public ErrorLoggingMiddleware(
            RequestDelegate next,
            ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"The following error happened: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles application exceptions.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// The task result.
        /// </returns>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = StatusCodes.Status500InternalServerError;

            if (exception is ArgumentException || exception is ArgumentNullException)
            {
                code = StatusCodes.Status400BadRequest;
            }

            var obj = new { errorMessage = exception.Message };
            var result = JsonConvert.SerializeObject(obj);

            context.Response.ContentType = Constants.ApplicationJson;
            context.Response.StatusCode = code;

            await context.Response.WriteAsync(result);
        }
    }
}