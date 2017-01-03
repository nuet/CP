using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProBusiness.UserAttrs;

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
        #region Ajax

        public JsonResult GetUserReport(string btime, string etime, int pageIndex)
        {
            int total = 0;
            int pageTotal = 0;
            var list = UserReportBussiness.GetReportList(btime, etime, CurrentUser.UserID, pageIndex, PageSize,
                ref total, ref pageTotal);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageTotal);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion
    }
}
