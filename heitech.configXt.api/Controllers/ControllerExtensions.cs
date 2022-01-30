using heitech.configXt;
using Microsoft.AspNetCore.Mvc;

public static class ControllerExtensions
{
    public static IActionResult ToActionResult(this ConfigResult configResult, object input)
    {
        if (configResult.IsSuccess)
        {
            return new OkObjectResult(configResult.Result);
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
