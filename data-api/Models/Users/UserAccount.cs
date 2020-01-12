using System;
using System.Text.Json.Serialization;

namespace DataAPI.Models.Users
{
    public class UserAccount
    {
        public Guid AccountID { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateCreated { get; }
        public bool IsActive { get; }
        [JsonIgnore] public string PasswordHash { get; }

        public UserAccount(Guid accountId, string email, string firstName, string lastName, string passwordHash,
            DateTime dateCreated, bool isActive)
        {
            AccountID = accountId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PasswordHash = passwordHash;
            DateCreated = dateCreated;
            IsActive = isActive;
        }
    }
}