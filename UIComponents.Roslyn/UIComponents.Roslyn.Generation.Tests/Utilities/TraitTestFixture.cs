using UIComponents.Roslyn.Generation.Generators.Traits;

namespace UIComponents.Roslyn.Generation.Tests.Utilities
{
    public class TraitTestFixture
    {
        public Task Verify_Traits(string fieldTypeName, bool useUnityEngine = false)
        {
            var fieldTypeNameCap = char.ToUpper(fieldTypeName[0]) + fieldTypeName.Substring(1);

            var source = $@"
using UIComponents;
using UIComponents.Experimental;

public partial class {fieldTypeNameCap}ComponentWithUsing : UIComponent
{{
    [Trait]
    public {fieldTypeName} FieldTrait;

    [Trait]
    public {fieldTypeName} PropertyTrait {{ get; set; }}

    [Trait]
    public {fieldTypeName} PropertyWithoutSetter {{ get; }}
}}";
            if (useUnityEngine)
                source = "using UnityEngine;\r\n" + source;

            return GeneratorTester.Verify<UxmlTraitsAugmentGenerator>(source);
        }
    }
}
