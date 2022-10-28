using JetBrains.Annotations;
using UIComponents.InterfaceModifiers;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When implemented by a <see cref="UIComponent"/>,
    /// a callback for <see cref="GeometryChangedEvent"/> is
    /// automatically registered in the UIComponent constructor.
    /// </summary>
    [RegistersCallback(typeof(GeometryChangedEvent))]
    public interface IOnGeometryChanged
    {
        void OnGeometryChanged([NotNull] GeometryChangedEvent evt);
    }
}