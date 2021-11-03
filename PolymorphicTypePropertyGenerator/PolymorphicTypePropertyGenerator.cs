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
        public virtual {1}Type Type {{ get; }}
    }}
}}
";

        private const string BaseTypeTemplate = @"
namespace {0}
{{
    public partial class {1} : {2}
    {{
        public override {2}Type Type {{ get=> {2}Type.{1}; }}
    }}
}}
";
        
        public void Execute(GeneratorExecutionContext context)
        {
            if(context.SyntaxContextReceiver is SyntaxReceiver syntaxReceiver)
            {
                foreach (var root in syntaxReceiver.Roots)
                {
                    var baseTypes = syntaxReceiver.GetBaseTypes(root.DisplayName).ToArray();
                    var source = String.Format(RootTemplate, root.Namespace, root.Name, string.Join(",\n", baseTypes.Select(t=>t.Name)));
                    context.AddSource($"{root.Name}_PolymorphicTypePropertyGenerator.cs", SourceText.From(source, Encoding.UTF8));
                    foreach (var baseType in baseTypes)
                    {
                        var baseSource = string.Format(BaseTypeTemplate, baseType.Namespace, baseType.Name, root.Name);
                        context.AddSource($"{baseType.Name}_PolymorphicTypePropertyGenerator.cs", SourceText.From(baseSource, Encoding.UTF8));
                    }
                }
            }
        }
    }
}