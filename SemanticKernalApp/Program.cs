using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using SemanticKernalApp;

Console.WriteLine("BookShop AI Assistant\n");


var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var apiKey = config["OpenAI:ApiKey"]!;
var modelId = config["OpenAI:ModelId"]!;
var connectionString = config["ConnectionStrings:Default"]!;


var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(
        modelId: modelId,
        apiKey: apiKey)
    .Build();

//BookShop plugin
kernel.ImportPluginFromObject(new BookShopPlugin(connectionString), "BookShop");

Console.WriteLine("Ask question.\n");

while (true)
{
    Console.Write("You: ");
    var question = Console.ReadLine();
    if (string.IsNullOrEmpty(question) || question.ToLower() == "exit") break;

    var result = await kernel.InvokePromptAsync(question);
    Console.WriteLine($"AI: {result}\n");
}
