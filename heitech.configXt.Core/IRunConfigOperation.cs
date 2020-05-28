using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core
{
    public interface IRunConfigOperation
    {
        Task<Result> ExecuteAsync();
    }

    internal class NullOperation : IRunConfigOperation
    {
        public Task<Result> ExecuteAsync()
        {
            var empty = new ConfigEntity
            { 
                Id = Guid.Empty,
                Name = "empty",
                Value = "null-object",
                NecessaryClaims = new List<ConfigClaim>()
            };
            return Task.FromResult(new Result
            {
                Before = empty,
                After = empty,
                Success = true,
                
            });
        }
    }
}