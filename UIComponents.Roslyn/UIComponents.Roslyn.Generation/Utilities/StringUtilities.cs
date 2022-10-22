namespace UIComponents.Roslyn.Generation.Utilities
{
    internal class StringUtilities
    {
        public static string AddQuotesToString(string str)
        {
            if (str == null)
                return null;

            return $"\"{str}\"";
        }
    }
}
