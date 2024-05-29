using System.ComponentModel.DataAnnotations;

namespace WebApp.Model;

public class FahrzeugModel
{
    [Display(Name = "Id")]
    public int? FId { get; set; }

    [StringLength(maximumLength: 9, MinimumLength = 5,
        ErrorMessage = "Kennzeichen muss zwischen 5 und 9 Zeichen lang sein")]
    [Display(Name = "Kennzeichen")]
    [Required(ErrorMessage = "Kennzeichen ist erforderlich")]
    public String? Kennzeichen { get; set; }

    [Display(Name = "Leistung")]
    [Range(0, 1000, ErrorMessage = "Leistung muss zwischen 0 und 1000 liegen")]
    [Required(ErrorMessage = "Leistung ist erforderlich")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Leistung muss eine Zahl sein")]
    public int Leistung { get; set; }


    [Display(Name = "Kilometerstand")]
    [Range(0, 1000000, ErrorMessage = "Kilometerstand muss zwischen 0 und 1000000 liegen")]
    [Required(ErrorMessage = "Kilometerstand ist erforderlich")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Kilometerstand muss eine Zahl sein")]
    public int Kilometerstand { get; set; }

    [Display(Name = "Belegt")] public bool Belegt { get; set; }


    public int BelegtInt => Belegt ? 1 : 0;
    
    public FahrzeugModel()
    {
        FId = 0;
    }
}