using System.Runtime.CompilerServices;

namespace UIComponents.Roslyn.Generation.Tests
{
    internal class ModuleInitializer
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifySourceGenerators.Enable();
        }
    }
}
