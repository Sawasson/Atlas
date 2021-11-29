using HawiyyahGenerator;
using Jawabkom_Generator3;
using Jawabkom_Generator3.Core;
using Jawabsale_Generator;
using JawabTawzeef;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Webside.Models;
using MongoDB;
using MongoHelper = MongoDB.MongoHelper;
using JawabTawzeef.Core;
using NewLTVSameMonth = JawabTawzeef.Core.NewLTVSameMonth;

namespace Webside.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Jawabkom_RawCurrency()
        {
            var list = await MongoHelper.GetAll<Jawabkom_Generator3.Core.Currency>("RawCurrency");
                
            return View("Jawabkom/RawCurrency", list);
        }

        public async Task<ActionResult> Jawabkom_RawRevenuesLastMonthly()
        {
            var list = await MongoHelper.GetAll<Jawabkom_Generator3.Core.RawRevenuesLastMonthly>("Jawabkom_RawRevenuesLastMonthly");
            return View("Jawabkom/RawRevenuesLastMonthly", list);
        }

        public async Task<ActionResult> Jawabkom_RawFinalReportMonthly()
        {
            var list = await MongoHelper.GetAll<Jawabkom_Generator3.Core.RawFinalReportMonthly>("Jawabkom_RawFinalReportMonthly");

            return View("Jawabkom/RawFinalReportMonthly", list);
        }

        public async Task<ActionResult> Jawabkom_RawDailyCost()
        {
            var list = await MongoHelper.GetAll<Jawabkom_Generator3.Core.RawDailyCost>("Jawabkom_RawDailyCost");

            return View("Jawabkom/RawDailyCost", list);
        }

        public async Task<ActionResult> Jawabkom_RawMonthlyClicks()
        {
            var list = await MongoHelper.GetAll<Jawabkom_Generator3.Core.RawMonthlyClicks>("Jawabkom_RawMonthlyClicks");

            return View("Jawabkom/RawMonthlyClicks", list);
        }


        public async Task<ActionResult> Jawabsale_RevenuesLast()
        {
            var list = await MongoHelper.GetAll<Jawabsale_Generator.Core.RevenuesLast>("Jawabsale_RevenuesLast");

            return View("Jawabsale/RevenuesLast", list);
        }

        public async Task<ActionResult> Jawabsale_NewLTVSamemonth()
        {
            var list = await MongoHelper.GetAll<Jawabsale_Generator.Core.NewLTVSameMonth>("Jawabsale_NewLTVSameMonth");

            return View("Jawabsale/NewLTVSamemonth", list);
        }

        public async Task<ActionResult> Jawabsale_FirstSubReport()
        {
            var list = await MongoHelper.GetAll<Jawabsale_Generator.Core.FirstSubReport>("Jawabsale_FirstSubReport");

            return View("Jawabsale/FirstSubReport", list);
        }

        public async Task<ActionResult> Jawabsale_LTVModels()
        {
            var list = await MongoHelper.GetAll<Jawabsale_Generator.Core.LTVModels>("Jawabsale_LTVModels");

            return View("Jawabsale/LTVModels", list);
        }

        public async Task<ActionResult> Hawiyyah_RevenuesLastALL()
        {
            var list = await Hawiyyah.RevenuesLast();

            return View("Hawiyyah/RevenuesLastALL", list.Item1);
        }

        public async Task<ActionResult> Hawiyyah_RevenuesLastHawiyyah()
        {
            var list = await Hawiyyah.RevenuesLast();

            return View("Hawiyyah/RevenuesLastALL", list.Item3);
        }

        public async Task<ActionResult> Hawiyyah_RevenuesLastPeopleReveal()
        {
            var list = await Hawiyyah.RevenuesLast();

            return View("Hawiyyah/RevenuesLastALL", list.Item2);
        }

        public async Task<ActionResult> Hawiyyah_NewLTVSamemonth()
        {
            var list = await Hawiyyah.NewLTVSameMonth();

            return View("Hawiyyah/NewLTVSamemonth", list.Item1);
        }

        public async Task<ActionResult> Hawiyyah_FirstSubReportALL()
        {
            var lists = await Hawiyyah.NewLTVSameMonth();
            var list = await Hawiyyah.FirstSubReport(lists);
            ViewBag.Title = "ALL";
            return View("Hawiyyah/FirstSubReportALL", list.Item1);
        }

        public async Task<ActionResult> Hawiyyah_FirstSubReportHawiyyah()
        {
            var lists = await Hawiyyah.NewLTVSameMonth();
            var list = await Hawiyyah.FirstSubReport(lists);
            ViewBag.Title = "Hawiyyah";
            return View("Hawiyyah/FirstSubReportALL", list.Item2);
        }

        public async Task<ActionResult> Hawiyyah_FirstSubReportPeopleReveal()
        {
            var lists = await Hawiyyah.NewLTVSameMonth();
            var list = await Hawiyyah.FirstSubReport(lists);
            ViewBag.Title = "PeopleReveal";
            return View("Hawiyyah/FirstSubReportALL", list.Item3);
        }

        public async Task<ActionResult> JawabTawzeef_RawRevenuesLastDaily()
        {
            var list = await MongoHelper.GetAll<RawRevenuesLastDaily>("JawabTawzeef_RawRevenuesLastDaily");

            return View("JawabTawzeef/RawRevenuesLastDaily", list);
        }

        public async Task<ActionResult> JawabTawzeef_RawRevenuesLastMonthly()
        {
            var list = await MongoHelper.GetAll<JawabTawzeef.Core.RawRevenuesLastMonthly>("JawabTawzeef_RawRevenuesLastMonthly");

            return View("JawabTawzeef/RawRevenuesLastMonthly", list);
        }

        public async Task<ActionResult> JawabTawzeef_New_LTV_SAMEMONTH()
        {
            var list = await MongoHelper.GetAll<NewLTVSameMonth>("JawabTawzeef_NewLTVSamemonth");

            return View("JawabTawzeef/NewLTVSamemonth", list);
        }

        public async Task<ActionResult> JawabTawzeef_RawFinalReportMonthly()
        {
            var list = await MongoHelper.GetAll<JawabTawzeef.Core.RawFinalReportMonthly>("JawabTawzeef_RawFinalReportMonthly");

            return View("JawabTawzeef/RawFinalReportMonthly", list);
        }

        public async Task<ActionResult> JawabTawzeef_RawDailyCost()
        {
            var list = await MongoHelper.GetAll<JawabTawzeef.Core.RawDailyCost>("JawabTawzeef_RawDailyCost");

            return View("JawabTawzeef/RawDailyCost", list);
        }

        public async Task<ActionResult> JawabTawzeef_RawMonthlyClicks()
        {
            var list = await MongoHelper.GetAll<JawabTawzeef.Core.RawMonthlyClicks>("JawabTawzeef_RawMonthlyClicks");

            return View("JawabTawzeef/RawMonthlyClicks", list);
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
