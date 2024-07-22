using System.ComponentModel.DataAnnotations;

namespace Banking_Api.DataTransferObjects.Accounts
{
    public class RegisterDto
    {

        [Required]

        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Invaild Email Address")]
        public string Email { get; set; }
        [Required]
        [StringLength(12, MinimumLength =6 , ErrorMessage = "The password must be at least 6 characters and at most 12 characters long.")]
        public string Password { get; set; }

    }
}
