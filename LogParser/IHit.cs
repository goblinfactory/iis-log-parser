using System;

namespace Goblinfactory.LogParser
{
    public interface IHit
    {
        DateTime Date { get; set; }         
        string SiteName { get; set; }
        string ServerName { get; set; }
        string ServerIP { get; set; }
        string Method { get; set; }
        string UriStem { get; set; }
        string UriQuery { get; set; }
        string Port { get; set; }
        string UserName { get; set; }  
        string ClientIP { get; set; }
        string Resource { get; set; }
        int ScStatus { get; set; }
        int ScSubstatus { get; set; }
        int ScWin32Status { get; set; }
        int BytesSent { get; set; }
        int BytesReceived { get; set; }
        int TimeTaken { get; set; }
        string HostHeaderName { get; set; }

        string CSVersion { get; set; }
        string UserAgent { get; set; }
        string Cookie { get; set; }
        string Referrer { get; set; }
        string RawLine { get; set; }
        string ErrorMessage { get; set; }
    
    
    }
}