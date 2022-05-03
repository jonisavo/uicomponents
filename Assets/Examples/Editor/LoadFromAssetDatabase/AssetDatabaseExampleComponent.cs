using UIComponents.Core;
using UIComponents.Editor;

namespace UIComponentsExamples.Editor
{
    [AssetPath("Assets/Examples/Editor/LoadFromAssetDatabase")]
    [Layout("AssetDatabaseExampleComponent.uxml")]
    [Stylesheet("AssetDatabaseExampleComponent.uss")]
    [Dependency(typeof(IAssetResolver), provide: typeof(AssetDatabaseAssetResolver))]
    public class AssetDatabaseExampleComponent : UIComponent {}
}