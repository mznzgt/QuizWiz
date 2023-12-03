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
        [HttpGet("get/quiz")]
        public async Task<IActionResult> GetQuizAsync(string itemId)
        {
            var response = await _cosmosService.GetItemAsync(itemId);

            return Ok(response);
        }
    }
}
