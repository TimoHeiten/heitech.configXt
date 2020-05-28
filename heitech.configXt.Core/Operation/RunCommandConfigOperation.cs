using System.Threading.Tasks;

namespace heitech.configXt.Core.Operation
{
    public class RunCommandConfigOperation : IRunConfigOperation
    {
        private readonly IConfigureCommand _command;
        private readonly CommandContext _context;

        public RunCommandConfigOperation(IConfigureCommand command, CommandContext context)
        {
            _command = command;
            _context = context;
        }

        public async Task<Result> ExecuteAsync()
        {
            var storage = _context.StorageEngine;

            var entity = await storage.GetEntityByNameAsync(_context.ChangeRequest.Name);
            if (entity == null)
            {
                SanityChecks.NotFound(_context.ChangeRequest.Name, $"{nameof(RunCommandConfigOperation)}.{nameof(ExecuteAsync)}");
            }

            var commandResult = await _command.ExecuteCommandAsync(_context.Admin, entity);

            bool success = await storage.StoreConfigEntityAsync(entity);
            if (!success)
            {
                SanityChecks.StorageFailed(commandResult, $"{nameof(RunCommandConfigOperation)}.{nameof(ExecuteAsync)}");
            }

            return commandResult;
        }
    }
}