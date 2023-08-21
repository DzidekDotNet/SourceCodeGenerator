using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Schema;
using Microsoft.CodeAnalysis;

namespace SourceCodeGeneratorLib
{
  [Generator]
  public class TelegramsGenerator : ISourceGenerator
  {
    public void Execute(GeneratorExecutionContext context)
    {
      // System.Diagnostics.Debugger.Launch();
      StringBuilder messages = new StringBuilder();
      foreach (var file in context.AdditionalFiles)
      {
        string inputFilename = Path.GetFileNameWithoutExtension(file.Path);
        string outputFilename = inputFilename + ".g.cs";
        messages.AppendLine($"procesing file inputFilename: '{inputFilename}'");
        try
        {
          var telegramDefinition = ReadDefinition(file);
          messages.AppendLine($"Parsed telegramDefinition: '{JsonSerializer.Serialize(telegramDefinition)}'");
          // var schemaPath = Path.Combine("telegram.schema.json");
          // var assembly = this.GetType().Assembly;
          // Stream resource = assembly.GetManifestResourceStream("SourceCodeGeneratorLib.telegram.schema.json");
                    //StreamReader reader = new StreamReader(resource);
                    //string text = reader.ReadToEnd();
                    // var schema = JsonSchema.FromFile(schemaPath);
          // var schemaValidationResult = schema.Evaluate(ReadJsonDocument(file));
          // messages.AppendLine($"Schema validation result: '{JsonSerializer.Serialize(schemaValidationResult)}'");

          context.AddSource(outputFilename, $"public class {inputFilename} {{public string Name {{ get; set; }}}}");
          messages.AppendLine($"Created class for file inputFilename: '{inputFilename}'");
        }
        catch (Exception ex)
        {
          messages.AppendLine($"ERROR during processing file inputFilename: '{inputFilename}', error: '{ex.Message.ToString()}', '{ex.StackTrace.ToString()}'");
        }
      }
      context.AddSource("error.txt.g.cs", $"public class errorTxt {{/*{messages.ToString()}*/}}");
    }

    public void Initialize(GeneratorInitializationContext context)
    {
      // Method intentionally left empty.
    }

    private TelegramDefinition ReadDefinition(AdditionalText text)
    {
      var jsonText = text.GetText();
      if (jsonText == null)
      {
        throw new ArgumentException("Invalid argument", nameof(text));
      }

      return JsonSerializer.Deserialize<TelegramDefinition>(jsonText.ToString());
    }
    private JsonDocument ReadJsonDocument(AdditionalText text)
    {
      var jsonText = text.GetText();
      if (jsonText == null)
      {
        throw new ArgumentException("Invalid argument", nameof(text));
      }

      return JsonDocument.Parse(jsonText.ToString());
    }
    
  }
}
