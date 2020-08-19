using System;

namespace feeddcity.Data
{
    public class User
    {
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public bool Disabled { get; set; }
        public string Username { get; }
        public DateTime LastSignIn { get; set; }
        public string HashedPassword { get; }
    }
}