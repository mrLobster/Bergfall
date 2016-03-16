using System.Data.Entity;

namespace Bergfall.Data
{
    public class BergfallDataContext : DbContext
    {
        public BergfallDataContext() : base("BergfallDataContext") { }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<JobSearch> JobSearch { get; set; }
        public DbSet<SearchTerm> SearchTerms { get; set; }
    }
}
