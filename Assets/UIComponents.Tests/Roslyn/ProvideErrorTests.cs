using System;
using System.Collections.Generic;
using NUnit.Framework;
using UIComponents.DependencyInjection;
using UnityEngine;
using UnityEngine.TestTools;

namespace UIComponents.Tests.Roslyn
{
    public interface IMissingDependency {}
    
    public partial class ProvideErrorComponent : UIComponent
    {
        [Provide] public ILogger MyLogger;
        [Provide] private IMissingDependency _dependency;
    }

    [Dependency(typeof(ILogger), provide: typeof(DebugLogger))]
    public partial class ProvideErrorClass : IDependencyConsumer
    {
        [Provide] public ILogger MyLogger;
        [Provide] private IMissingDependency _dependency;
        private readonly DependencyInjector _dependencyInjector;

        public ProvideErrorClass()
        {
            DiContext.Current.RegisterConsumer(this);
            _dependencyInjector = DiContext.Current.GetInjector(GetType());
            UIC_PopulateProvideFields();
        }

        private T Provide<T>() where T : class
        {
            return _dependencyInjector.Provide<T>();
        }
    }
    
    [TestFixture]
    public class ProvideErrorTests
    {
        [Test]
        public void Error_Is_Printed_With_Logger()
        {
            var component = new ProvideErrorComponent();
            
            Assert.That(component.MyLogger, Is.InstanceOf<DebugLogger>());
            LogAssert.Expect(LogType.Error, "[ProvideErrorComponent] Could not provide IMissingDependency to _dependency");
        }

        [Test]
        public void Error_Is_Printed_With_DebugLog()
        {
            var instance = new ProvideErrorClass();
            
            Assert.That(instance.MyLogger, Is.InstanceOf<DebugLogger>());
            LogAssert.Expect(LogType.Error, "Could not provide IMissingDependency to _dependency");
        }
    }
}