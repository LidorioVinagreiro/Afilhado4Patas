﻿using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class TarefaViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Funcionario")]
        public string FuncionarioId { get; set; }
        public virtual Utilizadores Utilizador { get; set; }
        [Display(Name = "Data de Inicio")]
        public DateTime Inicio { get; set; }
        [Display(Name = "Data de Fim")]
        public DateTime Fim { get; set; }
        [Display(Name = "Descricao")]
        public string Descricao { get; set; }
        [Display(Name = "Completa")]
        public bool Completada { get; set; }

        public List<Utilizadores> ListaFuncionarios { get; set; }
    }
}
