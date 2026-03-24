using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = UIComponents.Roslyn.Analyzers.Test.CSharpCodeFixVerifier<
    UIComponents.Roslyn.Analyzers.UIComponentIdenticalStylesheetAnalyzer,
    UIComponents.Roslyn.Analyzers.InvalidAttributeCodeFixProvider>;

namespace UIComponents.Roslyn.Analyzers.Test
{
    [TestClass]
    public class UIComponentIdenticalStylesheetAnalyzerTests
    {
        [TestMethod]
        public async Task It_Reports_And_Fixes_Usage_Of_Duplicate_Stylesheets()
        {
            var test = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
        }
    }

    namespace Application
    {
        [UIComponents.Stylesheet(""Stylesheet1"")]
        [{|#0:UIComponents.Stylesheet(""Stylesheet1"")|}, UIComponents.Stylesheet(""Stylesheet2"")]
        public class FirstComponent : UIComponents.UIComponent {}

        [UIComponents.Stylesheet(""Stylesheet2""), UIComponents.Stylesheet(""Stylesheet1""), {|#1:UIComponents.Stylesheet(""Stylesheet2"")|}]
        public class SecondComponent : UIComponents.UIComponent {}
    }";

     
            var fixedTest = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
        }
    }

    namespace Application
    {
        [UIComponents.Stylesheet(""Stylesheet1"")]
        [UIComponents.Stylesheet(""Stylesheet2"")]
        public class FirstComponent : UIComponents.UIComponent {}

        [UIComponents.Stylesheet(""Stylesheet2""), UIComponents.Stylesheet(""Stylesheet1"")]
    public class SecondComponent : UIComponents.UIComponent {}
    }";

            var firstResult = VerifyCS.Diagnostic("UIC101")
                .WithLocation(0)
                .WithArguments("Stylesheet1", "FirstComponent");
            var secondResult = VerifyCS.Diagnostic("UIC101")
                .WithLocation(1)
                .WithArguments("Stylesheet2", "SecondComponent");
            await VerifyCS.VerifyCodeFixAsync(test, new[] { firstResult, secondResult }, fixedTest);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Empty_Source()
        {
            var test = @"";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Reports_And_Fixes_Usage_Of_Duplicate_Stylesheets_In_Subclasses()
        {
            var test = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
        }
    }

    namespace Application
    {
        [UIComponents.Stylesheet(""Stylesheet1"")]
        public class BaseComponent : UIComponents.UIComponent {}

        [UIComponents.Stylesheet(""Stylesheet2""), {|#0:UIComponents.Stylesheet(""Stylesheet1"")|}]
        public class SecondComponent : BaseComponent {}

        [{|#1:UIComponents.Stylesheet(""Stylesheet2"")|}]
        [UIComponents.Stylesheet(""Stylesheet3"")]
        public class ThirdComponent : SecondComponent {}
    }";

            var fixedTest = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
        }
    }

    namespace Application
    {
        [UIComponents.Stylesheet(""Stylesheet1"")]
        public class BaseComponent : UIComponents.UIComponent {}

        [UIComponents.Stylesheet(""Stylesheet2"")]
    public class SecondComponent : BaseComponent {}

    [UIComponents.Stylesheet(""Stylesheet3"")]
        public class ThirdComponent : SecondComponent {}
    }";

            var firstResult = VerifyCS.Diagnostic("UIC101")
                .WithLocation(0)
                .WithArguments("Stylesheet1", "SecondComponent");
            var secondResult = VerifyCS.Diagnostic("UIC101")
                .WithLocation(1)
                .WithArguments("Stylesheet2", "ThirdComponent");
            await VerifyCS.VerifyCodeFixAsync(test, new[] { firstResult, secondResult }, fixedTest);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_If_UIComponent_Type_Does_Not_Exist()
        {
            var test = @"
    namespace Application
    {
        public class BaseComponent {}
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Handles_Parameterless_Stylesheet()
        {
            var test = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
            public StylesheetAttribute() {}
        }
    }

    namespace Application
    {
        [UIComponents.Stylesheet]
        [UIComponents.Stylesheet(""Stylesheet1"")]
        public class MyComponent : UIComponents.UIComponent {}
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Reports_And_Fixes_Duplicate_Shared_Stylesheets()
        {
            var test = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
            public StylesheetAttribute() {}
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class SharedStylesheetAttribute : Attribute
        {
            public SharedStylesheetAttribute(string name) {}
        }
    }

    namespace Application
    {
        [UIComponents.SharedStylesheet(""Shared/Base"")]
        public class BaseComponent : UIComponents.UIComponent {}

        [UIComponents.Stylesheet(""Components/Child"")]
        [{|#0:UIComponents.SharedStylesheet(""Shared/Base"")|}]
        public class ChildComponent : BaseComponent {}

        [UIComponents.Stylesheet(""Shared/Common"")]
        [{|#1:UIComponents.SharedStylesheet(""Shared/Common"")|}]
        public class DuplicateOnSameTypeComponent : UIComponents.UIComponent {}
    }";

            var fixedTest = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
            public StylesheetAttribute() {}
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class SharedStylesheetAttribute : Attribute
        {
            public SharedStylesheetAttribute(string name) {}
        }
    }

    namespace Application
    {
        [UIComponents.SharedStylesheet(""Shared/Base"")]
        public class BaseComponent : UIComponents.UIComponent {}

        [UIComponents.Stylesheet(""Components/Child"")]
    public class ChildComponent : BaseComponent {}

        [UIComponents.Stylesheet(""Shared/Common"")]
    public class DuplicateOnSameTypeComponent : UIComponents.UIComponent {}
    }";

            var inheritedResult = VerifyCS.Diagnostic("UIC101")
                .WithLocation(0)
                .WithArguments("Shared/Base", "ChildComponent");
            var sameTypeResult = VerifyCS.Diagnostic("UIC101")
                .WithLocation(1)
                .WithArguments("Shared/Common", "DuplicateOnSameTypeComponent");

            await VerifyCS.VerifyCodeFixAsync(test, new[] { inheritedResult, sameTypeResult }, fixedTest);
        }

        [TestMethod]
        public async Task It_Reports_And_Fixes_Parameterless_Stylesheet_Duplicates()
        {
            var test = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
            public StylesheetAttribute() {}
        }
    }

    namespace Application
    {
        [UIComponents.Stylesheet]
        [{|#0:UIComponents.Stylesheet(""MyComponent.style"")|}]
        public class MyComponent : UIComponents.UIComponent {}

        [UIComponents.Stylesheet]
        public class BaseComponent : UIComponents.UIComponent {}

        [{|#1:UIComponents.Stylesheet(""BaseComponent.style"")|}]
        public class DerivedComponent : BaseComponent {}
    }";

            var fixedTest = @"
    using System;

    namespace UIComponents
    {
        public abstract class UIComponent {}
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class StylesheetAttribute : Attribute
        {
            public StylesheetAttribute(string path) {}
            public StylesheetAttribute() {}
        }
    }

    namespace Application
    {
        [UIComponents.Stylesheet]
    public class MyComponent : UIComponents.UIComponent {}

        [UIComponents.Stylesheet]
        public class BaseComponent : UIComponents.UIComponent {}

    public class DerivedComponent : BaseComponent {}
    }";

            var sameTypeResult = VerifyCS.Diagnostic("UIC101")
                .WithLocation(0)
                .WithArguments("MyComponent.style", "MyComponent");
            var inheritedResult = VerifyCS.Diagnostic("UIC101")
                .WithLocation(1)
                .WithArguments("BaseComponent.style", "DerivedComponent");

            await VerifyCS.VerifyCodeFixAsync(test, new[] { sameTypeResult, inheritedResult }, fixedTest);
        }
    }
}
