using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine.UIElements;

namespace UIComponents.Samples.EventInterfaces
{
    public class EventLogService : IEventLogService
    {
        public delegate void OnMessageAddDelegate(string message);

        public delegate void OnMessageChangeDelegate(int index, string newMessage);
        
        public event OnMessageAddDelegate OnMessageAdd;

        public event OnMessageChangeDelegate OnMessageChange;
        
        public delegate void OnClearDelegate();

        public event OnClearDelegate OnClear;
        
        private readonly List<string> _messages = new List<string>(20);
        private long _previousEventTypeId;
        private int _latestMessageDuplicateCount;

        public void Log(EventBase evt)
        {
            if (evt.eventTypeId == _previousEventTypeId && _messages.Count > 0)
                UpdateLatestLogMessage(evt);
            else
                AddLogMessage(evt);

            _previousEventTypeId = evt.eventTypeId;
        }

        private void UpdateLatestLogMessage(EventBase evt)
        {
            _latestMessageDuplicateCount++;
            var index = _messages.Count - 1;
            var message = BuildMessageFromEvent(evt);
            _messages[index] = message;
            OnMessageChange?.Invoke(index, message);
        }

        private void AddLogMessage(EventBase evt)
        {
            _latestMessageDuplicateCount = 0;
            var message = BuildMessageFromEvent(evt);
            _messages.Add(message);
            OnMessageAdd?.Invoke(message);
        }

        private static readonly StringBuilder StringBuilder
            = new StringBuilder(100);

        private string BuildMessageFromEvent(EventBase evt)
        {
            StringBuilder.Clear();

            StringBuilder.Append("[");
            StringBuilder.Append(DateTime.Now.ToString(CultureInfo.CurrentUICulture));
            StringBuilder.Append("] ");

            StringBuilder.Append(evt.GetType().Name);

            if (_latestMessageDuplicateCount > 0)
                StringBuilder.Append($" (x{_latestMessageDuplicateCount + 1})");

            return StringBuilder.ToString();
        }

        public void Clear()
        {
            _messages.Clear();
            OnClear?.Invoke();
        }

        public IReadOnlyList<string> GetMessages()
        {
            return _messages;
        }
    }
}