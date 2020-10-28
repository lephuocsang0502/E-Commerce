using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nhap user name")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Nhap pass")]
        public string Password { set; get; }
        public bool RememberMe { set; get; }
    }
}