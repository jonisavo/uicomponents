using NUnit.Framework;
using UIComponents.Utilities;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyScopeTests
    {
        public interface IDependency {}
        
        public class DependencyClass : IDependency {}
        
        public class NewDependencyClass : IDependency {}

        public class UIComponentWithNoDependencyAttribute : UIComponent
        {
            public IDependency GetDependency() => Provide<IDependency>();
        }

        [Dependency(typeof(IDependency), provide: typeof(DependencyClass))]
        public class UIComponentWithDependency : UIComponentWithNoDependencyAttribute {}

        [Test]
        public void Should_Set_Dependency_And_Restore_Previous()
        {
            var component = new UIComponentWithDependency();
            
            using (new DependencyScope<UIComponentWithDependency, IDependency>(new NewDependencyClass()))
            {
                Assert.That(component.GetDependency(), Is.InstanceOf<NewDependencyClass>());
            }
            
            Assert.That(component.GetDependency(), Is.InstanceOf<DependencyClass>());
        }

        [Test]
        public void Should_Set_Dependency_And_Clear_It_If_No_Previous_Exists()
        {
            var component = new UIComponentWithNoDependencyAttribute();
            
            using (new DependencyScope<UIComponentWithNoDependencyAttribute, IDependency>(new NewDependencyClass()))
            {
                Assert.That(component.GetDependency(), Is.InstanceOf<NewDependencyClass>());
            }

            Assert.Throws<MissingProviderException>(() => component.GetDependency());
        }
    }
}