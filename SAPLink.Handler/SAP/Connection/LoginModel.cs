using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPLink.Handler.SAP.Connection
{
    public class LoginModel
    {
        public LoginTypes LoginType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
        public enum LoginTypes
        {
            Basic,
            Standard
        }
    }

}
