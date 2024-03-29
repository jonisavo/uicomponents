﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using UIComponents.Roslyn.Generation.Generators.AssetLoad;

namespace UIComponents.Roslyn.Generation.Tests.Utilities
{
    internal class GeneratorTester
    {
        public static Task Verify<TGenerator>(params string[] sources) where TGenerator : ISourceGenerator, new()
        {
            var syntaxTrees = new SyntaxTree[sources.Length];

            for (var i = 0; i < sources.Length; i++)
                syntaxTrees[i] = CSharpSyntaxTree.ParseText(sources[i]);

            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(UIComponent).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: syntaxTrees,
                references: references
            );

            var generator = new TGenerator();

            var driver = CSharpGeneratorDriver.Create(generator);

            driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);

            return Verifier.Verify(driver).UseDirectory("../Snapshots");
        }

        public static Task VerifyWithoutReferences<TGenerator>(string source) where TGenerator : ISourceGenerator, new()
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            var compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: new[] { syntaxTree }
            );

            var generator = new TGenerator();

            var driver = CSharpGeneratorDriver.Create(generator).RunGenerators(compilation);

            return Verifier.Verify(driver).UseDirectory("../Snapshots");
        }
    }
}
