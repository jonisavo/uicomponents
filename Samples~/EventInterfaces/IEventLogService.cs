using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UIComponents.Samples.EventInterfaces
{
    public interface IEventLogService
    {
        void Log(EventBase evt);

        void Clear();

        IReadOnlyList<string> GetMessages();
    }
}