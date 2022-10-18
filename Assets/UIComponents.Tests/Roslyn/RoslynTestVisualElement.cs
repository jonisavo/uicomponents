using UIComponents.Experimental;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Roslyn
{
    public partial class RoslynTestVisualElement : VisualElement
    {
        [UxmlTrait(DefaultValue = "None set")]
        public string Description;
        
        [UxmlTrait]
        public int Age;

        [UxmlTrait(Name = "unix-timestamp")]
        public long UnixTimestamp;
        
        [UxmlTrait(Name = "music-volume", DefaultValue = 1.0f)]
        public float MusicVolume { get; set; }
    }
}
