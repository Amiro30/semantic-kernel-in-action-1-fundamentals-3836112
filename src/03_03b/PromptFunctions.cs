using System.Runtime.InteropServices;
using Microsoft.SemanticKernel;

namespace _03_03b;

public class PromptFunctions
{
    private static string someText = "Effective prompt design is essential to achieving desired outcomes with LLM AI models. Prompt engineering, also known as prompt design, is an emerging field that requires creativity and attention to detail. It involves selecting the right words, phrases, symbols, and formats that guide the model in generating high-quality and relevant texts.\r\n\r\nIf you've already experimented with ChatGPT, you can see how the model's behavior changes dramatically based on the inputs you provide. For example, the following prompts produce very different outputs:";

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

        //await CreateAndExecutePrompt(kernel);

        await ImportPluginFromFolderAndExecuteIt(kernel);
    }

    public static async Task CreateAndExecutePrompt (Kernel kernel)
    {
        var prompt = "what is the meaning of life?";

        var kernelFunction = kernel.CreateFunctionFromPrompt(prompt);
        var result = await kernelFunction.InvokeAsync(kernel);
        Console.WriteLine(result);

    
    }

    public static async Task ImportPluginFromFolderAndExecuteIt(Kernel kernel)
    {
        var SummarizepluginDir = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Plugins",
            "SummarizePlugin");
        kernel.ImportPluginFromPromptDirectory(SummarizepluginDir);

        var summarizeResult = await kernel.InvokeAsync(
            "SummarizePlugin",
            "Summarize",
            new() {
                { "input", someText }
            }
        );

        Console.WriteLine($"result : {summarizeResult}");
    }
}