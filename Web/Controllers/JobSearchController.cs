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
        // GET: JobListing
        public ActionResult Index()
        {
            using (var db = new BergfallDataContext())
            {
                DateAndNumberViewModel datesAndNumbers = new DateAndNumberViewModel();
                datesAndNumbers.SearchTerms = new List<SearchTerm>();
                datesAndNumbers.SearchTerms.AddRange(db.SearchTerms);
                List<DateTime> dates = db.JobSearch.Select(d => d.RetrievedDate).Distinct().OrderBy(d => d).ToList();
                datesAndNumbers.Dates = dates;
                datesAndNumbers.DatesAndNumbers = new Dictionary<DateTime, int[]>();

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
                datesAndNumbers.GoogleChartsJavaScriptArray = DictionaryToJavaScriptArray(datesAndNumbers.DatesAndNumbers);
                return View(datesAndNumbers);
            }
        }
        string DictionaryToJavaScriptArray(Dictionary<DateTime, int[]> dict)
        {
            var entries = dict.Select(d =>
                string.Format("[{0},{1}]", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }
        public ActionResult JobSearch(string searchTerm)
        {
            //var jobSearch = new JobSearch { SearchTerm = searchTerm, NumberOfHits = 666 };
            return View();
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

    
