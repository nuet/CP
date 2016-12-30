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
    [CPiao.Common.UserAuthorize]
    public class LotteryController : BaseController
    {
        //
        // GET: /Lottery/

        public ActionResult Index(string id)
        {
            ViewBag.CPCode = id;
            ViewBag.Model = WebSetBusiness.GetLotteryDetail(id);
            if (id == "TJSSC" || id == "XJSSC" || id == "HLJSSC")
            {
                return RedirectToAction("HighLottery", "Lottery", new {id = id});
            }
            else if (id == "SHSSL" || id == "FC3D")
            {
                return RedirectToAction("SSC3D", "Lottery", new { id = id });
            }
            return View();
        }

        public ActionResult HighLottery(string id)
        {
            ViewBag.CPCode = id;
            ViewBag.Model = WebSetBusiness.GetLotteryDetail(id);
            return View();
        }
        public ActionResult SSC3D(string id)
        {
            ViewBag.CPCode = id;
            ViewBag.Model = WebSetBusiness.GetLotteryDetail(id);
            return View();
        }

        public ActionResult LotteryRecord()
        {
            return View();
        }

        public ActionResult BettAutoRecord()
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
            var items = LotteryOrderBusiness.GetLotteryOrder("", cpcode, CurrentUser.UserID, "","","", -1,-1, 20, 1, ref total,
                ref pageTotal, 0, DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd"));
            JsonDictionary.Add("items", items); 
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetLotteryRecord(string cpcode, int status, string lcode, string issuenum, string type, int selfrange,int winType, string btime, string etime, int pageIndex)
        {
            int total = 0;
            int pageTotal = 0;
            var items = LotteryOrderBusiness.GetLotteryOrder("", cpcode, CurrentUser.UserID, lcode, issuenum, type, status, winType, PageSize, pageIndex, ref total,
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
        public JsonResult GetlotteryResult(string cpcode,int status=2,int pagesize=4,bool orderby=false,string btime="",string etime="")
        {
            int total = 0;
            int pageTotal = 0;

            var items = LotteryResultBusiness.GetPagList(cpcode, status, orderby, pagesize, 1, ref total, ref pageTotal, btime,etime);
            JsonDictionary.Add("item",
                LotteryResultBusiness.GetLotteryResult(cpcode, 0, DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00",
                    DateTime.Now.ToString("yyyy-MM-dd")));
            JsonDictionary.Add("items", items);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetBettAutoRecord(string cpcode, int status, string lcode, string issuenum, string type, int selfrange, int winType, string btime, string etime, int pageIndex)
        {
            int total = 0;
            int pageTotal = 0;
            var items = LotteryOrderBusiness.GetBettAutoRecord("", cpcode, CurrentUser.UserID, lcode, issuenum, type, status, winType, PageSize, pageIndex, ref total,
                ref pageTotal, selfrange, btime, etime);
            JsonDictionary.Add("items", items);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageTotal);
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
        public JsonResult AddLotteryBett(string list, int isStart = 0)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<LotteryBettAuto> models = serializer.Deserialize<List<LotteryBettAuto>>(list);
            string msg = "";
            var result = LotteryOrderBusiness.CreateBettOrderList(models, CurrentUser, OperateIP, isStart, ref msg);
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
    