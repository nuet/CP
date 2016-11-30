using ProBusiness.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProBusiness;
using ProEntity;

namespace CPiao.Controllers
{
    public class LotteryController : BaseController
    {
        //
        // GET: /Lottery/

        public ActionResult Index(string id)
        {
            ViewBag.CPCode = id;
            ViewBag.Model = WebSetBusiness.GetLotteryDetail(id); 
            return View();
        }

        #region MyRegion 

        #endregion
        public JsonResult GetNavsList(string cpcode)
        {
            List<Plays> model = WebSetBusiness.GetLotteryPlaysesByCode(cpcode);
            model.ForEach(x =>
            { 
                if (x.Layer == 2)
                {
                    List<Plays> templay = model.Where(y => y.PIDS == (x.PIDS + "_" + y.PCode) && y.Layer == 3).ToList();
                    x.ChildPlays = templay;
                }
            });
            model.ForEach(x =>
            { 
                if (x.Layer == 1)
                {
                    List<Plays> templay = model.Where(y => y.PIDS == (x.PIDS + "_" + y.PCode) && y.Layer == 2).ToList();
                    x.ChildPlays = templay;
                }
            });
            JsonDictionary.Add("items", model.Where(x=>x.Layer==1).ToList());
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUserLottery(string cpcode)
        {
            int total = 0;
            int pageTotal = 0;
            var items = LotteryOrderBusiness.GetUserOrders("", cpcode, CurrentUser.UserID, "", -1, 5, 1, ref total,
                ref pageTotal, "", "");
            JsonDictionary.Add("items","");
            JsonDictionary.Add("items","");
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetlotteryResult(string cpcode)
        {
            int total = 0;
            int pageTotal = 0;
            var items=LotteryResultBusiness.GetPagList(cpcode, 2, 4, 1, ref total, ref pageTotal);
            JsonDictionary.Add("item", LotteryResultBusiness.GetLotteryResult(cpcode, 0, DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"));
            JsonDictionary.Add("items", items);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddLotteryOrders(string list)
        {

        }

    }
}
    