using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netbullAPI.Entidade
{
    public class Endereco
    {
        [Key]
        public int endereco_id { get; set; }
        public string endereco_logradouro { get; set; }
        public int endereco_numero { get; set; }
        public string endereco_complemento { get; set; }
        [ForeignKey("Pessoa")]
        public int endereco_idpessoa { get; set; }

    }
}
