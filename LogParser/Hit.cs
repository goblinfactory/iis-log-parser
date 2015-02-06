using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO : I will setup the IIS log parser to automatically work out which fields, and in which order to parse lines based on the fields selected. For now, assuming all fields selected, in the following order.
// TODO : design a means to "watch" the tail in realtime and keep the totals and queries up to date without having to requery all the logfiles.

namespace Goblinfactory.LogParser
{
    /// <summary>
    /// Hit class to hold all the fixed length (short) data about a hit. Assumes you've selected *ALL* the IIS fields in custom logging.
    /// </summary>
    /// <remarks>Allows you to easily .Dump() out the result of a query in linqpad or bind to a datagrid without having to do another select statement to exclude long text fields that would make eyeballing outputs difficult.</remarks>
    public class Hit : IHit
    {
        // description of the different fields at link below.
        // http://www.microsoft.com/technet/prodtechnol/WindowsServer2003/Library/IIS/676400bc-8969-4aa7-851a-9319490a9bbb.mspx?mfr=true
        //#Fields: date time s-sitename s-computername s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs-version cs(User-Agent) cs(Cookie) cs(Referer) cs-host sc-status sc-substatus sc-win32-status sc-bytes cs-bytes time-taken
        public DateTime Date { get; set; } 		// 0 + 1		2015-02-05 08:01:02 
        public string SiteName { get; set; } 	// 2 			W3SVC8                          (The Internet service name and instance number that was running on the client.)
        public string ServerName { get; set; } 	// 3			WIN-N29LAC9AE6L 
        public string ServerIP { get; set; } 	// 4			46.20.114.238 
        public string Method { get; set; } 		// 5			GET 
        public string UriStem { get; set; } 	// 6			/
        public string Resource { get; set; }    // 6(b)         (filename and extension)
        public string UriQuery { get; set; } 	// 7			-
        public string Port { get; set; } 		// 8			80
        public string UserName { get; set; }    // 9            -
        public string ClientIP { get; set; } 	// 10			149.254.250.146 
        public string CSVersion { get; set; }   // 11           HTTP/1.1 
        public string UserAgent { get; set; }   // 12           Mozilla/5.0+(Macintosh;+Intel+Mac+OS+X+10_9_5)+AppleWebKit/537.36+(KHTML,+like+Gecko)+Chrome/40.0.2214.94+Safari/537.36
        public string Cookie { get; set; }      // 13           - 
        public string Referrer { get; set; }    // 14           - 
        public string HostHeaderName { get; set; } 	// 15			www.goblinfactory.co.uk 
        public int ScStatus { get; set; } 		// 16			200
        public int ScSubstatus { get; set; } 	// 17			0
        public int ScWin32Status { get; set; }	// 18			0
        public int BytesSent { get; set; } 		// 19			16133 
        public int BytesReceived { get; set; } 	// 20			387 
        public int TimeTaken { get; set; } 		// 21			1279

        public bool ParseError { get; set; }
        public string RawLine { get; set; }
        public string ErrorMessage { get; set; }

        private static int GetInt(string number)
        {
            int num = 0;
            int.TryParse(number, out num);
            return num;
        }

        private static string GetResource(string uristem)
        {
            if (string.IsNullOrWhiteSpace(uristem)) return "";
            var parts = uristem.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            return !parts.Any() ? "" : parts.Last();
        }

        public static Hit Create(string line)
        {
            var items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            return new Hit(items, line);
        }

        public Hit(string[] items, string rawline)
        {
            int item = 0;
            try
            {
                ParseError = false;
                Date = DateTime.Parse(items[item++] + " " + items[item++]);
                SiteName = items[item++]; //Site(items[2]); lookup the domain name from a map of w3x to domain name...
                ServerName = items[item++];
                ServerIP = items[item++];
                Method = items[item++];
                UriStem = items[item++];
                UriQuery = items[item++];
                Port = items[item++];
                UserName = items[item++];
                ClientIP = items[item++];
                CSVersion = items[item++];
                UserAgent = items[item++];
                Cookie = items[item++];
                Referrer = items[item++];
                HostHeaderName = items[item++];
                ScStatus = GetInt(items[item++]);
                ScSubstatus = GetInt(items[item++]);
                ScWin32Status = GetInt(items[item++]);
                BytesSent = GetInt(items[item++]);
                BytesReceived = GetInt(items[item++]);
                TimeTaken = GetInt(items[item++]);
                Resource = GetResource(UriStem);
            }
            catch (Exception ex)
            {
                ParseError = true;
                RawLine = rawline;
                Console.WriteLine("{0}", item);
                ErrorMessage = string.Format("Error parsing item({0}) -> '{1}' item:{2}", item, ex.Message, items[item]);
            }

        }
    }


}
