using System.Linq;

namespace SecretBook.Helper
{
    public static class Extension
    {
        public static bool Contains(this string value, params string[] values)
        {
            if (values.Length <= 0)
                return true;
            else
            {
                values = values.Select(x => x.ToLower().Trim()).ToArray();
                return values.Contains(value);
            }
        }
    }
}
