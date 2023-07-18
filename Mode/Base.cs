using Newtonsoft.Json;
using OpenSearch.Client;
using OpenSearch.Net;
using Serilog;
using System.Security.Cryptography.X509Certificates;


namespace Idnaf.LazyOpenSearch.Mode
{
    public class Base
    {
        internal static readonly string configFile = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".lazyopensearch", "profile.json");
        internal static OpenSearchClient? client = null;
        public static void Init()
        {
            var profile = JsonConvert.DeserializeObject<Model.Profile>(File.ReadAllText(configFile));
            if (profile == null)
            {
                Console.WriteLine("Empty profile, please create one...");
                client = null;
                return;
            }
            var uris = new List<Uri>();
            foreach (string s in profile.URL.Split(","))
            {
                uris.Add(new Uri(s));
            }

            var connectionPool = new StaticConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool);
            switch (profile.DebugMode)
            {
                case Model.DebugMode.Console:
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();
                    settings.EnableDebugMode(apiCallDetails =>
                    {
                        Log.Information($"{apiCallDetails.DebugInformation}");
                    });
                    break;
                case Model.DebugMode.File:
                    string logFile = profile.DebugLogFile == string.Empty ? "lazyopensearch.log" : profile.DebugLogFile;
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.File($"{logFile}",
                            rollingInterval: RollingInterval.Day,
                            rollOnFileSizeLimit: true)
                        .CreateLogger();
                    settings.EnableDebugMode(apiCallDetails =>
                    {
                        Log.Information($"{apiCallDetails.DebugInformation}");
                    });
                    break;
            }

            if (profile.IgnoreTLSCertValidation)
            {
                // Disable certificate validation
                settings.ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);
            }
            switch (profile.Authentication.Mode)
            {
                case Model.AuthMode.UsernamePassword:
                    settings.BasicAuthentication(profile.Authentication.Username, profile.Authentication.Password);
                    break;
                case Model.AuthMode.MutualAuth:
                    var certificate = new X509Certificate2(profile.Authentication.AuthFile, profile.Authentication.AuthFilePassword);
                    settings.ClientCertificate(certificate);
                    break;
            }
            client = new OpenSearchClient(settings);
        }
    }
}
