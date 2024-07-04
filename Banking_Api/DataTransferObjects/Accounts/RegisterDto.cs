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
        [StringLength(12, MinimumLength =6 , ErrorMessage ="The password word must be at least 6 charcters and at most 12 characters")]
        public string Password { get; set; }

    }
}
