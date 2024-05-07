namespace CovidTrackerWebApp.Models
{
    public class CovidDataModel 
    {
        public long updated { get; set; }
        public int cases { get; set; }
        public int deaths { get; set; }
        public int recovered { get; set; }
        public int active { get; set; }
        public long tests { get; set; }
        public long population { get; set; }       
        public int affectedCountries { get; set; }

    }
}
