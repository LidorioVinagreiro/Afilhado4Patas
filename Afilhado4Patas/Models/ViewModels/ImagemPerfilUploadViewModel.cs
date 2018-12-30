using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Http;

namespace Afilhado4Patas.Models.ViewModels
{
    public class ImagemPerfilUploadViewModel
    {

        [DataType(DataType.Upload)]
        [Display(Name = "Foto de Perfil")]
        [Required(ErrorMessage = "Por favor escolha um ficheiro")]
        [ImageTypeValidation]
        public IFormFile File { get; set; }        
    }
}
