using WebApp.Dal;
using WebApp.Model;

namespace WebApp.Service
{
    /// <summary>
    /// Dienstklasse zur Verwaltung von Fahrzeugen und zugehörigen Operationen.
    /// </summary>
    public class ServicesFahrzeuge
    {
        private readonly IAccessableFahrzeuge _sqlDal;

        /// <summary>
        /// Initialisiert eine neue Instanz der ServicesFahrzeuge-Klasse mit der angegebenen Konfiguration.
        /// </summary>
        /// <param name="configuration">Die Konfiguration, die die Verbindungszeichenfolge enthält.</param>
        public ServicesFahrzeuge(IConfiguration configuration)
        {
            string _connectionString = configuration.GetConnectionString("FahrzeugDal");
            _sqlDal = new FahrzeugDal(_connectionString);
        }

        #region Fahrzeug Methoden

        /// <summary>
        /// Ruft alle Fahrzeuge aus der Datenbank ab.
        /// </summary>
        /// <returns>Ein FahrzeugDTO-Objekt, das eine Liste aller Fahrzeuge enthält.</returns>
        public FahrzeugDTO GetAllFahrzeuge()
        {
            return new FahrzeugDTO
            {
                Fahrzeuge = _sqlDal.GetAllFahrzeuge()
            };
        }

        /// <summary>
        /// Ruft ein Fahrzeug anhand seiner ID aus der Datenbank ab.
        /// </summary>
        /// <param name="id">Die ID des Fahrzeugs.</param>
        /// <returns>Ein FahrzeugModel-Objekt, das das Fahrzeug darstellt.</returns>
        public FahrzeugModel GetFahrzeugById(int id)
        {
            return _sqlDal.GetFahrzeugById(id);
        }

        /// <summary>
        /// Fügt ein neues Fahrzeug in die Datenbank ein.
        /// </summary>
        /// <param name="fahrzeug">Das FahrzeugModel-Objekt, das die Details des Fahrzeugs enthält.</param>
        /// <returns>Die Anzahl der betroffenen Zeilen in der Datenbank.</returns>
        public int AddFahrzeug(FahrzeugModel fahrzeug)
        {
            return _sqlDal.AddFahrzeug(fahrzeug);
        }

        /// <summary>
        /// Aktualisiert die Details eines vorhandenen Fahrzeugs in der Datenbank.
        /// </summary>
        /// <param name="fahrzeug">Das FahrzeugModel-Objekt, das die aktualisierten Details des Fahrzeugs enthält.</param>
        /// <returns>True, wenn die Aktualisierung erfolgreich war, andernfalls False.</returns>
        public bool UpdateFahrzeug(FahrzeugModel fahrzeug)
        {
            return _sqlDal.UpdateFahrzeug(fahrzeug);
        }

        /// <summary>
        /// Löscht ein Fahrzeug aus der Datenbank.
        /// </summary>
        /// <param name="id">Die ID des zu löschenden Fahrzeugs.</param>
        /// <returns>True, wenn das Löschen erfolgreich war, andernfalls False.</returns>
        public bool DeleteFahrzeug(int id)
        {
            return _sqlDal.DeleteFahrzeug(id);
        }

        /// <summary>
        /// Fügt Fahrzeuge aus einer JSON-Datei in die Datenbank ein.
        /// </summary>
        /// <param name="json">Die JSON-Daten als Zeichenkette.</param>
        /// <returns>True, wenn der Import erfolgreich war, andernfalls False.</returns>
        public bool InsertFahrzeugeFromJson(string json)
        {
            return _sqlDal.InsertFahrzeugeFromJson(json);
        }

        #endregion

        #region Personen Methoden

        /// <summary>
        /// Fügt eine neue Person in die Datenbank ein.
        /// </summary>
        /// <param name="name">Der Name der Person.</param>
        /// <param name="eMail">Die E-Mail-Adresse der Person.</param>
        /// <param name="id">Die ID des Benutzers.</param>
        /// <returns>True, wenn das Einfügen erfolgreich war, andernfalls False.</returns>
        public bool InsertPerson(string name, string eMail, int id)
        {
            return _sqlDal.InsertPerson(name, eMail, id) > 0;
        }

        /// <summary>
        /// Ruft die ID der Person anhand der Benutzer-ID ab.
        /// </summary>
        /// <param name="userId">Die Benutzer-ID.</param>
        /// <returns>Die ID der Person.</returns>
        public int GetPersonIdByuserId(int userId)
        {
            return _sqlDal.GetPersonIdByuserId(userId);
        }

        /// <summary>
        /// Ruft eine Person anhand ihrer ID aus der Datenbank ab.
        /// </summary>
        /// <param name="id">Die ID der Person.</param>
        /// <returns>Ein PersonDatenModel-Objekt, das die Person darstellt, oder null, wenn keine Person gefunden wurde.</returns>
        public PersonDatenModel GetPersonById(int id)
        {
            return _sqlDal.GetPersonById(id);
        }

        #endregion

        #region Ausleihungen Methoden

        /// <summary>
        /// Fügt eine neue Ausleihe in die Datenbank ein.
        /// </summary>
        /// <param name="fId">Die ID des Fahrzeugs.</param>
        /// <param name="pId">Die ID der Person.</param>
        /// <param name="rueckgabeDatum">Das Rückgabedatum der Ausleihe.</param>
        /// <returns>True, wenn das Einfügen erfolgreich war, andernfalls False.</returns>
        public bool InsertAusleihe(int fId, int pId, DateTime rueckgabeDatum)
        {
            return _sqlDal.InsertAusleihe(fId, pId, rueckgabeDatum) > 0;
        }

        /// <summary>
        /// Ruft alle Ausleihungen aus der Datenbank ab.
        /// </summary>
        /// <returns>Eine Liste von AusleihungenModel-Objekten, die alle Ausleihungen repräsentieren.</returns>
        public List<AusleihungenModel> GetAllAusleihungen()
        {
            return _sqlDal.GetAllAusleihungen();
        }

        #endregion
    }
}
