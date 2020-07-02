using heitech.configXt.Core;
using Microsoft.Extensions.DependencyInjection;

namespace heitech.configXt.Application.Components
{
    public static class InteractComponent
    {
        ///<summary>
        /// Create an InMemory Store that uses Microsoft.Extensions.DependencyInjection for a MemoryStore Instance
        ///</summary>
        public static IServiceCollection AddInMemoryInteraction(this IServiceCollection services)
        {
            services.AddSingleton<IInteract>(config => 
            {
                IStorageModel model = config.GetRequiredService<IStorageModel>();
                IAuthStorageModel authModel = config.GetRequiredService<IAuthStorageModel>();

                return new MemoryInteract(model, authModel);
            });
            return services;
        }
    }
}