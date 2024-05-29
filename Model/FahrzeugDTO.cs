namespace WebApp.Model;

public class FahrzeugDTO
{
    public FahrzeugModel Fahrzeug { get; set; }
    public List<FahrzeugModel> Fahrzeuge { get; set; }
    
    public PersonDatenModel Person { get; set; }
    public List<PersonDatenModel> Personen { get; set; }
    
    public AusleihungenModel Ausleihe { get; set; }
    public List<AusleihungenModel> Ausleihen { get; set; }
}