using NSubstitute;
using NUnit.Framework;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;

namespace UIComponents.Tests
{
    [TestFixture]
    public class PathAttributeTests
    {
        private class BasicPathAttribute : PathAttribute
        {
            public BasicPathAttribute(string path)
            {
                Path = path;
            }
        }
        
        [TestFixture]
        public class GetAssetPathForComponent
        {
            private class UIComponentWithNoAssetPaths : UIComponent {}
            
            [AssetPath("Assets/Valid/Path")]
            private class UIComponentWithValidAssetPath : UIComponent {}
            
            [AssetPath("Assets/Invalid/Path")]
            private class UIComponentWithInvalidAssetPath : UIComponent {}

            private TestBed _testBed;
            private IAssetResolver _mockResolver;

            [SetUp]
            public void SetUp()
            {
                _mockResolver = MockUtilities.CreateMockResolver();
                _testBed = TestBed.Create()
                    .WithSingleton(_mockResolver)
                    .Build();
            }

            [Test]
            public void Should_Return_Empty_String_If_No_Path_Is_Configured()
            {
                var pathAttribute = new BasicPathAttribute(null);
                var component = _testBed.CreateComponent<UIComponentWithNoAssetPaths>();
                
                var path = pathAttribute.GetAssetPathForComponent(component);
                
                Assert.That(path, Is.EqualTo(""));
            }

            [Test]
            public void Should_Ignore_AssetPath_In_Case_Of_Complete_Path()
            {
                var assetsPathAttribute = new BasicPathAttribute("Assets/Asset");
                var packagesPathAttribute = new BasicPathAttribute("Packages/Asset");
                var component = _testBed.CreateComponent<UIComponentWithValidAssetPath>();

                var assetPath = assetsPathAttribute.GetAssetPathForComponent(component);
                var packagePath = packagesPathAttribute.GetAssetPathForComponent(component);
                
                Assert.That(assetPath, Is.EqualTo("Assets/Asset"));
                Assert.That(packagePath, Is.EqualTo("Packages/Asset"));
            }

            [Test]
            public void Should_Use_Valid_AssetPath_In_Case_Of_Incomplete_Path()
            {
                var pathAttribute = new BasicPathAttribute("MyAsset");
                
                _mockResolver.AssetExists("Assets/Valid/Path/MyAsset").Returns(true);
                _mockResolver.AssetExists("Assets/Invalid/Path/MyAsset").Returns(false);

                var componentWithValidPath = _testBed.CreateComponent<UIComponentWithValidAssetPath>();
                var componentWithInvalidPath = _testBed.CreateComponent<UIComponentWithInvalidAssetPath>();

                var filledPath = pathAttribute.GetAssetPathForComponent(componentWithValidPath);
                var ignoredPath = pathAttribute.GetAssetPathForComponent(componentWithInvalidPath);
                
                Assert.That(filledPath, Is.EqualTo("Assets/Valid/Path/MyAsset"));
                Assert.That(ignoredPath, Is.EqualTo("MyAsset"));
            }
        } 
    }
}