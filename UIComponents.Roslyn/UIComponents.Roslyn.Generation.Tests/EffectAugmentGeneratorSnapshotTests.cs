using UIComponents.Roslyn.Generation.Generators.Effects;
using UIComponents.Roslyn.Generation.Tests.Utilities;

namespace UIComponents.Roslyn.Generation.Tests
{
    [UsesVerify]
    public class EffectAugmentGeneratorSnapshotTests
    {
        private const string TestEffectDeclarations = @"
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class TestEffectAttribute : UIComponentEffectAttribute {}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class TestEffectWithArgumentsAttribute : UIComponentEffectAttribute
{
    public string Name { get; set; }

    public float Number { get; set; }

    public TestEffectWithArguments(int integer, string text) {}
}
";
        [Fact]
        public Task It_Generates_Code_For_Applying_Effects()
        {
            var source = $@"
using UIComponents;

{TestEffectDeclarations}

[TestEffect]
[TestEffectWithArguments(8, ""Hello world"", Name = ""John Smith"", Number = 3.14f)]
public partial class BasicEffectComponent : UIComponent {{}}
";
            return GeneratorTester.Verify<EffectAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Allows_Overriding_Priority()
        {
            var source = $@"
using UIComponents;

{TestEffectDeclarations}

[TestEffect(Priority = 8)]
[TestEffectWithArguments(8, ""Hello world"", Name = ""John Smith"", Number = 3.14f, Priority = -5)]
public partial class PriorityEffectComponent : UIComponent {{}}
";
            return GeneratorTester.Verify<EffectAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Works_With_Subclasses()
        {
            var source = $@"
using UIComponents;

{TestEffectDeclarations}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class ExtraEffectAttribute : UIComponentEffectAttribute
{{
    public override int Priority {{ get; set; }} = 5;
}}

[TestEffect]
public partial class BaseEffectComponent : UIComponent {{}}

[TestEffectWithArguments(8, ""Hello world"", Name = ""John Smith"", Number = 3.14f, Priority = -5)]
public partial class SubclassEffectComponent : BaseEffectComponent {{}}

[ExtraEffect]
public partial class SecondSubclassEffectComponent : SubclassEffectComponent {{}}
";
            return GeneratorTester.Verify<EffectAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_If_There_Are_No_Effects()
        {
            var source = @"
using UIComponents;

public partial class NoEffectsComponent : UIComponent {}
";
            return GeneratorTester.Verify<EffectAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_If_The_UIComponentEffectAttribute_Type_Is_Missing()
        {
            var source = $@"
namespace UIComponents
{{
    public class UIComponent {{}}

    public class AssetPrefixAttribute {{}}
}}

{TestEffectDeclarations}

[TestEffect]
public partial class NoEffectsComponent : UIComponents.UIComponent {{}}
";
            return GeneratorTester.VerifyWithoutReferences<EffectAugmentGenerator>(source);
        }
    }
}
