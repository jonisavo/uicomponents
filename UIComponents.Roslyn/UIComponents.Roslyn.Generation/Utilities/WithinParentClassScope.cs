using System;
using System.Text;

namespace UIComponents.Roslyn.Generation.Utilities
{
    internal class WithinParentClassScope : IDisposable
    {
        private readonly StringBuilder _stringBuilder;
        private readonly int _parentsCount;

        public WithinParentClassScope(ParentClass parentClass, StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder;
            _parentsCount = 0;

            var current = parentClass;

            // Loop through the full parent type hierarchy, starting with the outermost
            while (current != null)
            {
                stringBuilder
                    .Append(current.Accessibility)
                    .Append(" partial ")
                    .Append(current.Keyword) // e.g. class/struct/record
                    .Append(' ')
                    .Append(current.Name) // e.g. Outer/Generic<T>
                    .Append(' ')
                    .Append(current.Constraints) // e.g. where T: new()
                    .AppendLine("\n{");
                _parentsCount++;
                current = current.Child;
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < _parentsCount; i++)
                _stringBuilder.AppendLine(@"}");
        }
    }
}
