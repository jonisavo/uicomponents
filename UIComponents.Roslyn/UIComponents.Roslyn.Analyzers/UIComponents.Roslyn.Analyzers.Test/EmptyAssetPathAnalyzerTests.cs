using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = UIComponents.Roslyn.Analyzers.Test.CSharpAnalyzerVerifier<
    UIComponents.Roslyn.Analyzers.EmptyAssetPathAnalyzer>;

namespace UIComponents.Roslyn.Analyzers.Test
{
    [TestClass]
    public class EmptyAssetPathAnalyzerTests
    {
        private const string UIComponentsDefinition = @"
    namespace UIComponents
    {
        public abstract class UIComponent {}

        public class LayoutAttribute : Attribute
        {
            public LayoutAttribute(string path) {}
            public LayoutAttribute() {}
        }

        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
            public StylesheetAttribute() {}
        }

        public class SharedStylesheetAttribute : Attribute
        {
            public SharedStylesheetAttribute(string name) {}
        }

        public class AssetRootAttribute : Attribute
        {
            public AssetRootAttribute(string path) {}
        }

        public class AssetPrefixAttribute : Attribute
        {
            public AssetPrefixAttribute(string path) {}
        }

        public class OtherAttribute : Attribute
        {
            public OtherAttribute(string path) {}
        }
    }
";

        [TestMethod]
        public async Task It_Reports_Empty_And_Whitespace_Asset_Literals()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        public static class Constants
        {{
            public const string Blank = ""   "";
        }}

        [UIComponents.Layout({{|#0:""""|}})]
        [UIComponents.Stylesheet({{|#1:null|}})]
        [UIComponents.SharedStylesheet({{|#2:Constants.Blank|}})]
        [UIComponents.AssetRoot({{|#3:"" ""|}})]
        [UIComponents.AssetPrefix({{|#4:""\t""|}})]
        public class BrokenComponent : UIComponents.UIComponent {{}}
    }}";

            var layoutResult = VerifyCS.Diagnostic("UIC104")
                .WithLocation(0)
                .WithArguments("LayoutAttribute");
            var stylesheetResult = VerifyCS.Diagnostic("UIC104")
                .WithLocation(1)
                .WithArguments("StylesheetAttribute");
            var sharedStylesheetResult = VerifyCS.Diagnostic("UIC104")
                .WithLocation(2)
                .WithArguments("SharedStylesheetAttribute");
            var assetRootResult = VerifyCS.Diagnostic("UIC104")
                .WithLocation(3)
                .WithArguments("AssetRootAttribute");
            var assetPrefixResult = VerifyCS.Diagnostic("UIC104")
                .WithLocation(4)
                .WithArguments("AssetPrefixAttribute");

            await VerifyCS.VerifyAnalyzerAsync(
                test,
                layoutResult,
                stylesheetResult,
                sharedStylesheetResult,
                assetRootResult,
                assetPrefixResult);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Parameterless_Convention_Attributes_Or_Non_Empty_Values()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        [UIComponents.AssetRoot(""UI/Components/"")]
        [UIComponents.Layout]
        [UIComponents.Stylesheet]
        [UIComponents.SharedStylesheet(""Shared/Common"")]
        public class MyComponent : UIComponents.UIComponent {{}}
    }}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Non_UIComponent_Classes()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        [UIComponents.Layout("""")]
        public class PlainClass {{}}
    }}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Unrelated_Attributes()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        [UIComponents.Other("""")]
        public class MyComponent : UIComponents.UIComponent {{}}
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
