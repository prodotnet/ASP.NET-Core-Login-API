using System.ComponentModel.DataAnnotations;

namespace Banking_Api.DataTransferObjects.Accounts
{
    public class LoginDto
    {
        [Required]
        public string Username {  get; set; }
        [Required]
        public string Password { get; set; }
    }
}
