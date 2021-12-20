using System.ComponentModel.DataAnnotations;

namespace netbullAPI.Security.Models
{
    public class User
    {
        [Key]
        [Required]
        public int user_id { get; set; }
        [Required]
        public string user_nome { get; set; }
        [Required]
        public string user_email { get; set; }
        [Required]
        public string user_accessKey { get; set; }
    }
}
