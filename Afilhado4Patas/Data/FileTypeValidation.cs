using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class FileTypeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                IFormFile file = (IFormFile)value;
                if (file.FileName.EndsWith(".pdf") || file.FileName.EndsWith(".PDF"))
                {
                    return ValidationResult.Success;
                }
                else return new ValidationResult("Insira um ficheiro no formato PDF.");
            }
            else return new ValidationResult("Insira um ficheiro no formato PDF.");
        }
    }
}
