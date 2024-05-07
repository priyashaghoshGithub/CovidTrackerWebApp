using CovidTrackerWebApp.Interface;
using CovidTrackerWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CovidTrackerWebApp.Controllers
{
    public class HomeController : Controller
    {
        #region Private Variables
        private readonly ILogger<HomeController> _logger;
        private readonly ICovidDataService _covidDataService;
        #endregion

        #region Constructor
        public HomeController(ILogger<HomeController> logger, ICovidDataService covidDataService)
        {
            _logger = logger;
            _covidDataService = covidDataService;
        }
        #endregion
        /// <summary>
        /// Action Method to fetch and display Covid-19 data
        /// </summary>
        /// <returns>The view with Covid-19 data</returns>
        public async Task<IActionResult> Index()
        { 
            //Fetch Covid-19 data using the service
            var covidData = await _covidDataService.GetCovidDataAsync();
            if (covidData != null)
            {
                //Pass the data to the view
                _logger.LogInformation("Success Response COVID-19 data");
                return View(covidData);
            }
            else
            {
                _logger.LogInformation("Failed to fetch COVID-19 data");
                ViewBag.Error = "Failed to fetch COVID-19 data.";
                return View();

            }

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
