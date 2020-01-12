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
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<bool> AddUser(UserAccount user)
        {
            bool success = false;
            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();

                MySqlCommand cmd = db.Connection.CreateCommand();
                string query = "INSERT INTO user_account (AccountID, Email, FirstName, LastName, PasswordHash, " +
                               "CreationDate, IsActive) " +
                               "VALUES (@AccountID, @Email, @FirstName, @LastName, @PasswordHash, @CreationDate, " +
                               "@IsActive)";

                cmd.Parameters.AddWithValue("@AccountID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@CreationDate", user.CreationDate);
                cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                cmd.CommandText = query;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    success = true;
                }
                catch
                {
                    // Ignored, success still false
                }
            }

            return success;
        }

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
                    "SELECT Email, FirstName, LastName, PasswordHash, CreationDate, IsActive FROM " +
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
                    DateTime creationDate = reader["CreationDate"] as DateTime? ?? DateTime.UnixEpoch;
                    bool isActive = reader["IsActive"] as bool? ?? false;

                    account = new UserAccount(accountID, email, firstName, lastName, passwordHash, creationDate,
                        isActive);
                }
            }

            return account;
        }


    }
}