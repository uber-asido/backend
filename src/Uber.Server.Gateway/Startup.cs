using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using File = Uber.Module.File;
using Geocoding = Uber.Module.Geocoding;
using Movie = Uber.Module.Movie;
using Search = Uber.Module.Search;

namespace Uber.Server.Gateway
{
    public class Startup
    {
        public readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionStringFile = new File.EFCore.ConnectionString(Configuration.GetConnectionString("File"));
            var connectionStringGeocoding = new Geocoding.EFCore.ConnectionString(Configuration.GetConnectionString("Geocoding"));
            var connectionStringMovie = new Movie.EFCore.ConnectionString(Configuration.GetConnectionString("Movie"));
            var connectionStringSearch = new Search.EFCore.ConnectionString(Configuration.GetConnectionString("Search"));

            services.AddSingleton(connectionStringFile);
            services.AddSingleton(connectionStringGeocoding);
            services.AddSingleton(connectionStringMovie);
            services.AddSingleton(connectionStringSearch);

            services.AddInstaller();

            services.AddFile(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionStringFile.Value)));
            services.AddGeocoding(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionStringGeocoding.Value)));
            services.AddGeocodingGoogle(Configuration.GetSection("Google")["ApiKey"]);
            services.AddMovie(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionStringMovie.Value)));
            services.AddSearch(builder => builder.UseEFCoreStores(options => options.UseNpgsql(connectionStringSearch.Value)));

            services.AddHangfireServer(Configuration.GetConnectionString("Hangfire"));

            services
                .AddMvc()
                .AddFluentValidation();

            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ServicePointManager.DefaultConnectionLimit = 500;
            ServicePointManager.Expect100Continue = false;

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(100))
                .Build());

            app.UseMvc(routeBuilder =>
            {
                var odataBuilder = new ODataConventionModelBuilder(app.ApplicationServices);
                odataBuilder.EnableLowerCamelCase(NameResolverOptions.ProcessDataMemberAttributePropertyNames | NameResolverOptions.ProcessExplicitPropertyNames | NameResolverOptions.ProcessReflectedPropertyNames);

                app.UseSearchApi(odataBuilder);
                app.UseMovieApi(odataBuilder);

                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(1000).Count();
                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", odataBuilder.GetEdmModel());
            });

            app.UseHangfireServer();
        }
    }
}
