using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class Date3DaysFromNow : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                double dias = (Convert.ToDateTime(value) - DateTime.Now).TotalDays;
                if (dias <= 2 )
                {
                    return new ValidationResult("Insira uma data a pelos menos 3 dias a partir da data de hoje");
                }
                else return ValidationResult.Success;
            }
            else return new ValidationResult("Insira uma data");
        }        
    }
}
