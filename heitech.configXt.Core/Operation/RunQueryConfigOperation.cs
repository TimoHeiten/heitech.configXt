using System;
using System.Threading.Tasks;
using heitech.configXt.Core.Queries;

namespace heitech.configXt.Core.Operation
{
    public class RunQueryConfigOperation : IRunConfigOperation
    {
        private readonly Func<QueryContext, Task<OperationResult>> _query;
        private readonly QueryContext _context;
        public RunQueryConfigOperation(Func<QueryContext, Task<OperationResult>> query, QueryContext context)
        {
            SanityChecks.CheckNull(query, $"ctor-{nameof(RunQueryConfigOperation)}");
            SanityChecks.CheckNull(context, $"ctor-{nameof(RunQueryConfigOperation)}");

            _query = query;
            _context = context;
        }
        public Task<OperationResult> ExecuteAsync()
        {
            return _query(_context);
        }
    }
}