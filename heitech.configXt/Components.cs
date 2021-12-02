using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace heitech.configXt
{
    ///<summary>
    /// Create the Configuration Service
    ///</summary>
    public static class Components
    {
        public static IServiceCollection AddServices(this IServiceCollection services, string dbName = "configStore")
        {
            string path = Path.Combine(Environment.CurrentDirectory, dbName);
            
            services.AddDbContext<SqliteStore>(x => x.UseSqlite($"Data Source={path}.db;"));
            services.AddScoped<IStore>(x => x.GetRequiredService<SqliteStore>());
            services.AddScoped<IService, DbService>();
            services.AddScoped<IRepository, Repository>();

            return services;
        }
    }
}