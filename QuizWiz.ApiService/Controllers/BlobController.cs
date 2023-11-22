using Microsoft.AspNetCore.Mvc;
using QuizWiz.Persistence.BlobStorage;

namespace QuizWiz.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpGet("download/{articleName}")]
        public async Task<IActionResult> GetArticle(string articleName)
        {
            try
            {
                var stream = await _blobService.DownloadBlobAsync(articleName);

                // Return the stream as part of the response
                return File(stream, "application/octet-stream", articleName);
            }
            catch (Exception ex)
            {
                // Handle errors and return appropriate response
                Console.WriteLine($"Error downloading blob: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("upload/article")]
        public async Task<IActionResult> UploadArticleAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File is empty or null");
                }

                // Generate a unique blob name or use the original file name
                var blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                var fileBytes = StreamToByteArray(file.OpenReadStream());

                // Upload the blob and get the URI
                var blobUri = _blobService.CreateBlobAsync(fileBytes, file.FileName);

                // You can return the URI in the response
                return Ok(new { BlobUri = blobUri });
            }
            catch (Exception ex)
            {
                // Handle errors and return appropriate response
                Console.WriteLine($"Error uploading blob: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        private static byte[] StreamToByteArray(Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
