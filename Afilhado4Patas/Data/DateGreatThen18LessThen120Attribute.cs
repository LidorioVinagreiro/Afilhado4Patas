using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class DateGreatThen18LessThen120Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (CalculateAge(Convert.ToDateTime(value)) < 18 || CalculateAge(Convert.ToDateTime(value)) >= 120)
                {
                    return new ValidationResult("Para utilizar o nosso sistema deverá ter +18 anos e -120anos. Por favor insira uma data valida!");
                }
                else return ValidationResult.Success;
            }
            else return new ValidationResult("Insira uma data");
        }

        public static int CalculateAge(DateTime birthDay)
        {
            int years = DateTime.Now.Year - birthDay.Year;
            if ((birthDay.Month > DateTime.Now.Month) || (birthDay.Month == DateTime.Now.Month && birthDay.Day > DateTime.Now.Day))
            {
                years--;
            }
            return years;
        }
    }
}
