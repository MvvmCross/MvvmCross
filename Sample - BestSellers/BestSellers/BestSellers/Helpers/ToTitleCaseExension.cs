namespace BestSellers.Helpers
{
    public static class ToTitleCaseExension
    {
        public static string ToTitleCase(this string str)
        {
            string result = str = str.ToLower();
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }

    }
}
