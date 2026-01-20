using Microsoft.SemanticKernel;

namespace _02_05b;

public class TryingOutTheKernel
{
    public static async Task Execute()
    {
        var modelDeploymentName = "gpt-5-chat";
        var azureOpenAIEndpoint = Environment.GetEnvironmentVariable("AZUREOPENAI_ENDPOINT");
        var azureOpenAIKey = Environment.GetEnvironmentVariable("AZUREOPENAI_APIKEY");
        
        var builder = Kernel.CreateBuilder();    

        builder.Services.AddAzureOpenAIChatCompletion(
            modelDeploymentName,
            azureOpenAIEndpoint,
            azureOpenAIKey
        );

        var kernel = builder.Build();

        var topic = "semantic kerneld sda was born somehow and lead us to future od IT industry.";

        var prompt = $"Genereate funny poem about givenevent. Event {topic} ";

        var result = await kernel.InvokePromptAsync(prompt);

        Console.WriteLine(result);



    }
}