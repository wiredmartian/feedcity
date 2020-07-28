using System.ComponentModel.DataAnnotations;

namespace feeddcity.Models.User
{
    public class LoginUserModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class CreateUserModel
    {
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MinLength(8, ErrorMessage = "Password must at least be 8 characters long")]
        public string Password { get; set; }
    }
    public class AuthenticatedUserClaimsModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}