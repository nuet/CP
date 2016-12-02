using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProDAL;
using ProDAL.UserAttrs;
using ProEntity;
using ProEntity.Manage;

namespace ProBusiness
{
   public class LotteryOrderBusiness
    {
       public static LotteryOrderBusiness BaseBusiness = new LotteryOrderBusiness();

        #region 查询

       public static List<LotteryOrder> GetUserOrders(string keyWords, string cpcode,string userid, string type, int status,int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string begintime = "", string endtime = "")
        {
            string tablename = "LotteryOrder  a left join M_Users b  on a.UserID =b.UserID ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and (b.UserName like '%" + keyWords + "%' or a.IssueNum like '%" + keyWords + "%' or a.LCode like '%" + keyWords + "%'  or a.TypeName like '%" + keyWords + "%')";
            }
            if (!string.IsNullOrEmpty(type))
            {
                sqlwhere += " and a.Type='" + type+"'";
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
            DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.UserName ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
            List<LotteryOrder> list = new List<LotteryOrder>();
            foreach (DataRow dr in dt.Rows)
            {
                LotteryOrder model = new LotteryOrder();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
       public static LotteryOrder GetUserOrderDetail(string lcode)
       {
           DataTable dt = LotteryOrderDAL.BaseProvider.GetLotteryOrderDetail(lcode);
           LotteryOrder model = null;
           if (dt.Rows.Count == 1)
           {
               model = new LotteryOrder();
               model.FillData(dt.Rows[0]);
           }
           return model;
       }
        #endregion

        #region 添加.删除



       public static int CreateUserOrderList(List<LotteryOrder> models, M_Users user,string ip,int usedisFee,ref string errmsg )
       {
           int k = 0;
           string msg = "";
           models.ForEach(x =>
           {
               string orderCode = DateTime.Now.ToString("yyyyMMddhhMMssfff") + user.AutoID;
               var result = CreateLotteryOrder(orderCode, x.IssueNum, x.Type,x.TypeName,x.CPCode, x.CPName, x.Content,
                   x.Num, x.PayFee, user.UserID, x.PMuch, x.RPoint, ip,usedisFee,ref msg);
               if (!result)
               {
                   msg += x.Content + "    余额不足/n";
               }
               else
               {
                   k++;
               }
           });
           errmsg = msg;
           return k;
       }

       public static bool CreateLotteryOrder(string ordercode, string issueNum, int type, string typename,string cpcode, string cpname, string content,  int num,
           decimal payfee, string userID, int pmuch, decimal rpoint, string operatip,int usedisFee, ref string errormsg)
       {
           var orderid = Guid.NewGuid().ToString();
           return LotteryOrderDAL.BaseProvider.CreateLotteryOrder(ordercode, orderid,issueNum, type, cpcode, cpname, content, typename, num,
            payfee, userID, pmuch, rpoint, operatip,usedisFee,ref errormsg);
       }
        public static bool DeleteOrder(string ordercode)
        {
            bool bl = CommonBusiness.Update("LotteryOrder", "Status", 9, "LCode='" + ordercode + "'");
            return bl;
        }
        public static bool BoutOrder(string ordercode)
        {
            bool bl = CommonBusiness.Update("LotteryOrder", "Status", 3, "LCode='" + ordercode + "' and Status=0");
            return bl;
        }
        

       #endregion 
    }
}
