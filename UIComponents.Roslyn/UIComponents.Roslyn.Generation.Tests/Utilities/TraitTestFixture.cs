using UIComponents.Roslyn.Generation.Generators.Uxml;

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
    [UxmlTrait]
    public {fieldTypeName} FieldTrait;

    [UxmlTrait]
    public {fieldTypeName} PropertyTrait {{ get; set; }}

    [UxmlTrait]
    public {fieldTypeName} PropertyWithoutSetter {{ get; }}
}}";
            if (useUnityEngine)
                source = "using UnityEngine;\r\n" + source;

            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }
    }
}
