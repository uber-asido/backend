using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Uber.Core.EFCore;
using Uber.Core.Setup;

namespace Uber.Module.Movie.EFCore.Setup
{
    public class Migrate : IInstallStep
    {
        public string Name => "Migrate movies EF Core store";
        public int Priority => 0;
        public InstallPhase Phase => InstallPhase.Upgrade;

        private readonly ConnectionString connectionString;

        public Migrate(ConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }

        public Task<IEnumerable<string>> Run()
        {
            var result = DataMigrate.PerformUpgrade(connectionString.Value, Assembly.GetExecutingAssembly());
            return Task.FromResult(result);
        }

        public Task<bool> ShouldRun() => Task.FromResult(true);
    }
}
