using System;
using System.Threading.Tasks;

namespace heitech.configXt
{
    public class DbService : IService
    {
        private readonly IStore _store;
        private readonly IRepository _repository;

        public DbService(IStore store, IRepository repository)
        {
            _store = store;
            _repository = repository;
        }

        public async Task<ConfigResult> RetrieveAsync(string key)
        {
            var result = await _repository.Get(key);
            return result == null
                   ? ConfigResult.Failure(ConfigurationException.Create(Crud.Retrieve, ConfigModel.PlaceHolder(key)))
                   : ConfigResult.Success(result);
        }

        public Task<ConfigResult> CreateAsync(ConfigModel model)
            => DoAsync(() => { _repository.Add(model); return Task.FromResult(model); }, model);

        public Task<ConfigResult> UpdateAsync(ConfigModel model)
        => DoAsync(() => _repository.UpdateAsync(model), model);

        public Task<ConfigResult> DeleteAsync(string key)
            => DoAsync(() => _repository.DeleteAsync(key), ConfigModel.PlaceHolder(key));


        private async Task<ConfigResult> DoAsync(Func<Task<ConfigModel>> callback, ConfigModel input)
        {
            try
            {
                var model = await callback();
                if (model is null)
                {
                    return ConfigResult.Failure(ConfigurationException.Create(Crud.Delete, input));
                }
                else
                {
                    await _store.FlushAsync();
                    return ConfigResult.Success(model);
                }
            }
            catch (System.Exception ex)
            {
                return ConfigResult.Failure(ex);
            }
        }
    }
}
