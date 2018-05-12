using DbUp;
using System.Collections.Generic;
using System.Reflection;

namespace Uber.Core.EFCore
{
    public static class DataMigrate
    {
        public static IEnumerable<string> PerformUpgrade(string connectionString, Assembly sqlScriptAssembly)
        {
            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(sqlScriptAssembly)
                .LogToConsole()
                .Build();

            var upgradeResult = upgrader.PerformUpgrade();
            var result = upgradeResult.Successful ? null : new[] { upgradeResult.Error.ToString() };
            return result;
        }
    }
}
