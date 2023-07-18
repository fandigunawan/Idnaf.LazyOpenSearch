using OpenSearch.Client;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idnaf.LazyOpenSearch.Mode
{
    public class Indices : Base
    {
        public static void IndicesList()
        {
            Init();
            if (client == null)
            {
                return;
            }
            var response = client.Cat.Indices(new CatIndicesRequest());

            if (response.IsValid)
            {
                var table = new Table();
                table.AddColumn("Indices");
                table.AddColumn("Health");
                table.AddColumn("Status");
                table.AddColumn("Docs Count");
                table.AddColumn("Store Size");
                table.AddColumn("Primary Shards");
                table.AddColumn("Replica Shards");

                foreach (var index in response.Records)
                {

                    table.AddRow(new string[] {
                        index.Index,
                        Util.HealthColor(index.Health),
                        index.Status,
                        index.DocsCount,
                        index.StoreSize,
                        index.Primary,
                        index.Replica
                    });
                }
                AnsiConsole.Write(table);
            }
            else
            {
                Console.WriteLine($"Error: {response.OriginalException.Message}");
            }
        }
        public enum IndicesOperations
        {
            Delete,
            Close,
            Open
        }
        public static void IndicesOps(IndicesOperations ops)
        {
            Init();
            if (client == null)
            {
                return;
            }

            var indices = client.Cat.Indices(new CatIndicesRequest()).Records.Select(i => i.Index).ToList();

            var selectedIndices = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Select [green]indices[/] to delete")
                .NotRequired()
                .PageSize(25)
                .MoreChoicesText("[grey](Move up and down to reveal more indices)[/]")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a index, " +
                    "[green]<enter>[/] to accept)[/]")
                .AddChoices(indices)
                );
            if(AnsiConsole.Confirm($"Are you sure to delete:\n{string.Join('\n', selectedIndices)}\n", false))
            {
                foreach (var i in selectedIndices)
                {
                    bool isValid = false;
                    switch(ops)
                    {
                        case IndicesOperations.Delete:
                            isValid = client.LowLevel.Indices.Delete<DeleteIndexResponse>(i).IsValid;
                            break;
                        case IndicesOperations.Close:
                            isValid = client.LowLevel.Indices.Close<CloseIndexResponse>(i).IsValid;
                            break;
                        case IndicesOperations.Open:
                            isValid = client.LowLevel.Indices.Open<OpenIndexResponse>(i).IsValid;
                            break;
                    }
                    
                    if(isValid)
                    {
                        AnsiConsole.WriteLine($"[green]Sucess[/] {ops} {i}");
                    }
                    else
                    {
                        AnsiConsole.WriteLine($"[red]Failed[/] {ops} {i}");
                    }
                }
                AnsiConsole.WriteLine("Done...");
            }
        }
    }
}
