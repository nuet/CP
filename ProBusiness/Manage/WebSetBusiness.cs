using ProDAL;
using ProEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProBusiness.Manage
{
    public class WebSetBusiness
    {
        public static string FILEPATH =ProTools.Common.GetKeyValue("UploadFilePath") + "Active/" + DateTime.Now.ToString("yyyyMM") + "/";
        public static string TempPath = ProTools.Common.GetKeyValue("UploadTempPath");
        #region 查询   

        public static List<Active> GetActiveList(string keyWords, int pageIndex, int pageSize, ref int totalCount, ref int pageCount, string btime = "", string etime = "", int type=-1)
        { 
            string whereSql = " a.Status<>9";
            if (!string.IsNullOrEmpty(keyWords))
            {
                whereSql += " and (a.Title like '%" + keyWords + "%' or a.Tips like '%" + keyWords + "%' )";
            }
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and ( ( a.BTime<='" + etime + "' and a.BTime>='" + btime + "') or ( a.BTime>='" + btime + "' and a.ETime <='" + etime + "' ) or (a.Etime>='" + btime + "' and a.Etime<='" + etime + "'))";
            }
            if (type > -1)
            {
                whereSql += " and a.Type ="+ type +" ";
            }
            string cstr = @" a.*,b.UserName as CreateUser ";
            DataTable dt = CommonBusiness.GetPagerData(" Active a join M_Users b on a.CreateUserID=b.UserID ", cstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            
            List<Active> list = new List<Active>(); 
            foreach (DataRow dr in dt.Rows)
            {
                Active model = new Active();
                model.FillData(dr);
                list.Add(model);
            } 
            return list;
        }
        public static List<Active> GetActiveList(int type=0 ,int tops=9)
        {
            DataTable dt = WebSetDAL.GetDataTable("select top " + tops + " a.*  from  Active a  where a.Status<>9 and Type=" + type);

            List<Active> list = new List<Active>();
            foreach (DataRow dr in dt.Rows)
            {
                Active model = new Active();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
        public static Active GetActiveByID(string AutoID)
        {

            DataTable dt = WebSetDAL.BaseProvider.GetActiveByID(AutoID);
            Active model = null;
            if (dt.Rows.Count == 1)
            {
                model = new Active();
                model.FillData(dt.Rows[0]);
            }
            return model; 
        }
        public static List<ChargeSet> GetChargeSet(string keyWords, string userid,int status, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string begintime = "", string endtime = "")
        {
            string tablename = " ChargeSet  a left join M_Users b  on a.UserID =b.UserID ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and (b.Name like '%" + keyWords + "%' or a.Remark like '%" + keyWords + "%' or a.View like '%" + keyWords + "%')";
            } 
            if (status > -1)
            {
                sqlwhere += " and a.status=" + status;
            } 
            if (!string.IsNullOrEmpty(userid))
            {
                sqlwhere += " and a.UserID='" + userid + "' ";
            }
            if (!string.IsNullOrEmpty(begintime))
            {
                sqlwhere += " and a.CreateTime>='" + begintime + " 00:00:00'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sqlwhere += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
            }
            DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.Name as UserName ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
            List<ChargeSet> list = new List<ChargeSet>();
            foreach (DataRow dr in dt.Rows)
            {
                ChargeSet model = new ChargeSet();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

        public static ChargeSet GetChargeSetDetail(string view)
        {
            DataTable dt = WebSetDAL.BaseProvider.GetChargeSetDetail(view.Trim());
            ChargeSet model = null;
            if (dt.Rows.Count == 1)
            {
                model = new ChargeSet();
                model.FillData(dt.Rows[0]);
            }
            return model;
        }

        #endregion
        #region 新增 
        public static bool InsertActive(Active model)
        {
            model.Img = GetUploadImgurl(model.Img);
            return WebSetDAL.BaseProvider.InsertActive(model.CreateUserID,model.Title, model.Content, model.Tips, model.Img, model.BTime,model.ETime,model.Type);
        }

        public static bool InsertChargeSet(ChargeSet model)
        {
            return WebSetDAL.BaseProvider.InsertChargeSet(model.UserID, model.View.ToLower(), model.Remark, model.Golds);
        }

        #endregion

        #region 改 

        public static bool UpdateActive(Active model)
        {
            return WebSetDAL.BaseProvider.UpdateActive(model.AutoID, model.Title, model.Content, model.Tips, model.Img, model.BTime, model.ETime, model.UpdUserID);
        }
        public static bool UpdateChargeSet(ChargeSet model)
        {
            return WebSetDAL.BaseProvider.UpdateChargeSet(model.AutoID, model.View, model.Remark, model.Golds);
        }
        public static bool UpdateChargeSetStatus(int autoid,int status)
        {
            return CommonBusiness.Update("ChargeSet", "Status", status, " where Status<>9 and AutoID=" + autoid);
        }
        public static bool DeleteActive(int autoid)
        {
            return WebSetDAL.BaseProvider.DeleteActive(autoid);  
        }
        #endregion
        public static string GetUploadImgurl(string imgurl)
        {
            if (!string.IsNullOrEmpty(imgurl) && imgurl.IndexOf(TempPath) >= 0)
            {
                DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath(FILEPATH));
                if (!directory.Exists)
                {
                    directory.Create();
                }
                if (imgurl.IndexOf("?") > 0)
                {
                    imgurl = imgurl.Substring(0, imgurl.IndexOf("?"));
                }
                FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath(imgurl));
                imgurl = FILEPATH + file.Name;
                if (file.Exists)
                {
                    file.MoveTo(HttpContext.Current.Server.MapPath(imgurl));
                }
            }
            return imgurl;
        }
    }
}
