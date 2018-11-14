using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        [DisplayName("Password")]
        [DataType(DataType.Password,ErrorMessage ="Password Inválida")]
        public String Password { get; set; }
    }
}
