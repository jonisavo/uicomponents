﻿using NUnit.Framework;
using UIComponents.Addressables;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Editor
{
    [TestFixture]
    public class AddressableAssetResolverTests : AssetResolverTestSuite<AddressableAssetResolver>
    {
        [Test]
        public void Should_Be_Able_To_Load_Existing_Asset_Synchronously()
        {
            Assert_Loads_Existing_Asset<StyleSheet>(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uss"
            );
            Assert_Loads_Existing_Asset<VisualTreeAsset>(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uxml"
            );
        }

        [Test]
        public void Should_Be_Able_To_Tell_If_Asset_Exists()
        {
            Assert_Tells_If_Asset_Exists(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uss"
            );
            Assert_Tells_If_Asset_Exists(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uxml"
            );
        }
    }
}