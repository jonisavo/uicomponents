using System;
using System.Text;

namespace UIComponents.Roslyn.Generation.Utilities
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendPadding(this StringBuilder stringBuilder, uint tabs = 1)
        {
            for (var i = 0; i < tabs; i++)
                stringBuilder.Append("    ");

            return stringBuilder;
        }

        public static StringBuilder AppendCodeGeneratedAttribute(this StringBuilder stringBuilder)
        {
            return stringBuilder.AppendLine(Constants.GeneratedCodeAttribute);
        }

        public static StringBuilder AppendWithPadding(this StringBuilder stringBuilder, string text, uint tabs = 1)
        {
            return stringBuilder.AppendPadding(tabs).Append(text);
        }

        public static StringBuilder AppendLineWithPadding(this StringBuilder stringBuilder, string text, uint tabs = 1)
        {
            return stringBuilder.AppendPadding(tabs).AppendLine(text);
        }
    }
}

