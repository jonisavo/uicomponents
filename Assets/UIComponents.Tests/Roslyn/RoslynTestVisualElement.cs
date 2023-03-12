using UnityEngine.UIElements;

namespace UIComponents.Tests.Roslyn
{
    public partial class RoslynTestVisualElement : VisualElement
    {
        [UxmlTrait]
        public string Description = "None set";
        
        [UxmlTrait]
        public int Age;

        [UxmlTrait(Name = "timestamp")]
        public long UnixTimestamp = -1;

        [UxmlTrait]
        public float MusicVolume { get; set; } = 1.0f;
    }
}
