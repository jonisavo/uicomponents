using System;
using System.Diagnostics;
using UnityEngine.TestTools;

namespace UIComponents.Experimental
{
    /// <summary>
    /// When used in Unity 2021.2 or later, this attribute will generate
    /// an UxmlTraits implementation when applied to a field or property
    /// which has a set method. The class must be partial.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class MyComponent : UIComponent
    /// {
    ///     [Trait]
    ///     public string Description;
    ///
    ///     [Trait(Name = "a-color")]
    ///     public Color Color;
    ///
    ///     [Trait(DefaultValue = 3)]
    ///     public int Lives;
    /// }
    ///
    /// // This generates:
    ///
    /// public partial class MyComponent
    /// {
    ///     public new class UxmlFactory : UxmlFactory&lt;MyComponent, UxmlTraits&gt; {}
    /// 
    ///     public new class UxmlTraits : UIElements.UxmlTraits
    ///     {
    ///         UxmlStringAttributeDescription m_Description = new UxmlStringAttributeDescription { name = "description" };
    ///         UxmlColorAttributeDescription m_Color = new UxmlColorAttributeDescription { name = "a-color" };
    ///         UxmlIntAttributeDescription m_Lives = new UxmlIntAttributeDescription { name = "lives", defaultValue = 3 };
    ///
    ///         public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
    ///         {
    ///             base.Init(ve, bag, cc);
    ///             m_Lives.defaultValue = 3;
    ///             ((MyComponent)ve).Description = m_Description.GetValueFromBag(bag, cc);
    ///             ((MyComponent)ve).Color = m_Color.GetValueFromBag(bag, cc);
    ///             ((MyComponent)ve).Lives = m_Lives.GetValueFromBag(bag, cc);
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    [Conditional("UNITY_EDITOR")]
    [ExcludeFromCoverage]
    public sealed class TraitAttribute : Attribute
    {
        /// <summary>
        /// Defines a custom UXML name for the trait.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Defines a default value for the trait.
        /// </summary>
        public object DefaultValue { get; set; }
    }
}
