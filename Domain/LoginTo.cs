using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class LoginTo
    {
          [Required]
        public string Email { get; set; }
 
        [Required]
        public string Password { get; set; }
         
    }
}