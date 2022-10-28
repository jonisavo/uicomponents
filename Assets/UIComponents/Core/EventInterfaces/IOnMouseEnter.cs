using UIComponents.InterfaceModifiers;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When implemented by a <see cref="UIComponent"/>,
    /// a callback for <see cref="MouseEnterEvent"/> is
    /// automatically registered in the UIComponent constructor.
    /// </summary>
    [RegistersCallback(typeof(MouseEnterEvent))]
    public interface IOnMouseEnter
    {
        void OnMouseEnter(MouseEnterEvent evt);
    }
}