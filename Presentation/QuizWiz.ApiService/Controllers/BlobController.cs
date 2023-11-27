using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizWiz.Application.QuizGenerator.Commands;
using QuizWiz.Application.QuizGenerator.Queries;

namespace QuizWiz.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("download/{articleName}")]
        public async Task<IActionResult> GetArticleAsync(string articleName)
        {
            try
            {
                var query = new GetArticleQuery(articleName);
                var result = await _mediator.Send(query);
                // Return the stream as part of the response
                return File(result, "application/octet-stream", articleName);
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

                var request = new AddArticleCommand(file.FileName, file.OpenReadStream());

                var result = await _mediator.Send(request);

                // You can return the URI in the response
                return Ok(new { BlobUri = result });
            }
            catch (Exception ex)
            {
                // Handle errors and return appropriate response
                Console.WriteLine($"Error uploading blob: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}
