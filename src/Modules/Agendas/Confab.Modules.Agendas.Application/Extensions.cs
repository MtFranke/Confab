using Confab.Modules.Agendas.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Modules.Agendas.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IEventMapper, EventMapper>();
            return services;
        }
    }
}