using Xunit;

namespace PolymorphicTypePropertyGenerator.Tests
{
    public class GeneratorTests
    {
        [Fact]
        public void Foo()
        {
            Tools.Run<PolymorphicTypePropertyGenerator>(@"
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
        }
    }
}