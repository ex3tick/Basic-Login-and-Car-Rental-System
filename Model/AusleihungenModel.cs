namespace WebApp.Model;

public class AusleihungenModel
{
    public int? AusleiheId { get; set; }
    public int FId{ get; set; }
    public int PersonenId { get; set; }
    public DateTime Ausleihdatum { get; set; }
    public DateTime Rueckgabedatum { get; set; }
}