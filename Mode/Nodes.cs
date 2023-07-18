using OpenSearch.Client;
using Spectre.Console;

namespace Idnaf.LazyOpenSearch.Mode
{
    public class Nodes : Base
    {
        public static void NodesList()
        {
            Init();
            if (client == null)
            {
                return;
            }

            var nodesInfoResponse = client.Nodes.Info(new NodesInfoRequest());
            var nodesStatsResponse = client.Nodes.Stats(new NodesStatsRequest());
            if (nodesInfoResponse.IsValid)
            {
                var table = new Table();
                table.AddColumn("Node Name");
                table.AddColumn("Hostname");
                table.AddColumn("IP Address");
                table.AddColumn("Version");
                table.AddColumn("Roles");
                table.AddColumn("Reseources");
                table.AddColumn("Plugins");
                foreach (var node in nodesInfoResponse.Nodes)
                {
                    string plugins = string.Empty;
                    foreach (var plugin in node.Value.Plugins)
                    {
                        plugins += $"{plugin.Name}({plugin.Version}) ";
                    }
                    table.AddRow(new string[] {
                            node.Key,
                            node.Value.Name,
                            node.Value.Ip,
                            // OpenSearch Version and JVM
                            string.Join("\n", new string[] {
                                $"OS: {node.Value.Version}",
                                $"JVM: {node.Value.Jvm.Version}"
                            }),
                            string.Join(", ", node.Value.Roles),

                            // Resource
                            string.Join("\n", new string[] {
                                "Mem: " + Util.FormatFileSize(nodesStatsResponse.Nodes[node.Key].OperatingSystem.Memory.UsedInBytes)
                                    + "/" +
                                    Util.FormatFileSize(nodesStatsResponse.Nodes[node.Key].OperatingSystem.Memory.TotalInBytes),
                                "Disk: " + Util.FormatFileSize(nodesStatsResponse.Nodes[node.Key].FileSystem.Total.TotalInBytes - nodesStatsResponse.Nodes[node.Key].FileSystem.Total.AvailableInBytes)
                                    + "/" +
                                    Util.FormatFileSize(nodesStatsResponse.Nodes[node.Key].FileSystem.Total.TotalInBytes)                          
                            }),
                            plugins
                    });
                }

                AnsiConsole.Write(table);
            }
            else
            {
                Console.WriteLine($"Error: {nodesInfoResponse.OriginalException.Message}");
            }

        }
    }
}
