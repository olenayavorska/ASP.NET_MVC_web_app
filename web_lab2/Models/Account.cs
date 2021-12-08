using System.ComponentModel.DataAnnotations;

namespace web_lab2.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Missing Username")]
        public string Username { get; set; }
 
        [Required(ErrorMessage = "Missing Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }
    }
    
    public class LoginModel
    {
        [Required(ErrorMessage = "Missing Username")]
        public string Username { get; set; }
 
        [Required(ErrorMessage = "Missing Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}