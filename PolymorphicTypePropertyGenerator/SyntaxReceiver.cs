using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PolymorphicTypePropertyGenerator
{
    internal class SyntaxReceiver : ISyntaxContextReceiver
    {
        public List<ClassNodeInfo> Nodes { get; } = new List<ClassNodeInfo>();
        public List<ClassNodeInfo> Roots { get; } = new List<ClassNodeInfo>();

        public IEnumerable<ClassNodeInfo> GetAllDerivedTypes(string name)
        {
            var result = Nodes.Where(t => t.DerivedNodeName == name);
            return result;
        }

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                if (typeDeclarationSyntax is InterfaceDeclarationSyntax or StructDeclarationSyntax)
                    return;
                var classSymbol = context.SemanticModel.GetDeclaredSymbol(typeDeclarationSyntax);
                if (classSymbol is null)
                    return;
                var classNodeInfo = new ClassNodeInfo(
                    classSymbol.ToDisplayString(),
                    classSymbol.ContainingNamespace.ToDisplayString(),
                    classSymbol.BaseType?.ToDisplayString()
                );
                if (classSymbol.GetAttributes().Any(t =>
                    t.AttributeClass?.ToDisplayString() == typeof(PolymorphicTypePropertyAttribute).ToString()))
                {
                    Roots.Add(classNodeInfo);
                }
                else
                {
                   Nodes.Add(classNodeInfo); 
                }
            }
        } 
    }

    record ClassNodeInfo(string DisplayName, string Namespace, string DerivedNodeName)
    {
        public string Name => DisplayName.Split('.').Last();
    }
}