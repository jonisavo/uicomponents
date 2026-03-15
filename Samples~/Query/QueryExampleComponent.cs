using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Samples.Query
{
    [Layout("QueryExampleComponent")]
    [Stylesheet("QueryExampleComponent.styles")]
    public partial class QueryExampleComponent : UIComponent
    {
        [Query("my-label")]
        private Label _myLabel;
    
        [Query(Name = "my-foldout")]
        private Foldout _myFoldout;

        [Query(Class = "description")]
        private Label[] _descriptionLabels;

        [Query]
        private VisualElement[] _everything;

        public override void OnInit()
        {
            _myLabel.text = "This text was set in the component's OnInit function.";
            _myFoldout.Add(new Label("This label was added in the component's OnInit function."));

            foreach (var label in _descriptionLabels)
                label.style.color = Color.green;

            foreach (var element in _everything)
                element.tooltip = "Everything has this tooltip.";
        }
    }
}
