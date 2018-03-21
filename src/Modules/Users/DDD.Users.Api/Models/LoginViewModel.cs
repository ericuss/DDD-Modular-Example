using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DDD.Users.Api.Models
{
    public class LoginViewModel : LoginInputModel
    {
        public bool AllowRememberLogin { get; set; }
        public bool EnableLocalLogin { get; set; }
    }


    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
