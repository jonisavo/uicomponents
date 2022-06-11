using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UIComponents.Samples.EventInterfaces
{
    public interface IEventLogService
    {
        delegate void OnMessageAddDelegate(string message);

        event OnMessageAddDelegate OnMessageAdd;

        delegate void OnMessageChangeDelegate(int index, string newMessage);

        event OnMessageChangeDelegate OnMessageChange;

        void Log(EventBase evt);

        delegate void OnClearDelegate();

        event OnClearDelegate OnClear;

        void Clear();

        IReadOnlyList<string> GetMessages();
    }
}