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
    
        [Query("my-foldout")]
        private readonly Foldout MyFoldout;

        [QueryClass("description")]
        private readonly Label[] DescriptionLabels;
    
        public QueryExampleComponent()
        {
            MyLabel.text = "This text was set in the component constructor.";
            MyFoldout.Add(new Label("This label was added in the component constructor."));

            foreach (var label in DescriptionLabels)
                label.style.color = Color.green;
        }
    }
}
