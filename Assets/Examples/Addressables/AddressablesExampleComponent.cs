using UIComponents;
using UIComponents.Addressables;

namespace UIComponentsExamples.Addressables
{
    [AssetPath("Assets/Examples/Addressables/Data")]
    [Layout("AddressablesExampleComponent.uxml")]
    [Stylesheet("AddressablesExampleComponent.uss")]
    [Dependency(typeof(IAssetResolver), provide: typeof(AddressableAssetResolver))]
    public class AddressablesExampleComponent : UIComponent {}
}