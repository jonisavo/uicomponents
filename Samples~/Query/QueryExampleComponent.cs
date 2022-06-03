using UIComponents.Experimental;
using UnityEngine.UIElements;

namespace UIComponents.Samples.Query
{
    [Layout("QueryExampleComponent")]
    [Stylesheet("QueryExampleComponent.styles")]
    public class QueryExampleComponent : UIComponent
    {
        [Query("my-label")]
        public readonly Label MyLabel;
    
        [Query("my-foldout")]
        public readonly Foldout MyFoldout;
    
        public QueryExampleComponent()
        {
            MyLabel.text = "This text was set in the component constructor.";
            MyFoldout.Add(new Label("This label was added in the component constructor."));
        }
    }
}
