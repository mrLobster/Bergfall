using Bergfall.Data;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Bergfall.Web.Controllers
{
    public class BackendController : Controller
    {
       
            
            public void GetFinnArticles()
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
        }
    }
