using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class DateGreaterThen0LessThen30Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (CalculateAge(Convert.ToDateTime(value)) < 0 || CalculateAge(Convert.ToDateTime(value)) >= 30)
                {
                    return new ValidationResult("Insira uma data com pelo menos 1 ano e menos de 30 anos");
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
