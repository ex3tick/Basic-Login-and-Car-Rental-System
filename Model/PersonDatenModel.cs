using System.ComponentModel.DataAnnotations;

namespace WebApp.Model;

public class PersonDatenModel
{
    public int? PersonenId { get; set; }

    [Display(Name = "Name")]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name is too long")]
    [MinLength(3, ErrorMessage = "Name is too short")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
    public string Name { get; set; }


    [Display(Name = "Email")]
    [DataType("email")]
    [Required(ErrorMessage = "Email is required")]
    [StringLength(50, ErrorMessage = "Email is too long")]
    [MinLength(3, ErrorMessage = "Email is too short")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|de|net|org|info|biz|gov|edu|mil)$",
        ErrorMessage = "Email is not valid")]
    public string Email { get; set; }
}