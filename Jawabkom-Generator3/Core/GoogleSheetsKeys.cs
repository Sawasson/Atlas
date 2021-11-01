using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3.Core
{
    public class GoogleSheetsKeys
    {
        public string SheetTitle { get; set; }
        public string FilePath { get; set; } = "client_key.json";
        public string Key { get; set; }
        public List<GoogleSheetsKeys> Keys { get; set; }

        public static GoogleSheetsKeys RawCurrency()
        {
            
            GoogleSheetsKeys RawCurrency = new GoogleSheetsKeys();
            RawCurrency.SheetTitle = "Raw_Currency";
            RawCurrency.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawCurrency;
        }

        public static GoogleSheetsKeys RawRevenuesLastMonthly()
        {
            GoogleSheetsKeys RawRevenuesLastMonthly = new GoogleSheetsKeys();
            RawRevenuesLastMonthly.SheetTitle = "Raw_Revenues_Last_Monthly";
            RawRevenuesLastMonthly.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawRevenuesLastMonthly;
        }

        public static GoogleSheetsKeys NewLtvSameMonth()
        {
            GoogleSheetsKeys NewLtvSameMonth = new GoogleSheetsKeys();
            NewLtvSameMonth.SheetTitle = "New_LTV_SAMEMONTH";
            NewLtvSameMonth.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return NewLtvSameMonth;
        }

        public static GoogleSheetsKeys RawFinalReportMonthly()
        {
            GoogleSheetsKeys RawFinalReportMonthly = new GoogleSheetsKeys();
            RawFinalReportMonthly.SheetTitle = "Raw_Final_Report_Monthly";
            RawFinalReportMonthly.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawFinalReportMonthly;
        }

        public static GoogleSheetsKeys RawDailyCost()
        {
            GoogleSheetsKeys RawFinalReportMonthly = new GoogleSheetsKeys();
            RawFinalReportMonthly.SheetTitle = "Raw_daily_cost";
            RawFinalReportMonthly.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawFinalReportMonthly;
        }

        public static GoogleSheetsKeys RawMonthlyClicks()
        {
            GoogleSheetsKeys RawMonthlyClicks = new GoogleSheetsKeys();
            RawMonthlyClicks.SheetTitle = "Raw_monthly_clicks";
            RawMonthlyClicks.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawMonthlyClicks;
        }

        public static GoogleSheetsKeys RevenuesLast()
        {
            GoogleSheetsKeys RevenuesLast = new GoogleSheetsKeys();
            RevenuesLast.SheetTitle = "RevenuesLast";
            RevenuesLast.Key = "1m6FznRu0q7hTrLS5KWkorYHdpxRhlQhF0hF-opaA6gw";
            return RevenuesLast;
        }


    }




}
