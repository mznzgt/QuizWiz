using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWiz.Domain.Constants;
using QuizWiz.Persistence.Cosmos;

namespace QuizWiz.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ICosmosService _cosmosService;

        public StudentController(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        [Authorize(Roles = UserRole.Student)]
        [HttpGet("get/quiz/id")]
        public async Task<IActionResult> GetQuizAsync(string itemId, string email)
        {
            var response = await _cosmosService.GetItemAsync(itemId, email);

            return Ok(response);
        }

        [Authorize(Roles = UserRole.Student)]
        [HttpGet("get/quiz/email")]
        public async Task<IActionResult> GetQuizByEmailAsync(string email)
        {
            var response = await _cosmosService.GetDocumentsByPartitionKeyAsync(email);

            return Ok(response);
        }
    }
}
