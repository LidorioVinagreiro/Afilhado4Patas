using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Afilhado4Patas.Data;
namespace Afilhado4Patas.Models
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Utilizadores")]
        public string UtilizadorId { get; set; }

        public virtual Utilizadores Utilizador { get; set; }

        [Display(Name = "Primeiro Nome")]
        public string FirstName { get; set; }

        [Display(Name = "Ultimo Nome")]
        public string LastName { get; set; }

        [Display(Name = "Morada")]
        public string Street { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Display(Name = "Código de Postal")]
        [RegularExpression(@"\d{4}-\d{3}", ErrorMessage = "Insira um código válido")]
        public string Postalcode { get; set; }

        [RegularExpression(@"\d{9}")]
        [Display(Name = "Nif")]
        public string NIF { get; set; }
   
        public string Directoria { get; set; }
        
        [Display(Name = "Foto")]
        public string Photo { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Género")]
        public string Genre { get; set; }

        //esta lista vai servir para ser enchida através da tabela Amizades
        public List<Perfil> Amigos { get; set; } = new List<Perfil>();

    }
}
