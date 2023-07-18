using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Idnaf.LazyOpenSearch
{
    public class Util
    {
        public static string FormatFileSize(long bytes)
        {
            var unit = 1024;
            if (bytes < unit) { return $"{bytes} B"; }

            var exp = (int)(Math.Log(bytes) / Math.Log(unit));
            return $"{bytes / Math.Pow(unit, exp):F2}{("KMGTPE")[exp - 1]}B";
        }
        public static string HealthColor(string health)
        {
            return health switch
            {
                "green" => "[green]Green[/]",
                "yellow" => "[yellow]Yellow[/]",
                "red" => "[red]Red[/]",
                _ => "Unkown",
            };
        }

    }
}
