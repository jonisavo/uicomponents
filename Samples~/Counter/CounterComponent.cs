using UnityEngine.UIElements;

namespace UIComponents.Samples.Counter
{
    [Dependency(typeof(ICounterService), provide: typeof(CounterService))]
    public partial class CounterComponent : UIComponent
    {
        private readonly ICounterService _counterService;
    
        private readonly Label _countLabel;

        public CounterComponent()
        {
            _counterService = Provide<ICounterService>();
        
            _countLabel = new Label(_counterService.GetCount().ToString());
            Add(_countLabel);
        
            var incrementButton = new Button(IncrementCount);
            incrementButton.text = "Increment";
            Add(incrementButton);
        }

        private void IncrementCount()
        {
            _counterService.IncrementCount();
            _countLabel.text = _counterService.GetCount().ToString();
        }
    }
}
