using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ProEntity;

namespace ProBusiness.UserAttrs
{
    public class UserReportBussiness
    {
        public static List<UserReportDay> GetReportList(string btime, string etime,string userid, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {

            string whereSql = " b.Status<>9 ";

            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.ReportTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.ReportTime<'" + etime + "'";
            }
            string clumstr = "a.*,b.UserName";
            DataTable dt = CommonBusiness.GetPagerData("UserReportDay a join M_Users b on a.Userid=b.Userid ", clumstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }

            return list;
        }
    }
}
