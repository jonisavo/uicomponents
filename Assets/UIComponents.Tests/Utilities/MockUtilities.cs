using System.Threading.Tasks;
using NSubstitute;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Utilities
{
    public static class MockUtilities
    {
        public static IAssetResolver CreateMockResolver()
        {
            var resolver = Substitute.For<IAssetResolver>();
            resolver.LoadAsset<VisualTreeAsset>(Arg.Any<string>())
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<VisualTreeAsset>()));
            resolver.LoadAsset<StyleSheet>(Arg.Any<string>())
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<StyleSheet>()));
            return resolver;
        }
    }
}
