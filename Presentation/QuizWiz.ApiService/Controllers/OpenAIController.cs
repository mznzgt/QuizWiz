using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizWiz.ApiService.Settings;
using QuizWiz.Application.SharedModel;
using QuizWiz.Infrastructure.OpenAI;

namespace QuizWiz.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIController(IOpenAIService openAIService, ILogger<OpenAIService> logger)
        {
            _openAIService = openAIService;
            _logger = logger;
        }

        [HttpPost("quiz/create")]
        public async Task<ActionResult<QuizResponse>> GetChatCompletionsAsync([FromBody] string userInput)
        {
            try
            {
                //var completionOptions = new ChatCompletionsOptions()
                //{
                //    MaxTokens = 2048,
                //    Temperature = 0.7f,
                //    NucleusSamplingFactor = 0.95f,
                //    DeploymentName = "gpt-3.5-turbo"
                //};

                //completionOptions.Messages.Add(new ChatMessage(ChatRole.System, ApiServiceConstants.QuizPrompt));
                //completionOptions.Messages.Add(new ChatMessage(ChatRole.User, userInput));

                //var response = await _openAIService.GetChatCompletionsAsync(completionOptions);

                //var quizContent = response.Choices.First().Message.Content;
                //var result = JsonConvert.DeserializeObject<QuizResponse>(quizContent);
                var result = new QuizResponse();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while communicating with OpenAI");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
