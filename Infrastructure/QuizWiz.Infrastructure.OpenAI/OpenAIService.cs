using Azure.AI.OpenAI;
using Azure;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace QuizWiz.Infrastructure.OpenAI;

public class OpenAIService : IOpenAIService
{
    private readonly Uri _proxyUrl;
    private readonly AzureKeyCredential _token;
    private readonly ILogger<OpenAIService> _logger;
    private readonly OpenAIClient _openAIClient;
    private readonly OpenAIServiceSettings _setting;

    public OpenAIService(ILogger<OpenAIService> logger, IOptions<OpenAIServiceSettings> setting)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _setting = setting.Value ?? throw new ArgumentNullException(nameof(setting));

        _proxyUrl = new Uri(_setting.ProxyUrl + "/v1/api");
        _token = new AzureKeyCredential(_setting.ApiKey + "/" + _setting.GitHubAlias);
        _openAIClient = new OpenAIClient(_proxyUrl, _token);
    }

    public async Task<ChatCompletions> GetChatCompletionsAsync(ChatCompletionsOptions options)
    {
        try
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var response = await _openAIClient.GetChatCompletionsAsync(options);
            return response;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "OpenAI API request failed");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while communicating with OpenAI");
            throw;
        }
    }
}

public class OpenAIServiceSettings
{
    public string ProxyUrl { get; set; }
    public string ApiKey { get; set; }
    public string GitHubAlias { get; set; }
}
