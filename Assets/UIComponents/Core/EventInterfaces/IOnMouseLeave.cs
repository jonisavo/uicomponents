using UIComponents.InterfaceModifiers;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When implemented by a <see cref="UIComponent"/>,
    /// a callback for <see cref="MouseLeaveEvent"/> is
    /// automatically registered in the UIComponent constructor.
    /// </summary>
    [RegistersCallback(typeof(MouseLeaveEvent))]
    public interface IOnMouseLeave
    {
        void OnMouseLeave(MouseLeaveEvent evt);
    }
}