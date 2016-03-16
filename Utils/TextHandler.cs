using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Bergfall.Utils
{
    internal class TextHandler
    {
        private static List<string> AllAllowedTopDomains
        {
            get
            {
                //string response = HtmlHandler.MobstaWebRequest(UrlToOfficialListOfTopLevelDomains);
                string response = File.ReadAllText(@"A:\Development\BergfallUtilsOLD\allowed_domains.txt");
                return response.Split('\n').Skip(1).ToList();
            }
        }

        // STRING EXTENSIONS
        public static bool VerifyEmail(string email, StringVerifier filter)
        {
            Regex verifierRefex;

            switch (filter)
            {
                case StringVerifier.Email:
                    // First check if at least the top-level domain is correct and legal
                    string[] dotArray = email.Split('.');
                    int len = dotArray.Length;
                    string topDomain = dotArray[len - 1];
                    if (AllAllowedTopDomains.Contains(topDomain.ToUpper()))
                    {
                        //verifierRefex = new Regex(@"^((([\w!#\'$%\*\+-\/=\?\^_{}~)+)\.?)+@([a-å]((\w|!|#|$|%|\*|\+|-|\/|=|\?|\^|_|{|}|~|)*)+\.)+[a-z]{2,4}$", RegexOptions.IgnoreCase);
                        verifierRefex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$");
                        return verifierRefex.IsMatch(email);
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }

        public static bool ChangeBergfallConnectionString()
        {
            try
            {
                StreamReader sr = File.OpenText(@"G:\Development\Bergfall\web.config");
                string webConfig = sr.ReadToEnd();
                sr.Close();

                Regex activeConnectionString = new Regex(@"[^-](<add name=""LocalSqlServer"".*\n.*\n.*\n.*)");
                Regex commentedConnectionString = new Regex(@"<!--(<add name=""LocalSqlServer"".*\n.*\n.*\n.*)-->");

                Match activeMatch = activeConnectionString.Match(webConfig);
                Match commentedMatch = commentedConnectionString.Match(webConfig);

                string newCommented = "<!--" + activeMatch.Groups[1].Value + "-->";
                string newActive = commentedMatch.Groups[1].Value;

                webConfig = webConfig.Replace(activeMatch.Value, newCommented);
                webConfig = webConfig.Replace(commentedMatch.Value, newActive);

                StreamWriter sw = File.CreateText(@"G:\Development\Bergfall\web.config");
                sw.Write(webConfig);
                sw.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Dictionary<string, string> ParseEverestTextfile(string absolutePath)
        {
            Dictionary<string, string> resultKVP = new Dictionary<string, string>();

            string text = File.ReadAllText(absolutePath);

            Regex headerRegex = new Regex(@"-+?\[(?<header>[^\[]+)\]-+");
            Regex q9550Regex = new Regex(@"Q9550.*\W(?<result>\d+)\W?(?<unit>[^\n]+)");
            MatchCollection headerMatches = headerRegex.Matches(text);
            MatchCollection q9550Matches = q9550Regex.Matches(text);

            int nr = headerMatches.Count > q9550Matches.Count ? headerMatches.Count : q9550Matches.Count;
            for (int i=0 ; i < nr ; i++)
            {
                string result = "";
                if (i < q9550Matches.Count)
                    if (!String.IsNullOrEmpty(q9550Matches[i].Groups["result"].Value))
                    {
                        if (!String.IsNullOrEmpty(q9550Matches[i].Groups["unit"].Value))
                        {
                            result += q9550Matches[i].Groups["result"].Value + q9550Matches[i].Groups["unit"].Value;
                        }
                        else
                        {
                            result += q9550Matches[i].Groups["result"].Value;
                        }
                    }
                    else
                    {
                        result = "none";
                    }
                if (i < headerMatches.Count)
                {
                    if (!String.IsNullOrEmpty(headerMatches[i].Groups["header"].Value))
                    {
                        resultKVP[headerMatches[i].Groups["header"].Value] = result;
                    }
                }
                else
                {
                    resultKVP["error"] = "error";
                }
            }

            return resultKVP;
        }

        public string GetSystemReport()
        {
            System.Management.ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("select * from Win32_Processor");
            ManagementObjectCollection objects = objectSearcher.Get();
            string hum = "";
            foreach (ManagementObject o in objects)
            {
                foreach (PropertyData prop in o.Properties)
                {
                    if (!String.IsNullOrEmpty(prop.Name))
                    {
                        hum += prop.Name + "<br/>";
                    }
                    hum += o.ToString();
                    if (prop.Value != null)
                    {
                        if (!String.IsNullOrEmpty(prop.Value.ToString()))
                        {
                            hum += prop.Value.ToString() + "<br/>";
                        }
                    }
                }
            }

            return hum;
        }

        public static string MD5HashString(string text)
        {
            byte[] toHash = UnicodeEncoding.UTF8.GetBytes(text);
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            byte[] hash = md5Provider.ComputeHash(toHash);
            string hashedText = Convert.ToBase64String(hash);
            return hashedText;
        }
    }
}