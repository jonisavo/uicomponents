using JetBrains.Annotations;
using UIComponents.InterfaceModifiers;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When implemented by a <see cref="UIComponent"/>,
    /// a callback for <see cref="AttachToPanelEvent"/> is
    /// automatically registered in the UIComponent constructor.
    /// </summary>
    [RegistersEventCallback(typeof(AttachToPanelEvent))]
    public interface IOnAttachToPanel
    {
        void OnAttachToPanel([NotNull] AttachToPanelEvent evt);
    }
}
