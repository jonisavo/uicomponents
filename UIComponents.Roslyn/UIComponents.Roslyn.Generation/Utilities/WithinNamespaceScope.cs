using System;
using System.Text;

namespace UIComponents.Roslyn.Generation.Utilities
{
    /// <summary>
    /// A scope for StringBuilders, where the output will be written
    /// inside a namespace block.
    /// </summary>
    internal class WithinNamespaceScope : IDisposable
    {
        private readonly bool _hasNamespace;
        private readonly StringBuilder _stringBuilder;

        public WithinNamespaceScope(string fullNamespace, StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder;

            _hasNamespace = !string.IsNullOrEmpty(fullNamespace);

            if (_hasNamespace)
            {
                stringBuilder
                    .Append("namespace ")
                    .Append(fullNamespace)
                    .AppendLine("\n{");
            }
        }

        public void Dispose()
        {
            if (_hasNamespace)
                _stringBuilder.AppendLine("}");
        }
    }
}
