using System;
using System.Threading.Tasks;

namespace heitech.configXt.Core
{
    public class RunConfigNullOperation : IRunConfigOperation
    {
        private readonly ConfigurationContext _ctxt;
        private readonly Func<OperationResult> _opFactory;
        public RunConfigNullOperation(ConfigurationContext ctxt, Func<OperationResult> opFactory)
        {
            _ctxt = ctxt;
            _opFactory = opFactory;
        }
        public Task<OperationResult> ExecuteAsync()
        {
            return Task.FromResult(_opFactory());
        }
    }
}