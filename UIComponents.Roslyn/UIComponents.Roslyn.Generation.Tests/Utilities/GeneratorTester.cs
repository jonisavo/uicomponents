using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace UIComponents.Roslyn.Generation.Tests.Utilities
{
    internal class GeneratorTester
    {
        public static Task Verify<TGenerator>(string source) where TGenerator : ISourceGenerator, new()
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(UIComponent).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: new[] { syntaxTree },
                references: references
            );

            var generator = new TGenerator();

            var driver = CSharpGeneratorDriver.Create(generator);

            driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);

            return Verifier.Verify(driver).UseDirectory("../Snapshots");
        }
    }
}
