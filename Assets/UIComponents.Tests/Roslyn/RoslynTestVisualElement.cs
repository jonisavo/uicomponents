using UIComponents.Experimental;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Roslyn
{
    public partial class RoslynTestVisualElement : VisualElement
    {
        [Trait(DefaultValue = "None set")]
        public string Description;
        
        [Trait]
        public int Age;

        [Trait(Name = "unix-timestamp")]
        public long UnixTimestamp;
        
        [Trait(Name = "music-volume", DefaultValue = 1.0f)]
        public float MusicVolume { get; set; }
    }
}
