using System;
using System.Threading.Tasks;
using heitech.configXt.Core.Commands;

namespace heitech.configXt.Core.Operation
{
    public class RunCommandConfigOperation : IRunConfigOperation
    {
        private readonly Func<CommandContext, Task<Result>> _command;
        private readonly CommandContext _context;

        public RunCommandConfigOperation(Func<CommandContext, Task<Result>> command, CommandContext context)
        {
            _command = command;
            _context = context;
        }

        public async Task<Result> ExecuteAsync()
        {
            Result result = await _command(_context);

            return result;
        }
    }
}