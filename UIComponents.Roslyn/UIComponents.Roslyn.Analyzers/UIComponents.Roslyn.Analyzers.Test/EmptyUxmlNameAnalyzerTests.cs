using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = UIComponents.Roslyn.Analyzers.Test.CSharpAnalyzerVerifier<
    UIComponents.Roslyn.Analyzers.EmptyUxmlNameAnalyzer>;

namespace UIComponents.Roslyn.Analyzers.Test
{
    [TestClass]
    public class EmptyUxmlNameAnalyzerTests
    {
        private const string UIComponentsDefinition = @"
    namespace UIComponents
    {
        public abstract class UIComponent {}
        public class UxmlNameAttribute : Attribute
        {
            public UxmlNameAttribute(string name) {}
        }
        public class OtherAttribute : Attribute
        {
            public OtherAttribute(string name = """") {}
        }
    }
";
        [TestMethod]
        public async Task It_Reports_Empty_UxmlNameAttribute()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        [UIComponents.UxmlName({{|#0:""""|}})]
        public class FirstComponent : UIComponents.UIComponent {{}}

        [UIComponents.UxmlName({{|#1:null|}})]
        public class SecondComponent : UIComponents.UIComponent {{}}
    }}";

            var firstResult = VerifyCS.Diagnostic("UIC102")
                .WithLocation(0);
            var secondResult = VerifyCS.Diagnostic("UIC102")
                .WithLocation(1);
            await VerifyCS.VerifyAnalyzerAsync(test, firstResult, secondResult);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Non_Empty_UxmlNameAttribute()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        [UIComponents.UxmlName(""Test"")]
        public class TestComponent : UIComponents.UIComponent {{}}
    }}";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Non_UxmlNameAttribute()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        [UIComponents.Other("""")]
        public class TestComponent : UIComponents.UIComponent {{}}
    }}";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_If_UxmlNameAttribute_Does_Not_Exist()
        {
            var test = @"
    namespace Application
    {
        public class BaseComponent {}
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
