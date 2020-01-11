using System;
using System.Threading.Tasks;
using DataAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace DefaultNamespace
{
    [Route("users")]
    public class UsersController
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<string>> GetFullName(string userAccountId)
        {
            string fullName = "";
            Guid id;
            if (Guid.TryParse(userAccountId, out id))
            {
                UserAccount account = await UsersModel.GetUserByID(id);
                if (account != null)
                {
                    fullName = account.FirstName + account.LastName;
                }
            }

            return fullName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [HttpPost("create-user")]
        public async Task<ActionResult> CreateUser(string firstName, string lastName)
        {
            if (await UsersModel.AddUser(new UserAccount(firstName, lastName)))
                // Status 200
                return new OkResult();
            // Status 400
            return new BadRequestResult();
        }
    }
}