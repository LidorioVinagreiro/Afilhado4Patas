using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.Estatisticas
{
    public class DataGroup : IDataGroupMethods
    {
        public string title { get; set; }
        public IEnumerable<DataObject> dataObjects { get; set; }

        public DataGroup(string _title)
        {
            title = _title;
        }
        public double totalizador()
        {
            double aux = 0;
            foreach (DataObject i in dataObjects)
            {
                aux += i.value;
            }
            return aux;
        }
    }
}
