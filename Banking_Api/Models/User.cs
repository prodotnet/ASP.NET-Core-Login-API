using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Banking_Api.Models
{
    public class User:IdentityUser
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    }
}
