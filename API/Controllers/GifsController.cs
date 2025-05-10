using Application.Core;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class GifsController(IGifService gifService) : BaseApiController
{

    [HttpGet("search")]
    public async Task<ActionResult> Search([FromQuery] string query, [FromQuery] int limit = 10, [FromQuery] string pos = null)
    {
        var result = await gifService.Search(query, limit, pos);
        return Ok(result);
    }

    [HttpGet("trending")]
    public async Task<ActionResult<string>> GetTrending()
    {
        var result = await gifService.GetTrending();
        return Ok(result);
    }
}