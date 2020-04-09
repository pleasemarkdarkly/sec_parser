using System.Collections.Generic;

namespace COI.Util
{
    public class NameAnalyzer
    {
        public static void AutoAliases(string name,List<string> aliases)
        {
            name = FilterName(name);
            var tokens = new List<string>();
            foreach (var token in name.Split(' ')) if (token.Length>2) tokens.Add(token);
            if (tokens.Count>=2)
            {
                AddIfNotDupplicate(aliases,string.Format("{0} {1}",tokens[0],tokens[1]));
                AddIfNotDupplicate(aliases, string.Format("{1} {0}", tokens[0], tokens[1]));
            }
            if (tokens.Count >= 3)
            {
                AddIfNotDupplicate(aliases, string.Format("{0} {1}", tokens[2], tokens[1]));
                AddIfNotDupplicate(aliases, string.Format("{1} {0}", tokens[2], tokens[1]));
                AddIfNotDupplicate(aliases, string.Format("{0} {1} {2}", tokens[0], tokens[1],tokens[2]));
                AddIfNotDupplicate(aliases, string.Format("{0} {2} {1}", tokens[0], tokens[1], tokens[2]));
                AddIfNotDupplicate(aliases, string.Format("{1} {0} {2}", tokens[0], tokens[1], tokens[2]));
                AddIfNotDupplicate(aliases, string.Format("{1} {2} {0}", tokens[0], tokens[1], tokens[2]));
                AddIfNotDupplicate(aliases, string.Format("{2} {0} {1}", tokens[0], tokens[1], tokens[2]));
                AddIfNotDupplicate(aliases, string.Format("{2} {1} {0}", tokens[0], tokens[1], tokens[2]));
            }
        }
        public static void AddIfNotDupplicate(List<string> names, string name)
        {
            foreach (var name1 in names)
                if (name1.ToLower().Trim()==name.ToLower().Trim()) return;
            names.Add(name);
        }
        public static string FilterName(string name)
        {
            if (name.Trim() == string.Empty) return string.Empty;
            var n = name.ToLower();
            n = n.Replace(",", string.Empty).Replace(".", string.Empty);
            while (n.Contains("  ")) 
                n = n.Replace("  ", " ");
            if (n.IndexOf('(',3) > 1)
                n = n.Substring(0,n.IndexOf('(',3));
            return n.Trim();
        }
        public static bool IsCompanyName(string name )
        {
            name = name.ToLower();
            return name.Contains(" llc") || name.Contains(" inc")
                || name.Contains("company")
                || name.Contains("group")
                || name.Contains("trust")
                || name.Contains("manage")
                || name.Contains("partner")
                || name.Contains("holding")
                || name.Contains("enterprise")
                || name.Contains("corporation")
                || name.Contains("incorporated")
                || name.Contains(" copr")
                || name.Contains(" agency")
                || name.Contains(" llp")
                || name.Contains(" ltd")
                || name.Contains("nevada")
                || name.EndsWith("llc")
                || name.EndsWith("inc")
                || name.EndsWith("corp")
                || name.EndsWith("llc.")
                || name.EndsWith("inc.")
                || name.EndsWith("corp.")
                || name.EndsWith("ltd");
        }
        public static string NameCapitalizer(string name)
        {
            name = name.Trim().ToLower();
            if (name.Length == 0) return name;
            var chars = name.ToCharArray();
            chars[0] = chars[0].ToString().ToUpper().ToCharArray()[0];
            for (var i = 1; i < chars.Length; i++)
            {
                if (chars[i - 1] == ' ' || chars[i - 1] == '-')
                    chars[i] = chars[i].ToString().ToUpper().ToCharArray()[0];
            }
            name = string.Empty;
            foreach (char c in chars)
            {
                name += c;
            }
            return name;
        }
    }
}
