namespace COI.DAL.Util
{
    public class Dbo
    {
        public static string FilterName(string name)
        {
            var n = name.ToLower();
            try
            {
                n = n.Replace(",", "");
                n = n.Replace(".", "");
                n = n.Replace("     ", " ");
                n = n.Replace("    ", " ");
                n = n.Replace("   ", " ");
                n = n.Replace("  ", " ");
                if (n.IndexOf('(', 3) > 0) n = n.Substring(0, n.IndexOf('(', 3) - 1);
                return n.Trim();
            }
            catch{return n;}
        }
    }
}