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
                .Returns(ScriptableObject.CreateInstance<VisualTreeAsset>());
            resolver.LoadAsset<StyleSheet>(Arg.Any<string>())
                .Returns(ScriptableObject.CreateInstance<StyleSheet>());
            return resolver;
        }
    }
}