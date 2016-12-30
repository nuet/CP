using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPiao.Controllers
{
    [CPiao.Common.UserAuthorize]
    public class ReportController : BaseController
    {
        //
        // GET: /Report/

        public ActionResult ReportList()
        {
            return View();
        }

        public ActionResult ReportSee()
        {
            return View();
        }

        public ActionResult ReportToday()
        {
            return View();
        }

        public ActionResult ReportProfit()
        {
            return View();
        }
    }
}
