using Admin.Data;
using Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly UserContext _context;

        public UploadFileController(UserContext context)
        {
            _context = context;
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Aucun fichier sélectionné.");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var uploadedFile = new UploadedFile
            {
                FileName = file.FileName,
                FileData = ms.ToArray(), // ⚠️ Vérifie précisément ce nom dans ton modèle
                ContentType = file.ContentType,
                UploadedOn = DateTime.UtcNow
            };

            _context.UploadedFiles.Add(uploadedFile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Fichier uploadé avec succès !" });
        }
        [HttpGet("view-file/{id}")]
        public IActionResult ViewFile(int id)
        {
            var file = _context.UploadedFiles.FirstOrDefault(f => f.Id == id);

            if (file == null)
                return NotFound("Fichier introuvable");

            return File(file.FileData, file.ContentType); // ✅ Envoi direct de l'image
        }

    }
}