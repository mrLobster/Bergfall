using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bergfall.Utils
{
    internal class TorrentSearch
    {
        public List<TorrentQuery> SearchPirateBay(string query)
        {
            List<TorrentQuery> queries = new List<TorrentQuery>();
            int hits = 0;
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            string response = Html.MobstaWebRequest("http://thepiratebay.org/search/" + query + "/0/7/0");
            Regex regex = new Regex("<tr(\\s|>).*?class=\"detLink\"[^>]*>([^<]*)</a></td>.*?<td>([^<]*)</td>.*?<td><a[^>]*?href=\"([^\"]*?)\"[^>]*?>.*?<td\\salign=\"right\">([^<]*?)</td>.*?<td\\salign=\"right\">([^<]*?)</td>.*?<td\\salign=\"right\">([^<]*?)</td>.*?</tr>", RegexOptions.Singleline);
            Regex titleRegex = new Regex("title=\"([^\"]*)\"");
            Regex hrefRegex = new Regex("href=\"([^\"]*)\"");
            Dictionary<string, string> hitDictionary = new Dictionary<string, string>();
            DateTime uploadDate;
            foreach (Match match in regex.Matches(response))
            {
                hits++;
                try
                {
                    TorrentQuery searchHit = new TorrentQuery
                    {
                        Title = match.Groups[2].Value,
                        Url = match.Groups[4].Value,
                        Size = match.Groups[5].Value,
                        Seeds = Int32.Parse(match.Groups[6].Value),
                        Leechers = Int32.Parse(match.Groups[7].Value)
                    };

                    string uploadDateString = match.Groups[3].Value;
                    if (uploadDateString.Contains(':'))
                    {
                        if (uploadDateString.Contains("Y-day"))
                        {
                            uploadDate = DateTime.Today.AddDays(-1);
                        }
                        else if (uploadDateString.Contains("Today"))
                        {
                            uploadDate = DateTime.Today;
                        }
                        else
                        {
                            uploadDate = DateTime.ParseExact(uploadDateString, "MM-dd HH-mm", currentCulture);
                        }
                    }
                    queries.Add(searchHit);
                }
                catch { }
            }
            return queries;
        }
    }
}