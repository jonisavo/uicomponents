using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = UIComponents.Roslyn.Analyzers.Test.CSharpCodeFixVerifier<
    UIComponents.Roslyn.Analyzers.UIComponentPartialAnalyzer,
    UIComponents.Roslyn.Analyzers.PartialKeywordCodeFixProvider>;

namespace UIComponents.Roslyn.Analyzers.Test
{
    [TestClass]
    public class UIComponentPartialAnalyzerTests
    {
        [TestMethod]
        public async Task It_Reports_And_Fixes_Non_Partial_UIComponent()
        {
            var test = @"
    namespace UIComponents
    {
        public abstract class UIComponent {}
    }

    namespace Application
    {
        public class {|#0:MyComponent|} : UIComponents.UIComponent {}
    }";

            var fixtest = @"
    namespace UIComponents
    {
        public abstract class UIComponent {}
    }

    namespace Application
    {
        public partial class MyComponent : UIComponents.UIComponent {}
    }";

            var expected = VerifyCS.Diagnostic("UIC001").WithLocation(0).WithArguments("MyComponent");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Empty_Source()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Abstract_UIComponent()
        {
            var test = @"
    namespace UIComponents
    {
        public abstract class UIComponent {}
    }

    namespace Application
    {
        public abstract class MyComponent : UIComponents.UIComponent {}
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Partial_UIComponent()
        {
            var test = @"
    namespace UIComponents
    {
        public abstract class UIComponent {}
    }

    namespace Application
    {
        public partial class MyComponent : UIComponents.UIComponent {}
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_If_UIComponent_Type_Is_Missing()
        {
            var test = @"
    namespace Application
    {
        public partial class MyComponent : UIComponents.UIComponent {}
    }";
            var expected = DiagnosticResult.CompilerError("CS0246").WithSpan(4, 44, 4, 56).WithArguments("UIComponents");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Non_Class_Types()
        {
            var test = @"
    namespace Application
    {
        public struct MyStruct {}
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Non_UIComponent_Classes()
        {
            var test = @"
    namespace UIComponents
    {
        public abstract class UIComponent {}
    }

    namespace Application
    {
        public partial class MyClass {}
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
