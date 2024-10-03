using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TAM.VMS.Domain
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username field is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password field is required.")]
        public string TamUserPwd { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}