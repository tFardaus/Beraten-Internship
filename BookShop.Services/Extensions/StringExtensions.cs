namespace BookShop.Services.Extensions
{
    public static class StringExtensions
    {
        
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        
        public static string Truncate(this string str, int maxLength)
        {
            if (str.IsEmpty() || str.Length <= maxLength)
                return str;
            
            return str.Substring(0, maxLength) + "...";
        }

    }
}
