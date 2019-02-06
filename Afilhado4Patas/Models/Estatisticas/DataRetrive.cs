using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.Estatisticas
{
    public class DataRetrive
    {
        private ApplicationDbContext _context;

        public DataRetrive(ApplicationDbContext contexto)
        {
            _context = contexto;
        }

        public DataGroup dataGroupMF()
        {

            DataGroup groupo = new DataGroup("Pie Char MF");


            groupo.dataObjects = from dados in _context.PerfilTable
                                 group dados by dados.Genre into Totais
                                 select
                                 new DataObject
                                 {
                                     label = Totais.Key,
                                     value = Totais.Count()
                                 };
            return groupo;
        }

        public DataGroup dataGroupTipoAnimais()
        {
            DataGroup groupo = new DataGroup("Pie Char Animais");


            groupo.dataObjects = from categorias in _context.Categorias
                                     join racas in _context.Racas on  categorias.Id equals racas.CategoriaId 
                                     join animais in _context.Animais on racas.Id equals animais.RacaId
                                     group categorias by categorias.Nome into t
                                     select new DataObject
                                     {
                                         label = t.Key,
                                         value = t.Count()
                                     };
            return groupo; 
        }
    }
}
