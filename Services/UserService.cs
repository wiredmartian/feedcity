using feeddcity.Data;
using feeddcity.Interfaces;
using feeddcity.Models.User;

namespace feeddcity.Services
{
    public class UserService: IUser
    {
        public int CreateUser(CreateUserModel user)
        {
            throw new System.NotImplementedException();
        }

        public User GetUser(string emailAddress)
        {
            throw new System.NotImplementedException();
        }

        public User AuthenticateUser(string emailAddress, string password)
        {
            throw new System.NotImplementedException();
        }

        public string GenerateAuthToken(User user)
        {
            throw new System.NotImplementedException();
        }

        public AuthenticatedUserClaimsModel GetUserClaims()
        {
            throw new System.NotImplementedException();
        }
    }
}