namespace CovidTrackerWebApp.Models
{
    public class CovidDataModel 
    {
        public long updated { get; set; }
        public int cases { get; set; }
        public int todayCases { get; set; }
        public int deaths { get; set; }
        public int todayDeaths { get; set; }
        public int recovered { get; set; }
        public int todayRecovered { get; set; }
        public int active { get; set; }
        public int critical { get; set; }
        public int casesPerOneMillion { get; set; }
        public double deathsPerOneMillion { get; set; }
        public long tests { get; set; }
        public double testsPerOneMillion { get; set; }
        public long population { get; set; }
        public int oneCasePerPeople { get; set; }
        public int oneDeathPerPeople { get; set; }
        public int oneTestPerPeople { get; set; }
        public double activePerOneMillion { get; set; }
        public double recoveredPerOneMillion { get; set; }
        public double criticalPerOneMillion { get; set; }
        public int affectedCountries { get; set; }

    }
}
