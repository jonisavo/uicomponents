using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.Uxml
{
    internal readonly struct QueryDescription
    {
        public readonly ISymbol MemberSymbol;
        public readonly string UxmlName;
        public readonly string ClassName;

        public QueryDescription(ISymbol memberSymbol, string uxmlName, string className)
        {
            MemberSymbol = memberSymbol;
            UxmlName = uxmlName ?? "null";
            ClassName = className ?? "(string) null";
        }

        public static QueryDescription CreateFromMember(ISymbol memberSymbol, Dictionary<string, TypedConstant> arguments)
        {
            string uxmlName = null;
            string ussClassName = null;

            if (arguments.TryGetValue("constructor_0", out var uxmlConstructorArg))
                uxmlName = StringUtilities.AddQuotesToString(uxmlConstructorArg.Value as string);

            if (arguments.TryGetValue("Name", out var uxmlNameArg))
                uxmlName = StringUtilities.AddQuotesToString(uxmlNameArg.Value as string);

            if (arguments.TryGetValue("Class", out var ussClassNameArg))
                ussClassName = StringUtilities.AddQuotesToString(ussClassNameArg.Value as string);

            return new QueryDescription(memberSymbol, uxmlName, ussClassName);
        }
    }
}
