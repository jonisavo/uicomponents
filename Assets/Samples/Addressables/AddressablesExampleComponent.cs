using UIComponents.Addressables;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Samples.Addressables
{
    [AssetPrefix("Assets/Samples/Addressables/Data/")]
    [Layout("AddressablesExampleComponent.uxml")]
    [Stylesheet("AddressablesExampleComponent.uss")]
    [Stylesheet("Box.uss")]
    [Dependency(typeof(IAssetResolver), provide: typeof(AddressableAssetResolver))]
    public partial class AddressablesExampleComponent : UIComponent
    {
        private readonly Label _loadingLabel;
        
        public AddressablesExampleComponent()
        {
            _loadingLabel = new Label("Loading...");
            _loadingLabel.style.alignSelf = Align.Center;
            _loadingLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            _loadingLabel.style.fontSize = 50;
            Add(_loadingLabel);
        }

        public override void OnInit()
        {
            _loadingLabel.RemoveFromHierarchy();
        }
    }
}
