using System.ComponentModel.DataAnnotations;

namespace Identity.Model
{
    public class UserModel
    {
        [Required]
        public  string UserName { get; set; }
        
        [Required]
        public  string Password { get; set; }
    }
}