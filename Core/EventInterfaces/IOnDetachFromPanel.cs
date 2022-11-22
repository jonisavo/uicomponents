using JetBrains.Annotations;
using UIComponents.InterfaceModifiers;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When implemented by a <see cref="UIComponent"/>,
    /// a callback for <see cref="DetachFromPanelEvent"/> is
    /// automatically registered in the UIComponent constructor.
    /// </summary>
    [RegistersEventCallback(typeof(DetachFromPanelEvent))]
    public interface IOnDetachFromPanel
    {
        void OnDetachFromPanel([NotNull] DetachFromPanelEvent evt);
    }
}
