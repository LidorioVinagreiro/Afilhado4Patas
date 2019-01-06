﻿using Microsoft.AspNetCore.Http;
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
            IFormFile file = (IFormFile)value;
            if (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".JPG") || file.FileName.EndsWith(".jpeg") || file.FileName.EndsWith(".JPEG")
                || file.FileName.EndsWith(".png") || file.FileName.EndsWith(".PNG") )
            {
                return ValidationResult.Success;               
            }
            else return new ValidationResult("Insira uma imagem no formato JPG, JPEG ou PNG.");
        }
    }
}
