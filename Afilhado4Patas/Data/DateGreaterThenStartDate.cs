using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class DateGreaterThenStartDate : ValidationAttribute
    {
        private string _startDatePropertyName;

        public DateGreaterThenStartDate(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (Convert.ToDateTime(value) < Convert.ToDateTime(propertyValue))
            {
                return new ValidationResult("Data de Fim inserida terá de ser maior que a Data de Inicio da Tarefa!");
            }
            else if (Convert.ToDateTime(value) > DateTime.Now) {
                return new ValidationResult("Data de Fim inserida terá de ser maior que a data de hoje");
            } else return ValidationResult.Success;
        }
    }
}
