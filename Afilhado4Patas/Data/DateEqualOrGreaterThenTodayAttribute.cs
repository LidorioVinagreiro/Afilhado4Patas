using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class DateEqualOrGreaterThenTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = Convert.ToDateTime(value);

            if (date >= DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else return new ValidationResult("Data de Inicio deverá ser maior que a data de hoje");            
        }        
    }
}
