using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreSL.Entity
{
    public class UDFEntity
    {
        public string TableName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // db_Alpha , 
        public string SubType { get; set; }
        public int Size { get; set; }
        public string DefaultValue { get; set; }
        public List<ValueKey> ValidValuesMD { get; set; }

        public string Mandatory { get; set; }
    }

    public class ValueKey
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
