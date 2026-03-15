using UnityEngine.UIElements;

namespace UIComponents.Samples.EventInterfaces
{
    [Layout("EventInterfacesView")]
    [Dependency(typeof(IEventLogService), provide: typeof(EventLogService))]
    public partial class EventInterfacesView : UIComponent,
        IOnAttachToPanel, IOnGeometryChanged, IOnDetachFromPanel,
        IOnMouseLeave, IOnMouseEnter, IOnClick
    {
        [Provide]
        private IEventLogService _eventLogService;

        [Query("log-clear-button")]
        private Button _clearButton;

        public override void OnInit()
        {
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
        
        public void OnClick(ClickEvent evt)
        {
            _eventLogService.Log(evt);
        }

        private void OnClearButtonClicked()
        {
            _eventLogService.Clear();
        }
    }
}
