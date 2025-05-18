using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace B1CoreSL.Entity
{
    public class LoginResp : LoginRequest
    {
        public string SessionId { get; set; }
        public Error Error { get; set; }
        public string RouteId { get; set; }


    }
}
