using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace netbullAPI_Testes.Models
{
    public class RetornoDeleteTelefone
    {
        public HttpStatusCode status { get; set; }
        public bool sucesso { get; set; }
    }
}
