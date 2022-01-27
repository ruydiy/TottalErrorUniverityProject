using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class RegisterViewModel
    {
        private const string CONFIRM_PASSWORD_STRING = "ConfirmPassword";

        public string Email { get; set; }

        [Compare(CONFIRM_PASSWORD_STRING)]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
