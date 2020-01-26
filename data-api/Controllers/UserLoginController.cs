using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using DataAPI.Constants;
using DataAPI.Models.Users;
using DataAPI.Utilitiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DataAPI.Controllers
{
    [ApiController]
    public class UserLoginController : Controller
    {
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser([FromQuery] string email,
            [FromQuery] string candidatePassword)
        {
            JwtSecurityToken token = null;
            UserAccount account = await UserAccountsModel.GetUserByEmail(email);
            bool passwordCorrect = false;

            if (account != null)
                passwordCorrect = PasswordHashUtility.ValidatePassword(candidatePassword, account.PasswordHash);

            if (account != null && passwordCorrect)
            {
                // Create Session
                Guid sessionID = await UserSessionsModel.CreateUserSession(account.AccountID);

                // Create JWT
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    Environment.GetEnvironmentVariable("JWT_SECRET")));
                SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                token = new JwtSecurityToken(issuer: "DataAPISever", signingCredentials: creds);

                token.Payload[AuthTokenPayload.ACCOUNT_ID_FIELD_NAME] = account.AccountID.ToString();
                token.Payload[AuthTokenPayload.SESSION_ID_FIELD_NAME] = sessionID.ToString();

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
                return new UnauthorizedResult();
        }
    }
}