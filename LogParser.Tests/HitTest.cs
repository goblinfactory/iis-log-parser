using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Goblinfactory.LogParser;
using NUnit.Framework;
using ObjectPrinter;

namespace LogParser.Tests
{
    [TestFixture]
    public class HitTest
    {
        [Test]
        public void ParseValidLoglineShouldParseWithoutError()
        {
            // #Fields: date time s-sitename s-computername s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs-version cs(User-Agent) cs(Cookie) cs(Referer) cs-host sc-status sc-substatus sc-win32-status sc-bytes cs-bytes time-taken
            var line = @"2015-02-05 08:01:02 W3SVC8 DEV-X41LBD3AE6F 100.100.100.100 GET / - 80 - 66.66.66.66 HTTP/1.1 Mozilla/5.0+(Macintosh;+Intel+Mac+OS+X+10_9_5)+AppleWebKit/537.36+(KHTML,+like+Gecko)+Chrome/40.0.2214.94+Safari/537.36 - - www.goblinfactory.co.uk 200 0 0 16133 387 1279";
            var hit = Hit.Create(line);
            hit.ParseError.Should().BeFalse(hit.ErrorMessage);
        }

        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void ParseValidLoglineShouldParseAllFieldsCorrectly()
        {
            // #Fields: date time s-sitename s-computername s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs-version cs(User-Agent) cs(Cookie) cs(Referer) cs-host sc-status sc-substatus sc-win32-status sc-bytes cs-bytes time-taken
            var line = @"2015-02-05 08:01:02 W3SVC8 DEV-X41LBD3AE6F 100.100.100.100 GET / - 80 - 66.66.66.66 HTTP/1.1 Mozilla/5.0+(Macintosh;+Intel+Mac+OS+X+10_9_5)+AppleWebKit/537.36+(KHTML,+like+Gecko)+Chrome/40.0.2214.94+Safari/537.36 cookie Referer www.goblinfactory.co.uk 200 0 0 16133 387 1279";
            var hit = Hit.Create(line);
            hit.ParseError.Should().BeFalse(hit.ErrorMessage);
            Approvals.Verify(hit.Dump());
        }

    }
}
