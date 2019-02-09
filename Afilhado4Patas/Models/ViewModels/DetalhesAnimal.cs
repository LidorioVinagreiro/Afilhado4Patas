using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class DetalhesAnimal
    {
        public Animal Animal { get; set; }

        public string CaminhoGaleria { get; set; }

        public List<string> FicheirosGaleria { get; set; }

        public string CaminhoAnexos { get; set; }

        public List<string> FicheirosAnexos { get; set; }
    }
}
