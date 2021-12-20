using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace netbullAPI.Entidade
{
    public class Produto
    {
        [Key]
        [Required]
        public int produto_id { get; set; } 
        [Required]
        public string produto_nome { get; set; }
        [Required]
        public decimal produto_valor { get; set; }
    }
}
