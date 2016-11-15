using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProBusiness;
using ProBusiness.Common;
using ProEntity.Manage;

namespace CPiao.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult UserList(string id="")
        {
            ViewBag.UserID = id;
            return View();
        }

        public ActionResult UserAdd()
        {
            var model = M_UsersBusiness.GetUserDetail(CurrentUser.UserID);
            ViewBag.Rebate = model.Rebate;
            ViewBag.UsableRebate = model.UsableRebate;
            return View();
        }
        public ActionResult UserEdit()
        {
            var model = M_UsersBusiness.GetUserDetail(CurrentUser.UserID);
            var  logmd=LogBusiness.GetLogsByUserID(CurrentUser.UserID,2);
            ViewBag.UserName = model.UserName;
            ViewBag.LastIP = model.LastLoginIP == "::1"?"127.0.0.1":model.LastLoginIP ; 
            ViewBag.LastTime = logmd.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.SafeLevel = model.SafeLevel;
            return View();
        }



        #region Ajax

        public JsonResult UserInfoList(int type, bool orderby, string username, string accountmin, string accountmax, string clumon, string rebatemin, string rebatemax, int pageIndex, int pageSize)
        {
            int total = 0;
            int pageCount = 0;
            var list = M_UsersBusiness.GetUsers(pageSize, pageIndex, ref total, ref pageCount, type, -1, username, clumon, orderby, rebatemin, rebatemax,accountmin,accountmax);
            
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public JsonResult UserAdd(int type, string username, string loginpwd, string loginname, decimal rebate)
        {
            string Errmsg = "";
            M_Users user=new M_Users()
            {
                Type = type,
                SourceType = 0, 
                UserName = username,
                LoginName = loginname,
                LoginPwd = loginpwd,
                Rebate = rebate
            };
            var result = M_UsersBusiness.CreateM_User(user, ref Errmsg,CurrentUser.UserID);
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("Errmsg", Errmsg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public JsonResult UserEdit(string username)
        {
            string Errmsg = ""; 
            var result = M_UsersBusiness.UpdateM_UserName(CurrentUser.UserID, username);
            if (result)
            {
                var model = CurrentUser;
                model.UserName = username;
                Session["Manage"] = model;
            }
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
