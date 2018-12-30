using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class DateGreaterThanZeroLessThan100 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (CalculateAge(Convert.ToDateTime(value))>=100 || CalculateAge(Convert.ToDateTime(value)) < 0)
            {
                return new ValidationResult("A idade do animal deve estar comprrendida entre 0 e 100");
            }
            else return ValidationResult.Success;
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
