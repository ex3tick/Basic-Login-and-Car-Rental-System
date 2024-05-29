using MySqlConnector;
using WebApp.Model;

namespace WebApp.Dal
{
    /// <summary>
    /// Klasse für den Datenzugriff auf Benutzerinformationen.
    /// </summary>
    public class UserDal : IAccessableUser
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initialisiert eine neue Instanz der UserDal-Klasse mit der angegebenen Verbindungszeichenfolge.
        /// </summary>
        /// <param name="connectionString">Die Verbindungszeichenfolge für die Datenbankverbindung.</param>
        public UserDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Login und Registrierung

        /// <summary>
        /// Überprüft die Anmeldeinformationen eines Benutzers.
        /// </summary>
        /// <param name="user">Das UserModel-Objekt, das die Anmeldeinformationen enthält.</param>
        /// <returns>True, wenn die Anmeldeinformationen korrekt sind, andernfalls False.</returns>
        public bool Login(UserModel user)
        {
            bool result = false;
            string pepper = "kleberAsche12";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT Password, Salt FROM Users WHERE Username = @username";
                    command.Parameters.AddWithValue("@username", user.Username);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string storedPasswordHash = reader["Password"].ToString();
                        string salt = reader["Salt"].ToString();

                        string userPasswordHash = HashHelper.HashHelper.HashGenerate(user.Password, salt, pepper);

                        if (storedPasswordHash == userPasswordHash)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// Registriert einen neuen Benutzer.
        /// </summary>
        /// <param name="user">Das UserModel-Objekt, das die Registrierungsinformationen enthält.</param>
        /// <returns>Die ID des neu registrierten Benutzers.</returns>
        public int Register(UserModel user)
        {
            int newId = 0;
            string pepper = "kleberAsche12";
            string salt = HashHelper.HashHelper.SaltGenerate();
            string passwordHash = HashHelper.HashHelper.HashGenerate(user.Password, salt, pepper);

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Users (Username, Password, Salt, IsAdmin) VALUES (@username, @password, @salt, @isAdmin)";
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", passwordHash);
                    command.Parameters.AddWithValue("@salt", salt);
                    command.Parameters.AddWithValue("@isAdmin", user.IsAdmin);
                    command.ExecuteNonQuery();

                    // Abfrage zur Ermittlung der zuletzt eingefügten ID
                    command.CommandText = "SELECT LAST_INSERT_ID()";
                    newId = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return newId; // Rückgabe der neuen ID
        }

        #endregion

        #region Benutzerverwaltung

        /// <summary>
        /// Überprüft, ob ein Benutzer Administratorrechte hat.
        /// </summary>
        /// <param name="username">Der Benutzername des Benutzers.</param>
        /// <returns>True, wenn der Benutzer Administrator ist, andernfalls False.</returns>
        public bool IsAdmin(string username)
        {
            bool result = false;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT IsAdmin FROM Users WHERE Username = @username";
                    command.Parameters.AddWithValue("@username", username);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int isAdminValue = reader.GetInt32(reader.GetOrdinal("IsAdmin"));
                        result = isAdminValue == 1;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Ruft die Benutzer-ID anhand des Benutzernamens ab.
        /// </summary>
        /// <param name="username">Der Benutzername des Benutzers.</param>
        /// <returns>Die Benutzer-ID.</returns>
        public int GetUserId(string username)
        {
            int userId = 0;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT UserId FROM Users WHERE Username = @username";
                    command.Parameters.AddWithValue("@username", username);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return userId;
        }

        /// <summary>
        /// Ruft einen Benutzer anhand seiner ID aus der Datenbank ab.
        /// </summary>
        /// <param name="id">Die ID des Benutzers.</param>
        /// <returns>Ein UserModel-Objekt, das den Benutzer darstellt, oder null, wenn kein Benutzer gefunden wurde.</returns>
        public UserModel GetUserById(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Users WHERE UserId = @id";
                    command.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        UserModel user = new UserModel
                        {
                            UserID = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            IsAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin"))
                        };
                        return user;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return null;
        }

        #endregion
        
       
    }
}
