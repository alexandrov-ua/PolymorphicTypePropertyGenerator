using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace PolymorphicTypePropertyGenerator
{
    [Generator]
    public class PolymorphicTypePropertyGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        private const string RootTemplate = @"
namespace {0}
{{
    public enum {1}Type
    {{
        {2}
    }}

    public partial class {1}
    {{
        public {1}Type Type {{ get; }}
    }}
}}
";

        private const string DerivedTemplate = @"
namespace {0}
{{
    public partial class {1} : {2}
    {{
        public {2}Type Type {{ get=> {2}Type.{1}; }}
    }}
}}
";
        
        public void Execute(GeneratorExecutionContext context)
        {
            if(context.SyntaxContextReceiver is SyntaxReceiver syntaxReceiver)
            {
                foreach (var root in syntaxReceiver.Roots)
                {
                    var derived = syntaxReceiver.GetAllDerivedTypes(root.DisplayName).ToArray();
                    var source = String.Format(RootTemplate, root.Namespace, root.Name, string.Join(",\n", derived.Select(t=>t.Name)));
                    context.AddSource($"{root.Name}_PolymorphicTypePropertyGenerator.cs", SourceText.From(source, Encoding.UTF8));
                    foreach (var d in derived)
                    {
                        var src = string.Format(DerivedTemplate, d.Namespace, d.Name, root.Name);
                        context.AddSource($"{d.Name}_PolymorphicTypePropertyGenerator.cs", SourceText.From(src, Encoding.UTF8));
                    }
                }
            }
        }
    }
}