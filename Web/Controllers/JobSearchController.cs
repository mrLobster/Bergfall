using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Bergfall.Web.ViewModels;
using Bergfall.Data;
using Bergfall.Utils;

namespace Bergfall.Web.Controllers
{
    public class JobSearchController : Controller
    {
        string[] SearchItems = new string[] { "C#", "Java", "JavaScript", "Python" };
        string FinnSearchURL = "http://m.finn.no/job/fulltime/search.html?sort=1&rows=400&q=";
        Regex justNumbersRegex = new Regex(@"^(\d+)$");

        // GET: JobListing
        public ActionResult Index()
        {

            GetFinnNumbers();

            using (var db = new BergfallDataContext())
            {
                DateAndNumberViewModel datesAndNumbers = new DateAndNumberViewModel();
                datesAndNumbers.SearchTerms = new List<SearchTerm>();
                datesAndNumbers.SearchTerms.AddRange(db.SearchTerms);
                List<DateTime> dates = db.JobSearch.Select(d => d.RetrievedDate).Distinct().OrderBy(d => d).ToList();
                datesAndNumbers.Dates = dates;

                var list = db.JobSearch.OrderBy(j => j.RetrievedDate).ThenBy(j => j.SearchTerm).ToList();
                var nrOfSearchTerms = db.SearchTerms.Count();

                foreach (DateTime currDateTime in dates)
                {
                    int[] numbers = new int[nrOfSearchTerms];

                    for (int i = 0; i < nrOfSearchTerms; i++)
                    {
                        var sTerm = datesAndNumbers.SearchTerms[i].Value;
                        JobSearch jobSearch = db.JobSearch.Where(job => job.SearchTerm.Value == sTerm && job.RetrievedDate == currDateTime).FirstOrDefault();
                        numbers[i] = jobSearch != null ? jobSearch.NumberOfJobs : 0;
                    }
                    datesAndNumbers.DatesAndNumbers.Add(currDateTime, numbers);
                }
                return View(datesAndNumbers);
            }
        }
        public ActionResult JobSearch(string searchTerm)
        {
            //var jobSearch = new JobSearch { SearchTerm = searchTerm, NumberOfHits = 666 };
            return View();
        }
        public void GetFinnNumbers()
        {
            string FinnSearchURL = "http://m.finn.no/job/fulltime/search.html?sort=1&rows=400&q=";
            var htmlWeb = new HtmlWeb();

            using (var db = new BergfallDataContext())
            {
                if (db.JobSearch.Count(j => j.RetrievedDate == DateTime.Today) > 0)
                {
                    return;
                }
                foreach (var searchItem in db.SearchTerms)
                {
                    var jobSearch = new JobSearch();
                    jobSearch.SearchTerm = searchItem;

                    string url = FinnSearchURL + WebUtility.UrlEncode(searchItem.Value);

                    var doc = htmlWeb.Load(url);
                    var resultNode = doc.DocumentNode.SelectNodes("//b[@data-count]");
                    var jobCount = int.Parse(resultNode[0].InnerText.Trim());
                    var jobListingsCount = int.Parse(resultNode[1].InnerText.Trim());

                    jobSearch.NumberOfJobs = jobListingsCount;
                    jobSearch.NumberOfPositions = jobCount;
                    jobSearch.Source = "url";
                    jobSearch.RetrievedDate = DateTime.Today;
                    db.JobSearch.Add(jobSearch);

                    //var jobNodes = doc.DocumentNode.GetNodesContainingAttributeValue("div", "class", "result-item");

                    //foreach (var jobNode in jobNodes)
                    //{
                    //    var applicationDate = jobNode.GetNodeContainingAttributeValue("span", "class", "fleft").InnerText.Trim();

                    //    var title = jobNode.GetNodeContainingAttributeValue("span", "class", "prm").InnerText.Trim();

                    //    var listingURL = "m.finn.no" + jobNode.SelectSingleNode(".//a").Attributes["href"].Value;

                    //}
                }
                db.SaveChanges();
            }
        }
        //public ActionResult DatesAndNumbers(int? id)
        //{


        //    using (var db = new BergfallDataContext())
        //    {
        //        var searchTerm = db.SearchTerms.Find(id);
        //        DateAndNumberViewModel datesAndNumbers = new DateAndNumberViewModel() { Search = searchTerm.Value };
        //        var res = db.JobSearch.Where(j => j.SearchTerm.Value.Trim().ToLower() == searchTerm.Value.Trim().ToLower()).ToList();

        //        foreach (var item in res)
        //        {

        //            datesAndNumbers.DatesAndNumbers[item.RetrievedDate] = item;

        //        }


        //        return View(datesAndNumbers);
        //    }
        //}

        
           
        

            }
        }

    
