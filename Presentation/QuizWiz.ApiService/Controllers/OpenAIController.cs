using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizWiz.ApiService.Settings;
using QuizWiz.Application.SharedModel;
using QuizWiz.Domain.Constants;
using QuizWiz.Infrastructure.OpenAI;
using QuizWiz.Persistence.Cosmos;
using System.Security.Claims;

namespace QuizWiz.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;
        private readonly ICosmosService _cosmosService;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIController(
            IOpenAIService openAIService, 
            ILogger<OpenAIService> logger,
            ICosmosService cosmosService)
        {
            _openAIService = openAIService;
            _logger = logger;
            _cosmosService = cosmosService;
        }

        [Authorize(Roles = UserRole.Teacher)]
        [HttpPost("quiz/create")]
        public async Task<ActionResult<QuizResponse>> GetChatCompletionsAsync([FromBody] string userInput)
        {
            try
            {
                var completionOptions = new ChatCompletionsOptions()
                {
                    MaxTokens = 2048,
                    Temperature = 0.7f,
                    NucleusSamplingFactor = 0.95f,
                    DeploymentName = "gpt-3.5-turbo"
                };

                completionOptions.Messages.Add(new ChatMessage(ChatRole.System, ApiServiceConstants.QuizPrompt));
                completionOptions.Messages.Add(new ChatMessage(ChatRole.User, userInput));

                var response = await _openAIService.GetChatCompletionsAsync(completionOptions);

                var quizContent = response.Choices.FirstOrDefault().Message.Content;
                var quizResult = JsonConvert.DeserializeObject<QuizResponse>(quizContent);

                var emailClaim  = User.Identity.Name;

                if (string.IsNullOrEmpty(emailClaim))
                {
                    return BadRequest("Email does not exist, try login again");
                }

                quizResult.Email = emailClaim;
                await _cosmosService.CreateItemAsync(quizResult);

                return Ok($"Quiz {quizResult.Id} has successfully created!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while communicating with OpenAI");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
