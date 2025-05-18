using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1CoreSL.Entity
{
    public class LoginRequest
    {
        public string Host { get; set; }
        public string CompanyDB { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
