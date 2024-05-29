using Microsoft.AspNetCore.Mvc;
using WebApp.Model;
using WebApp.Service;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ServicesFahrzeuge _servicesFahrzeuge;
        private readonly ServicesUser _servicesUser;

        /// <summary>
        /// Initialisiert eine neue Instanz des HomeController mit der angegebenen Konfiguration.
        /// </summary>
        /// <param name="configuration">Die Konfiguration, die die Verbindungszeichenfolge enthält.</param>
        public HomeController(IConfiguration configuration)
        {
            _servicesUser = new ServicesUser(configuration);
            _servicesFahrzeuge = new ServicesFahrzeuge(configuration);
        }

        #region Fahrzeug Management

        /// <summary>
        /// Ruft die Index-Ansicht auf und zeigt alle Fahrzeuge an.
        /// </summary>
        /// <returns>Die Index-Ansicht mit einer Liste aller Fahrzeuge.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.Role = HttpContext.Session.GetString("role");
            
            return View(_servicesFahrzeuge.GetAllFahrzeuge());
        }

        /// <summary>
        /// Handhabt POST-Anfragen an die Index-Ansicht.
        /// </summary>
        /// <param name="fahrzeug">Das FahrzeugDTO-Objekt, das die Details des Fahrzeugs enthält.</param>
        /// <returns>Eine Umleitung zur Index-Ansicht.</returns>
        [HttpPost]
        public IActionResult Index(FahrzeugDTO fahrzeug)
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Ruft die AddFahrzeug-Ansicht auf.
        /// </summary>
        /// <returns>Die AddFahrzeug-Ansicht.</returns>
        [HttpGet]
        public IActionResult AddFahrzeug()
        {
            return View(new FahrzeugModel());
        }

        /// <summary>
        /// Fügt ein neues Fahrzeug hinzu und leitet zur Index-Ansicht um, wenn die Eingaben gültig sind.
        /// </summary>
        /// <param name="fahrzeug">Das FahrzeugModel-Objekt, das die Details des neuen Fahrzeugs enthält.</param>
        /// <returns>Eine Umleitung zur Index-Ansicht oder die AddFahrzeug-Ansicht bei ungültigen Eingaben.</returns>
        [HttpPost]
        public IActionResult AddFahrzeug(FahrzeugModel fahrzeug)
        {
            if(HttpContext.Session.GetString("role") != "admin") return RedirectToAction("Index");
            
            if (ModelState.IsValid)
            {
                _servicesFahrzeuge.AddFahrzeug(fahrzeug);
                return RedirectToAction("Index");
            }
            return View(fahrzeug);
        }

        /// <summary>
        /// Ruft die UpdateFahrzeug-Ansicht auf, um ein Fahrzeug zu aktualisieren.
        /// </summary>
        /// <param name="id">Die ID des Fahrzeugs, das aktualisiert werden soll.</param>
        /// <returns>Die UpdateFahrzeug-Ansicht mit den Fahrzeugdetails oder NotFound, wenn das Fahrzeug nicht gefunden wird.</returns>
        [HttpGet]
        public IActionResult UpdateFahrzeug(int id)
        {
            if( HttpContext.Session.GetString("role") != "admin") return RedirectToAction("Index");
            var fahrzeug = _servicesFahrzeuge.GetFahrzeugById(id);
            if (fahrzeug == null)
            {
                return NotFound();
            }
            return View(fahrzeug);
        }

        /// <summary>
        /// Aktualisiert die Details eines vorhandenen Fahrzeugs und leitet zur Index-Ansicht um, wenn die Eingaben gültig sind.
        /// </summary>
        /// <param name="fahrzeugModel">Das FahrzeugModel-Objekt, das die aktualisierten Details des Fahrzeugs enthält.</param>
        /// <returns>Eine Umleitung zur Index-Ansicht oder die UpdateFahrzeug-Ansicht bei ungültigen Eingaben.</returns>
        [HttpPost]
        public IActionResult UpdateFahrzeug(FahrzeugModel fahrzeugModel)
        {
            if (ModelState.IsValid)
            {
                _servicesFahrzeuge.UpdateFahrzeug(fahrzeugModel);
                return RedirectToAction("Index");
            }
            return View(fahrzeugModel);
        }

        /// <summary>
        /// Löscht ein Fahrzeug anhand seiner ID und leitet zur Index-Ansicht um.
        /// </summary>
        /// <param name="id">Die ID des zu löschenden Fahrzeugs.</param>
        /// <returns>Eine Umleitung zur Index-Ansicht.</returns>
        [HttpPost]
        public IActionResult DeleteFahrzeug(int id)
        {
            if(HttpContext.Session.GetString("role") != "admin") return RedirectToAction("Index");
            _servicesFahrzeuge.DeleteFahrzeug(id);
            return RedirectToAction("Index");
        }

        #endregion

        #region Benutzerverwaltung

        /// <summary>
        /// Ruft die Register-Ansicht auf.
        /// </summary>
        /// <returns>Die Register-Ansicht.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            UserModel user = new UserModel();
            return View(user);
        }

        /// <summary>
        /// Registriert einen neuen Benutzer und leitet zur Index-Ansicht um, wenn die Eingaben gültig sind.
        /// </summary>
        /// <param name="user">Das UserModel-Objekt, das die Details des neuen Benutzers enthält.</param>
        /// <returns>Eine Umleitung zur Index-Ansicht oder die Register-Ansicht bei ungültigen Eingaben.</returns>
        [HttpPost]
        public IActionResult Register(UserModel user)
        {
            if (ModelState.IsValid)
            {
                user.UserID = _servicesUser.RegisterUser(user);
                bool success = user.UserID > 0;
                if (success) 
                    success = _servicesFahrzeuge.InsertPerson(user.PersonDaten.Name, user.PersonDaten.Email, user.UserID);
                if (success)
                    return RedirectToAction("Index");
            }
            return View(user);
        }

        /// <summary>
        /// Ruft die Login-Ansicht auf.
        /// </summary>
        /// <returns>Die Login-Ansicht.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            UserModel user = new UserModel();
            return View(user);
        }

        /// <summary>
        /// Authentifiziert den Benutzer und leitet zur Index-Ansicht um, wenn die Anmeldeinformationen korrekt sind.
        /// </summary>
        /// <param name="user">Das UserModel-Objekt, das die Anmeldeinformationen des Benutzers enthält.</param>
        /// <returns>Eine Umleitung zur Index-Ansicht oder die Login-Ansicht bei ungültigen Anmeldeinformationen.</returns>
        [HttpPost]
        public IActionResult Login(UserModel user)
        {
            bool success = _servicesUser.LoginUser(user);
            if (success)
            {
                int userId = _servicesUser.GetUserId(user.Username);
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetInt32("userid", userId);
                success = _servicesUser.IsAdmin(user.Username);
                if (success)
                {
                    HttpContext.Session.SetString("role", "admin");
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        /// <summary>
        /// Meldet den Benutzer ab und leitet zur Index-Ansicht um.
        /// </summary>
        /// <returns>Eine Umleitung zur Index-Ansicht.</returns>
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        #endregion

        #region Reservierung

        /// <summary>
        /// Ruft die Reservierung-Ansicht auf, um ein Fahrzeug zu reservieren.
        /// </summary>
        /// <param name="id">Die ID des Fahrzeugs, das reserviert werden soll.</param>
        /// <returns>Die Reservierung-Ansicht mit den Fahrzeugdetails.</returns>
        [HttpGet]
        public IActionResult Reservierung(int id)
        {
            if (HttpContext.Session.GetString("username") == null) return RedirectToAction("Login");
            AusleiheDto ausleihe = new AusleiheDto
            {
                Fahrzeug = _servicesFahrzeuge.GetFahrzeugById(id)
            };
            HttpContext.Session.SetInt32("fahrzeugId", id);
            return View(ausleihe);
        }

        /// <summary>
        /// Reserviert ein Fahrzeug und leitet zur Index-Ansicht um, wenn die Eingaben gültig sind.
        /// </summary>
        /// <param name="ausleiheDto">Das AusleiheDto-Objekt, das die Reservierungsdetails enthält.</param>
        /// <returns>Eine Umleitung zur Index-Ansicht oder die Reservierung-Ansicht bei ungültigen Eingaben.</returns>
        [HttpPost]
        public IActionResult Reservierung(AusleiheDto ausleiheDto)
        {
            if (HttpContext.Session.GetString("username") == null) return RedirectToAction("Login");

            int personId = _servicesFahrzeuge.GetPersonIdByuserId((int)HttpContext.Session.GetInt32("userid"));
            int fahrzeugId = (int)HttpContext.Session.GetInt32("fahrzeugId");
    
            try
            {
                _servicesFahrzeuge.InsertAusleihe(fahrzeugId, personId, ausleiheDto.Rueckgabe);
                FahrzeugModel fahrzeug = _servicesFahrzeuge.GetFahrzeugById(fahrzeugId);
                fahrzeug.Belegt = true;
                _servicesFahrzeuge.UpdateFahrzeug(fahrzeug);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError(string.Empty, "Es ist ein Fehler aufgetreten. Bitte versuchen Sie es erneut.");
                return View(ausleiheDto);
            }
        }
            
        /// <summary>
        /// Ruft die Ansicht mit allen Reservierungen auf.
        /// </summary>
        /// <returns>Die Ansicht mit allen Reservierungen.</returns>
        public IActionResult AlleReservierungen()
        {
            if (HttpContext.Session.GetString("role") != "admin") return RedirectToAction("Index");
            AusleihungenDto ausleihungen = new AusleihungenDto();
            ausleihungen.Ausleihungen = _servicesFahrzeuge.GetAllAusleihungen();
            List<FahrzeugModel> fahrzeuge = new List<FahrzeugModel>();
            List<PersonDatenModel> personen = new List<PersonDatenModel>();
            
            foreach (var ausleihe in ausleihungen.Ausleihungen)
            {
                fahrzeuge.Add(_servicesFahrzeuge.GetFahrzeugById(ausleihe.FId));
                personen.Add(_servicesFahrzeuge.GetPersonById(ausleihe.PersonenId));
            }
            
            ausleihungen.FahrzeugeList = fahrzeuge;
            ausleihungen.PersonenList = personen;
            
            return View(ausleihungen);
        }

        #endregion

        #region FahrzeugJsonUpload

        /// <summary>
        /// Ruft die FahrzeugJsonUpload-Ansicht auf.
        /// </summary>
        /// <returns>Die FahrzeugJsonUpload-Ansicht.</returns>
        [HttpGet]
        public IActionResult FahrzeugJsonUpload()
        {
            if (HttpContext.Session.GetString("role") != "admin") return RedirectToAction("Index");
            return View();
        }

        /// <summary>
        /// Handhabt den Upload einer JSON-Datei, um Fahrzeuge hinzuzufügen.
        /// </summary>
        /// <param name="file">Die hochgeladene JSON-Datei.</param>
        /// <returns>Eine Umleitung zur Index-Ansicht oder die FahrzeugJsonUpload-Ansicht bei ungültigen Eingaben.</returns>
        [HttpPost]
        public IActionResult FahrzeugJsonUpload(IFormFile file)
        {
            if (HttpContext.Session.GetString("role") != "admin") return RedirectToAction("Index");

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Bitte wählen Sie eine Datei aus.");
                return View();
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        _servicesFahrzeuge.InsertFahrzeugeFromJson(json);
                    }
                }
                ViewBag.Message = "Datei erfolgreich hochgeladen und verarbeitet.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError(string.Empty, "Es ist ein Fehler aufgetreten. Bitte versuchen Sie es erneut.");
                return View();
            }
        }

        #endregion

        #region QR Code

        /// <summary>
        /// Generiert einen QR-Code für das angegebene Fahrzeug.
        /// </summary>
        /// <param name="id">Die ID des Fahrzeugs, für das der QR-Code generiert werden soll.</param>
        /// <returns>Die Ansicht mit dem generierten QR-Code.</returns>
        [HttpGet]
        public IActionResult QrCode(int id)
        {
            if (HttpContext.Session.GetString("role") != "admin") return RedirectToAction("Index");
            FahrzeugModel fahrzeug = new FahrzeugModel();
            fahrzeug = _servicesFahrzeuge.GetFahrzeugById(id);
            QRCodeService qrCodeService = new QRCodeService();
            string imagePath = qrCodeService.GenerateQrCode(fahrzeug);
            ViewBag.ImagePath = imagePath;
            return View();
        }

        #endregion
    }
}
