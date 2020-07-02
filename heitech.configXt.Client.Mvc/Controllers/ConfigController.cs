using System.Threading.Tasks;
using heitech.configXt.Core;
using heitech.configXt.Core.Entities;
using heitech.configXt.Models;
using Microsoft.AspNetCore.Mvc;

namespace heitech.configXt.Client.Mvc.Controllers
{
    public class ConfigController : Controller
    {
        private readonly IInteract _interact;
        public ConfigController(IInteract interact)
        {
            _interact = interact;
        }

        public async Task<IActionResult> Overview(string Name, string Password)
        {
            var model = new AuthModel(Name, Password);

            var context = new ContextModel
            {
                User = model,
                Key = null,
                Type = ContextType.ReadAllEntries
            };

            OperationResult result = await _interact.Run(context);
            if (result.IsSuccess)
            {
                return View(result.Result as ConfigCollection);
            }
            else
            {
                if (result.ResultType == ResultType.Forbidden)
                {
                    ViewBag.From = "Config/Overview";
                    return RedirectToAction(actionName:"Index", controllerName: "User");
                }
                else 
                {
                    // todo
                }
            }

            IActionResult views = View();
            return views;
        }
    }
}