using netbullAPI.Interfaces;
using netbullAPI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace netbullAPI_Testes.Models
{
    public class RetornoNotFound
    {
        public HttpStatusCode status { get; set; }
        public List<Notificacao> error { get; set; }
    }
}
