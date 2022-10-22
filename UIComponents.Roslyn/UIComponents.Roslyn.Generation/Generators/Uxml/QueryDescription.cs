using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.Uxml
{
    internal readonly struct QueryDescription
    {
        public readonly ISymbol MemberSymbol;

        public readonly struct QueryCall
        {
            public readonly string UxmlName;
            public readonly string ClassName;

            public QueryCall(string uxmlName, string className)
            {
                UxmlName = uxmlName ?? "null";
                ClassName = className ?? "(string) null";
            }
        }

        public readonly List<QueryCall> QueryCalls;

        public QueryDescription(ISymbol memberSymbol, List<QueryCall> queryCalls)
        {
            MemberSymbol = memberSymbol;
            QueryCalls = queryCalls;
        }

        public static QueryDescription CreateFromMember(ISymbol memberSymbol, Dictionary<AttributeData, Dictionary<string, TypedConstant>> attributes)
        {
            var queryCalls = new List<QueryCall>();

            foreach (var attribute in attributes)
            {
                string uxmlName = null;
                string ussClassName = null;

                var arguments = attribute.Value;

                if (arguments.TryGetValue("constructor_0", out var uxmlConstructorArg))
                    uxmlName = StringUtilities.AddQuotesToString(uxmlConstructorArg.Value as string);

                if (arguments.TryGetValue("Name", out var uxmlNameArg))
                    uxmlName = StringUtilities.AddQuotesToString(uxmlNameArg.Value as string);

                if (arguments.TryGetValue("Class", out var ussClassNameArg))
                    ussClassName = StringUtilities.AddQuotesToString(ussClassNameArg.Value as string);

                queryCalls.Add(new QueryCall(uxmlName, ussClassName));
            }

            return new QueryDescription(memberSymbol, queryCalls);
        }
    }
}
