using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class LoginForm
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Wrong Password")]
        public string PasswordConfirm { get; set; }
        public string urlReturn { get; set; }

    }
}
