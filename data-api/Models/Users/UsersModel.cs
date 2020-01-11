using System;
using System.Data.Common;
using System.Threading.Tasks;
using DataAPI.Models.DBs;
using MySql.Data.MySqlClient;

namespace DataAPI.Models.Users
{
    public class UsersModel
    {
        public static async Task<bool> AddUser(UserAccount user)
        {
            bool success = false;
            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();

                MySqlCommand cmd = db.Connection.CreateCommand();
                string query = "INSERT INTO user_account (UserAccountID, FirstName, LastName) " +
                               "VALUES (@UserAccountID, @FirstName, @LastName)";

                cmd.Parameters.AddWithValue("@UserAccountID", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);

                cmd.CommandText = query;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    success = true;
                }
                catch
                {
                    // Ignored)
                }
            }

            return success;
        }

        public static async Task<UserAccount> GetUserByID(Guid id)
        {
            UserAccount account = null;

            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();

                MySqlCommand cmd = db.Connection.CreateCommand();
                string query = "SELECT UserAccountID, FirstName, LastName FROM " +
                               "user_account WHERE UserAccountID = @UserAccountID";

                cmd.Parameters.AddWithValue("@UserAccountID", id);
                cmd.CommandText = query;

                DbDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    string firstName = reader["FirstName"] as string ?? null;
                    string lastName = reader["LastName"] as string ?? null;

                    account = new UserAccount(id, firstName, lastName);
                }
            }

            return account;
        }
    }
}