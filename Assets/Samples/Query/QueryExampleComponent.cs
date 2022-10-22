using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Samples.Query
{
    [Layout("QueryExampleComponent")]
    [Stylesheet("QueryExampleComponent.styles")]
    public partial class QueryExampleComponent : UIComponent
    {
        [Query("my-label")]
        private Label MyLabel;
    
        [Query(Name = "my-foldout")]
        private Foldout MyFoldout;

        [Query(Class = "description")]
        private Label[] DescriptionLabels;

        [Query]
        private VisualElement[] Everything;

        public override void OnInit()
        {
            MyLabel.text = "This text was set in the component's OnInit function.";
            MyFoldout.Add(new Label("This label was added in the component's OnInit function."));

            foreach (var label in DescriptionLabels)
                label.style.color = Color.green;

            foreach (var element in Everything)
                element.tooltip = "Everything has this tooltip.";
        }
    }
}
