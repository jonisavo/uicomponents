using UIComponents.Addressables;

namespace UIComponents.Samples.Addressables
{
    [AssetPath("Assets/Samples/Addressables/Data")]
    [Layout("AddressablesExampleComponent.uxml")]
    [Stylesheet("AddressablesExampleComponent.uss")]
    [Dependency(typeof(IAssetResolver), provide: typeof(AddressableAssetResolver))]
    public class AddressablesExampleComponent : UIComponent {}
}