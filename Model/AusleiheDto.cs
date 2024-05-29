namespace WebApp.Model;

public class AusleiheDto
{
public FahrzeugModel Fahrzeug { get; set; }
public DateTime Rueckgabe { get; set; }
public DateTime Ausleihe { get; set; }

public AusleiheDto()
{
    Ausleihe = DateTime.Now; 
}


}