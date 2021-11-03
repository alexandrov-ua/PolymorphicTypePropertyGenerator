using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using FluentAssertions;

namespace PolymorphicTypePropertyGenerator.Tests
{
    public class SyntaxReceiverTests
    {
        [Fact]
        public void Test1()
        {
            var source = @"
using System;
using PolymorphicTypePropertyGenerator;

namespace Test.App
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

   public class Foo {};
   public class Bar : Foo {};
   [PolymorphicTypeProperty]
   record Baz {};
   record Qux : Baz {}; 
   [PolymorphicTypeProperty]
   class Zxc {};
}";

            var syntaxReceiver = Tools.GetSyntaxReceiverFromSources<SyntaxReceiver>(source);
            
            syntaxReceiver.Nodes.Should().Contain(new ClassNodeInfo("Test.App.Program", "Test.App", "object"));
            syntaxReceiver.Nodes.Should().Contain(new ClassNodeInfo("Test.App.Foo", "Test.App", "object"));
            syntaxReceiver.Nodes.Should().Contain(new ClassNodeInfo("Test.App.Bar", "Test.App", "Test.App.Foo"));
            syntaxReceiver.Nodes.Should().Contain(new ClassNodeInfo("Test.App.Qux", "Test.App", "Test.App.Baz"));

            syntaxReceiver.Roots.Should().Contain(new ClassNodeInfo("Test.App.Baz", "Test.App", "object"));
            syntaxReceiver.Roots.Should().Contain(new ClassNodeInfo("Test.App.Zxc", "Test.App", "object"));
        }
    }
}