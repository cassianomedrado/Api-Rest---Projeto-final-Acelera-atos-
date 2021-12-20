using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netbullAPI.Entidade
{
    public class Pedido
    {
        [Key]
        [Required]
        public int pedido_id { get; set; }
        public string pedido_time { get; set; }
        [ForeignKey("Endereco")]
        public int pedido_idEndereco { get; set; }
        public decimal pedido_valor { get; set; }
        [ForeignKey("Pessoa")]
        public int pedido_idPessoa { get; set; }
        [ForeignKey("User")]
        public int pedido_idUsuario { get; set; }
        public EnumStatusPedido pedido_status { get; set;}
    }
}
