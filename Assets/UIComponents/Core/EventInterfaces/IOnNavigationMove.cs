#if UNITY_2021_3_OR_NEWER
using UIComponents.InterfaceModifiers;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When implemented by a <see cref="UIComponent"/>,
    /// a callback for <see cref="NavigationMoveEvent"/> is
    /// automatically registered in the UIComponent constructor.
    /// </summary>
    [RegistersCallback(typeof(NavigationMoveEvents))]
    public interface IOnNavigationMove
    {
        void OnNavigationMove(NavigationMoveEvent evt);
    }
}
#endif
