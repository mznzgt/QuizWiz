using Microsoft.AspNetCore.Mvc;
using QuizWiz.Application.SharedModel;
using QuizWiz.Persistence.Cosmos;

namespace QuizWiz.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CosmosTestController : ControllerBase
    {
        private readonly ICosmosService _cosmosService;

        public CosmosTestController(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateItem()
        {
            var item = new QuizResponse()
            {
                Topic = "Computer",
                Quiz = new List<Quiz>()
            };

            var response = await _cosmosService.CreateItemAsync(item);

            return Ok(response);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetItem()
        {
            var response = await _cosmosService.GetItemAsync("Computer");

            return Ok(response);
        }
    }
}
