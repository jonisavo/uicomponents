using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UIComponents.Tests
{
    [TestFixture]
    public class UIComponentDebugLoggerTests
    {
        private class TestComponent : UIComponent {}

        private TestComponent _testComponent;

        private UIComponentDebugLogger _logger;
        
        [SetUp]
        public void SetUp()
        {
            _testComponent = new TestComponent();
            _logger = new UIComponentDebugLogger();
        }
        
        [Test]
        public void Log_Uses_Debug_Log()
        {
            _logger.Log("Hello world", _testComponent);
            
            LogAssert.Expect(LogType.Log, "[TestComponent] Hello world");
        }
        
        [Test]
        public void LogWarning_Uses_Debug_LogWarning()
        {
            _logger.LogWarning("Hello warning", _testComponent);
            
            LogAssert.Expect(LogType.Warning, "[TestComponent] Hello warning");
        }

        [Test]
        public void LogError_Uses_Debug_LogError()
        {
            _logger.LogError("Error message", _testComponent);
            
            LogAssert.Expect(LogType.Error, "[TestComponent] Error message");
        }
    }
}