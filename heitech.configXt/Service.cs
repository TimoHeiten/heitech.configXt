using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace heitech.configXt
{
    internal class Service : IService
    {
        private readonly IStore store;
        private readonly Dictionary<string, ConfigModel> _inMemoryMap;

        public Service(IStore store)
        {
            this.store = store;
            _inMemoryMap = new Dictionary<string, ConfigModel>();
        }

        internal void Initialize(Dictionary<string, ConfigModel> map)
            => map.ToList().ForEach(x => _inMemoryMap.Add(x.Key, x.Value));

        public Task<ConfigResult> CreateAsync(ConfigModel model)
            => Build(
                () => _inMemoryMap.ContainsKey(model.Key), 
                () => 
                {
                    _inMemoryMap.Add(model.Key, model); 
                    return model; 
                }, 
                Crud.Create);

        public Task<ConfigResult> DeleteAsync(string key)
            => Build
            (
                () => !_inMemoryMap.ContainsKey(key), 
                () => 
                {
                    var model =  _inMemoryMap[key];
                    _inMemoryMap.Remove(model.Key); 
                    return model; 
                }, 
                Crud.Delete
            );

        public Task<ConfigResult> RetrieveAsync(string key)
            => Build
            (
                () => !_inMemoryMap.ContainsKey(key), 
                () =>  _inMemoryMap[key], 
                Crud.Retrieve
            );

        public Task<ConfigResult> UpdateAsync(ConfigModel model)
            => Build
            (
                () => !_inMemoryMap.ContainsKey(model.Key), 
                () => 
                { 
                    _inMemoryMap[model.Key] = model; 
                    return model; 
                }, 
                Crud.Update
            );

        private async Task<ConfigResult> Build(Func<bool> mapPredicate, Func<ConfigModel> mapCallback, Crud operation)
        {
            bool predicateFullfilled = mapPredicate();
            if (predicateFullfilled)
                return ConfigResult.Failure(ConfigurationException.Create(operation, ConfigModel.Empty));

            var resultModel = mapCallback();
            if (operation != Crud.Retrieve)
                await store.FlushAsync(_inMemoryMap);

            return ConfigResult.Success(resultModel);
        }
    }
}