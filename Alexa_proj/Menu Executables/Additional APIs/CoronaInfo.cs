
using System;
using System.Collections.Generic;

namespace Alexa_proj.Additional_APIs
{
    public class CoronaInfo : ExecutableInfo
    {
        public class GlobalInfo 
        {
            public int NewConfirmed { get; set; }
            public int TotalConfirmed { get; set; }
            public int NewDeaths { get; set; }
            public int TotalDeaths { get; set; }
            public int NewRecovered { get; set; }
            public int TotalRecovered { get; set; }
        }

        public class CountryInfo
        {
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public string Slug { get; set; }
            public int NewConfirmed { get; set; }
            public int TotalConfirmed { get; set; }
            public int NewDeaths { get; set; }
            public int TotalDeaths { get; set; }
            public int NewRecovered { get; set; }
            public int TotalRecovered { get; set; }
            public DateTime Date { get; set; }
        }
        public GlobalInfo Global { get; set; }
        public List<CountryInfo> Countries { get; set; }
        public DateTime Date { get; set; }
    }
}
