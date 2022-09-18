using System.Collections;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Internal;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;
using UnityEngine.TestTools;

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

            [UnityTest]
            public IEnumerator Should_Return_Empty_String_If_No_Path_Is_Configured()
            {
                var pathAttribute = new BasicPathAttribute(null);
                var component = _testBed.CreateComponent<UIComponentWithNoAssetPaths>();

                var assetPathTask = pathAttribute.GetAssetPathForComponent(component);

                yield return assetPathTask.AsEnumerator();

                var path = assetPathTask.Result;

                Assert.That(path, Is.EqualTo(""));
            }

            [UnityTest]
            public IEnumerator Should_Ignore_AssetPath_In_Case_Of_Complete_Path()
            {
                var assetsPathAttribute = new BasicPathAttribute("Assets/Asset");
                var packagesPathAttribute = new BasicPathAttribute("Packages/Asset");
                var component = _testBed.CreateComponent<UIComponentWithValidAssetPath>();
            
                var assetPathTask = assetsPathAttribute.GetAssetPathForComponent(component);
                var packagePathTask = packagesPathAttribute.GetAssetPathForComponent(component);

                yield return Task.WhenAll(assetPathTask, packagePathTask).AsEnumerator();
                
                var assetPath = assetPathTask.Result;
                var packagePath = packagePathTask.Result;

                Assert.That(assetPath, Is.EqualTo("Assets/Asset"));
                Assert.That(packagePath, Is.EqualTo("Packages/Asset"));
            }
            
            [UnityTest]
            public IEnumerator Should_Use_Valid_AssetPath_In_Case_Of_Incomplete_Path()
            {
                var pathAttribute = new BasicPathAttribute("MyAsset");
                
                _mockResolver.AssetExists("Assets/Valid/Path/MyAsset").Returns(true);
                _mockResolver.AssetExists("Assets/Invalid/Path/MyAsset").Returns(false);
            
                var componentWithValidPath = _testBed.CreateComponent<UIComponentWithValidAssetPath>();
                var componentWithInvalidPath = _testBed.CreateComponent<UIComponentWithInvalidAssetPath>();
            
                var filledPathTask = pathAttribute.GetAssetPathForComponent(componentWithValidPath);
                var ignoredPathTask = pathAttribute.GetAssetPathForComponent(componentWithInvalidPath);
                
                yield return Task.WhenAll(filledPathTask, ignoredPathTask).AsEnumerator();
                
                var filledPath = filledPathTask.Result;
                var ignoredPath = ignoredPathTask.Result;

                Assert.That(filledPath, Is.EqualTo("Assets/Valid/Path/MyAsset"));
                Assert.That(ignoredPath, Is.EqualTo("MyAsset"));
            }
        } 
    }
}
