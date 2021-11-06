using System;
using Newtonsoft.Json;

namespace PolymorphicTypePropertyGenerator.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Foo bar = new Baz();
            Console.WriteLine(JsonConvert.SerializeObject(bar));
            Console.WriteLine(bar.Type);
        }
    }

    [PolymorphicTypeProperty]
    public partial class Foo
    {
    }

    public partial class Bar : Foo
    {
    }

    public partial class Baz : Foo
    {
    }

}