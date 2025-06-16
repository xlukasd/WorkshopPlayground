using LuDurCustomExtension;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

string yourGitHubAppName = "LuDurCustomExtension";
string githubCopilotCompletionsUrl = "https://api.githubcopilot.com/chat/completions";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/info", () => "Hello Ludur's Copilot!");

app.MapGet("/callback", () => "You may close this tab and " +
    "return to GitHub.com (where you should refresh the page " +
    "and start a fresh chat). If you're using VS Code or " +
    "Visual Studio, return there.");

app.MapPost("/", async (
    [FromHeader(Name = "X-GitHub-Token")] string githubToken,
    [FromBody] Request userRequest) =>
{
    userRequest.Messages.Insert(0, new Message
    {
        Role = "system",
        Content =
        "You are a helpful assistant that replies only to questions related to Python programming language, otherwise respond 'This is not about Python.'"
    });

    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", githubToken);
    userRequest.Stream = false;

    var copilotLLMResponse = await httpClient.PostAsJsonAsync(githubCopilotCompletionsUrl, userRequest);

    var responseStream = await copilotLLMResponse.Content.ReadAsStreamAsync();

    return Results.Stream(responseStream);
});

app.Run();