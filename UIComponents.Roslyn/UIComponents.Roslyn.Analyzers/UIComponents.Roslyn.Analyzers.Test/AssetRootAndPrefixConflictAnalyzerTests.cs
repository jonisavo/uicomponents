using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = UIComponents.Roslyn.Analyzers.Test.CSharpAnalyzerVerifier<
    UIComponents.Roslyn.Analyzers.AssetRootAndPrefixConflictAnalyzer>;

namespace UIComponents.Roslyn.Analyzers.Test
{
    [TestClass]
    public class AssetRootAndPrefixConflictAnalyzerTests
    {
        private const string UIComponentsDefinition = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}

        public class AssetRootAttribute : Attribute
        {
            public AssetRootAttribute(string path) {}
        }

        public class AssetPrefixAttribute : Attribute
        {
            public AssetPrefixAttribute(string path) {}
        }
    }
";

        [TestMethod]
        public async Task It_Reports_When_A_Class_Declares_Both_AssetRoot_And_AssetPrefix()
        {
            var test = $@"
    {UIComponentsDefinition}

    namespace Application
    {{
        [{{|#0:UIComponents.AssetPrefix(""Old/"")|}}]
        [UIComponents.AssetRoot(""New/"")]
        public class MyComponent : UIComponents.UIComponent {{}}
    }}";

            var result = VerifyCS.Diagnostic("UIC105")
                .WithLocation(0)
                .WithArguments("MyComponent");

            await VerifyCS.VerifyAnalyzerAsync(test, result);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_When_AssetRoot_Overrides_Inherited_AssetPrefix()
        {
            var test = $@"
    {UIComponentsDefinition}

    namespace Application
    {{
        [UIComponents.AssetPrefix(""Base/"")]
        public class BaseComponent : UIComponents.UIComponent {{}}

        [UIComponents.AssetRoot(""Child/"")]
        public class ChildComponent : BaseComponent {{}}
    }}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_For_Non_UIComponent_Types()
        {
            var test = $@"
    {UIComponentsDefinition}

    namespace Application
    {{
        [UIComponents.AssetPrefix(""Old/"")]
        [UIComponents.AssetRoot(""New/"")]
        public class PlainClass {{}}
    }}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_If_Required_Types_Do_Not_Exist()
        {
            var test = @"
    namespace Application
    {
        public class PlainClass {}
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
