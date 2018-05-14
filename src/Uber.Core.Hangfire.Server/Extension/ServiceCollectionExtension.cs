using Hangfire;
using Hangfire.PostgreSql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHangfireServer(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<JobActivator, Uber.Core.Hangfire.Server.Compatibility.JobActivator>();

            services.AddHangfire(configuration =>
            {
                configuration.UseStorage(new PostgreSqlStorage(connectionString));
            });

            return services;
        }
    }
}
