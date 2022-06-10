using UIComponents.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Samples.Query
{
    [Layout("QueryExampleComponent")]
    [Stylesheet("QueryExampleComponent.styles")]
    public class QueryExampleComponent : UIComponent
    {
        [Query("my-label")]
        private readonly Label MyLabel;
    
        [Query(Name = "my-foldout")]
        private readonly Foldout MyFoldout;

        [Query(Class = "description")]
        private readonly Label[] DescriptionLabels;

        [Query]
        private readonly VisualElement[] Everything;
    
        public QueryExampleComponent()
        {
            MyLabel.text = "This text was set in the component constructor.";
            MyFoldout.Add(new Label("This label was added in the component constructor."));

            foreach (var label in DescriptionLabels)
                label.style.color = Color.green;

            foreach (var element in Everything)
                element.tooltip = "Everything has this tooltip.";
        }
    }
}
