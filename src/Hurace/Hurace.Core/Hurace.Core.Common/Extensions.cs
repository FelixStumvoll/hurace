namespace Hurace.Core.Common
{
    public static class Extensions
    {
        public static string ToLowerFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0])) return str;
            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}