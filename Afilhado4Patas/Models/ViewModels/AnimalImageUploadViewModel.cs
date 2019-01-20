using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class AnimalImageUploadViewModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Foto de Galeria")]
        [Required(ErrorMessage = "Por favor escolha um ficheiro")]
        [ImageTypeValidation]
        public IFormFile File { get; set; }

        public Animal animal { get; set; }
    }
}
