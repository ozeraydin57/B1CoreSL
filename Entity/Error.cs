using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreSL.Entity
{
    public class Error
    {
        public string code { get; set; }
        public List<Details> details { get; set; }
        public string message { get; set; }
    }

    public class Root
    {
        public Error error { get; set; }
    }

    public class Details
    {
        public string code { get; set; }
        public string message { get; set; }
    }
    //public class Message
    //{
    //    public string lang { get; set; }
    //    public string value { get; set; }
    //}
}
