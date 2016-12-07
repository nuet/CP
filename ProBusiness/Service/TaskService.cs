using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProBusiness
{
    public class TaskService
    {
        public static TaskService BasService = new TaskService();

        public void InsertAllLottery()
        {
            LotteryResultBusiness.InsertAllLottery();
        }

        public string BettAutoInsert()
        { 
            var list = LotteryOrderBusiness.GetBettAutoByStatus();
             list.ForEach(x =>
             {
                 string issue = DateTime.Now.ToString("yyyyMMdd").Substring(2, 6);
                 var issuenum=GetIssueNum(x.CPCode, x.StartNum, x.ComNum);
                 LotteryOrderBusiness.CreateLotteryOrder(x.BCode,issuenum,x.Type,x.TypeName,x.CPCode,x.CPName,x.Content,x.num)
             });
        }


        public string GetIssueNum(string cpcode, string issuenum, int comnum)
        {
            string comissuenum = "";
             if (cpcode == "SD11X5")
            {
                int num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 2, 2));
                num = num + comnum;
                comissuenum = DateTime.Now.ToString("yyyyMMdd").Substring(2, 6);
                if (num > 78)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd").Substring(2, 6);
                    num = num - 78;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(2,'0');
            }
            return comissuenum;
        }
        public string GetLotteryNum(string cpcode, string issuenum, int comnum)
        {
            string comissuenum = "";
            if (cpcode == "SD11X5")
            {
                int num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 2, 2));
                num = num + comnum;
                comissuenum = DateTime.Now.ToString("yyyyMMdd").Substring(2, 6);
                if (num > 78)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd").Substring(2, 6);
                    num = num - 78;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(2, '0');
            }
            return comissuenum;
        }

    }
}
