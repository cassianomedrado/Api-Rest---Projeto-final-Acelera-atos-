using netbullAPI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace netbullAPI_Testes.Models
{
    public class RetornoErrorPessoa
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public HttpStatusCode Status { get; set; }
        public string TraceId { get; set; }
        public List<Notificacao> Errors { get; set; }
    }
}
