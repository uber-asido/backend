using Hangfire;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHangfireActivators(this IServiceCollection services)
        {
            services.AddSingleton<JobActivator, Uber.Core.Hangfire.Server.Compatibility.JobActivator>();
            return services;
        }
    }
}
