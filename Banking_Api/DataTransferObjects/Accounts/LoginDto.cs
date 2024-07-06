using System.ComponentModel.DataAnnotations;

namespace Banking_Api.DataTransferObjects.Accounts
{
    public class LoginDto
    {
        [Required]
        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Invaild Email Address")]
        public string Email {  get; set; }
        [Required]
        public string Password { get; set; }
    }
}
