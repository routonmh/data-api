using System;
using System.Data.Common;
using System.Threading.Tasks;
using DataAPI.Models.DBs;
using MySql.Data.MySqlClient;

namespace DataAPI.Models.Users
{
    public static class UserAccountsModel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public static async Task<UserAccount> GetUserByID(Guid accountID)
        {
            UserAccount account = null;

            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();

                MySqlCommand cmd = db.Connection.CreateCommand();
                string query =
                    "SELECT Email, FirstName, LastName, PasswordHash, DateCreated, IsActive FROM " +
                    "user_account WHERE UserAccountID = @AccountID";

                cmd.Parameters.AddWithValue("@AccountID", accountID);
                cmd.CommandText = query;

                DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    string email = reader["Email"] as string ?? null;
                    string firstName = reader["FirstName"] as string ?? null;
                    string lastName = reader["LastName"] as string ?? null;
                    string passwordHash = reader["PasswordHash"] as string ?? null;
                    DateTime creationDate = reader["DateCreated"] as DateTime? ?? DateTime.UnixEpoch;
                    bool isActive = Convert.ToBoolean(reader["IsActive"] as int? ?? 0);

                    account = new UserAccount(accountID, email, firstName, lastName, passwordHash, creationDate,
                        isActive);
                }
            }

            return account;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static async Task<UserAccount> GetUserByEmail(string email)
        {
            UserAccount account = null;

            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();

                MySqlCommand cmd = db.Connection.CreateCommand();
                string query =
                    "SELECT AccountID, Email, FirstName, LastName, PasswordHash, DateCreated, IsActive FROM " +
                    "user_account WHERE Email = @Email";

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.CommandText = query;

                DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    Guid accountID = reader["AccountID"] as Guid? ?? Guid.Empty;
                    string firstName = reader["FirstName"] as string ?? null;
                    string lastName = reader["LastName"] as string ?? null;
                    string passwordHash = reader["PasswordHash"] as string ?? null;
                    DateTime creationDate = reader["DateCreated"] as DateTime? ?? DateTime.UnixEpoch;
                    bool isActive = Convert.ToBoolean(reader["IsActive"] as int? ?? 0);

                    account = new UserAccount(accountID, email, firstName, lastName, passwordHash, creationDate,
                        isActive);
                }
            }

            return account;
        }
    }
}