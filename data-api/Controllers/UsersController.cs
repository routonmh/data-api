using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using DataAPI.Constants;
using DataAPI.Middlewares;
using DataAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DataAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        /// <summary>
        /// Get the account details of the current logged in user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("account-details")]
        public async Task<ActionResult<UserAccount>> GetAccountDetails(
            [FromHeader(Name = HeaderFields.ACCOUNT_ID_HEADER_NAME)]
            string accountID)
        {
            return await UserAccountsModel.GetUserByID(Guid.Parse(accountID));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionID">From SessionID Header</param>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<ActionResult> LogoutUserSession(
            [FromHeader(Name = HeaderFields.SESSION_ID_HEADER_NAME)]
            string sessionID)
        {
            Guid parsedSessionID;
            bool success = false;

            if (Guid.TryParse(sessionID, out parsedSessionID))
            {
                await UserSessionsModel.RemoveUserSession(parsedSessionID);
                success = true;
            }

            if (success)
                return new OkResult();
            else
                return new NotFoundResult();
        }
    }
}