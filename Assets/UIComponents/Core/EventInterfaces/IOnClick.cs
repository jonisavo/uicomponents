﻿using UIComponents.InterfaceModifiers;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When implemented by a <see cref="UIComponent"/>,
    /// a callback for <see cref="ClickEvent"/> is
    /// automatically registered in the UIComponent constructor.
    /// </summary>
    [RegistersEventCallback(typeof(ClickEvent))]
    public interface IOnClick
    {
        void OnClick(ClickEvent evt);
    }
}
