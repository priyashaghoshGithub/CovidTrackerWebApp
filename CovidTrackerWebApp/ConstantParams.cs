using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CovidTrackerWebApp
{
    public static class ConstantParams
    {

        public const string Api_Url = "AppSettings:CovidApiUrl";
        public const string SucessMessagefromHttpResponse = "Successfully fetched COVID-19 data.Status code: {0}";
        public const string FailureMessagefromHttpResponse = "Failed to fetch COVID-19 data.Status code: {0}";
        public const string ApiUrlNotConfigured = "Covid API URL is not configured.";
        public const string ErrorfromHttpResponse = "An error occurred while fetching COVID-19 data: {0}";
        public const string SuccessResponse = "Success Reponse from Covid API";
        public const string FailureResponse = "Failed to fetch COVID-19 data";
    }
}
