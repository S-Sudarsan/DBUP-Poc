using System;
using System.Reflection;
using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using McMaster.Extensions.CommandLineUtils;

namespace DBUp_Poc
{
    public class Program
    {

        private static string connectionStringOptionTemplate = "-c|--connectionString";
        private static string defaultConnectionString =
            @"Server=<ServerName>;Database=<DatabaseName>;User Id=<userid>;Password=<password>;";

        public static void Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                // Get the app name from the assembly via reflection instead of hard-coding
                Name = Assembly.GetExecutingAssembly().GetName().Name
            };
            app.HelpOption(inherited: true);

            var connectionStringOption = app.Option(
                connectionStringOptionTemplate,
                $"(Optional) A SQL Server connection string. Default value is: \"{defaultConnectionString}\"",
                CommandOptionType.SingleValue,
                true);

            app.OnExecute(() =>
            {
                var connectionString = connectionStringOption.HasValue() ? connectionStringOption.Value() : defaultConnectionString;

                // Creates the DB if it doesn't exist 
                EnsureDatabase.For.SqlDatabase(connectionString);

                // Execute Migration scripts
                Console.WriteLine("Executing Migration scripts:");

                var upgrader =
                    DeployChanges.To
                        .SqlDatabase(connectionString)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), IsMigrationScripts)
                        .LogToConsole()
                        .Build();

                var result = upgrader.PerformUpgrade();

                if (IsError(result))
                    return;

                // Execute Compiled scripts
                Console.WriteLine("Executing SPs/Functions/Views:");

                var upgradeCompiledScripts = DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), IsCompiledScripts)
                    .JournalTo(new NullJournal())
                    .Build();

                var scriptsResult = upgradeCompiledScripts.PerformUpgrade();

                if (IsError(scriptsResult))
                    return;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.ResetColor();
                Console.ReadLine();
            });

            app.Execute(args);
        }

        private static bool IsMigrationScripts(string path)
        {
            return path.StartsWith("DBUp_Poc.Migration");
        }

        private static bool IsCompiledScripts(string path)
        {
            return path.StartsWith("DBUp_Poc.CompiledScripts");
        }

        private static bool IsError(DatabaseUpgradeResult result)
        {
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                Console.ReadLine();
                return true;
            }

            return false;
        }
    }
}
