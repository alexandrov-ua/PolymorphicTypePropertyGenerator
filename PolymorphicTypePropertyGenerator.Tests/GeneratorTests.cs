using System.Linq;
using Xunit;
using FluentAssertions;

namespace PolymorphicTypePropertyGenerator.Tests
{
    public class GeneratorTests
    {
        [Fact]
        public void Foo()
        {
            var results = Tools.Run<PolymorphicTypePropertyGenerator>(@"
using System;
using PolymorphicTypePropertyGenerator;
namespace Qwe
{
    [PolymorphicTypeProperty]
    public partial abstract class Foo
    {
    }

    public partial class Bar : Foo
    {
    }

    public partial class Baz : Foo
    {
    }
}
");

            var generatedSources = results.Single().GeneratedSources;
            generatedSources.Should().Contain(t => t.HintName == "Foo_PolymorphicTypePropertyGenerator.cs");
            generatedSources.Should().Contain(t => t.HintName == "Bar_PolymorphicTypePropertyGenerator.cs");
            generatedSources.Should().Contain(t => t.HintName == "Baz_PolymorphicTypePropertyGenerator.cs");

        }
    }
}