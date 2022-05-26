using heitech.configXt.api.Models;
using Microsoft.AspNetCore.Mvc;

namespace heitech.configXt.api.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IService _service;
    private readonly IStore _store;
    private readonly ILogger<ConfigController> _logger;
    public ConfigController(ILogger<ConfigController> logger, IService service, IStore store)
    {
        _store = store;
        _logger = logger;
        _service = service;
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> Get(string key)
    {
        ConfigResult result = await _service.RetrieveAsync(key);
        return result.ToActionResult(new { Key = key });
    }

    [HttpPut("{key}")]
    public async Task<IActionResult> Post(string key, [FromBody] UpsertModel input)
    {
        bool isValid = ConfigModel.IsValidJson(input!.Value);
        if (!isValid)
            return BadRequest(new { error = $"input '{input.Value}' must be valid json" });

        var model = ConfigModel.From(key, input!.Value);

        var retrieved = await _service.RetrieveAsync(key);
        var result = retrieved.IsSuccess
                     ? await _service.UpdateAsync(model)
                     : await _service.CreateAsync(model);

        string operation = retrieved.IsSuccess ? "updated" : "created";
        return result.ToActionResult(new { Key = key, Value = model, Operation = operation });
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key)
    {
        ConfigResult result = await _service.DeleteAsync(key);

        return result.ToActionResult(new { Key = key });
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var d = await _store.GetAll();
        return Ok(d.Select(x => x.ToOutput()));
    }
}