using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class ImageTypeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (((IFormFile) value).FileName.EndsWith(".jpg"))
            {
                return ValidationResult.Success;               
            }
            else return new ValidationResult("Insira um ficheiro em formato JPG.");
        }
    }
}
