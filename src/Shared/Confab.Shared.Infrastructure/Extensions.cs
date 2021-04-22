using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Confab.Shared.Abstractions;
using Confab.Shared.Abstractions.Contexts;
using Confab.Shared.Abstractions.Modules;
using Confab.Shared.Infrastructure.Api;
using Confab.Shared.Infrastructure.Auth;
using Confab.Shared.Infrastructure.Contexts;
using Confab.Shared.Infrastructure.Exceptions;
using Confab.Shared.Infrastructure.Modules;
using Confab.Shared.Infrastructure.Services;
using Confab.Shared.Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

[assembly:InternalsVisibleTo("Confab.Bootstrapper")]
namespace Confab.Shared.Infrastructure
{
    internal static class Extensions
    {
        private const string CorsPolicy = "cors";
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IList<Assembly> assemblies,
            IList<IModule> modules)
        {

            var disabledModules = new List<string>();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                foreach (var (key, value) in configuration.AsEnumerable())
                {
                    if (!key.Contains(":module:enabled"))
                    {
                        continue;
                    }

                    if (!bool.Parse(value))
                    {
                        disabledModules.Add(key.Split(":")[0]);
                    }
                }
            }

            services.AddCors(cors =>
            {
                cors.AddPolicy(CorsPolicy, x =>
                {
                    x.WithOrigins("*")
                        .WithMethods("POST", "PUT", "DELETE")
                        .WithHeaders("Content-Type","Authorization");
                });
            });

            services.AddSwaggerGen(swagger =>
            {
                swagger.CustomSchemaIds( x => x.FullName);
                swagger.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Confab API",
                    Version = "v1"
                });
            });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IContext>(sp => sp.GetRequiredService<IContextFactory>().Create());
            services.AddSingleton<IContextFactory, ContextFactory>();
            
            services.AddErrorHandling();
            services.AddSingleton<IClock,UtcClock>();
            services.AddHostedService<AppInitializer>();
            services.AddAuth(modules);
            services.AddModuleInfo(modules);
            
            services
                .AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    var removedParts = new List<ApplicationPart>();
                    foreach (var disabledModule in disabledModules)
                    {
                        var parts = manager.ApplicationParts
                            .Where(x => x.Name.Contains(disabledModule, StringComparison.InvariantCultureIgnoreCase));
                        
                        removedParts.AddRange(parts);
                    }

                    foreach (var part in removedParts)
                    {
                        manager.ApplicationParts.Remove(part);
                    }
                    
                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                });
            
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseCors(CorsPolicy);
            app.UseErrorHandling();
            app.UseSwagger();
            app.UseReDoc(options =>
            {
                options.RoutePrefix = "docs";
                options.SpecUrl("/swagger/v1/swagger.json");
                options.DocumentTitle = "Confab API";
            });
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
        
            return app;
        }

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T: new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuation = serviceProvider.GetRequiredService<IConfiguration>();
            return configuation.GetOptions<T>(sectionName);
        }
        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T: new()
        {
            var options = new T();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}