using System.ComponentModel.DataAnnotations;

namespace WebApp.Model;

public class UserModel
{
    [Display (Name = "Id")]
    public int UserID { get; set; }
    
    [Display (Name = "Username")]
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username is too long")]
    [ MinLength(3, ErrorMessage = "Username is too short")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers")]
    public string Username { get; set; }
    
    [Display (Name = "Password")]
    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, ErrorMessage = "Password is too long")]
    [ MinLength(3, ErrorMessage = "Password is too short")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit and one special character")]
    [DataType("Password")]
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public string? Salt { get; set; }
    
    public PersonDatenModel PersonDaten { get; set; }
    
    
}