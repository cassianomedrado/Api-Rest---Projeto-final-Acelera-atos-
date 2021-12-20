using netbullAPI.Entidade;
using netbullAPI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace netbullAPI_Testes.Models
{
    public class RetornoGetTelefone
    {
        public HttpStatusCode status { get; set; }
        public List<Notificacao> Erros { get; set; }
        public IEnumerable<Telefone> telefones { get; set; }
    }
}
