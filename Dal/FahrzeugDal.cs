using System;
using System.Collections.Generic;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using WebApp.Model;

namespace WebApp.Dal
{
    public class FahrzeugDal : IAccessableFahrzeuge
    {
        private readonly string connectionStringLogin;

        /// <summary>
        /// Initialisiert eine neue Instanz der FahrzeugDal-Klasse mit der angegebenen Verbindungszeichenfolge.
        /// </summary>
        /// <param name="connectionString">Die Verbindungszeichenfolge für die Datenbankverbindung.</param>
        public FahrzeugDal(string connectionString)
        {
            connectionStringLogin = connectionString;
        }

        #region Fahrzeug Methoden ohne Dapper

        /// <summary>
        /// Ruft alle Fahrzeuge aus der Datenbank ab (ohne Dapper).
        /// </summary>
        /// <returns>Eine Liste von FahrzeugModel-Objekten, die alle Fahrzeuge repräsentieren.</returns>
        public List<FahrzeugModel> GetAllFahrzeuge()
        {
            List<FahrzeugModel> fahrzeuge = new List<FahrzeugModel>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM Fahrzeuge", connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fahrzeuge.Add(new FahrzeugModel
                            {
                                FId = reader.GetInt32(0),
                                Kennzeichen = reader.GetString(1),
                                Leistung = reader.GetInt32(2),
                                Kilometerstand = reader.GetInt32(3),
                                Belegt = reader.GetBoolean(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return fahrzeuge;
        }

        #endregion

        #region Fahrzeug Methoden mit Dapper

        /// <summary>
        /// Ruft ein Fahrzeug anhand seiner ID aus der Datenbank ab (mit Dapper).
        /// </summary>
        /// <param name="id">Die ID des Fahrzeugs.</param>
        /// <returns>Ein FahrzeugModel-Objekt, das das Fahrzeug darstellt, oder null, wenn kein Fahrzeug gefunden wurde.</returns>
        public FahrzeugModel GetFahrzeugById(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
                {
                    string sql = "SELECT * FROM Fahrzeuge WHERE FId = @id";
                    FahrzeugModel fahrzeug = connection.QuerySingleOrDefault<FahrzeugModel>(sql, new { id });
                    return fahrzeug;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Fügt ein neues Fahrzeug in die Datenbank ein (mit Dapper).
        /// </summary>
        /// <param name="fahrzeug">Das FahrzeugModel-Objekt, das die Details des Fahrzeugs enthält.</param>
        /// <returns>Die Anzahl der betroffenen Zeilen in der Datenbank.</returns>
        public int AddFahrzeug(FahrzeugModel fahrzeug)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
                {
                    string sql = "INSERT INTO Fahrzeuge (Kennzeichen, Leistung, Kilometerstand, Belegt) VALUES (@Kennzeichen, @Leistung, @Kilometerstand, @BelegtInt)";
                    int affectedRows = connection.Execute(sql, new { fahrzeug.Kennzeichen, fahrzeug.Leistung, fahrzeug.Kilometerstand, BelegtInt = fahrzeug.Belegt ? 1 : 0 });
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Aktualisiert die Details eines vorhandenen Fahrzeugs in der Datenbank (mit Dapper).
        /// </summary>
        /// <param name="fahrzeug">Das FahrzeugModel-Objekt, das die aktualisierten Details des Fahrzeugs enthält.</param>
        /// <returns>True, wenn die Aktualisierung erfolgreich war, andernfalls False.</returns>
        public bool UpdateFahrzeug(FahrzeugModel fahrzeug)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
                {
                    string sql = "UPDATE Fahrzeuge SET Kennzeichen = @Kennzeichen, Leistung = @Leistung, Kilometerstand = @Kilometerstand, Belegt = @BelegtInt WHERE FId = @FId";
                    int affectedRows = connection.Execute(sql, new { fahrzeug.Kennzeichen, fahrzeug.Leistung, fahrzeug.Kilometerstand, BelegtInt = fahrzeug.Belegt ? 1 : 0, fahrzeug.FId });
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Löscht ein Fahrzeug aus der Datenbank (mit Dapper).
        /// </summary>
        /// <param name="id">Die ID des zu löschenden Fahrzeugs.</param>
        /// <returns>True, wenn das Löschen erfolgreich war, andernfalls False.</returns>
        public bool DeleteFahrzeug(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
                {
                    string sql = "DELETE FROM Fahrzeuge WHERE FId = @id";
                    int affectedRows = connection.Execute(sql, new { id });
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Personen Methoden

        /// <summary>
        /// Fügt eine neue Person in die Datenbank ein.
        /// </summary>
        /// <param name="name">Der Name der Person.</param>
        /// <param name="eMail">Die E-Mail-Adresse der Person.</param>
        /// <param name="Id">Die ID des Benutzers.</param>
        /// <returns>Die Anzahl der betroffenen Zeilen in der Datenbank.</returns>
        public int InsertPerson(string name, string eMail, int Id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
            {
                string sql = "INSERT INTO Personen (Name, EMail, userId) VALUES (@Name, @EMail, @userId)";
                int affectedRows = connection.Execute(sql, new { Name = name, EMail = eMail, userId = Id });
                return affectedRows;
            }
        }

        /// <summary>
        /// Ruft eine Person anhand ihrer ID aus der Datenbank ab.
        /// </summary>
        /// <param name="id">Die ID der Person.</param>
        /// <returns>Ein PersonDatenModel-Objekt, das die Person darstellt, oder null, wenn keine Person gefunden wurde.</returns>
        public PersonDatenModel GetPersonById(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
            {
                string sql = "SELECT * FROM Personen WHERE PersonenId = @id";
                PersonDatenModel person = connection.QuerySingleOrDefault<PersonDatenModel>(sql, new { id });
                return person;
            }
        }

        /// <summary>
        /// Ruft die ID der Person anhand der Benutzer-ID ab.
        /// </summary>
        /// <param name="userId">Die Benutzer-ID.</param>
        /// <returns>Die ID der Person.</returns>
        public int GetPersonIdByuserId(int userId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
            {
                string sql = "SELECT PersonenId FROM Personen WHERE userId = @userId";
                int id = connection.QuerySingleOrDefault<int>(sql, new { userId });

                Console.WriteLine($"GetPersonIdByuserId: userId={userId}, PersonenId={id}");

                return id;
            }
        }

        #endregion

        #region Ausleihungen Methoden

        /// <summary>
        /// Fügt eine neue Ausleihe in die Datenbank ein.
        /// </summary>
        /// <param name="fId">Die ID des Fahrzeugs.</param>
        /// <param name="personenId">Die ID der Person.</param>
        /// <param name="rueckgabeDatum">Das Rückgabedatum der Ausleihe.</param>
        /// <returns>Die Anzahl der betroffenen Zeilen in der Datenbank.</returns>
        public int InsertAusleihe(int fId, int personenId, DateTime rueckgabeDatum)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
            {
                string sql = "INSERT INTO Ausleihungen (FId, PersonenId, Ausleidatum, Rueckgabedatum) VALUES (@FId, @PersonenId, @Ausleidatum, @Rueckgabedatum)";
                int affectedRows = connection.Execute(sql, new { FId = fId, PersonenId = personenId, Ausleidatum = DateTime.Now, Rueckgabedatum = rueckgabeDatum });
                return affectedRows;
            }
        }

        /// <summary>
        /// Ruft alle Ausleihungen aus der Datenbank ab.
        /// </summary>
        /// <returns>Eine Liste von AusleihungenModel-Objekten, die alle Ausleihungen repräsentieren.</returns>
        public List<AusleihungenModel> GetAllAusleihungen()
        {
            List<AusleihungenModel> ausleihungen = new List<AusleihungenModel>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM Ausleihungen", connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ausleihungen.Add(new AusleihungenModel
                            {
                                AusleiheId = reader.GetInt32(0),
                                FId = reader.GetInt32(1),
                                PersonenId = reader.GetInt32(2),
                                Ausleihdatum = reader.GetDateTime(3),
                                Rueckgabedatum = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ausleihungen;
        }

        #endregion

        #region JSON Import

        /// <summary>
        /// Fügt Fahrzeuge aus einer JSON-Datei in die Datenbank ein.
        /// </summary>
        /// <param name="json">Die JSON-Daten als Zeichenkette.</param>
        /// <returns>True, wenn der Import erfolgreich war, andernfalls False.</returns>
        public bool InsertFahrzeugeFromJson(string json)
        {
            try
            {
                var fahrzeuge = JsonConvert.DeserializeObject<List<FahrzeugModel>>(json);

                using (MySqlConnection connection = new MySqlConnection(connectionStringLogin))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        string sql = "INSERT INTO Fahrzeuge (Kennzeichen, Leistung, Kilometerstand, Belegt) VALUES (@Kennzeichen, @Leistung, @Kilometerstand, @BelegtInt)";

                        foreach (var fahrzeug in fahrzeuge)
                        {
                            var parameters = new { Kennzeichen = fahrzeug.Kennzeichen, Leistung = fahrzeug.Leistung, Kilometerstand = fahrzeug.Kilometerstand, BelegtInt = fahrzeug.Belegt ? 1 : 0 };
                            connection.Execute(sql, parameters, transaction);
                        }

                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
