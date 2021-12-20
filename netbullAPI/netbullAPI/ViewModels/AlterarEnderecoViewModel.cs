using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netbullAPI.ViewModels
{
    public class AlterarEnderecoViewModel
    {
        public string? endereco_logradouro { get; set; }
        [Required(ErrorMessage = "O número do endereço é obrigatório.")]
        public int endereco_numero { get; set; }
        public string endereco_complemento { get; set; }

    }
}
