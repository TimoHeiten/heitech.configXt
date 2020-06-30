using System;
using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Models;

namespace heitech.configXt.Application
{
    public interface IRequestBus : IDisposable
    {
        void Connect();
        Task<UiOperationResult> RequestAsync(ContextModel request);
        void Close();
    }

    public interface IResponseBus : IDisposable
    {
        void Connect();
        Task RespondAsync(Func<ContextModel, Task<OperationResult>> response);
        void Close();
    }
}