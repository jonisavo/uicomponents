using UnityEngine.UIElements;

namespace UIComponents.Samples.EventInterfaces
{
    [Layout("EventInterfacesView")]
    [Dependency(typeof(IEventLogService), provide: typeof(EventLogService))]
    public class EventInterfacesView : UIComponent,
        IOnAttachToPanel, IOnGeometryChanged, IOnDetachFromPanel,
        IOnMouseLeave, IOnMouseEnter
#if UNITY_2020_3_OR_NEWER
        , IOnClick
#endif
    {
        private readonly IEventLogService _eventLogService;

        private Button _clearButton;
        
        public EventInterfacesView()
        {
            _eventLogService = Provide<IEventLogService>();
        }

        public override void OnInit()
        {
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

#if UNITY_2020_3_OR_NEWER
        public void OnClick(ClickEvent evt)
        {
            _eventLogService.Log(evt);
        }
#endif
        
        private void OnClearButtonClicked()
        {
            _eventLogService.Clear();
        }
    }
}
