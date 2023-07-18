using OpenSearch.Client;
using Spectre.Console;

namespace Idnaf.LazyOpenSearch.Mode
{
    public class Cluster : Base
    {
        public static void ClusterInfo()
        {
            Init();
            if (client == null)
            {
                return;
            }
            var catHealthResponse = client.Cat.Health(new CatHealthRequest());

            if (catHealthResponse.IsValid)
            {
                var table = new Table();
                table.AddColumn("Name");
                table.AddColumn("Status");
                table.AddColumn("Data node");
                table.AddColumn("Total node");
                table.AddColumn("Primary");
                table.AddColumn("Shards");
                table.AddColumn("Relocating");
                table.AddColumn("Unassigned");

                foreach (var cat in catHealthResponse.Records)
                {
                    table.AddRow(new string[] {
                        cat.Cluster,
                        Util.HealthColor(cat.Status),
                        cat.NodeData,
                        cat.NodeTotal,
                        cat.Primary,
                        cat.Shards,
                        cat.Relocating,
                        cat.Unassigned
                    });
                }

                AnsiConsole.Write(table);
            }
            else
            {
                Console.WriteLine($"Error: {catHealthResponse.OriginalException.Message}");
            }
        }
    }
}
