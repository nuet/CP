using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Newtonsoft.Json;
using LLibrary;
using ProEntity;

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
            string msg = "";
             list.ForEach(x =>
             {
                 string errmsg = "";
                 var issuenum = GetIssueNum(x.ComNum, x.JsonContent);
                 var lottery = LotteryResultBusiness.GetLotteryResult(x.CPCode, "1,2");
                 if (IsEquelNum(x.CPCode, lottery.IssueNum, issuenum))
                 {
                     var totalmuch = GetIssueNum(x.ComNum, x.JsonContent, 1);
                     var pMuch = string.IsNullOrEmpty(totalmuch) ? x.BMuch : Convert.ToInt32(totalmuch);
                     try
                     {
                         if (!string.IsNullOrEmpty(issuenum))
                         {
                             LotteryOrderBusiness.CreateLotteryOrder(x.BCode, issuenum, x.Type, x.TypeName, x.CPCode,
                                 x.CPName,
                                 x.Content, x.Num, x.PayFee*pMuch/x.PMuch, x.UserID, pMuch, x.RPoint, x.IP, 0,
                                 ref errmsg);
                             if (!string.IsNullOrEmpty(errmsg))
                             {
                                 errmsg = issuenum + ":" + errmsg + ";";
                             }
                         }
                     }
                     catch (Exception ex)
                     {
                         errmsg = x.BCode + "第" + (x.ComNum + 1) + "期插入失败";

                         L.Log("[BettAutoInsert] ", x.BCode + "第" + (x.ComNum + 1) + "期插入失败");
                     }
                     msg += errmsg;
                     LotteryOrderBusiness.UpdateBettAutoByCode(x.BCode, x.ComNum + 1, pMuch*x.PayFee, errmsg);
                 }
             });
             return msg;
        }

        public bool IsEquelNum(string cpcode, string issuenum, string nowNum)
        {
            bool result =false;
            if (cpcode == "SD11X5")
            {
                int num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 2, 2)); 
                var comissuenum = DateTime.Now.ToString("yyyyMMdd").Substring(2, 6);
                if (num == 78)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd").Substring(2, 6);
                    num = num - 78;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(2, '0');
                return (comissuenum == nowNum);
            }
            return result;
        }

        public string GetIssueNum(string cpcode, string issuenum, int comnum, string jsoncontent)
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
        public string GetIssueNum(int comnum, string jsoncontent,int type=0)
        {
            string comissuenum = "";
            var jsonarr = jsoncontent.Split('|');
            if (jsonarr.Length>0 && !string.IsNullOrEmpty(jsonarr[comnum]))
            {
                comissuenum= jsonarr[comnum].Split(',')[type];
            } 
            return comissuenum;
        } 

    }
}
