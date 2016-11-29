using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data; 
using ProDAL; 
using ProEntity;
using ProEnum;


namespace ProBusiness
{
    public class LotteryResultBusiness
    {
        #region 查询   
        #endregion
        public static List<LotteryResult> GetPagList(string cpCode,int status, int pageSize, int pageIndex, ref int totalCount, ref int pageCount)
        {
            string sqlwhere = " b.cpCode='" + cpCode + "'";
            if (status > -1)
            {
                sqlwhere += " and b.Status=" + status;
            }
            DataTable dt = CommonBusiness.GetPagerData(" LotteryResult b ",
                "b.*", sqlwhere, "b.AutoID ", pageSize, pageIndex,
                out totalCount, out pageCount,false);
            List<LotteryResult> list = new List<LotteryResult>();
            foreach (DataRow dr in dt.Rows)
            {
                LotteryResult model = new LotteryResult();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
        public static LotteryResult GetLotteryResult(string cpCode, int status ,string btime ,string etime)
        {
            string sqlwhere = " where  b.cpCode='" + cpCode + "'";
            if (status > -1)
            {
                sqlwhere += " and b.Status=" + status;
            }

            if (!string.IsNullOrEmpty(btime))
            {
                sqlwhere += " and b.CreateTime>='" + btime + "'";
            } 
            if (!string.IsNullOrEmpty(etime))
            {
                sqlwhere += " and b.CreateTime<='" + etime+"'";
            }

            DataTable dt = LotteryResultDAL.GetDataTable("select top 1 *  from  LotteryResult b " + sqlwhere + " Order by AutoID asc  ");
            LotteryResult model = new LotteryResult();
            foreach (DataRow dr in dt.Rows)
            {
                model.FillData(dr); 
            }
            return model;
        }

        #region 改  
        public static  bool UpdateLotteryResult(string issuenum,string cpcode,int status) {
            return LotteryResultDAL.BaseProvider.UpdateLotteryResult(issuenum, cpcode,status);
        }
        #endregion

    }

    

    
}
