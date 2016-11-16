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
            ViewBag.Layers = CurrentUser.Layers;
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

        public JsonResult UserInfoList(int type, bool orderby, string username, string userid, string accountmin, string accountmax, string clumon, string rebatemin, string rebatemax, int pageIndex, int pageSize, bool mytype=false)
        {
            int total = 0;
            int pageCount = 0;
            var list = M_UsersBusiness.GetUsersRelationList(pageSize, pageIndex, ref total, ref pageCount, string.IsNullOrEmpty(userid)?CurrentUser.UserID:userid, type, -1, username, clumon, orderby, rebatemin, rebatemax, accountmin, accountmax, mytype);
            
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
                Description="用户新增",
                Rebate = rebate,
                RoleID=(type==1?"dd87ca0a-b425-4e1e-b7ec-7a1e02dad0f8":"48eb0491-d92c-4664-ab27-37320ac7de38")
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

        [HttpPost]
        public JsonResult GetChildList(string userid="",bool type=false)
        {
            var list=M_UsersBusiness.GetUsersRelationList(userid==""?CurrentUser.UserID:userid, type);
            JsonDictionary.Add("items", list);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

    }
}
