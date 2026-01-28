using System.Runtime.InteropServices;
using _03_05b;
using Microsoft.SemanticKernel;

public class NativeFunction
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

        builder.Plugins.AddFromType<MyMathPlugin>();

        var kernel = builder.Build();


        var numberToSquareroot = 81;
    var res = await kernel.InvokeAsync(
        "MyMathPlugin",
        "Sqrt",
        new ()
        {
            { "number", numberToSquareroot}
        });

        Console.WriteLine($" Result {res} from squareRoot {numberToSquareroot}");
    }
}