using NUnit.Framework;
using UIComponents.Core;
using UIComponents.Core.Exceptions;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyInjectorStaticTests
    {
        private interface IDependency {}
        
        private class DependencyProvider : IDependency {}

        [Dependency(typeof(IDependency), provide: typeof(DependencyProvider))]
        private class UIComponentWithDependency : UIComponent
        {
            public IDependency GetDependency() => Provide<IDependency>();
        }
        
        [TestFixture]
        public class ClearDependency
        {
            [Test]
            public void Removes_The_Dependency_Instance()
            {
                var component = new UIComponentWithDependency();
                
                DependencyInjector.ClearDependency<UIComponentWithDependency, IDependency>();

                Assert.Throws<MissingProviderException>(() => component.GetDependency());
            }
            
            [TearDown]
            public void TearDown()
            {
                DependencyInjector.SetDependency<UIComponentWithDependency, IDependency>(
                    new DependencyProvider()
                );
            }
        }
    }
}