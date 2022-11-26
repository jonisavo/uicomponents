using UnityEngine.UIElements;

namespace UIComponents.Samples.EventInterfaces
{
    [Layout("EventInterfaceLog")]
    [Stylesheet("EventInterfaceLog.style")]
    [Dependency(typeof(IEventLogService), provide: typeof(EventLogService))]
    public partial class EventInterfaceLog : UIComponent
    {
        public new class UxmlFactory : UxmlFactory<EventInterfaceLog> {}
        
        [Provide]
        private IEventLogService _eventLogService;
        
        [Query("event-scroll-view")]
        private ScrollView _scrollView;

        public override void OnInit()
        {
            foreach (var message in _eventLogService.GetMessages())
                _scrollView.Add(new Label(message));
            
            _eventLogService.OnMessageAdd += OnLogMessageAdd;
            _eventLogService.OnMessageChange += OnLogMessageChange;
            _eventLogService.OnClear += OnLogClear;
        }

        ~EventInterfaceLog()
        {
            _eventLogService.OnMessageAdd -= OnLogMessageAdd;
            _eventLogService.OnMessageChange -= OnLogMessageChange;
            _eventLogService.OnClear -= OnLogClear;
        }

        private void OnLogMessageAdd(string message)
        {
            _scrollView.Add(new Label(message));
        }

        private void OnLogMessageChange(int index, string newMessage)
        {
            if (_scrollView[index] is Label label)
                label.text = newMessage;
        }

        private void OnLogClear()
        {
            _scrollView.Clear();
        }
    }
}
