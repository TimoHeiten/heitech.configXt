using heitech.configXt.Core;
using heitech.configXt.Models;
using Microsoft.Extensions.DependencyInjection;

namespace heitech.configXt.Application.Components
{
    public static class StoreComponents
    {
        ///<summary>
        /// Create an InMemory Store that uses Microsoft.Extensions.DependencyInjection for a MemoryStore Instance
        ///</summary>
        public static IServiceCollection InMemoryStore(this IServiceCollection services, string initialAdminName, string pw, string appName = "testAdmin")
        {
            string hash = PasswordHasher.GenerateHash(pw);
            services.AddSingleton<IStorageModel>(new InMemoryStore(initialAdminName, hash, appName));
            return services;
        }
    }
}