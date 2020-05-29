using System;
using System.Threading.Tasks;
using heitech.configXt.Core.Commands;

namespace heitech.configXt.Core.Operation
{
    public class RunCommandConfigOperation : IRunConfigOperation
    {
        private readonly Func<CommandContext, Task<OperationResult>> _command;
        private readonly CommandContext _context;

        public RunCommandConfigOperation(Func<CommandContext, Task<OperationResult>> command, CommandContext context)
        {
            _command = command;
            _context = context;
        }

        public Task<OperationResult> ExecuteAsync()
        {
            return _command(_context);
        }
    }
}