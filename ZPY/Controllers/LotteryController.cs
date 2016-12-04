using ProBusiness.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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

        public ActionResult LotteryRecord()
        {
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
            var items = LotteryOrderBusiness.GetLotteryOrder("", cpcode, CurrentUser.UserID, "","","", -1, 20, 1, ref total,
                ref pageTotal);
            JsonDictionary.Add("items", items); 
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetLotteryRecord(string cpcode, int status, string lcode, string issnuenum, string type, int selfrange, string btime, string etime, int pageIndex)
        {
            int total = 0;
            int pageTotal = 0;
            var items = LotteryOrderBusiness.GetLotteryOrder("", cpcode, CurrentUser.UserID, lcode, issnuenum, type, status, PageSize, pageIndex, ref total,
                ref pageTotal, selfrange, btime,etime);
            JsonDictionary.Add("items", items);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageTotal);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetlotteryResult(string cpcode,int status=2,int pagesize=4,bool orderby=false)
        {
            int total = 0;
            int pageTotal = 0;
            var items = LotteryResultBusiness.GetPagList(cpcode, status, pagesize, 1, ref total, ref pageTotal);
            JsonDictionary.Add("item", LotteryResultBusiness.GetLotteryResult(cpcode, 0, DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"));
            JsonDictionary.Add("items", items);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddLotteryOrders(string list, int usedisFee=0)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<LotteryOrder> models = serializer.Deserialize<List<LotteryOrder>>(list);
            string msg = "";
            var result = LotteryOrderBusiness.CreateUserOrderList(models, CurrentUser, OperateIP, usedisFee, ref msg);
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("ErrMsg", msg);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
    