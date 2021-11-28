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
            var list = await MongoHelper.GetAll<Currency>("RawCurrency");
            return View("Jawabkom/RawCurrency", list);
        }

        public async Task<ActionResult> Jawabkom_RawRevenuesLastMonthly()
        {
            var list = await MongoHelper.GetAll<RawRevenuesLastMonthly>("Jawabkom_RawRevenuesLastMonthly");
            return View("Jawabkom/RawRevenuesLastMonthly", list);
        }

        public async Task<ActionResult> Jawabkom_RawFinalReportMonthly()
        {
            var list = await MongoHelper.GetAll<RawFinalReportMonthly>("Jawabkom_RawFinalReportMonthly");

            return View("Jawabkom/RawFinalReportMonthly", list);
        }

        public async Task<ActionResult> Jawabkom_RawDailyCost()
        {
            var list = await MongoHelper.GetAll<RawDailyCost>("Jawabkom_RawDailyCost");

            return View("Jawabkom/RawDailyCost", list);
        }

        public async Task<ActionResult> Jawabkom_RawMonthlyClicks()
        {
            var list = await MongoHelper.GetAll<RawMonthlyClicks>("Jawabkom_RawMonthlyClicks");

            return View("Jawabkom/RawMonthlyClicks", list);
        }


        public async Task<ActionResult> Jawabsale_RevenuesLast()
        {
            var list = await Jawabsale.RevenuesLast();

            return View("Jawabsale/RevenuesLast", list);
        }

        public async Task<ActionResult> Jawabsale_NewLTVSamemonth()
        {
            var list = await Jawabsale.NewLTVSameMonth();

            return View("Jawabsale/NewLTVSamemonth", list.Item1);
        }

        public async Task<ActionResult> Jawabsale_FirstSubReport()
        {
            var lists = await Jawabsale.NewLTVSameMonth();
            var list = await Jawabsale.FirstSubReport(lists);

            return View("Jawabsale/FirstSubReport", list.Item2);
        }

        public async Task<ActionResult> Jawabsale_LTVMODELS()
        {
            var lists = await Jawabsale.NewLTVSameMonth();
            var lists2 = await Jawabsale.FirstSubReport(lists);
            var list = await Jawabsale.LTVModels(lists2);

            return View("Jawabsale/LTVMODELS", list);
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
            var list = await JawabTawzeef.JawabTawzeef.RawRevenuesLastDaily();

            ViewBag.Title = "Daily";

            return View("JawabTawzeef/RawRevenuesLast", list);
        }

        public async Task<ActionResult> JawabTawzeef_RawRevenuesLastMonthly()
        {
            var lists = await JawabTawzeef.JawabTawzeef.RawRevenuesLastDaily();

            var list = await JawabTawzeef.JawabTawzeef.RawRevenuesLastMonthly(lists);

            ViewBag.Title = "Monthly";

            return View("JawabTawzeef/RawRevenuesLast", list);
        }

        public async Task<ActionResult> JawabTawzeef_NewLTVSamemonth()
        {
            var list = await JawabTawzeef.JawabTawzeef.New_LTV_SAMEMONTH();

            return View("JawabTawzeef/NewLTVSamemonth", list.Item1);
        }

        public async Task<ActionResult> JawabTawzeef_RawFinalReportMonthly()
        {
            var list2 = await JawabTawzeef.JawabTawzeef.New_LTV_SAMEMONTH();
            var list = await JawabTawzeef.JawabTawzeef.RawFinalReportMonthly(list2);
            return View("JawabTawzeef/RawFinalReportMonthly", list);
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
