using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bergfall.Data
{
    public class JobSearch
    {
        public int Id { get; set; }
        public int SearchTermId { get; set; }
        public virtual SearchTerm SearchTerm { get; set; }
        public int NumberOfPositions { get; set; }
        public int NumberOfJobs { get; set; }
        public DateTime RetrievedDate { get; set; }
        public string Source { get; set; }

    }
}