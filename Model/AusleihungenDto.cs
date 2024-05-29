namespace WebApp.Model;

public class AusleihungenDto
{
    public List<AusleihungenModel> Ausleihungen { get; set; }
    public FahrzeugModel Fahrzeuge { get; set; }
    public PersonDatenModel Personen { get; set; }
    public UserModel User { get; set; }
    
    public List<FahrzeugModel> FahrzeugeList { get; set; }
    public List<PersonDatenModel> PersonenList { get; set; }

    public AusleihungenDto()
    {
        Ausleihungen = new List<AusleihungenModel>();
        Fahrzeuge = new FahrzeugModel();
        Personen = new PersonDatenModel();
        User = new UserModel();
    }
}