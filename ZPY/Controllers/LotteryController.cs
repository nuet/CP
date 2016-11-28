using ProBusiness.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            JsonDictionary.Add("items","");
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
