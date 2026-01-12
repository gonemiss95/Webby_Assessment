using System.ComponentModel.DataAnnotations;

namespace UserManagement.Model
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
