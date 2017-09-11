using System.ComponentModel.DataAnnotations;

namespace TestImplementation.SetCookie.Models
{
    public class LoginVM
    {
        public string ReturnUrl { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}