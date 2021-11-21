using System.Threading.Tasks;

namespace heitech.configXt
{
    ///<summary>
    /// Create the Configuration Service
    ///</summary>
    public static class Entry
    {
        // todo make configurable? 
        // IStore 
        public static async Task<IService> StartAsync(string path)
        {
            var store = new FileStore(path);
            var service = new Service(store);
            var map = await store.LoadAsync();
            service.Initialize(map);

            return service;
        }
    }
}