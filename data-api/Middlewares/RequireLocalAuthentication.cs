using System;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using DataAPI.Models.DBs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace DataAPI.Middlewares
{
    public class RequireLocalAuthentication
    {
        public static readonly string USER_ACCOUNT_ID_HEADER_NAME = "UserAccountID";
        public static readonly string USER_SESSION_ID_HEADER_NAME = "SessionID";
        public static readonly string AUTHORIZATION_HEADER_NAME = "authorization";

        private static readonly int SESSION_VALID_DAYS = 14; // Sessions are valid for 14 days
        private static readonly int SESSION_MAX_INACTIVE_DAYS = 5; // Sessions not used in # days are invalid.

        private readonly RequestDelegate next;
        private readonly ILogger logger;

        private string jwtSecret;
        private SymmetricSecurityKey signingKey;
        private TokenValidationParameters validationParameters;
        private JwtSecurityTokenHandler tokenHandler;

        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public RequireLocalAuthentication(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger("RequireLocalAuthentication");

            jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret));
            validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = signingKey,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            logger.LogInformation("Request authorization . . .");

            bool isAuthorized = false;
            string jwt = context.Request.Headers[AUTHORIZATION_HEADER_NAME];

            Guid accountID = Guid.Empty;
            Guid sessionID = Guid.Empty;

            if (tokenHandler.CanReadToken(jwt))
            {
                try
                {
                    SecurityToken securityTokenResult = null;
                    // Validate token, exception thrown if does not comply with validationParameters
                    tokenHandler.ValidateToken(jwt, validationParameters, out securityTokenResult);

                    JwtSecurityToken token = tokenHandler.ReadJwtToken(jwt);
                    accountID = Guid.Parse(token.Payload["AccountID"] as string ?? "");
                    sessionID = Guid.Parse(token.Payload["SessionID"] as string ?? "");

                    if (await validateUserSession(accountID, sessionID))
                        isAuthorized = true;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                }
            }

            // Set Header with
            if (isAuthorized)
            {
                context.Request.Headers.Remove(USER_ACCOUNT_ID_HEADER_NAME);
                context.Request.Headers.Remove(USER_SESSION_ID_HEADER_NAME);

                context.Request.Headers.Add(USER_ACCOUNT_ID_HEADER_NAME, accountID.ToString());
                context.Request.Headers.Add(USER_SESSION_ID_HEADER_NAME, sessionID.ToString());

                await next(context);
            }
            else
            {
                string unauthRequestString = string.Format("Unauthorized request from: {0}:{1}",
                    context.Request.Host.Host, context.Request.Host.Port);
                logger.LogWarning(unauthRequestString);

                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("401 - Unauthorized.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        private async Task<bool> validateUserSession(Guid sessionID, Guid accountID)
        {
            bool isValidSession = false;

            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();
                MySqlCommand cmd = db.Connection.CreateCommand();

                string query = "SELECT DateIssued, DateLastRequest FROM user_session " +
                               "WHERE SessionID = @SessionID AND AccountID = @AccountID; ";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@SessionID", sessionID);
                cmd.Parameters.AddWithValue("@AccountID", accountID);

                DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    // Non-null columns
                    DateTime dateIssued = (DateTime) reader["DateIssued"];
                    DateTime dateLastRequest = (DateTime) reader["DateLastRequest"];

                    DateTime expirationDate = dateIssued.AddDays(SESSION_VALID_DAYS);
                    DateTime inactivityExpirationDate = dateLastRequest.AddDays(SESSION_MAX_INACTIVE_DAYS);

                    DateTime now = DateTime.Now;

                    if (now < expirationDate && now < inactivityExpirationDate)
                        isValidSession = true;

                }
            }

            return isValidSession;
        }
    }
}