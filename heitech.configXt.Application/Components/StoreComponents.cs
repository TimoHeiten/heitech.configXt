using heitech.configXt.Core;
using Microsoft.Extensions.DependencyInjection;

namespace heitech.configXt.Application.Components
{
    public static class StoreComponents
    {
        public static IServiceCollection InMemoryStore(this IServiceCollection services)
        {
            services.AddSingleton<IStorageModel>(new InMemoryStore());
            return services;
        }
    }
}