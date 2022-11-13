using UnityEngine.UIElements;
using UIComponents.DependencyInjection;

namespace UIComponents
{
    public abstract class UIComponent : VisualElement, IDependencyConsumer
    {
        public class UxmlFactory<T> where T : UIComponent {}
    }
}
