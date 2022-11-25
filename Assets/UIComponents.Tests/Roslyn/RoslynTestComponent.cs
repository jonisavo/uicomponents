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

        [UxmlTrait]
        public Color TextColor = Color.cyan;

        [UxmlTrait(Name = "time")]
        public double CurrentTime;

        [UxmlTrait(DefaultValue = Greetings.Morning)]
        public Greetings Greeting;

        [UxmlTrait(DefaultValue = true)]
        public bool Enabled;
    }
}
