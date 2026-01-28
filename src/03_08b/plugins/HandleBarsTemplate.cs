using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

namespace _03_08b;


public class HandleBarsTemplate
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

        builder.Plugins.AddFromType<WhatTimeIsIt>();

        var kernel = builder.Build();


            // Create agenda
        List<string> todaysCalendar = ["8am - wakeup", "9am - work", "12am - lunch", "1pm - work", "6pm - exercise", "7pm - study", "10pm - sleep"];
        
        var handlebarsTemplate = @"
                           Please explain in a fun way the day agenda
                           {{ set ""dayAgenda"" (todaysCalendar)}}
                           {{ set ""whatTimeIsIt"" (WhatTimeIsIt-Time) }}
        
                           {{#each dayAgenda}}
                               Explain what you are doing at {{this}} in a fun way.
                           {{/each}}
        
                           Explain what you will be doing next at {{whatTimeIsIt}} in a fun way.";

            var handlebarsFunction = kernel.CreateFunctionFromPrompt(
    new PromptTemplateConfig()
    {
        Template = handlebarsTemplate,
        TemplateFormat = "handlebars",
        
        InputVariables = [
            new() { Name = "todaysCalendar", AllowDangerouslySetContent = true }
        ]
    },
    new HandlebarsPromptTemplateFactory()
);

            

        Console.WriteLine($" todaysCalendar {todaysCalendar}");
    }
}