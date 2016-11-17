using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proc.Controllers;
using ProBusiness.Manage;
using ProEntity;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Schema;
using ProBusiness;
using ProEnum;
using ProTools;

namespace Proc1.Controllers
{
    public class WebSetController : BaseController
    {
        //
        // GET: /WebSet/

        public ActionResult Member()
        {
            return View();
        }
        public ActionResult Advert()
        {
            ViewBag.Url = Common.GetKeyValue("WebUrl");
            return View();
        }

        public ActionResult ChargeSet()
        {
            return View();
        }

        public ActionResult Activity()
        {
            ViewBag.integerFee = CommonBusiness.getSysSetting(EnumSettingKey.GoldScale, "DValue");
            return View();
        }

        #region Ajax 




        public JsonResult SaveGoldRule(int integerFee)
        {
            var result = false;
            JsonDictionary.Add("result", CommonBusiness.SetSysSetting(EnumSettingKey.GoldScale,integerFee,CurrentUser.UserID));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }

        public JsonResult GetChargeList(int status, string keywords, string userID, string beginTime,
            string endTime, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            int pageCount = 0;
            var result = WebSetBusiness.GetChargeSet(keywords, userID, status, pageSize, pageIndex, ref totalCount, ref pageCount, beginTime, endTime);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            JsonDictionary.Add("items", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }

        public JsonResult SaveCharge(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ChargeSet model = serializer.Deserialize<ChargeSet>(entity);
            var result = false;
            if (model.AutoID == -1)
            {
                model.UserID = CurrentUser.UserID;
                result = WebSetBusiness.InsertChargeSet(model);
            }
            else
            {
                result = WebSetBusiness.UpdateChargeSet(model);
            }
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdateChargeStatus(int autoid,int status)
        { 
            var result = false;
            result = WebSetBusiness.UpdateChargeSetStatus(autoid,status); 
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion

    }
}
