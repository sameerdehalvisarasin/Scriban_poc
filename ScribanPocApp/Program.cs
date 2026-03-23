using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using Scriban;

class Program
{
static async Task Main(string[] args)
{
// Determine paths relative to the application's base directory
string baseDir = AppContext.BaseDirectory;
string templatePath = Path.Combine(baseDir, "Templates", "ClassTemplate.sbncs");
string outputDir = Path.Combine(baseDir, "GeneratedClasses");
string outputPath = Path.Combine(outputDir, "GeneratedClass.cs");

// Accept an optional natural language description from command-line args
string description = args.Length > 0
? string.Join(" ", args)
: "A Person class with a string property called Name";

Console.WriteLine($"Generating class from description: \"{description}\"");

// Use Claude to extract class specification from the natural language description
ClassSpec? classSpec = await GetClassSpecFromClaudeAsync(description);
if (classSpec == null)
{
Console.Error.WriteLine("Failed to get class specification from Claude.");
return;
}

Console.WriteLine($"Claude suggested: class={classSpec.ClassName}, property={classSpec.PropertyType} {classSpec.PropertyName}");

var classData = new
{
class_name = classSpec.ClassName,
property_type = classSpec.PropertyType,
property_name = classSpec.PropertyName
};

GenerateClassFromTemplate(templatePath, outputPath, classData);
Console.WriteLine($"Class generated at {outputPath}");
}

/// <summary>
/// Calls Claude to extract a structured class specification from a natural language description.
/// Reads the API key from the ANTHROPIC_API_KEY environment variable.
/// </summary>
static async Task<ClassSpec?> GetClassSpecFromClaudeAsync(string description)
{
var client = new AnthropicClient();

string prompt =
"Given the following description of a C# class, extract the class specification and respond with ONLY a JSON object (no markdown, no extra text) with these fields:\n" +
"- \"class_name\": the PascalCase class name\n" +
"- \"property_type\": the C# type of the primary property (e.g. string, int, bool)\n" +
"- \"property_name\": the PascalCase property name\n\n" +
$"Description: {description}\n\n" +
"Example response:\n" +
"{\"class_name\":\"Vehicle\",\"property_type\":\"string\",\"property_name\":\"Make\"}";

var messages = new List<Message>
{
new Message(RoleType.User, prompt)
};

var parameters = new MessageParameters
{
Messages = messages,
MaxTokens = 256,
Model = AnthropicModels.Claude46Sonnet,
Stream = false,
Temperature = 0m
};

var response = await client.Messages.GetClaudeMessageAsync(parameters);
string json = response.Message.ToString().Trim();

try
{
return JsonSerializer.Deserialize<ClassSpec>(json, new JsonSerializerOptions
{
PropertyNameCaseInsensitive = true
});
}
catch (JsonException ex)
{
Console.Error.WriteLine($"Failed to parse Claude response as JSON: {ex.Message}");
Console.Error.WriteLine($"Response was: {json}");
return null;
}
}

static void GenerateClassFromTemplate(string templateFile, string outputFile, object data)
{
if (!File.Exists(templateFile))
{
Console.Error.WriteLine($"Template file not found: {templateFile}");
return;
}
string templateText = File.ReadAllText(templateFile);
var template = Template.Parse(templateText);
string result = template.Render(data);
Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);
File.WriteAllText(outputFile, result);
}
}

record ClassSpec(string ClassName, string PropertyType, string PropertyName);
