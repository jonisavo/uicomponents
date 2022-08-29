using UnityEngine.UIElements;

namespace UIComponents.Samples.EventInterfaces
{
    [Layout("EventInterfaceLog")]
    [Stylesheet("EventInterfaceLog.style")]
    [Dependency(typeof(IEventLogService), provide: typeof(EventLogService))]
    public class EventInterfaceLog : UIComponent
    {
        public new class UxmlFactory : UxmlFactory<EventInterfaceLog> {}
        
        private readonly EventLogService _eventLogService;
        
        private ScrollView _scrollView;

        public EventInterfaceLog()
        {
            // Unity 2019 does not support interfaces with delegates and events,
            // so we need to get the class itself
            _eventLogService = Provide<IEventLogService>() as EventLogService;
        }

        public override void OnInit()
        {
            _scrollView = this.Q<ScrollView>("event-scroll-view");
            
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
