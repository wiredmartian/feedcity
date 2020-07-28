using feeddcity.Data;
using feeddcity.Models.User;

namespace feeddcity.Interfaces
{
    public interface IUser
    {
        int CreateUser(CreateUserModel user);
        User GetUser(string emailAddress);
        User AuthenticateUser(string emailAddress, string password);
        string GenerateAuthToken(User user);
        AuthenticatedUserClaimsModel GetUserClaims();
    }
}