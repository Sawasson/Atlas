using System;
using System.Collections.Generic;
using System.Text;

namespace CsvFramework
{
    public class CsvColumn

    {
        public CsvColumn(string Name)
        {
            this.PropertyName = Name;        
        }

        public Func<string,string> Formatter { get; set; }

        public string PropertyName { get; set; }
        public string ColumnName { get; set; }
        public Type Type { get; set; }



        public int? Index { get; set; }       
        public bool IsKey { get; set; }

    }
}
