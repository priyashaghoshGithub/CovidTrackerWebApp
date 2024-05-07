using CovidTrackerWebApp.Models;

namespace CovidTrackerWebApp.Interface
{
    public interface ICovidDataService
    {
        Task<CovidDataModel> GetCovidDataAsync();
    }
}
