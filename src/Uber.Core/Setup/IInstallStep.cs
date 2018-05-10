using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uber.Core.Setup
{
    public interface IInstallStep
    {
        string Name { get; }
        int Priority { get; }
        InstallPhase Phase { get; }

        Task<bool> ShouldRun();
        Task<IEnumerable<string>> Run();
    }
}
