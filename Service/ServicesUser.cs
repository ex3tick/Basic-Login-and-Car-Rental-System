using WebApp.Dal;
using WebApp.Model;

namespace WebApp.Service
{
    /// <summary>
    /// Klasse zur Verwaltung der benutzerbezogenen Dienste.
    /// </summary>
    public class ServicesUser
    {
        private readonly IAccessableUser _userDal;

        /// <summary>
        /// Initialisiert eine neue Instanz der ServicesUser-Klasse mit der angegebenen Konfiguration.
        /// </summary>
        /// <param name="configuration">Die Konfiguration, die die Verbindungszeichenfolge enthält.</param>
        public ServicesUser(IConfiguration configuration)
        {
            _userDal = new UserDal(configuration.GetConnectionString("UserDal"));
        }

        #region Benutzerregistrierung und -anmeldung

        /// <summary>
        /// Registriert einen neuen Benutzer.
        /// </summary>
        /// <param name="user">Das UserModel-Objekt, das die Registrierungsinformationen enthält.</param>
        /// <returns>Die ID des neu registrierten Benutzers.</returns>
        public int RegisterUser(UserModel user)
        {
            return _userDal.Register(user);
        }

        /// <summary>
        /// Überprüft die Anmeldeinformationen eines Benutzers.
        /// </summary>
        /// <param name="user">Das UserModel-Objekt, das die Anmeldeinformationen enthält.</param>
        /// <returns>True, wenn die Anmeldeinformationen korrekt sind, andernfalls False.</returns>
        public bool LoginUser(UserModel user)
        {
            return _userDal.Login(user);
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
            return _userDal.IsAdmin(username);
        }

        /// <summary>
        /// Ruft die Benutzer-ID anhand des Benutzernamens ab.
        /// </summary>
        /// <param name="username">Der Benutzername des Benutzers.</param>
        /// <returns>Die Benutzer-ID.</returns>
        public int GetUserId(string username)
        {
            return _userDal.GetUserId(username);
        }

        /// <summary>
        /// Ruft einen Benutzer anhand seiner ID aus der Datenbank ab.
        /// </summary>
        /// <param name="id">Die ID des Benutzers.</param>
        /// <returns>Ein UserModel-Objekt, das den Benutzer darstellt, oder null, wenn kein Benutzer gefunden wurde.</returns>
        public UserModel GetUserById(int id)
        {
            return _userDal.GetUserById(id);
        }

        #endregion
    }
}
