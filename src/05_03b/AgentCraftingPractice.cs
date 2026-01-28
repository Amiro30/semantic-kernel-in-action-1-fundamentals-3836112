using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Experimental.Agents;

namespace _05_03b;

#pragma warning disable SKEXP0101
public class AgentCraftingPractice
{
    // Сделали поля статическими, чтобы статический метод Execute их видел
    private static readonly List<IAgent> _agents = new();
    private static IAgentThread? _agentsThread = null;

    public static async Task Execute()
    {
        var openAIFunctionEnabledModelId = "gpt-4o";
        var openAIApiKey = Environment.GetEnvironmentVariable("OPENAI_APIKEY") ?? "";
        
        Console.WriteLine(openAIApiKey.Length);
        var builder = Kernel.CreateBuilder();

    
     var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v2");

builder.Services.AddOpenAIChatCompletion(
    openAIFunctionEnabledModelId,
    openAIApiKey,
    httpClient: httpClient // Передаем кастомный клиент
);

        var kernel = builder.Build();

        

try 
{
    var codeAgent = await new AgentBuilder()
        .WithOpenAIChatCompletion(openAIFunctionEnabledModelId, openAIApiKey)
        .WithInstructions("You are a pirate.")
        .WithName("CodeParrot")
        .BuildAsync();

        _agents.Add(codeAgent);
}
catch (Exception ex)
{
    Console.WriteLine("--- ДЕТАЛИ ОШИБКИ ---");
    Console.WriteLine(ex.ToString());
    if (ex is HttpOperationException httpEx)
    {
        Console.WriteLine($"Response: {httpEx.ResponseContent}");
    }
    throw;
}

        // // 1. Создание агента через код
        // var codeAgent = await new AgentBuilder()
        //     .WithOpenAIChatCompletion(openAIFunctionEnabledModelId, openAIApiKey)
        //     .WithInstructions("Repeat the user message in the voice of a pirate and then end with parrot sounds.")
        //     .WithName("CodeParrot")
        //     .WithDescription("A fun chat bot that repeats the user message in the voice of a pirate.")
        //     .BuildAsync();

        //_agents.Add(codeAgent);

        // 2. Создание агента из файла
        var pathToPlugin = Path.Combine(Directory.GetCurrentDirectory(), "Agents", "ParrotAgent.yaml");
        var fileAgent = await new AgentBuilder()
                .WithOpenAIChatCompletion(openAIFunctionEnabledModelId, openAIApiKey)
                .FromTemplatePath(pathToPlugin)
                .BuildAsync();
                
        _agents.Add(fileAgent);

        try
        {
            var response = await fileAgent.AsPlugin().InvokeAsync(
        "Practice makes perfect.", // Это попадет в {{input}}
        new KernelArguments { { "count", 2 } });

           Console.WriteLine($"Response: {response}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            // Теперь статический метод может вызвать статический CleanUpAsync
            await CleanUpAsync();
        }
    }

    private static async Task CleanUpAsync()
    {
        if (_agentsThread != null)
        {
            await _agentsThread.DeleteAsync(); // Добавили await
            _agentsThread = null;
        }

        if (_agents.Any())
        {
            // Очищаем всех агентов
            await Task.WhenAll(_agents.Select(agent => agent.DeleteAsync()));
            _agents.Clear();
        }
    }
}