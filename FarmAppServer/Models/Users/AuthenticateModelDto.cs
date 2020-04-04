using System.ComponentModel.DataAnnotations;

namespace FarmAppServer.Models
{
    public class AuthenticateModelDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}