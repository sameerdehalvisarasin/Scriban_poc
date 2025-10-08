
using System;
using System.IO;
using Scriban;

class Program
{
	static void Main(string[] args)
	{
		// Use fixed template path as requested
	string templatePath = "/workspaces/Scriban_poc/ScribanPocApp/Templates/ClassTemplate.sbncs";
	string outputPath = "/workspaces/dotnet-codespaces/SampleApp/TemplateConsole/GeneratedClasses/Person.cs";
		var classData = new {
			class_name = "Person",
			property_type = "string",
			property_name = "Name"
		};
		GenerateClassFromTemplate(templatePath, outputPath, classData);
		Console.WriteLine($"Class generated at {outputPath}");
	}

	static void GenerateClassFromTemplate(string templateFile, string outputFile, object data)
	{
		if (!File.Exists(templateFile))
		{
			Console.WriteLine($"Template file not found: {templateFile}");
			return;
		}
		string templateText = File.ReadAllText(templateFile);
		var template = Template.Parse(templateText);
		string result = template.Render(data);
		Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
		File.WriteAllText(outputFile, result);
	}
}
