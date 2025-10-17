using System.Text.RegularExpressions;

public static class CheckString
{
    public static bool Check(string s1, string s2)
    {
        string input = CleanText(s1);
        string target = CleanText(s2);

        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        if (input.Equals(target, System.StringComparison.Ordinal))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    // Hàm lọc ký tự đặc biệt
    private static string CleanText(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        text = text.Trim();
        // loại bỏ mọi ký tự khoảng trắng vô hình, xuống dòng, tab...
        return Regex.Replace(text, @"\p{C}|\p{Z}", "");
    }
}
