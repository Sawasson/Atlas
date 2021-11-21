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

        public static GoogleSheetsKeys Jawabkom_RawCurrency()
        {
            
            GoogleSheetsKeys RawCurrency = new GoogleSheetsKeys();
            RawCurrency.SheetTitle = "Raw_Currency";
            RawCurrency.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawCurrency;
        }

        public static GoogleSheetsKeys Jawabkom_RawRevenuesLastMonthly()
        {
            GoogleSheetsKeys RawRevenuesLastMonthly = new GoogleSheetsKeys();
            RawRevenuesLastMonthly.SheetTitle = "Raw_Revenues_Last_Monthly";
            RawRevenuesLastMonthly.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawRevenuesLastMonthly;
        }

        public static GoogleSheetsKeys Jawabkom_NewLtvSameMonth()
        {
            GoogleSheetsKeys NewLtvSameMonth = new GoogleSheetsKeys();
            NewLtvSameMonth.SheetTitle = "New_LTV_SAMEMONTH";
            NewLtvSameMonth.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return NewLtvSameMonth;
        }

        public static GoogleSheetsKeys Jawabkom_RawFinalReportMonthly()
        {
            GoogleSheetsKeys RawFinalReportMonthly = new GoogleSheetsKeys();
            RawFinalReportMonthly.SheetTitle = "Raw_Final_Report_Monthly";
            RawFinalReportMonthly.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawFinalReportMonthly;
        }

        public static GoogleSheetsKeys Jawabkom_RawDailyCost()
        {
            GoogleSheetsKeys RawFinalReportMonthly = new GoogleSheetsKeys();
            RawFinalReportMonthly.SheetTitle = "Raw_daily_cost";
            RawFinalReportMonthly.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawFinalReportMonthly;
        }

        public static GoogleSheetsKeys Jawabkom_RawMonthlyClicks()
        {
            GoogleSheetsKeys RawMonthlyClicks = new GoogleSheetsKeys();
            RawMonthlyClicks.SheetTitle = "Raw_monthly_clicks";
            RawMonthlyClicks.Key = "1_aMRay1SBETIBBAaaAKrGfA286Hk_PdyxGyv3YiJWoc";
            return RawMonthlyClicks;
        }

        public static GoogleSheetsKeys Jawabsale_RevenuesLast()
        {
            GoogleSheetsKeys RevenuesLast = new GoogleSheetsKeys();
            RevenuesLast.SheetTitle = "RevenuesLast";
            RevenuesLast.Key = "1m6FznRu0q7hTrLS5KWkorYHdpxRhlQhF0hF-opaA6gw";
            return RevenuesLast;
        }

        public static GoogleSheetsKeys Jawabsale_NewLTVSAMEMONTH()
        {
            GoogleSheetsKeys NewLtvSameMonth = new GoogleSheetsKeys();
            NewLtvSameMonth.SheetTitle = "New_LTV_SAMEMONTH";
            NewLtvSameMonth.Key = "1m6FznRu0q7hTrLS5KWkorYHdpxRhlQhF0hF-opaA6gw";
            return NewLtvSameMonth;
        }

        public static GoogleSheetsKeys Jawabsale_FirstSubReport()
        {
            GoogleSheetsKeys FirstSubReport = new GoogleSheetsKeys();
            FirstSubReport.SheetTitle = "FirstSubReport";
            FirstSubReport.Key = "1m6FznRu0q7hTrLS5KWkorYHdpxRhlQhF0hF-opaA6gw";
            return FirstSubReport;
        }

        public static GoogleSheetsKeys Hawiyyah_RevenuesLast()
        {
            GoogleSheetsKeys RevenuesLast = new GoogleSheetsKeys();
            RevenuesLast.SheetTitle = "RevenuesLast";
            RevenuesLast.Key = "1aSdBBpsLbgVBOVRHdOp-QJIEcToyksxVcAfqgXV2aT4";
            return RevenuesLast;
        }

        public static GoogleSheetsKeys Hawiyyah_RevenuesLast2()
        {
            GoogleSheetsKeys RevenuesLast2 = new GoogleSheetsKeys();
            RevenuesLast2.SheetTitle = "RevenuesLast";
            RevenuesLast2.Key = "1YmscadeZNiZwSTIoCuLXR5ypVTtFpwJGKDcGi4CzhdE";
            return RevenuesLast2;
        }

        public static GoogleSheetsKeys Hawiyyah_RevenuesLast3()
        {
            GoogleSheetsKeys RevenuesLast3 = new GoogleSheetsKeys();
            RevenuesLast3.SheetTitle = "RevenuesLast";
            RevenuesLast3.Key = "16kOPmUyrm-Cir-VKCYpO5oM_dcBTSyjm-Gt859N4jqc";
            return RevenuesLast3;
        }

        public static GoogleSheetsKeys Hawiyyah_NewLTVSAMEMONTH()
        {
            GoogleSheetsKeys NewLtvSameMonth = new GoogleSheetsKeys();
            NewLtvSameMonth.SheetTitle = "New_LTV_SAMEMONTH";
            NewLtvSameMonth.Key = "1aSdBBpsLbgVBOVRHdOp-QJIEcToyksxVcAfqgXV2aT4";
            return NewLtvSameMonth;
        }

        public static GoogleSheetsKeys Hawiyyah_FirstSubReport()
        {
            GoogleSheetsKeys FirstSubReport = new GoogleSheetsKeys();
            FirstSubReport.SheetTitle = "FirstSubReport";
            FirstSubReport.Key = "1aSdBBpsLbgVBOVRHdOp-QJIEcToyksxVcAfqgXV2aT4";
            return FirstSubReport;
        }

        public static GoogleSheetsKeys JawabTawzeef_RawRevenuesLastDaily()
        {
            GoogleSheetsKeys RawRevenuesLastDaily = new GoogleSheetsKeys();
            RawRevenuesLastDaily.SheetTitle = "Raw_Revenues_Last_Daily";
            RawRevenuesLastDaily.Key = "1rNbaGnwJNHld2OE--AiRwbF7P_uYN3Um0j8NtDRKhjY";
            return RawRevenuesLastDaily;
        }

        public static GoogleSheetsKeys JawabTawzeef_RawRevenuesLastMonthly()
        {
            GoogleSheetsKeys RawRevenuesLastMonthly = new GoogleSheetsKeys();
            RawRevenuesLastMonthly.SheetTitle = "Raw_Revenues_Last_Monthly";
            RawRevenuesLastMonthly.Key = "1rNbaGnwJNHld2OE--AiRwbF7P_uYN3Um0j8NtDRKhjY";
            return RawRevenuesLastMonthly;
        }

        public static GoogleSheetsKeys JawabTawzeef_RawFinalReportMonthly()
        {
            GoogleSheetsKeys RawFinalReportMonthly = new GoogleSheetsKeys();
            RawFinalReportMonthly.SheetTitle = "Raw_Final_Report_Monthly";
            RawFinalReportMonthly.Key = "1rNbaGnwJNHld2OE--AiRwbF7P_uYN3Um0j8NtDRKhjY";
            return RawFinalReportMonthly;
        }

        public static GoogleSheetsKeys JawabTawzeef_RawDailyCost()
        {
            GoogleSheetsKeys RawDailyCost = new GoogleSheetsKeys();
            RawDailyCost.SheetTitle = "Raw_daily_cost";
            RawDailyCost.Key = "1rNbaGnwJNHld2OE--AiRwbF7P_uYN3Um0j8NtDRKhjY";
            return RawDailyCost;
        }

        public static GoogleSheetsKeys JawabTawzeef_RawMonthlyClicks()
        {
            GoogleSheetsKeys RawMonthlyClicks = new GoogleSheetsKeys();
            RawMonthlyClicks.SheetTitle = "Raw_monthly_clicks";
            RawMonthlyClicks.Key = "1rNbaGnwJNHld2OE--AiRwbF7P_uYN3Um0j8NtDRKhjY";
            return RawMonthlyClicks;
        }


        public static GoogleSheetsKeys DataArifDontTouch()
        {
            GoogleSheetsKeys DataArifDontTouch = new GoogleSheetsKeys();
            DataArifDontTouch.SheetTitle = "Data-Arif-Dont-Touch";
            DataArifDontTouch.Key = "1PaxAzMmtCENLz2MT24o65NqsMidazGkwiMYs9WexeDI";
            return DataArifDontTouch;
        }




    }




}
