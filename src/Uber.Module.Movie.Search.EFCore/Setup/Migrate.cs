using DbUp;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Uber.Core.Setup;

namespace Uber.Module.Movie.Search.EFCore
{
    public class Migrate : IInstallStep
    {
        public string Name => "Migrate Movie Search EF Core store";
        public int Priority => 0;
        public InstallPhase Phase => InstallPhase.Upgrade;

        private readonly ConnectionString connectionStrings;

        public Migrate(ConnectionString connectionStrings)
        {
            this.connectionStrings = connectionStrings;
        }

        public Task<IEnumerable<string>> Run()
        {
            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionStrings.Value)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var upgradeStatus = upgrader.PerformUpgrade();
            var result = upgradeStatus.Successful ? null : new[] { upgradeStatus.Error.ToString() };
            return Task.FromResult<IEnumerable<string>>(result);
        }

        public Task<bool> ShouldRun() => Task.FromResult(true);
    }
}
