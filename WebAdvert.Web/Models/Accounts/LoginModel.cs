using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models.Accounts
{
    public class LoginModel
    {
        [Display(Name ="Email")]
        [Required(ErrorMessage ="This field is required")]
        [EmailAddress]
        public string Email{ get; set; }
        [Required(ErrorMessage ="This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}
