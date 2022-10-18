using UIComponents.Experimental;
using UnityEngine;

namespace UIComponents.Tests.Roslyn
{
    public partial class RoslynTestComponent : UIComponent
    {
        public enum Greetings
        {
            Hello,
            Hi,
            Morning
        }

        [UxmlTrait(Name = "text-color")]
        public Color TextColor;

        [UxmlTrait(Name = "current-time")]
        public double CurrentTime;

        [UxmlTrait(DefaultValue = Greetings.Morning)]
        public Greetings Greeting;

        [UxmlTrait(DefaultValue = true)]
        public bool Enabled;
    }
}
