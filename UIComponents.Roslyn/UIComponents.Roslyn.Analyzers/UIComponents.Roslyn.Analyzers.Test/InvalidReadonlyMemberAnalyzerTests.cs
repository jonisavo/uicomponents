using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = UIComponents.Roslyn.Analyzers.Test.CSharpCodeFixVerifier<
    UIComponents.Roslyn.Analyzers.InvalidReadonlyMemberAnalyzer,
    UIComponents.Roslyn.Analyzers.InvalidReadonlyCodeFixProvider>;

namespace UIComponents.Roslyn.Analyzers.Test
{
    [TestClass]
    public class InvalidReadonlyMemberAnalyzerTests
    {
        private const string UIComponentsDefinition = @"
    namespace UIComponents
    {
        public class ProvideAttribute : Attribute
        {
            public ProvideAttribute() {}
        }
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
        public class QueryAttribute : Attribute
        {
            public QueryAttribute(string id) {}
        }
    }
";

        [TestMethod]
        public async Task It_Reports_Readonly_Query_And_Provide_Members()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        public class OtherAttribute : Attribute {{}}

        public class TestComponent
        {{
            {{|#0:[UIComponents.Provide]
            public readonly string first;|}}

            {{|#1:[Other, UIComponents.Query(""test"")]
            [UIComponents.Query(""test2"")]
            public readonly string second;|}}
        }}
    }}";

            var fixtest = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        public class OtherAttribute : Attribute {{}}

        public class TestComponent
        {{
            [UIComponents.Provide]
            public string first;

            [Other, UIComponents.Query(""test"")]
            [UIComponents.Query(""test2"")]
            public string second;
        }}
    }}";
            var firstResult = VerifyCS.Diagnostic("UIC003")
                .WithLocation(0)
                .WithArguments("ProvideAttribute");
            var secondResult = VerifyCS.Diagnostic("UIC003")
                .WithLocation(1)
                .WithArguments("QueryAttribute");
            await VerifyCS.VerifyCodeFixAsync(test,
                new DiagnosticResult[] { firstResult, secondResult },
                fixtest
            );
        }

        [TestMethod]
        public async Task It_Does_Not_Report_Non_Readonly_Fields()
        {
            var test = $@"
    using System;

    {UIComponentsDefinition}

    namespace Application
    {{
        public class OtherAttribute : Attribute {{}}

        public class TestComponent
        {{
            [UIComponents.Provide]
            public string first;

            [Other, UIComponents.Query(""test"")]
            public string second;
        }}
    }}";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}
