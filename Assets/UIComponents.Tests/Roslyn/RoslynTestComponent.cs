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

        [Trait(Name = "text-color")]
        public Color TextColor;

        [Trait(Name = "current-time")]
        public double CurrentTime;

        [Trait(DefaultValue = Greetings.Morning)]
        public Greetings Greeting;

        [Trait(DefaultValue = true)]
        public bool Enabled;
    }
}
