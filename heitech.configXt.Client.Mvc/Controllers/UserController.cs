using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using heitech.configXt.Client.Mvc.Models;
using heitech.configXt.Application;
using heitech.configXt.Models;
using heitech.configXt.Core;

namespace heitech.configXt.Client.Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IStorageModel _model;
        private readonly IAuthStorageModel _authStorageModel;
        public UserController(IStorageModel model, IAuthStorageModel authStore)
        {
            _model = model;
            _authStorageModel = authStore;
        }

        public Task<IActionResult> Index()
        {
            IActionResult result = View();
            ViewBag.From = "Config/Overview";
            return Task.FromResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthModelDto model, string from)
        {
            var authModel = new AuthModel(model.Name, model.Password);
            var context = _model.ReadUserContext(authModel, _authStorageModel);

            OperationResult result = await Factory.RunOperationAsync(context);
            if (result.IsSuccess)
            {
                string[] ab = from.Split('/');
                return RedirectToAction
                (
                    actionName: ab.Last(),
                    controllerName: ab.First(),
                    new { model.Name, model.Password }
                );
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
