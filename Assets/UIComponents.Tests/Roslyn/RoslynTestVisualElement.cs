using UnityEngine.UIElements;

namespace UIComponents.Tests.Roslyn
{
    public partial class RoslynTestVisualElement : VisualElement
    {
        [UxmlTrait(DefaultValue = "None set")]
        public string Description;
        
        [UxmlTrait]
        public int Age;

        [UxmlTrait(Name = "timestamp", DefaultValue = -1)]
        public long UnixTimestamp;
        
        [UxmlTrait(DefaultValue = 1.0f)]
        public float MusicVolume { get; set; }
    }
}
