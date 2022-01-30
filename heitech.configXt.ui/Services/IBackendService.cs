
namespace todo_list_frontend.Application
{
    ///<summary>
    /// Service für Ajax Calls zum Backend
    ///</summary>
    public interface IBackendService
    {
        Task<T> GetAsync<T>(string key)
           where T : class;

        Task<IEnumerable<T>> GetAllAsync<T>()
            where T : class;

        Task PutAsync<T>(T entity)
            where T : class;

        Task PostAsync<T>(T entity)
            where T : class;

        Task DeleteAsync<T>(T entity)
            where T : class;
    }
}
