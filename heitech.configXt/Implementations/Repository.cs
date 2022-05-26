using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static heitech.configXt.SqliteStore;

namespace heitech.configXt
{
    public class Repository : IRepository
    {
        private readonly SqliteStore _store;
        private readonly DbSet<ConfigModelEntity> _table;
        public Repository(SqliteStore dbStore)
        {
            _store = dbStore;
            _table = _store.Set<ConfigModelEntity>();
        }

        public void Add(ConfigModel model) => _table.Add(ConfigModelEntity.FromModel(model));


        public async Task<ConfigModel> DeleteAsync(string key)
        {
            var item = await get(key);
            if (item != null)
                _table.Remove(item);

            return item?.ToConfigModel();
        } 

        public async Task<ConfigModel> Get(string id) 
        {
            var entity = await get(id);
            return entity?.ToConfigModel();
        }

        private async Task<ConfigModelEntity> get(string id)
            => await _table.SingleOrDefaultAsync(x => x.Id == id);

        public async Task<ConfigModel> UpdateAsync(ConfigModel model)
        {
            var item = await get(model.Key);
            if (item != null)
            {
                item.Id = model.Key;
                item.Value = model.Value;
            }
            return item?.ToConfigModel();
        }
    }
}
