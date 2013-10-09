namespace FluentBuild.BuildFileConverter
{
    public static class Extensions
    {
        public static string ToVariableString(this string source)
        {
            return source.Replace(".", "_").Replace("-", "_");
        }
    }
}