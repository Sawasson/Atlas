using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_InitialPriceLTV
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
                case "-lab":
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

        public static string TimeSlot(string mins)
        {
            return "1";
        }

        public static string JawabsaleCategories(string utm_source)
        {
            switch (utm_source.ToLower())
            {
                case string a when a.Contains("sale_generic"):
                    return "Generic";
                case string a when a.Contains("phones"):
                case string b when b.Contains("mobiles"):
                case string c when c.Contains("mobile"):
                case string d when d.Contains("huawei"):
                case string e when e.Contains("oppo"):
                case string f when f.Contains("phone"):                  
                    return "Mobile";
                case string a when a.Contains("hotels"):
                    return "hotel";
                case string a when a.Contains("camera"):
                case string b when b.Contains("camira"):
                    return "camera";
                case string a when a.Contains("laptop"):
                case string b when b.Contains("labtop"):
                    return "laptop";
                case string a when a.Contains("computer"):
                    return "computer";
                case string a when a.Contains("electronics"):
                case string b when b.Contains("samsung"):
                case string c when c.Contains("powerbank"):
                    return "electronics";
                case string a when a.Contains("tablets"):
                    return "tablets";
                case string a when a.Contains("health"):
                    return "health";
                case string a when a.Contains("airpod"):
                    return "airpod";
                case string a when a.Contains("babies"):
                case string b when b.Contains("baby"):
                    return "babies";
                case string a when a.Contains("fashion"):
                case string b when b.Contains("tshirts"):
                case string c when c.Contains("clothes"):
                case string d when d.Contains("bag"):
                    return "fashion";
                case string a when a.Contains("accessories"):
                case string b when b.Contains("sunglasses"):
                case string c when c.Contains("glasses"):
                    return "Accessories";
                case string a when a.Contains("homeaplliance"):
                case string b when b.Contains("furniture"):
                case string c when c.Contains("home"):
                case string d when d.Contains("kitchen"):
                case string e when e.Contains("stove"):
                case string f when f.Contains("refrigerator"):
                case string g when g.Contains("microwave"):
                case string h when h.Contains("refrigator"):
                case string i when i.Contains("deepfreezer"):
                    return "HomeAplliance";
                case string a when a.Contains("iphone"):
                    return "Iphone";
                case string a when a.Contains("gaming"):
                case string b when b.Contains("playstation"):
                case string c when c.Contains("consoles"):
                    return "gaming";
                case string a when a.Contains("makeup"):
                    return "Makeup";
                case string a when a.Contains("sports"):
                case string b when b.Contains("fitness"):
                case string c when c.Contains("sport"):
                case string d when d.Contains("running"):
                //case "beach":
                    return "Sports";
                case string a when a.Contains("shoes"):
                    return "shoes";
                case string a when a.Contains("momkids"):
                case string b when b.Contains("kids"):
                    return "Momkids";
                case string a when a.Contains("perfume"):
                    return "Perfume";
                case string a when a.Contains("smarttv"):
                case string b when b.Contains("tv"):
                case string c when c.Contains("t.v"):
                    return "TV";
                case string a when a.Contains("watches"):
                case string b when b.Contains("watch"):
                    return "Watches";
                case string a when a.Contains("shopping"):
                    return "shopping";
                case string a when a.Contains("men"):
                    return "Men";
                case string a when a.Contains("women"):
                case string b when b.Contains("wms"):
                case string c when c.Contains("woman"):
                    return "Women";
                case string a when a.Contains("cars"):
                case string b when b.Contains("car"):
                    return "Cars";
                case string a when a.Contains("beach"):
                    return "Beach";
                case string a when a.Contains("christmast"):
                case string b when b.Contains("christmas"):
                case string c when c.Contains("valentine"):
                case string d when d.Contains("gift"):
                    return "SpecialDays";
                case string a when a.Contains("stories"):
                case string b when b.Contains("story"):
                //case "valentine":
                //case "gift":
                case string c when c.Contains("newyear"):
                    return "Stories";
                default:
                    return "None";
            }

        }
        }

}
