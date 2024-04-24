namespace QE.SVG
{
    public static class RegexPattern
    {
        public static string ViewBox = "viewBox?\\=\\\"[\\s\\S]+?\\\"";

        public static string SvgMain = "<svg[\\w\\W]+?<\\/svg>";
    }
}
