using System;

namespace feeddcity.Data
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime LastSignIn { get; set; }
        public string HashedPassword { get; set; }
    }
}