using System.ComponentModel.DataAnnotations;

namespace netbullAPI.Security.ViewModels
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        [StringLength(60, ErrorMessage = "Nome de usuário deve ter no máximo 60 caracteres.")]
        public string user_nome { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Senha deve ter de 6 a 20 caracteres.")]
        public string user_accessKey { get; set; }
    }
}
