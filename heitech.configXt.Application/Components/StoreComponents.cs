using heitech.configXt.Core;
using Microsoft.Extensions.DependencyInjection;

namespace heitech.configXt.Application.Components
{
    public static class StoreComponents
    {
        ///<summary>
        /// Create an InMemory Store that uses Microsoft.Extensions.DependencyInjection for a MemoryStore Instance
        ///</summary>
        public static IServiceCollection InMemoryStore(this IServiceCollection services)
        {
            services.AddSingleton<IStorageModel>(new InMemoryStore());
            return services;
        }
    }
}