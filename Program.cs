namespace WebApp
{
    /// <summary>
    /// Die Hauptprogrammklasse für die Webanwendung.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        /// <param name="args">Ein Array von Argumenten, die an die Anwendung übergeben werden.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Konfiguriert die Anwendung als MVC-Anwendung.
            builder.Services.AddControllersWithViews();
            
            // Konfiguriert die Sitzungseinstellungen.
            builder.Services.AddSession(options =>
            {
                // Leerlaufzeit der Sitzung auf 60 Sekunden setzen.
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                
                // Die Sitzungscookies sind nur für HTTP-Anfragen zugänglich.
                options.Cookie.HttpOnly = true;
                
                // Das Sitzungscookie ist essenziell für die Anwendung.
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Aktiviert die Bereitstellung statischer Dateien.
            app.UseStaticFiles();

            // Konfiguriert das Routing.
            app.UseRouting();

            // Aktiviert das Sitzungsmanagement.
            app.UseSession();

            // Richtet die Standardcontrollerroute ein.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Startet die Anwendung.
            app.Run();
        }
    }
}