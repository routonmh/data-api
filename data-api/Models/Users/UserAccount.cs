using System;

namespace DataAPI.Models.Users
{
    public class UserAccount
    {
        public Guid UserID { get; }
        public string FirstName { get; }
        public string LastName { get; }


        public UserAccount(string firstName, string lastName)
            : this(Guid.Empty, firstName, lastName)
        {
        }

        public UserAccount(Guid userId, string firstName, string lastName)
        {
            UserID = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}