namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            string lower = input.ToLower();
            string cln = "";
            foreach (char c in lower)
            {
                if (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))
                {
                    cln += c;
                }
            }
            if (cln.Length == 0)
            {
                return false;
            }
            string rvrs = "";
            for (int i = cln.Length - 1; i >= 0; i--)
            {
                rvrs +=cln[i];
            }
            return cln == rvrs;
        }
    }    
}



