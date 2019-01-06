using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class AnimalFileUploadViewModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Anexo")]
        [Required(ErrorMessage = "Por favor escolha um ficheiro")]
        [FileTypeValidation]
        public IFormFile File { get; set; }

        public Animal animal { get; set; }
    }
}
