using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = UIComponents.Roslyn.Analyzers.Test.CSharpAnalyzerVerifier<
    UIComponents.Roslyn.Analyzers.UIComponentInternalMethodAnalyzer>;

namespace UIComponents.Roslyn.Analyzers.Test
{
    [TestClass]
    public class UIComponentInternalMethodAnalyzerTests
    {
        [TestMethod]
        public async Task It_Reports_Usage_Of_Internal_UIC_Method()
        {
            var test = @"
    namespace UIComponents
    {
        public abstract class UIComponent
        {
            public virtual void UIC_Example() {}
            public virtual void UIC_Another() {}
        }
    }

    namespace Application
    {
        public class MyComponent : UIComponents.UIComponent
        {
            private void MyMethod()
            {
                {|#0:UIC_Example()|};
                {|#1:UIC_Another()|};
            }
        }
    }";

            var firstResult = VerifyCS.Diagnostic("UIC002")
                .WithLocation(0)
                .WithArguments("UIC_Example");
            var secondResult = VerifyCS.Diagnostic("UIC002")
                .WithLocation(1)
                .WithArguments("UIC_Another");
            await VerifyCS.VerifyAnalyzerAsync(test, firstResult, secondResult);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Empty_Source()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Public_UIComponent_Method()
        {
            var test = @"
    namespace UIComponents
    {
        public abstract class UIComponent
        {
            public virtual void OnInit() {}
        }
    }

    namespace Application
    {
        public class MyComponent : UIComponents.UIComponent
        {
            private void MyMethod()
            {
                OnInit();
            }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Usage_Of_Internal_UIC_Method_Inside_UIComponent()
        {
            var test = @"
    namespace UIComponents
    {
        public abstract class UIComponent
        {
            private void TestMethod()
            {
                UIC_Example();
                UIC_Another();
            }

            public virtual void UIC_Example() {}
            public virtual void UIC_Another() {}
        }
    }
";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Usage_Of_Internal_UIC_Method_When_It_Is_Owned_By_Other_Class()
        {
            var test = @"
    namespace UIComponents
    {
        public abstract class UIComponent
        {
            public virtual void UIC_Example() {}
            public virtual void UIC_Another() {}
        }
    }

    namespace Library
    {
        public abstract class LibraryBaseClass
        {
            public virtual void UIC_Example() {}
            public virtual void UIC_Another() {}
        }
    }

    namespace Application
    {
        public class MyComponent : Library.LibraryBaseClass
        {
            private void MyMethod()
            {
                UIC_Example();
                UIC_Another();
            }
        }
    }";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Usage_Of_Internal_UIC_Method_When_UIComponent_Does_Not_Exist()
        {
            var test = @"
namespace Application
{
    public class MyComponent
    {
        private void MyMethod()
        {
            UIC_Example();
            UIC_Another();
        }

        public void UIC_Example() {}
        public void UIC_Another() {}
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
