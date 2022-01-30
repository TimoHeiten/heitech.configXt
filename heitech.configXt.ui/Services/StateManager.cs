using System;
using todo_list_frontend.Application;

namespace heitech.configXt.ui.Services
{
    public class StateManager
    {
        private readonly IBackendService _service;
        public StateManager(IBackendService service)
            =>_service = service;

        internal async Task<ConfigResult> GetResultByKey(string key)
        {
            var result = await _service.GetAsync<ConfigResult>(key);

            return result;
        }
    }
}
