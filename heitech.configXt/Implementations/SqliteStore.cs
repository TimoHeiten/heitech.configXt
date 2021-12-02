using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace heitech.configXt
{
    public class SqliteStore : DbContext, IStore
    {
        public SqliteStore(DbContextOptions<SqliteStore> options) : base(options)
        { }



        protected override void OnModelCreating(ModelBuilder mb)
        {
            var model = mb.Entity<ConfigModelEntity>();
            model.HasKey(x => x.Id);
            model.Property(x => x.Value)
                 .IsRequired();
        }

        public Task FlushAsync()
            => SaveChangesAsync();

        public async Task<Dictionary<string, ConfigModel>> GetAll()
            => (await Set<ConfigModelEntity>().ToListAsync())
                                              .ToDictionary(x => x.Id, x => x.ToConfigModel());

        public class ConfigModelEntity
        {
            public string Id { get; set; }
            public string Value { get; set; }

            internal void SetStringFromModel(ConfigModel model)
                => Value = System.Text.Json.JsonSerializer.Serialize(model.Value);

            public ConfigModel ToConfigModel()
                => ConfigModel.From(Id, System.Text.Json.JsonSerializer.Deserialize<object>(Value));

            public static ConfigModelEntity FromModel(ConfigModel model)
               => new ConfigModelEntity { Id = model.Key, Value = System.Text.Json.JsonSerializer.Serialize(model.Value) };
        }
    }
}
