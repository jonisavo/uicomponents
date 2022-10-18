﻿using UIComponents.Experimental;

namespace UIComponents.Tests.Roslyn
{
    public partial class NestParentClass
    {
        [UxmlName("NestedRoslynTest")]
        public partial class NestedRoslynTestComponent : UIComponent
        {
            [UxmlTrait]
            public string Trait { get; set; }
        }   
    }
}
