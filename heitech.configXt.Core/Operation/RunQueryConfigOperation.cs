using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Operation
{
    public class RunQueryConfigOperation : IRunConfigOperation
    {
        private readonly IConfigureQuery _query;
        private readonly QueryContext _context;
        public RunQueryConfigOperation(IConfigureQuery query, QueryContext context)
        {
            SanityChecks.CheckNull(query, $"ctor-{nameof(RunQueryConfigOperation)}");
            SanityChecks.CheckNull(context, $"ctor-{nameof(RunQueryConfigOperation)}");

            _query = query;
            _context = context;
        }
        public async Task<Result> ExecuteAsync()
        {
            var entityName = _context.Request.Name;
            ConfigEntity entity = await _context.StorageEngine.GetEntityByNameAsync(entityName);

            var queryResult = await _query.ExecuteQueryAsync(_context.Admin, entity);

            return queryResult;
        }
    }
}