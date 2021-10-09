using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3
{
    public class SubcriptionDownloadInfo
    {
        public string Url { get; set; }

        public ProjectType Project { get; set; }
    }
    public static class SubsHelper
    {

      public  static string sale_url_subs = $@"https://www.lp.jawabsale.com/admin/user_subscription_flat/";
        static string tawzeef_url_subs = "https://www.lp.jawabtawzeef.com/admin/user_subscription_flat/";
        static string mehan_url_subs = "https://www.lp.jawabmehan.com/admin/user_subscription_flat/";
        static string playwin_url_subs = "https://lp.playwin.app/admin/user_subscription_flat/";
       public static string jawabcom_url_subs = "https://www.jawabkom.com/admin/user_subscription_flat/";
        static string hawiyyah_url_subs = "https://www.ad.hawiyyah.com/admin/user_subscription_flat";



        static string sale_url_contracts = $@"https://www.lp.jawabsale.com/admin/dcb_contracts/";
        static string tawzeef_url_contracts = "https://www.lp.jawabtawzeef.com/admin/dcb_contracts/";
        static string mehan_url_contracts = "https://www.lp.jawabmehan.com/admin/dcb_contracts/";
        static string playwin_url_contracts = "https://lp.playwin.app/admin/dcb_contracts/";
        static string jawabcom_url_contracts = "https://www.jawabkom.com/admin/dcb_contracts/";


        public static List<SubcriptionDownloadInfo> GetProjectSubsUrlList()
        {
            List<SubcriptionDownloadInfo> urls = new List<SubcriptionDownloadInfo>();

            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.JAWAB, Url = jawabcom_url_subs });
            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.SALE, Url = sale_url_subs });
            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.TAWZEEF, Url = tawzeef_url_subs });
            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.MEHAN, Url = mehan_url_subs });
            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.PLAYWIN, Url = playwin_url_subs });
            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.HAWIYYAH, Url = hawiyyah_url_subs });

            return urls;

        }

        public static List<SubcriptionDownloadInfo> GetProjectSubsContractUrlList()
        {
            List<SubcriptionDownloadInfo> urls = new List<SubcriptionDownloadInfo>();

            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.JAWAB, Url = jawabcom_url_contracts });
            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.SALE, Url = sale_url_contracts });
            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.TAWZEEF, Url = tawzeef_url_contracts });
            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.MEHAN, Url = mehan_url_contracts });
            urls.Add(new SubcriptionDownloadInfo { Project = ProjectType.PLAYWIN, Url = playwin_url_contracts });

            return urls;

        }

    
    }

    public enum ProjectType
    {
        JAWAB,
        SALE,
        TAWZEEF,
        MEHAN,
        PLAYWIN,
        HAWIYYAH
    }
}
