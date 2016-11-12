﻿using ProEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProBusiness;
using ProBusiness.Manage;
using ProEntity.Manage;
using ProTools;

namespace CPiao.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //if (Session["Manager"] == null)
            //{
            //    return Redirect("/Home/Login");
            //}
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Register2()
        {
            return View();
        }
        public ActionResult Login()
        {
            if (CurrentUser != null)
            {
                return Redirect("/User/UserInfo");
            }
            HttpCookie cook = Request.Cookies["zpy"];
            if (cook != null)
            {
                if (cook["status"] == "1")
                {
                    string operateip = OperateIP;
                    int result;
                    M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(cook["username"], cook["pwd"], operateip, out result);
                    if (model != null)
                    {
                        if (DateTime.Now.CompareTo(model.AuthorETime)<1)
                        {
                            model.AuthorType = 0;
                        }
                        Session["Manager"] = model;
                        return Redirect("/User/UserInfo");
                    }
                }
            }
            return View();
        }
        public ActionResult Logout()
        { 
            HttpCookie cook = Request.Cookies["zpy"];
            if (cook != null)
            {
                cook["status"] = "0";
                Response.Cookies.Add(cook);
            }
            if (CurrentUser!=null)
            {
                M_UsersBusiness.CreateUserReport(CurrentUser.UserID, CurrentUser.LoginName, " IsLogin=0 ", OperateIP);
            }
            Session["Manager"] = null;
            Session["PartManage"] = null;
            return Redirect("/Home/Index");
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public JsonResult UserLogin(string userName, string pwd,string remember="")
        {
            bool bl = false; 
            string operateip =OperateIP;
            int result = 0;
            string msg = "";
            ProEntity.Manage.M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(userName, pwd, operateip, out result);
            if (model != null)
            {
                if (model.Status == 0)
                {
                    result = 0;
                    Session["PartManage"] = model;
                    msg = "还没有注册完成,请继续注册";
                }
                else if (model.Status == 1)
                {
                    HttpCookie cook = new HttpCookie("zpy");
                    cook["username"] = userName;
                    cook["pwd"] = pwd;
                    if (remember == "1")
                    {
                        cook["status"] = remember;
                    }
                    cook.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cook);
                    CurrentUser = model;
                    Session["Manager"] = model;
                    result = 1;
                }
                else 
                {
                    msg = result == 3 ? "用户已被禁闭，请联系管理员" : "用户名或密码错误！";
                }
            }
            else
            {
                msg = result == 3 ? "用户已被禁闭，请联系管理员" : result == 2?"用户名不存在":"用户名或密码错误！";
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("errorMsg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UserNameCheck(string userName)
        {
            JsonDictionary.Add("result", ProBusiness.M_UsersBusiness.GetM_UserCountByLoginName(userName)==0);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UserRegister(string loginname,string pwd)
        { 
            var result = !string.IsNullOrEmpty(ProBusiness.M_UsersBusiness.CreateM_UserBase(loginname, pwd));
            if (result)
            {
                var outresult = 0;
                ProEntity.Manage.M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(loginname, pwd,
                    OperateIP, out outresult);
                if (model != null)
                {
                    Session["PartManage"] = model; 
                }
                else
                {
                    result = false; 
                }
            } 
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }
        public JsonResult UserRegister2(string entity)
        {
            M_Users users = JsonConvert.DeserializeObject<M_Users>(entity); 
            users.OfficePhone = "";
            users.Avatar = "";
            users.CreateUserID = "";
            users.IsAdmin = 0;
            users.RoleID = "";
            users.Description = "";
            users.UserID = ((M_Users) Session["PartManage"]).UserID;
            var result = ProBusiness.M_UsersBusiness.UpdateM_UserBase(users);
            if (result)
            {
                var outresult = 0;
                ProEntity.Manage.M_Users model = ProBusiness.M_UsersBusiness.GetUserDetail(((M_Users)Session["PartManage"]).UserID);
                if (model != null)
                {  
                    CurrentUser = model;
                    Session["Manager"] = model; 
                }
            }
            JsonDictionary.Add("result", result);

            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetAdvertList(string view, string imgtype)
        {
            var list = WebSetBusiness.GetAdvertSetList(imgtype, view.ToLower());
            JsonDictionary.Add("BaseUrl",ProTools.Common.GetKeyValue("Url"));
            JsonDictionary.Add("items", list);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
