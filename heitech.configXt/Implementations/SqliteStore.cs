using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.configXt.Interface;
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
            model.Property(x => x.Kind)
                 .HasConversion(x => Enum.Parse<ConfigKind>(x), x => $"{x}")
                 .HasMaxLength(10)
                 .HasDefaultValueSql("0")
                 .IsRequired();
        }

        public Task FlushAsync()
            => SaveChangesAsync();

        public async Task<IReadOnlyList<ConfigModel>> GetAll()
            => await Set<ConfigModelEntity>().OrderBy(x => x.Id)
                                             .Select(x => x.ToConfigModel())
                                             .ToListAsync();

        public class ConfigModelEntity
        {
            public string Id { get; set; }
            public string Value { get; set; }
            public string Kind { get; set; }

            public ConfigModel ToConfigModel() => ConfigModel.From(Id, Value);

            public static ConfigModelEntity FromModel(ConfigModel model)
               => new ConfigModelEntity { Id = model.Key, Value = model.Value, Kind = $"{model.Kind}" };
        }
    }
}
