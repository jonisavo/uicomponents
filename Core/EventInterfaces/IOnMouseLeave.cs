using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When implemented by a <see cref="UIComponent"/>,
    /// a callback for <see cref="MouseLeaveEvent"/> is
    /// automatically registered in the UIComponent constructor.
    /// </summary>
    public interface IOnMouseLeave
    {
        void OnMouseLeave(MouseLeaveEvent evt);
    }
}