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

        public DataGroup utilMF { get; set; }
        public DataGroup tiposAnimais { get; set; }
        public DataGroup adocMes { get; set; }
        public DataGroup apadMes { get; set; }
        public DataGroup pedPassMes { get; set; }
        public DataGroup pedFSMes { get; set; }

        public DataRetrive(ApplicationDbContext contexto)
        {
            _context = contexto;
            utilMF = dataGroupMF();
            tiposAnimais = dataGroupTipoAnimais();
            adocMes = adocoesPorMes();
            apadMes = apadrinhamentosPorMes();
            pedPassMes = numeroPedidosPasseioPorMes();
            pedFSMes = numeroPedidosFdsPorMes();
        }

        public DataGroup dataGroupMF()
        {

            DataGroup group = new DataGroup("Distribuição do Sexo dos Utilizadores");


            group.dataObjects = from dados in _context.PerfilTable
                                 group dados by dados.Genre into Totais
                                 select
                                 new DataObject
                                 {
                                     label = Totais.Key,
                                     value = Totais.Count()
                                 };
            return group;
        }

        public DataGroup dataGroupTipoAnimais()
        {
            DataGroup group = new DataGroup("Tipos de Animais");


            group.dataObjects = from categorias in _context.Categorias
                                     join racas in _context.Racas on  categorias.Id equals racas.CategoriaId 
                                     join animais in _context.Animais on racas.Id equals animais.RacaId
                                     group categorias by categorias.Nome into t
                                     select new DataObject
                                     {
                                         label = t.Key,
                                         value = t.Count()
                                     };
            return group; 
        }


        //na view está q definido que o tipo de adocao é parcial ou total
        //na view!!! onde é que isto já se viu!!
        public DataGroup adocoesPorMes() {
            Dictionary<int,string> meses = this.meses();

            DataGroup group = new DataGroup("Adoções por mês");
            int anoActual = DateTime.Now.Year;
            var adocoesDesteAno = _context.PedidosAdocao
                .Where(x => x.DataAprovacao.Year == anoActual && x.Aprovacao == "Aprovado" && x.TipoAdocao == "Total")
                .Select(z => new {
                    mes = z.DataAprovacao.Month,
                    valor = 1 //int.Parse(z.Aprovacao)
                }).GroupBy(f => f.mes)
                .Select( c => new DataObject
                {
                    label = meses.GetValueOrDefault(c.Key),
                    value = c.Count()
                });

            group.dataObjects = adocoesDesteAno;
                return group;
        }

        public DataGroup apadrinhamentosPorMes() {
            Dictionary<int, string> meses = this.meses();
            DataGroup group = new DataGroup("Apadrinhamentos por mês");
            int anoActual = DateTime.Now.Year;
            var adocoesDesteAno = _context.PedidosAdocao
                .Where(x => x.DataAprovacao.Year == anoActual && x.Aprovacao == "Aprovado" && x.TipoAdocao == "Parcial")
                .Select(z => new {
                    mes = z.DataAprovacao.Month,
                    valor = 1
                }).GroupBy(f => f.mes)
                .Select(c => new DataObject
                {
                    label = meses.GetValueOrDefault(c.Key),
                    value = c.Count()
                });

            group.dataObjects = adocoesDesteAno;
            return group;
        }

        public DataGroup numeroPedidosPasseioPorMes() {
            DataGroup group = new DataGroup("Pedidos Passeio Por Mês");
            Dictionary<int, string> meses = this.meses();

            int anoActual = DateTime.Now.Year;
            var pedidosPasseios = _context.PedidosPasseio
                .Where(x => x.DataPedido.Year == anoActual)
                .Select(z => new {
                    mes = z.DataPedido.Month,
                    valor = 1
                }).GroupBy(f => f.mes)
                .Select(c => new DataObject
                {
                    label = meses.GetValueOrDefault(c.Key),
                    value = c.Count()
                });

            group.dataObjects = pedidosPasseios;
            return group;
        }
        public DataGroup numeroPedidosFdsPorMes() {
            DataGroup group = new DataGroup("Pedidos de Fim de Semana por mês");
            Dictionary<int, string> meses = this.meses();

            int anoActual = DateTime.Now.Year;
            var pedidosDeFds = _context.PedidosFimSemana
                .Where(x => x.DataPedido.Year == anoActual)
                .Select(z => new {
                    mes = z.DataPedido.Month,
                    valor = 1
                }).GroupBy(f => f.mes)
                .Select(c => new DataObject
                {
                    label = meses.GetValueOrDefault(c.Key),
                    value = c.Count()
                });

            group.dataObjects = pedidosDeFds;
            return group;
        }
        private Dictionary<int,string> meses()
        {
            Dictionary<int,string> dicionario = new Dictionary<int,string>();
            dicionario.Add(1, "Janeiro");
            dicionario.Add(2, "Fevereiro");
            dicionario.Add(3, "Março");
            dicionario.Add(4, "Abril");
            dicionario.Add(5, "Maio");
            dicionario.Add(6, "Junho");
            dicionario.Add(7, "Julho");
            dicionario.Add(8, "Agosto");
            dicionario.Add(9, "Setembro");
            dicionario.Add(10, "Outobro");
            dicionario.Add(11, "Novembro");
            dicionario.Add(12, "Dezembro");
            return dicionario;
        }
    }
}
