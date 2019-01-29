using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class DateGreater1DayThenStartDateAttribute : ValidationAttribute
    {
        private string _startDatePropertyName;

        public DateGreater1DayThenStartDateAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (value != null)
            {
                if (Convert.ToDateTime(value) <= Convert.ToDateTime(propertyValue))
                {
                    return new ValidationResult("Data de Fim inserida terá de ser maior que a Data de Inicio!");
                }
                else if (Convert.ToDateTime(value) < DateTime.Now)
                {
                    return new ValidationResult("Data de Fim inserida terá de ser maior que a data de hoje");
                }
                else if ((Convert.ToDateTime(value) - Convert.ToDateTime(propertyValue)).TotalDays > 1)
                {
                    return new ValidationResult("Data Inserida ultrapassa o fim de semana");
                }
                else return ValidationResult.Success;
            }
            else return new ValidationResult("Insira uma data");
        }
    }
}
