using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace PolymorphicTypePropertyGenerator.Tests
{
    public static class Tools
    {
        public static IEnumerable<GeneratorRunResult> Run<T>(string source) where T : ISourceGenerator, new()
        {
            var compication = CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .Select(a => MetadataReference.CreateFromFile(a.Location))
                    .ToArray(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new T();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            driver = driver.RunGeneratorsAndUpdateCompilation(compication, out var outputCompilation,
                out var diagnostics);

            var result = driver.GetRunResult();

            return result.Results;
        }
        
        public static T GetSyntaxReceiverFromSources<T>(string source) where T : ISyntaxContextReceiver, new()
        {
            var compication = CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .Select(a => MetadataReference.CreateFromFile(a.Location))
                    .ToArray(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            SyntaxReceiverTestsGenerator<T> generator = new();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            driver = driver.RunGeneratorsAndUpdateCompilation(compication, out var outputCompilation,
                out var diagnostics);

            return generator.PublicSyntaxReceiver;
        }
        
        class SyntaxReceiverTestsGenerator<T>  : ISourceGenerator
            where T : ISyntaxContextReceiver, new()
        {
            public T PublicSyntaxReceiver { get; set; }

            public void Initialize(GeneratorInitializationContext context)
            {
                context.RegisterForSyntaxNotifications(() => new T());
            }

            public void Execute(GeneratorExecutionContext context)
            {
                if (context.SyntaxContextReceiver is T syntaxReceiver)
                {
                    PublicSyntaxReceiver = syntaxReceiver;
                }
            }
        }
    }
}