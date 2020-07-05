using heitech.configXt.Core;
using heitech.configXt.Models;
using Microsoft.EntityFrameworkCore;
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
            var store = new InMemoryStore(initialAdminName, hash, appName);
            services.AddSingleton<IStorageModel>(store);
            services.AddSingleton<IAuthStorageModel>(store);
            return services;
        }

        ///<summary>
        /// Add Persistent Store with Sqlite. Connection string is current dir or otherwise specified if not null
        ///</summary>
        public static IServiceCollection AddPersistent(this IServiceCollection services, string sqliteConnection = null)
        {
            var store = new EfStore(sqliteConnection);
            services.AddSingleton<IStorageModel>(store);
            services.AddSingleton<IAuthStorageModel>(store);

            return services;
        }
    }
}