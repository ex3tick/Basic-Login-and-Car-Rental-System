namespace WebApp.Model;

public interface IAccessableFahrzeuge
{
    List<FahrzeugModel> GetAllFahrzeuge();
    FahrzeugModel GetFahrzeugById(int id);
    int AddFahrzeug(FahrzeugModel fahrzeug);
    bool UpdateFahrzeug(FahrzeugModel fahrzeug);
    bool DeleteFahrzeug(int id);
    int InsertPerson(string name, string eMail, int id);
    int InsertAusleihe(int fId, int pId, DateTime rueckegabeDatum);
    int GetPersonIdByuserId(int userId);
    
    PersonDatenModel GetPersonById(int id);
    List<AusleihungenModel> GetAllAusleihungen();
    
    bool InsertFahrzeugeFromJson(string json);
}