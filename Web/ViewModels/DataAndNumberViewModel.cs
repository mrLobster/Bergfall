using Bergfall.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bergfall.Web.ViewModels
{
    public class DateAndNumberViewModel
    {
        public List<SearchTerm> SearchTerms { get; set; }
        public List<DateTime> Dates { get; set; }
        public Dictionary<DateTime, int[]> DatesAndNumbers { get; set; }
        public string GoogleChartsJavaScriptArray { get; set; }
    }
    
}