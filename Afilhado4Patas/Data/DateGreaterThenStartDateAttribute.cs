using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class DateGreaterThenStartDateAttribute : ValidationAttribute
    {
        private string _startDatePropertyName;

        public DateGreaterThenStartDateAttribute(string startDatePropertyName)
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
                else return ValidationResult.Success;
            }
            else return new ValidationResult("Insira uma data");
        }
    }
}
