using UnityEngine.UIElements;

namespace UIComponents
{
    public abstract class UIComponent : VisualElement
    {
        public class UxmlFactory<T> where T : UIComponent {}
    }
}
