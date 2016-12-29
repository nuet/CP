using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using FluentScheduler; 
using LLibrary;
using CPiao.TaskRun;
using ProBusiness;
using ProTools;

namespace CPiao.TaskRun
{
    public class TaskBase : Registry
    {
        #region
        string start = "09:03";
        string end = "21:55";
        #endregion
        private Object thisLock = new Object();
        private Object resultlock = new Object();
        private Object gdlock = new Object(); 
        public TaskBase()
        {
            NonReentrantAsDefault();

            InsertLottery();
            UpdateSD11X5Status();
            UpdateSD11X5Result();
            UpdateGD11X5Result();
            UpdateJX11X5Result();
            UpdateSHSSLResult()
            BeetAuto();
            //NonReentrant();
            //Reentrant();
            //Disable();

            //Faulty();
            //Removed();
            //Parameter();
            //Disposable();

            //FiveMinutes();
            //TenMinutes();
            //Hour();
            //Day();
            //Weekday();
            //Week();
        }

        private void InsertLottery()
        { 
            Schedule(() =>
            {
               L.Log("[insertlottery]", "Begin...");
               TaskService.BasService.InsertAllLottery();
               L.Log("[insertlottery]", "End...");
            }).NonReentrant().WithName("[insertlottery]").ToRunEvery(1).Days().At(09, 39);
        }
        private void UpdateSD11X5Status()
        {
            L.Register("[updatesd11x5status]");

            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse(start).TimeOfDay;
                TimeSpan endTime = DateTime.Parse(end).TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay;
                int min = DateTime.Now.Minute;
                int sec = DateTime.Now.Second;
                if (tmNow >= startTime && tmNow <= endTime)
                {
                    var s = min.ToString().Length > 1 ? min.ToString().Substring(1, 1) : min.ToString();
                    if (s == "3")
                    {
                        lock (thisLock)
                        {
                            LotteryResultBusiness.UpdateStatus("SD11X5", 1);
                        }
                    }
                    else if (s == "9")
                    {
                        lock (thisLock)
                        {
                            LotteryResultBusiness.UpdateStatus("SD11X5", 1);
                            LotteryResultBusiness.UpdateStatus("TJSSC", 1);
                            LotteryResultBusiness.UpdateStatus("GD11X5", 1);
                            LotteryResultBusiness.UpdateStatus("JX11X5", 1);
                            LotteryResultBusiness.UpdateStatus("HLJSSC", 1);
                            LotteryResultBusiness.UpdateStatus("XJSSC", 1);
                        }
                    }
                }
            }).NonReentrant().WithName("[updatesd11x5status]").ToRunNow().AndEvery(1).Minutes();
        }
        private void UpdateSD11X5Result()
        {
            L.Register("[updatesd11x5result]");
            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse("09:05").TimeOfDay;
                TimeSpan endTime = DateTime.Parse(end).TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay;
                int min = DateTime.Now.Minute;
                int sec = DateTime.Now.Second;
                if (tmNow >= startTime && tmNow <= endTime)
                {
                    var s = min.ToString().Length > 1 ? min.ToString().Substring(1, 1) : min.ToString();
                    if (s == "5")
                    {
                        lock (resultlock)
                        {
                            //方法处理
                            var reuslt="01 02 03 04 05"; var issueNum="2016120634";
                            var suc = TaskService.BasService.OpenLotteryResult(reuslt, issueNum, "SD11X5");
                            L.Log(issueNum + (suc ? "开奖成功!" : "开奖失败"));
                        }
                    } 
                }
            }).NonReentrant().WithName("[updatesd11x5result]").ToRunNow().AndEvery(1).Minutes();
        }
        private void UpdateJX11X5Result()
        {
            L.Register("[UpdateJX11X5Result]");
            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse("09:05").TimeOfDay;
                TimeSpan endTime = DateTime.Parse(end).TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay;
                int min = DateTime.Now.Minute;
                int sec = DateTime.Now.Second;
                if (tmNow >= startTime && tmNow <= endTime)
                {
                    var s = min.ToString().Length > 1 ? min.ToString().Substring(1, 1) : min.ToString();
                    if (s == "9")
                    {
                        lock (resultlock)
                        {
                            //方法处理
                            var reuslt = "01 02 03 04 05"; var issueNum = "2016120634";
                            var suc = TaskService.BasService.OpenLotteryResult(reuslt, issueNum, "JX11X5");
                            L.Log(issueNum + (suc ? "开奖成功!" : "开奖失败"));
                        } 
                    }
                }
            }).NonReentrant().WithName("[UpdateJX11X5Result]").ToRunNow().AndEvery(1).Minutes();
        }
        private void UpdateGD11X5Result()
        {
            L.Register("[UpdateGD11X5Result]");
            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse("09:05").TimeOfDay;
                TimeSpan endTime = DateTime.Parse(end).TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay;
                int min = DateTime.Now.Minute;
                int sec = DateTime.Now.Second;
                if (tmNow >= startTime && tmNow <= endTime)
                {
                    var s = min.ToString().Length > 1 ? min.ToString().Substring(1, 1) : min.ToString();
                    if (s == "9")
                    {
                        lock (gdlock)
                        {
                            var reuslt = "01 02 03 04 05";
                            var isseueNum = "";
                            var suc = TaskService.BasService.OpenLotteryResult(reuslt, isseueNum, "GD11X5");
                        }
                    }
                }
            }).NonReentrant().WithName("[UpdateGD11X5Result]").ToRunNow().AndEvery(1).Minutes();
        }
        private void UpdateSHSSLResult()
        {
            L.Register("[UpdateGD11X5Result]");
            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse("10:26").TimeOfDay;
                TimeSpan endTime = DateTime.Parse("22:10").TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay;
                int min = DateTime.Now.Minute;
                int sec = DateTime.Now.Second;
                if (tmNow >= startTime && tmNow <= endTime)
                {
                    var s = min.ToString().Length > 1 ? min.ToString().Substring(1, 1) : min.ToString();
                    if (s == "5")
                    {
                        lock (gdlock)
                        {
                            KCWBase<DataResult> kcwresult = ProTools.HttpRequest.RequestServer<KCWBase<DataResult>>(ProTools.KCWAppUrl.NewLy,
                                    Getparas("SHSSL")); 
                            if (kcwresult.Data.Count > 0)
                            {
                                 TaskService.BasService.OpenLotteryResult(kcwresult.Data[0].OpenCode, kcwresult.Data[0].Expect, "SHSSL");
                            } 
                        }
                    }
                }
            }).NonReentrant().WithName("[UpdateGD11X5Result]").ToRunNow().AndEvery(1).Minutes();
        }
        private void BeetAuto()
        {
            L.Register("[beetAuto]");
            Schedule(() =>
            { 
                TaskService.BasService.BettAutoInsert();
            }).WithName("[beetAuto]").ToRunNow().AndEvery(1).Minutes();
        }

        public Dictionary<string, object> Getparas(string code,int row=1,string format="json",string adate="")
        {
            //{row:"1-20",format:"json/xml",date:"2016-12-06",code:"彩票代码"}
            Dictionary<string, object> paraDir = new Dictionary<string, object>();
            paraDir.Add("code",code); 
            if (!string.IsNullOrEmpty(format))
            {
                paraDir.Add("format", 1);
            }
            if (!string.IsNullOrEmpty(adate))
            {
                paraDir.Add("date", adate);
            }
            paraDir.Add("row", row > 0 ? row : 5);
            return paraDir;
        }

        private void Disable()
        {
            L.Register("[disable]");

            Schedule(() =>
            {
                JobManager.RemoveJob("[reentrant]");
                JobManager.RemoveJob("[non reentrant]");
                L.Log("[disable]", "Disabled the reentrant and non reentrant jobs.");
            }).WithName("[disable]").ToRunOnceIn(3).Minutes();
        }

        private void Faulty()
        {
            L.Register("[faulty]");

            Schedule(() =>
            {
                L.Register("[faulty]", "I'm going to raise an exception!");
                throw new Exception("I warned you.");
            }).WithName("[faulty]").ToRunEvery(20).Minutes();
        }

        private void Removed()
        {
            L.Register("[removed]");

            Schedule(() =>
            {
                L.Register("[removed]", "SOMETHING WENT WRONG.");
            }).WithName("[removed]").ToRunOnceIn(2).Minutes();
        }

        private void Parameter()
        {
            Schedule(new ParameterJob { Parameter = "Foo" }).WithName("[parameter]").ToRunOnceIn(10).Seconds();
        }

        private void Disposable()
        {
            Schedule<DisposableJob>().WithName("[disposable").ToRunOnceIn(10).Seconds();
        }

        private void FiveMinutes()
        {
            L.Register("[five minutes]");

            Schedule(() => L.Log("[five minutes]", "Five minutes has passed."))
                .WithName("[five minutes]").ToRunOnceAt(DateTime.Now.AddMinutes(5)).AndEvery(5).Minutes();
        }

        private void TenMinutes()
        {
            L.Register("[ten minutes]");

            Schedule(() => L.Log("[ten minutes]", "Ten minutes has passed."))
                .WithName("[ten minutes]").ToRunEvery(10).Minutes();
        }

        private void Hour()
        {
            L.Register("[hour]");

            Schedule(() => L.Log("[hour]", "A hour has passed."))
                .WithName("[hour]").ToRunEvery(1).Hours();
        }

        private void Day()
        {
            L.Register("[day]");

            Schedule(() => L.Log("[day]", "A day has passed."))
                .WithName("[day]").ToRunEvery(1).Days();
        }

        private void Weekday()
        {
            L.Register("[weekday]");

            Schedule(() => L.Log("[weekday]", "A new weekday has started."))
                .WithName("[weekday]").ToRunEvery(1).Weekdays();
        }

        private void Week()
        {
            L.Register("[week]");

            Schedule(() => L.Log("[week]", "A new week has started."))
                .WithName("[week]").ToRunEvery(1).Weeks();
        }

    }

}
 