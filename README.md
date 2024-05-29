
# Schule CRUD Muster

Dieses Projekt ist ein einfaches ASP.NET Core MVC-Projekt zur Verwaltung von Fahrzeugen und Benutzern. Es ermöglicht das Hinzufügen, Aktualisieren, Anzeigen und Löschen von Fahrzeugen in einer MySQL- oder Microsoft SQL-Datenbank. Das Projekt verwendet Dapper als ORM für den Datenzugriff und Hashing für die Passwortsicherheit. Sitzungen werden für die Verwaltung von Benutzer-Logins verwendet.

## Projektstruktur

```
Schule_CRUD_Muster
├── WebApp
│   ├── Controllers
│   │   └── HomeController.cs
│   ├── Dal
│   │   ├── FahrzeugDal.cs
│   │   └── UserDal.cs
│   ├── Model
│   │   ├── AusleiheDto.cs
│   │   ├── AusleihungenDto.cs
│   │   ├── AusleihungenModel.cs
│   │   ├── FahrzeugDTO.cs
│   │   ├── FahrzeugModel.cs
│   │   ├── IAccessableFahrzeuge.cs
│   │   ├── IAccessableUser.cs
│   │   ├── PersonDatenModel.cs
│   │   └── UserModel.cs
│   ├── QRCode
│   │   └── FahrzeugQrCode.cs
│   ├── Service
│   │   ├── ServicesFahrzeuge.cs
│   │   └── ServicesUser.cs
│   ├── Views
│   │   └── Home
│   │       ├── AddFahrzeug.cshtml
│   │       ├── AlleReservierungen.cshtml
│   │       ├── FahrzeugJasonUpload.cshtml
│   │       ├── Index.cshtml
│   │       ├── Login.cshtml
│   │       ├── QrCode.cshtml
│   │       ├── Register.cshtml
│   │       ├── Reservierung.cshtml
│   │       └── UpdateFahrzeug.cshtml
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Program.cs
│   ├── README.md
│   └── .gitignore
```

### Ordner und Dateien

- **Controllers**
  - `HomeController.cs`: Enthält die Logik für die Verarbeitung von HTTP-Anfragen und die Interaktion mit den Services.

- **Dal (Data Access Layer)**
  - `FahrzeugDal.cs`: Datenzugriffsschicht für Fahrzeugdaten.
  - `UserDal.cs`: Datenzugriffsschicht für Benutzerdaten.

- **Model**
  - `AusleiheDto.cs`: Data Transfer Object für Ausleihen.
  - `AusleihungenDto.cs`: Data Transfer Object für eine Liste von Ausleihen.
  - `AusleihungenModel.cs`: Modell für Ausleihungen.
  - `FahrzeugDTO.cs`: Data Transfer Object für Fahrzeuge.
  - `FahrzeugModel.cs`: Modell für Fahrzeuge.
  - `IAccessableFahrzeuge.cs`: Schnittstelle für den Datenzugriff auf Fahrzeuge.
  - `IAccessableUser.cs`: Schnittstelle für den Datenzugriff auf Benutzer.
  - `PersonDatenModel.cs`: Modell für Personendaten.
  - `UserModel.cs`: Modell für Benutzer.

- **QRCode**
  - `FahrzeugQrCode.cs`: Klasse für die Generierung von QR-Codes für Fahrzeuge.

- **Service**
  - `ServicesFahrzeuge.cs`: Dienstklasse für die Geschäftslogik rund um Fahrzeuge.
  - `ServicesUser.cs`: Dienstklasse für die Geschäftslogik rund um Benutzer.

- **Views**
  - **Home**
    - `AddFahrzeug.cshtml`: Ansicht zum Hinzufügen eines Fahrzeugs.
    - `AlleReservierungen.cshtml`: Ansicht zur Anzeige aller Reservierungen.
    - `FahrzeugJasonUpload.cshtml`: Ansicht zum Hochladen von Fahrzeugdaten im JSON-Format.
    - `Index.cshtml`: Hauptansicht zur Anzeige aller Fahrzeuge.
    - `Login.cshtml`: Ansicht für die Benutzeranmeldung.
    - `QrCode.cshtml`: Ansicht zur Anzeige des generierten QR-Codes für ein Fahrzeug.
    - `Register.cshtml`: Ansicht für die Benutzerregistrierung.
    - `Reservierung.cshtml`: Ansicht zur Reservierung eines Fahrzeugs.
    - `UpdateFahrzeug.cshtml`: Ansicht zum Aktualisieren eines Fahrzeugs.

- **Konfigurationsdateien**
  - `appsettings.json`: Hauptkonfigurationsdatei.
  - `appsettings.Development.json`: Konfigurationsdatei für Entwicklungsumgebungen.
  - `Program.cs`: Einstiegspunkt des Programms.
  - `README.md`: Diese Datei.
  - `.gitignore`: Datei zur Angabe von Dateien und Verzeichnissen, die von der Versionskontrolle ausgeschlossen werden sollen.

### Funktionsweise des Programms

Das Programm ermöglicht die Verwaltung von Fahrzeugen und Benutzern durch folgende Funktionen:

- **Fahrzeuge verwalten**: Benutzer können Fahrzeuge hinzufügen, aktualisieren, anzeigen und löschen. Die Fahrzeugdaten werden in einer Datenbank gespeichert.
- **Benutzerverwaltung**: Benutzer können sich registrieren und anmelden. Administratorrechte werden überprüft und verwaltet.
- **Reservierungen**: Benutzer können Fahrzeuge reservieren. Alle Reservierungen werden angezeigt und verwaltet.
- **QR-Code-Generierung**: Für jedes Fahrzeug kann ein QR-Code generiert und angezeigt werden, der Informationen zum Fahrzeug enthält und einen Link zur Fahrzeugdetailseite bereitstellt.

Die Anwendung verwendet ASP.NET Core MVC für die Webentwicklung, Dapper für den Datenzugriff und ZXing für die QR-Code-Generierung. Die Konfiguration und Verbindung zur Datenbank erfolgt über die `appsettings.json`-Datei.
