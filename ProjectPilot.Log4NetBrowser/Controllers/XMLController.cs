using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using ProjectPilot.Framework.Metrics;

namespace ProjectPilot.Log4NetBrowser.Controllers
{
    public class XMLController : Controller
    {
        public ActionResult SelectSolutionFile()
        {
            return View();


        }

        public ActionResult GenerateXML(string solutionFilePath)
        {
            VSSolutionLocMetrics metrics =
                new VSSolutionLocMetrics(solutionFilePath.Substring(solutionFilePath.LastIndexOf(@"\") + 1));

            metrics.LocStatsMap.AddToMap(".cs", new CSharpLocStats());
            metrics.CalculateLocForSolution(solutionFilePath);
            metrics.GenerateXmlReport(@"XML_report.xml");

            return RedirectToAction("XML", "XML");
        }

        public ActionResult XML()
        {
            return View();
        }
    }
}
