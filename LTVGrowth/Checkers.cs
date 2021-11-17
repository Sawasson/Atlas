using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTVGrowth
{
    public class Checkers
    {
        public static string CategoryChecker(string utm_source)
        {
            switch (utm_source.Substring(0, Math.Min(utm_source.Length, 5)).ToLower())
            {
                case "kimia":
                case "mobid":
                case "mobip":
                case "imoca":
                case "mobus":
                    return "Apps";
                case "faceb":
                    return "Facebook";
                case "insta":
                    return "Instagram";
                case "snapc":
                    return "Snap";
                case "postq":
                case "nativ":
                    return "Postq";
                case "taboo":
                    return "Taboo";
                case "googl":
                case "gdnet":
                case "htcon":
                case "gdn_1":
                    return GoogleChecker(utm_source);
                case "seo":
                    return "SEO";
                case "smspr":
                    return "SMS";
                case "bing":
                case "bing-":
                    return "Bing";
                case "twitt":
                    return "Twitter";
                case "speak":
                    return "Speakol";
                case "tikt":
                    return "Tiktok";
                default:
                    return "None";
            }
        }

        public static string GoogleChecker(string utm_source)
        {
            switch (utm_source.Substring(0, Math.Min(utm_source.Length, 7)).ToLower())
            {
                case "googled":
                case "gdbetis":
                case "gdn_1pa":
                    return "GoogleDisplay";
                case "googlea":
                case "htcons":
                case "googles":
                    return "GoogleSearch";
                default:
                    return "None";
            }
        }

        public static string AppsChecker(string category)
        {
            switch (category)
            {
                case "Apps":
                    return "Yes";
                default:
                    return "No";
            }
        }

        public static bool TaboolaChecker(string utm_source)
        {
            switch (utm_source.Substring(0, Math.Min(utm_source.Length, 7)).ToLower())
            {
                case "taboola":
                    return true;
                default:
                    return false;
            }
        }

        public static bool PostquareChecker(string utm_source)
        {
            switch (utm_source.Substring(0, Math.Min(utm_source.Length, 5)).ToLower())
            {
                case "postq":
                case "posts":
                    return true;
                default:
                    return false;
            }
        }
    }

}
