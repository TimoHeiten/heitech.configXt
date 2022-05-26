using Microsoft.AspNetCore.Mvc;

namespace heitech.configXt.api.Controllers
{
    public static class ControllerExtensions
    {
        public static IActionResult ToActionResult(this ConfigResult configResult, object input)
        {
            if (configResult.IsSuccess)
            {
                return new OkObjectResult(configResult.Result.ToOutput());
            }
            else
            {
                if (configResult.Exception is ConfigurationException cEx)
                    return new BadRequestObjectResult(new { Message = $"Failed with: {cEx.Message}", For = input });
                else
                    // todo use input etc.
                    return new StatusCodeResult(500);
            }
        }
    }
}