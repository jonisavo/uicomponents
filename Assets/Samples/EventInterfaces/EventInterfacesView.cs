using UnityEngine.UIElements;

namespace UIComponents.Samples.EventInterfaces
{
    [Layout("EventInterfacesView")]
    [Dependency(typeof(IEventLogService), provide: typeof(EventLogService))]
    public class EventInterfacesView : UIComponent,
        IOnAttachToPanel, IOnGeometryChanged, IOnDetachFromPanel,
        IOnMouseLeave, IOnMouseEnter
    {
        private readonly IEventLogService _eventLogService;

        private readonly Button _clearButton;
        
        public EventInterfacesView()
        {
            _eventLogService = Provide<IEventLogService>();
            _clearButton = this.Q<Button>("log-clear-button");
            _clearButton.clicked += OnClearButtonClicked;
        }

        ~EventInterfacesView()
        {
            _clearButton.clicked -= OnClearButtonClicked;
        }

        public void OnAttachToPanel(AttachToPanelEvent evt)
        {
            _eventLogService.Log(evt);
        }

        public void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            _eventLogService.Log(evt);
        }

        public void OnGeometryChanged(GeometryChangedEvent evt)
        {
            _eventLogService.Log(evt);
        }

        public void OnMouseEnter(MouseEnterEvent evt)
        {
            _eventLogService.Log(evt);
        }

        public void OnMouseLeave(MouseLeaveEvent evt)
        {
            _eventLogService.Log(evt);
        }

        private void OnClearButtonClicked()
        {
            _eventLogService.Clear();
        }
    }
}