﻿using Microsoft.CodeAnalysis.CSharp;
using UIComponents.Roslyn.Generation.Generators.Uxml;
using UIComponents.Roslyn.Generation.Tests.Utilities;

namespace UIComponents.Roslyn.Generation.Tests
{
    [UsesVerify]
    public class QueryAugmentGeneratorSnapshotTests
    {
        [Fact]
        public Task It_Generates_Basic_Queries()
        {
            var source = @"
using UnityEngine.UIElements;
using UIComponents;

public partial class BasicQueryComponent : UIComponent
{
    [Query]
    public VisualElement element;

    [Query(""uxml-name"")]
    public VisualElement elementWithUxmlNameInConstructor;

    [Query(Name = ""second-uxml-name"")]
    public VisualElement elementWithUxmlNameAsNameArgument;

    [Query(Class = ""class-name"")]
    public VisualElement elementWithClassName;

    [Query(Name = ""third-uxml-name"", Class = ""second-class-name"")]
    public VisualElement elementWithNameAndClass;

    [Query(Name = ""fourth-uxml-name"", Class = ""third-class-name"")]
    public VisualElement elementProperty { get; set; }
}
";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Arrays()
        {
            var source = @"
using UnityEngine.UIElements;
using UIComponents;

public partial class ArrayQueryComponent : UIComponent
{
    [Query(Name = ""uxml-name"", Class = ""class-name"")]
    public VisualElement[] elements;
}
";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Lists()
        {
            var source = @"
using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;

public partial class ListQueryComponent : UIComponent
{
    [Query(Name = ""uxml-name"", Class = ""class-name"")]
    public List<VisualElement> elements;
}
";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Null_Arguments()
        {
            var source = @"
using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;

public partial class NullQueryComponent : UIComponent
{
    [Query(Name = null, Class = null)]
    public List<VisualElement> elements;
}
";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Subclasses()
        {
            var source = @"
using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;

public partial class BaseQueryComponent : UIComponent
{
    [Query]
    public VisualElement baseElement;

    [Query(""uxml-name"")]
    public VisualElement anotherBaseElement;
}

public partial class SubclassQueryComponent : BaseQueryComponent
{
    [Query(Name = ""foo"", Class = ""bar"")]
    public VisualElement[] subclassElements;

    [Query]
    public List<VisualElement> subclassList;

    [Query(Name = ""fourth-uxml-name"", Class = ""third-class-name"")]
    public BaseQueryComponent baseQueryComponent { get; set; }
}

public partial class SecondSubclassQueryComponent : SubclassQueryComponent
{
    [Query(Class = ""asdf"")]
    public SubclassQueryComponent subclassQueryComponent;
}
";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Nested_Types()
        {
            var source = @"
using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;

public partial class Parent
{
    private partial class PTwo
    {
        protected partial class PThree
        {
            internal partial class PFour
            {
                public partial class FirstNestedComponent : UIComponent
                {
                    [Query]
                    public VisualElement field;

                    [Query(Name = ""uxml-name"", Class = ""class-name"")]
                    public List<VisualElement> elements;
                }

                private partial class SecondNestedComponent : UIComponent
                {
                    [Query]
                    public FirstNestedComponent component;
                }
            }
        }
    }
}";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Namespace_And_Nested_Type()
        {
            var source = @"
using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;

namespace UILibrary
{
    namespace Components
    {
        public partial class ParentClass
        {
            public partial class NestedComponent : UIComponent
            {
                [Query]
                public VisualElement field;

                [Query(Name = ""uxml-name"", Class = ""class-name"")]
                public List<VisualElement> elements;
            }
        }
    }
}";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task Does_Not_Generate_If_No_Queries_Exist()
        {
            var source = @"
using UIComponents;

public partial class NoQueryComponent : UIComponent {}
";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task Does_Not_Generate_For_Property_Without_Set_Method()
        {
            var source = @"
using UIComponents;

public partial class NoPropertyQueryComponent : UIComponent
{
    [Query(Name = ""fourth-uxml-name"", Class = ""third-class-name"")]
    public BaseQueryComponent baseQueryComponent { get; }
}
";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task Does_Not_Generate_If_Used_On_Class_Or_Method()
        {
            var source = @"
using UnityEngine.UIElements;
using UIComponents;

[Query(""invalid-usage"")]
public partial class InvalidUsageQueryComponent : UIComponent
{
    [Query(""valid-usage"")]
    public VisualElement element;

    [Query(""more-invalid-usage"")]
    public void HelloWorld() {}
}
";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task Does_Not_Generate_If_Field_Type_Does_Not_Inherit_VisualElement()
        {
            var source = @"
using System.Collections.Generic;
using UIComponents;

public class MyClass {}

public partial class NoVisualElementQueryComponent : UIComponent
{
    [Query]
    public int integer;
    
    [Query]
    public float[] numbers;

    [Query]
    public List<MyClass> instances;
}
";
            return GeneratorTester.Verify<QueryAugmentGenerator>(source);
        }

        [Fact]
        public Task Does_Not_Generate_If_UIComponent_Type_Is_Missing()
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(@"
using UIComponents;

public class MyComponent : UIComponent {}

public partial class NoUIComponentTypeComponent : UIComponent
{
    [Query]
    public MyComponent element;

    [Query(""uxml-name"")]
    public MyComponent anotherElement;
}
");

            var compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: new[] { syntaxTree }
            );

            var generator = new QueryAugmentGenerator();

            var driver = CSharpGeneratorDriver.Create(generator).RunGenerators(compilation);

            return Verify(driver).UseDirectory("Snapshots");
        }

        [Fact]
        public Task Does_Not_Generate_If_QueryAttribute_Type_Is_Missing()
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(@"
namespace UIComponents
{
    public class UIComponent {}

    public class AssetPathAttribute {}
}

public class MyComponent : UIComponents.UIComponent {}

public partial class NoQueryTypeComponent : UIComponents.UIComponent
{
    [UIComponents.Query]
    public MyComponent element;

    [UIComponents.Query(""uxml-name"")]
    public MyComponent anotherElement;
}
");

            var compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: new[] { syntaxTree }
            );

            var generator = new QueryAugmentGenerator();

            var driver = CSharpGeneratorDriver.Create(generator).RunGenerators(compilation);

            return Verify(driver).UseDirectory("Snapshots");
        }
    }
}