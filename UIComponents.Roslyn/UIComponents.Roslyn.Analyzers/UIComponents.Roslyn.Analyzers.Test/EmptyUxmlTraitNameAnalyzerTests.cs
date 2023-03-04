using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = UIComponents.Roslyn.Analyzers.Test.CSharpAnalyzerVerifier<
    UIComponents.Roslyn.Analyzers.EmptyUxmlTraitNameAnalyzer>;

namespace UIComponents.Roslyn.Analyzers.Test
{
    [TestClass]
    public class EmptyUxmlTraitNameAnalyzerTests
    {
        private const string UIComponentsDefinition = @"
    namespace UIComponents
    {
        public abstract class UIComponent {}
        public class UxmlTraitAttribute : Attribute
        {
            public string Name { get; set; }

            public UxmlTraitAttribute(string argument = """") {}
        }
        public class OtherAttribute : Attribute
        {
            public string Name { get; set; }
        }
    }
";

        [TestMethod]
        public async Task It_Reports_Empty_UxmlTrait_Name()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        public class TestComponent : UIComponents.UIComponent
        {{
            [UIComponents.UxmlTrait({{|#0:Name = """"|}})]
            public string trait;

            [UIComponents.UxmlTrait({{|#1:Name = null|}})]
            public int OtherTrait {{ get; set; }}
        }}
    }}";

            var firstResult = VerifyCS.Diagnostic("UIC103")
                .WithLocation(0);
            var secondResult = VerifyCS.Diagnostic("UIC103")
                .WithLocation(1);
            await VerifyCS.VerifyAnalyzerAsync(test, firstResult, secondResult);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Non_Empty_UxmlTrait_Name()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        public class TestComponent : UIComponents.UIComponent
        {{
            [UIComponents.UxmlTrait(Name = ""Test1"")]
            public string trait;

            [UIComponents.UxmlTrait(Name = ""Test2"")]
            public int OtherTrait {{ get; set; }}
        }}
    }}";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Empty_Constructor_Argument()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        public class TestComponent : UIComponents.UIComponent
        {{
            [UIComponents.UxmlTrait("""")]
            public string trait;
        }}
    }}";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Non_UxmlTraitAttribute()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        public class TestComponent : UIComponents.UIComponent
        {{
            [UIComponents.Other(Name = """")]
            public string trait;
        }}
    }}";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_If_UxmlTraitAttribute_Is_Used_In_Struct()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        public struct MyStruct
        {{
            [UIComponents.UxmlTrait({{|#0:Name = """"|}})]
            public string trait;

            [UIComponents.UxmlTrait({{|#1:Name = null|}})]
            public int OtherTrait {{ get; set; }}
        }}
    }}";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task It_Does_Not_Report_If_UxmlTraitAttribute_Does_Not_Exist()
        {
            var test = @"
    namespace Application
    {
        public class BaseComponent
        {
            public string name;
            public int Age { get; set; }
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
