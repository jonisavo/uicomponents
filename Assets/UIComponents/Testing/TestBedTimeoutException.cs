using System;

namespace UIComponents.Testing
{
    /// <summary>
    /// Thrown when an asynchronous TestBed operation times out.
    /// </summary>
    public sealed class TestBedTimeoutException : Exception
    {
        public override string Message { get; }
        
        public TestBedTimeoutException(string componentTypeName, int timeoutMs)
        {
            Message = $"Creation of component {componentTypeName} timed out after {timeoutMs}ms.";
        }
    }
}
