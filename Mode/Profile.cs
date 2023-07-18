using Newtonsoft.Json;
using Spectre.Console;

namespace Idnaf.LazyOpenSearch.Mode
{
    public class Profile : Base
    {
        public static void ProfileCreate()
        {
            if (!File.Exists(configFile))
            {
                if (!Directory.Exists(Path.GetDirectoryName(configFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                }
                File.WriteAllText(configFile, JsonConvert.SerializeObject(new Model.Profile(), Formatting.Indented));
            }
        }
        public static void ProfileEdit()
        {
            if (!File.Exists(configFile))
            {
                ProfileCreate();
            }
            var profile = JsonConvert.DeserializeObject<Model.Profile>(File.ReadAllText(configFile));
            if(profile == null)
            {
                return;
            }

            if(string.IsNullOrEmpty(profile.Name))
            {
                profile.Name = AnsiConsole.Prompt(
                    new TextPrompt<string>("Profile [green]name[/]")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid input[/]")
                        .Validate(profile =>
                        {
                            return string.IsNullOrEmpty(profile) ? ValidationResult.Error("Invalid input") : ValidationResult.Success();
                        }));
            }
            else
            {
                AnsiConsole.WriteLine($"Profile name: {profile.Name}");
            }

            profile.URL = AnsiConsole.Prompt(
                new TextPrompt<string>("OpenSearch [green]URL[/] (comma separated)")
                    .DefaultValue(profile.URL)
                    .ValidationErrorMessage("[red]That's not a valid input[/]")
                    .Validate(profile =>
                    {
                        return string.IsNullOrEmpty(profile) ? ValidationResult.Error("Invalid input") : ValidationResult.Success();
                    }));
            profile.Authentication.Mode = AnsiConsole.Prompt(
                    new SelectionPrompt<Model.AuthMode>()
                    .Title("Select authentication mode")
                    .AddChoices(Enum.GetValues<Model.AuthMode>())
                ) ;
            switch(profile.Authentication.Mode)
            {
                case Model.AuthMode.UsernamePassword:
                    profile.Authentication.Username = AnsiConsole.Prompt(
                    new TextPrompt<string>("Authentication [green]username[/]")
                        .PromptStyle("green")
                        .DefaultValue(profile.Authentication.Username)
                        .ValidationErrorMessage("[red]That's not a valid input[/]")
                        .Validate(profile =>
                        {
                            return string.IsNullOrEmpty(profile) ? ValidationResult.Error("Invalid input") : ValidationResult.Success();
                        }));
                    profile.Authentication.Password = AnsiConsole.Prompt(
                    new TextPrompt<string>("Authentication [green]password[/]")
                        .PromptStyle("green")
                        .DefaultValue(profile.Authentication.Password) 
                        .Secret<string>()
                        .ValidationErrorMessage("[red]That's not a valid input[/]")
                        .Validate(profile =>
                        {
                            return string.IsNullOrEmpty(profile) ? ValidationResult.Error("Invalid input") : ValidationResult.Success();
                        }));
                    profile.Authentication.AuthFilePassword = string.Empty;
                    profile.Authentication.AuthFile = string.Empty;
                    break;
                case Model.AuthMode.MutualAuth:
                    profile.Authentication.AuthFile = AnsiConsole.Prompt(
                    new TextPrompt<string>("Authentication [green]file[/]")
                        .PromptStyle("green")
                        .DefaultValue(profile.Authentication.AuthFile)
                        .ValidationErrorMessage("[red]That's not a valid input[/]")
                        .Validate(profile =>
                        {
                            return string.IsNullOrEmpty(profile) ? ValidationResult.Error("Invalid input") : ValidationResult.Success();
                        }));
                    profile.Authentication.AuthFilePassword = AnsiConsole.Prompt(
                    new TextPrompt<string>("Authentication [green]password[/]")
                        .PromptStyle("green")
                        .DefaultValue(profile.Authentication.AuthFilePassword)
                        .Secret<string>()
                        .AllowEmpty()
                        );
                    profile.Authentication.Username = string.Empty;
                    profile.Authentication.Password = string.Empty;
                    break;
            }
            profile.DebugMode = AnsiConsole.Prompt(
                    new SelectionPrompt<Model.DebugMode>()
                    .Title("Select debug mode")
                    .AddChoices(Enum.GetValues<Model.DebugMode>())
                );
            switch(profile.DebugMode)
            {
                case Model.DebugMode.File:
                    profile.DebugLogFile = AnsiConsole.Prompt(
                    new TextPrompt<string>("Debug [green]log file[/]")
                        .PromptStyle("green")
                        .DefaultValue(profile.DebugLogFile)
                        .ValidationErrorMessage("[red]That's not a valid input[/]")
                        .Validate(profile =>
                        {
                            return string.IsNullOrEmpty(profile) ? ValidationResult.Error("Invalid input") : ValidationResult.Success();
                        }));
                    break;

            }

            File.WriteAllText(configFile, JsonConvert.SerializeObject(profile, Formatting.Indented));
        }
    }
}
