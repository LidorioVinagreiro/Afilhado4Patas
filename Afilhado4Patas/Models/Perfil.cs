﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Afilhado4Patas.Models
{
    public class Perfil
    {
        [Display(Name = "Primeiro Nome")]
        public string FirstName { get; set; }
        [Display(Name = "Ultimo Nome")]
        public string LastName { get; set; }
        [Display(Name = "Morada")]
        public string Street { get; set; }
        [Display(Name = "Cidade")]
        public string City { get; set; }
        [Display(Name = "Código de Postal")]
        [RegularExpression(@"\d{4}-\d{3}",ErrorMessage ="Insira um código válido")]
        public string Postalcode { get; set; }
        [RegularExpression(@"\d{9}")]
        [Display(Name = "Nif")]
        public string NIF { get; set; }
        [Display(Name = "Foto")]
        public string Photo { get; set; }
        [Display(Name = "Data de Nascimento")]
        public DateTime Birthday { get; set; }
        [Display(Name = "Idade")]
        public string Age { get { return ((DateTime.UtcNow - Birthday).TotalDays/365).ToString(); } }
        [Display(Name = "Género")]
        public string Genre
        {
            get { return Genre;
            }
            set {
                if (value.ToLower() != "m" || value.ToLower() != "f"){
                    Genre = "indefenido";
                }
                else {
                    Genre = value;
                }
            }
        }
    }
}
