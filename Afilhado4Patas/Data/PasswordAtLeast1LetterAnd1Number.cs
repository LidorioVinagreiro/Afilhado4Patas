using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class PasswordAtLeast1LetterAnd1Number : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value.ToString().Any(char.IsLetter) && value.ToString().Any(char.IsNumber)) { 
                return ValidationResult.Success;
            }
            else return new ValidationResult("Palavra Passe deverá conter pelo menos 1 numero e 1 letra");
        }
    }
}
