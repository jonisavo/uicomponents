using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When implemented by a <see cref="UIComponent"/>,
    /// a callback for <see cref="DetachFromPanelEvent"/> is
    /// automatically registered in the UIComponent constructor.
    /// </summary>
    public interface IOnDetachFromPanel
    {
        void OnDetachFromPanel([NotNull] DetachFromPanelEvent evt);
    }
}