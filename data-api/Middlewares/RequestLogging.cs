using System;
using System.Threading.Tasks;
using DataAPI.Constants;
using DataAPI.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DataAPI.Middlewares
{
    public class RequestLogging
    {
        private RequestDelegate next;
        private ILogger logger;

        public RequestLogging(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            logger = loggerFactory.CreateLogger("RequestLogging");
        }

        /// <summary>
        /// Middleware function
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            Guid sessionID = Guid.Parse(context.Request.Headers[HeaderFields.SESSION_ID_HEADER_NAME]);

            logger.LogInformation("Incrementing request count on session id: " + sessionID.ToString());

            // No need to await the increment log query.
            UserSessionsModel.IncrementSessionRequestCount(sessionID);
            UserSessionsModel.UpdateSessionLastRequestTime(sessionID);

            await next(context);
        }
    }
}