using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netbullAPI.Entidade
{
    public class Telefone
    {
        [Key]
        [Required]
        public int telefone_id { get; set; }
        public int telefone_numero { get; set; }
        [ForeignKey("Pessoa")]
        public int telefone_idPessoa { get; set; }
    }
}
