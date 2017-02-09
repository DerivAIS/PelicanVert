using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO
{

    public interface IGenericDatabaseLine
    {

        DateTime Date { get; set; }

        myDB Table { get; set; }

        List<string> GetDataFields();
        List<string> GetKeyFields();
        string GetTable();

        object this[string propertyName] { get; set; }
       

    }
}
