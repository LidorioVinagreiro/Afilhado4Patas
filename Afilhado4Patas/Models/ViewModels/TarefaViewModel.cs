using Afilhado4Patas.Data;
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
        public int? Id { get; set; }

        [Required(ErrorMessage = "Selecione um determinado funcionario")]
        [Display(Name = "Funcionario")]
        public string FuncionarioId { get; set; }

        public virtual Utilizadores Utilizador { get; set; }

        [Display(Name = "Data de Inicio")]
        [Required(ErrorMessage = "Preencha este campo com a Data de Inicio da Tarefa!")]
        [DateEqualOrGreaterThenToday]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }

        [Display(Name = "Data de Fim")]
        [Required(ErrorMessage = "Preencha este campo com a Data de Fim esperada para a Tarefa!")]
        [DateGreaterThenStartDate("Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }

        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "Preencha este campo com a descrição da Tarefa!")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "A descricao da tarefa apenas deverá conter letras (aA) e numeros e não deverá ter caracteres que não letras ou numeros")]
        [StringLength(100, ErrorMessage = "A {0} deverá ter pelo menos {2} e um maximo de {1} caracteres de comprimento.", MinimumLength = 10)]
        public string Descricao { get; set; }

        [Display(Name = "Completa")]
        public bool? Completada { get; set; }

        public List<Utilizadores> ListaFuncionarios { get; set; }
    }
}
