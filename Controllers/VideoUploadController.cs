using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]


public class VideoUploadController : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadVideo()
    {
        var files = Request.Form.Files;
        if (files.Count == 0)
            return BadRequest("No file uploaded.");

        var file = files[0];
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var videoUrl = $"{Request.Scheme}://{Request.Host}/videos/{file.FileName}";
        return Ok(new { url = videoUrl });
    }

}
