using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;

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
        var jsonText = file.GetText();
        if (jsonText != null)
        {
          try
          {
            var json = jsonText.ToString();
            var jsonObject = JsonDocument.Parse(json);
                        messages.AppendLine($"Parsed JsonObject: '{jsonObject}'");
                        //var jsonObject = JObject.Parse(json);
                        var schemaPath = Path.Combine("telegram.schema.json");
            //var schema = JsonSchema.Parse(schemaPath);
            messages.AppendLine($"json: '{json}'");


            context.AddSource(outputFilename, $"public class {inputFilename} {{public string Name {{ get; set; }}}}");
            messages.AppendLine($"Created class for file inputFilename: '{inputFilename}'");

          }
          catch (Exception ex)
          {
            messages.AppendLine($"ERROR during processing file inputFilename: '{inputFilename}', error: '{ex.Message.ToString()}', '{ex.StackTrace.ToString()}'");
          }

        }

      }
      context.AddSource("error.txt.g.cs", $"public class errorTxt {{/*{messages.ToString()}*/}}");
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
  }
}
