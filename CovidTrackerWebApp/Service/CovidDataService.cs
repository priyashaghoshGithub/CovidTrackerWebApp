using CovidTrackerWebApp.Interface;
using CovidTrackerWebApp.Models;
using System.Text.Json;

namespace CovidTrackerWebApp.Service
{
    public class CovidDataService : ICovidDataService
    {
        #region Private Variables
        private readonly ILogger<CovidDataService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public CovidDataService(ILogger<CovidDataService> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        #endregion

        /// <summary>
        /// Fetches Covid-19 data from an external API.
        /// </summary>
        /// <returns>The Covid-19 data</returns>
        public async Task<CovidDataModel> GetCovidDataAsync()
        {
            CovidDataModel? covidDataResponse = null;
            try
            {
                //Get the Api Url from app settings
                var urlFromConfig = _configuration["AppSettings:CovidApiUrl"];            
                if (urlFromConfig != null)
                {
                    Uri apiUrl = new Uri(urlFromConfig);
                    var httpClient = _httpClientFactory.CreateClient();
                    //Make a GET request to the API
                    HttpResponseMessage httpResponse = await httpClient.GetAsync(apiUrl);
                    //Success Response from API
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        covidDataResponse = new CovidDataModel();
                        //Read and deserialize the JSON response
                        string responseContent = httpResponse.Content.ReadAsStringAsync().Result;
                        covidDataResponse = JsonSerializer.Deserialize<CovidDataModel>(responseContent);
                        _logger.LogInformation("Successfully fetched COVID-19 data. Status code: {0}", httpResponse.StatusCode);
                    }
                    else
                    {
                        _logger.LogError("Failed to fetch COVID-19 data. Status code: {0}", httpResponse.StatusCode);

                    }
                }
                else
                {
                    _logger.LogError("Covid API URL is not configured.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching COVID-19 data: {0}", ex.Message);
            }
            return covidDataResponse;
        }
    }
}
