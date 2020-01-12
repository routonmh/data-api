using System;
using System.Threading.Tasks;
using DataAPI.Models.DBs;
using MySql.Data.MySqlClient;

namespace DataAPI.Models.Users
{
    public static class UserSessionsModel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public static async Task<Guid> CreateUserSession(Guid accountID)
        {
            Guid sessionID = Guid.NewGuid();

            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();
                MySqlCommand cmd = db.Connection.CreateCommand();

                string query = "INSERT INTO user_session (SessionID, AccountID, DateIssued, RequestCount, " +
                               "DateLastRequest) " +
                               "VALUES (@SessionID, @UserAccountID, @DateIssued, @RequestCount, @DateLastRequest)";

                DateTime now = DateTime.Now;

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@SessionID", sessionID);
                cmd.Parameters.AddWithValue("@AccountID", accountID);
                cmd.Parameters.AddWithValue("@DateIssued", now);
                cmd.Parameters.AddWithValue("@DateLastRequest", now);

                await cmd.ExecuteNonQueryAsync();
            }

            return sessionID;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public static async Task RemoveUserSession(Guid sessionID)
        {
            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();
                MySqlCommand cmd = db.Connection.CreateCommand();

                string query = "DELETE FROM user_session WHERE SessionID = @SessionID;";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@SessionID", sessionID);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}