using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bergfall.Data
{
    public class JobListing
    {
        public int Id { get; set; }
        public virtual ICollection<SearchTerm> SearchTerms { get; set; }
        public DateTime RetrievalDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Company { get; set; }
        public string URL { get; set; }
        public string Source { get; set; }
    }
}