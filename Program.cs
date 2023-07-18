using System;
using OpenSearch.Client;
using Spectre.Console;
using Newtonsoft.Json;
using OpenSearch.Net;
using Serilog;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Idnaf.LazyOpenSearch
{
    public static class Program
    {
        public static void ShowHelp()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Help:");
            AnsiConsole.WriteLine(" profile-create      Creates new profile in your $HOME/.lazyopensearch/profile.json");
            AnsiConsole.WriteLine("                       DebugMode default value is None but may also Console and File");
            AnsiConsole.WriteLine("                       If you select File as DebugMode, please set DebugLogFile path as well");
            AnsiConsole.WriteLine("                       Authentication.Mode default value is UsernamePassword but may also MutualAuth [work in progress]");
            AnsiConsole.WriteLine(" nodes-list          Lists OpenSearch nodes");
            AnsiConsole.WriteLine(" indices-list        Lists OpenSearch indices");
            AnsiConsole.WriteLine(" indices-delete      Delete OpenSearch indices");
            AnsiConsole.WriteLine(" indices-open        Open OpenSearch indices");
            AnsiConsole.WriteLine(" indices-close       Close OpenSearch indices");
            AnsiConsole.WriteLine(" cluster-info        CLuster info");
            AnsiConsole.WriteLine(" -h | --help | help  Shows this help");

        }
        [STAThread]
        public static int Main(string[] args)
        {
            
            if(args.Length == 0)
            {
                ShowHelp();
                return 0;
            }
            var command = args[0];

            switch (command)
            {
                case "profile-create":
                    Mode.Profile.ProfileCreate();
                    break;
                case "profile-edit":
                    Mode.Profile.ProfileEdit();
                    break;
                case "indices-list":
                    Mode.Indices.IndicesList();
                    break;
                case "indices-delete":
                    Mode.Indices.IndicesOps(Mode.Indices.IndicesOperations.Delete);
                    break;
                case "indices-open":
                    Mode.Indices.IndicesOps(Mode.Indices.IndicesOperations.Open);
                    break;
                case "indices-close":
                    Mode.Indices.IndicesOps(Mode.Indices.IndicesOperations.Close);
                    break;
                case "nodes-list":
                    Mode.Nodes.NodesList();
                    break;
                case "cluster-info":
                    Mode.Cluster.ClusterInfo();
                    break;
                case "help": case "-h": case "--help":
                    ShowHelp();
                    break;
                default:
                    Console.WriteLine($"Unknown command {args[0]}");
                    ShowHelp();
                    break;
            }
            return 0;
        }
    }
}