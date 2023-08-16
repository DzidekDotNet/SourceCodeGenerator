using Newtonsoft.Json.Linq;

namespace ConsoleApp;

partial class Program
{
    static void Main(string[] args)
    {
        HelloFrom("Generated Code test");
        var jsonObject = JObject.Parse("{}");
        //var t10 = new t70();

        Console.WriteLine("A"); 
    }

    static partial void HelloFrom(string name);
}