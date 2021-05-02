﻿using System.Collections.Generic;
using System.Reflection;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Events;
using Confab.Shared.Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Infrastructure.Commands
{
    internal static class Extensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.Scan(s =>
                s.FromAssemblies(assemblies)
                    .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
            
            return services;
        }
    }
}