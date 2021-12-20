using System.ComponentModel.DataAnnotations;

namespace netbullAPI.ViewModels
{
    public class RegistrarProdutoViewModel
    {
        [Required]
        public string produto_nome { get; set; }
        [Required]
        public decimal produto_valor { get; set; }
    }
}
