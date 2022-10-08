using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.Generators.Uxml
{
    internal readonly struct UxmlFactoryInfo
    {
        public readonly string Name;

        public UxmlFactoryInfo(string name)
        {
            Name = name;
        }

        public static UxmlFactoryInfo CreateFromArguments(Dictionary<string, TypedConstant> arguments)
        {
            string name = null;

            if (arguments.TryGetValue("constructor_0", out var nameArg))
                name = nameArg.Value as string;

            return new UxmlFactoryInfo(name);
        }
    }
}
