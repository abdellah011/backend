using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

[Route("api/[controller]")]
[ApiController]
public class FastApiController : ControllerBase
{
    private readonly FastApiService _fastApiService;

    public FastApiController(FastApiService fastApiService)
    {
        _fastApiService = fastApiService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Fichier invalide.");
        }

        var result = await _fastApiService.ExtractTextFromImage(file);
        return Ok(result);
    }
}
