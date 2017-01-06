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
        string end = "23:59";
        #endregion
        private Object thisLock = new Object(); 
        private Object gdlock = new Object();
        private Object sdlock = new Object();
        private Object hljlock = new Object();
        private Object jxlock = new Object();
        private Object xjlock = new Object();
        private Object tjlock = new Object();
        private Object fc3dlock = new Object(); 
        private Object shlock = new Object(); 
        public TaskBase()
        {
            NonReentrantAsDefault();

            InsertLottery();
            UpdateLotteryStatus();
            UpdateSD11X5Result();
            UpdateGD11X5Result();
            UpdateJX11X5Result();
            UpdateSHSSLResult();
            UpdateTJSSCResult();
            UpdateXJSSCResult();
            UpdateHLJSSCResult();
            UpdateFC3DResult();
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
        private void UpdateLotteryStatus()
        {
            L.Register("[updatelotterystatus]");

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
                            LotteryResultBusiness.UpdateStatus("SD11X5,", 1);
                        }
                    }
                    else if (s == "9")
                    {
                        lock (thisLock)
                        {  
                            LotteryResultBusiness.UpdateStatus("GD11X5,JX11X5,HLJSSC,XJSSC,TJSSC,", 1); 
                        }
                    }
                    else if (min == 55 && DateTime.Now.Hour==21)
                    {
                        lock (thisLock)
                        {
                            LotteryResultBusiness.UpdateStatus("FCSD,", 1);
                        }
                    }
                    if (min %2==0)
                    {
                        lock (thisLock)
                        {
                            LotteryResultBusiness.UpdateStatus("SHSSL,", 1);
                        }
                    }

                }
            }).NonReentrant().WithName("[updatelotterystatus]").ToRunNow().AndEvery(1).Minutes();
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
                    lock (sdlock)
                    {
                        //方法处理
                        KCWBase<DataResult> kcwresult = ProTools.HttpRequest.RequestServer<KCWBase<DataResult>>(ProTools.KCWAppUrl.NewLy,
                                Getparas("SD11X5"));
                        if (kcwresult!=null && kcwresult.Data.Count > 0)
                        {
                            var suc = TaskService.BasService.OpenLotteryResult(kcwresult.Data[0].OpenCode.Replace(',',' '),
                                kcwresult.Data[0].Expect, "SD11X5");
                            L.Log("SD11X5:" + kcwresult.Data[0].Expect + (suc ? "开奖成功!" : "开奖失败"));
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
                    lock (jxlock)
                    {
                        //方法处理
                        KCWBase<DataResult> kcwresult = ProTools.HttpRequest.RequestServer<KCWBase<DataResult>>(ProTools.KCWAppUrl.NewLy,
                                Getparas("JX11X5"));
                        if (kcwresult!=null && kcwresult.Data.Count > 0)
                        {
                            var suc = TaskService.BasService.OpenLotteryResult(kcwresult.Data[0].OpenCode.Replace(',',' '), kcwresult.Data[0].Expect, "JX11X5");
                            L.Log("JX11X5:" + kcwresult.Data[0].Expect + (suc ? "开奖成功!" : "开奖失败"));
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
                    lock (gdlock)
                    {
                        KCWBase<DataResult> kcwresult = ProTools.HttpRequest.RequestServer<KCWBase<DataResult>>(ProTools.KCWAppUrl.NewLy,
                                Getparas("GD11X5"));
                        if (kcwresult != null && kcwresult.Data.Count > 0)
                        {
                            var suc = TaskService.BasService.OpenLotteryResult(kcwresult.Data[0].OpenCode.Replace(',',' '), kcwresult.Data[0].Expect, "GD11X5");
                            L.Log("GD11X5:" + kcwresult.Data[0].Expect + (suc ? "开奖成功!" : "开奖失败"));
                        }
                    } 
                }
            }).NonReentrant().WithName("[UpdateGD11X5Result]").ToRunNow().AndEvery(1).Minutes();
        }
        private void UpdateSHSSLResult()
        {
            L.Register("[UpdateSHSSLResult]");
            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse("10:26").TimeOfDay;
                TimeSpan endTime = DateTime.Parse("22:10").TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay; 
                if (tmNow >= startTime && tmNow <= endTime)
                { 
                    lock (shlock)
                    {
                        KCWBase<DataResult> kcwresult = ProTools.HttpRequest.RequestServer<KCWBase<DataResult>>(ProTools.KCWAppUrl.NewLy,
                                Getparas("SHSSL"));
                        if (kcwresult != null && kcwresult.Data.Count > 0)
                        {
                                var suc=TaskService.BasService.OpenLotteryResult(kcwresult.Data[0].OpenCode.Replace(',',' '), kcwresult.Data[0].Expect, "SHSSL");
                                L.Log("SHSSL:" + kcwresult.Data[0].Expect + (suc ? "开奖成功!" : "开奖失败"));
                        } 
                    } 
                }
            }).NonReentrant().WithName("[UpdateSHSSLResult]").ToRunNow().AndEvery(1).Minutes();
        }
        private void UpdateTJSSCResult()
        {
            L.Register("[UpdateTJSSCResult]");
            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse("09:09").TimeOfDay;
                TimeSpan endTime = DateTime.Parse("23:02").TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay; 
                if (tmNow >= startTime && tmNow <= endTime)
                { 
                    lock (tjlock)
                    {
                        KCWBase<DataResult> kcwresult = ProTools.HttpRequest.RequestServer<KCWBase<DataResult>>(ProTools.KCWAppUrl.NewLy,
                                Getparas("TJSSC"));
                        if (kcwresult != null && kcwresult.Data.Count > 0)
                        {
                                var suc=TaskService.BasService.OpenLotteryResult(kcwresult.Data[0].OpenCode.Replace(',',' '), kcwresult.Data[0].Expect, "TJSSC");
                                L.Log("TJSSC:" + kcwresult.Data[0].Expect + (suc ? "开奖成功!" : "开奖失败"));
                        } 
                    }
                }
            }).NonReentrant().WithName("[UpdateTJSSCResult]").ToRunNow().AndEvery(1).Minutes();
        }
        private void UpdateHLJSSCResult()
        {
            L.Register("[UpdateHLJSSCResult]");
            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse("09:09").TimeOfDay;
                TimeSpan endTime = DateTime.Parse("23:02").TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay; 
                if (tmNow >= startTime && tmNow <= endTime)
                { 
                    lock (hljlock)
                    {
                        KCWBase<DataResult> kcwresult = ProTools.HttpRequest.RequestServer<KCWBase<DataResult>>(ProTools.KCWAppUrl.NewLy,
                                Getparas("HLJSSC"));
                        if (kcwresult != null && kcwresult.Data.Count > 0)
                        {
                                var suc=TaskService.BasService.OpenLotteryResult(kcwresult.Data[0].OpenCode.Replace(',',' '), kcwresult.Data[0].Expect, "HLJSSC");
                                L.Log("HLJSSC:" + kcwresult.Data[0].Expect + (suc ? "开奖成功!" : "开奖失败"));
                        } 
                    }
                }
            }).NonReentrant().WithName("[UpdateHLJSSCResult]").ToRunNow().AndEvery(1).Minutes();
        }
        private void UpdateFC3DResult()
        {
            L.Register("[UpdateFC3DResult]");
            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse("20:30").TimeOfDay;
                TimeSpan endTime = DateTime.Parse("20:46").TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay;
                if (tmNow >= startTime && tmNow <= endTime)
                {
                    if (DateTime.Now.Minute%2 == 0)
                    {
                        lock (fc3dlock)
                        {

                            KCWBase<DataResult> kcwresult =
                                ProTools.HttpRequest.RequestServer<KCWBase<DataResult>>(ProTools.KCWAppUrl.NewLy,
                                    Getparas("FC3D"));
                            if (kcwresult != null && kcwresult.Data.Count > 0)
                            {
                                var suc =
                                    TaskService.BasService.OpenLotteryResult(
                                        kcwresult.Data[0].OpenCode.Replace(',', ' '),
                                        kcwresult.Data[0].Expect, "FC3D");
                                L.Log("FC3D:" + kcwresult.Data[0].Expect + (suc ? "开奖成功!" : "开奖失败"));
                            }
                        }
                    }
                }
            }).NonReentrant().WithName("[UpdateFC3DResult]").ToRunNow().AndEvery(1).Minutes();
        }
        private void UpdateXJSSCResult()
        {
            L.Register("[UpdateXJSSCResult]");
            Schedule(() =>
            {
                TimeSpan startTime = DateTime.Parse("09:09").TimeOfDay;
                TimeSpan endTime = DateTime.Parse("23:02").TimeOfDay;
                TimeSpan tmNow = DateTime.Now.TimeOfDay;
                if (tmNow >= startTime && tmNow <= endTime)
                {
                    lock (xjlock)
                    {
                        KCWBase<DataResult> kcwresult =
                            ProTools.HttpRequest.RequestServer<KCWBase<DataResult>>(ProTools.KCWAppUrl.NewLy,
                                Getparas("XJSSC"));
                        if (kcwresult != null && kcwresult.Data.Count > 0)
                        {
                            var suc =
                                TaskService.BasService.OpenLotteryResult(kcwresult.Data[0].OpenCode.Replace(',', ' '),
                                    kcwresult.Data[0].Expect, "XJSSC");
                            L.Log("XJSSC:" + kcwresult.Data[0].Expect + (suc ? "开奖成功!" : "开奖失败"));
                        }
                    }
                }
            }).NonReentrant().WithName("[UpdateXJSSCResult]").ToRunNow().AndEvery(1).Minutes();
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
                paraDir.Add("format", format);
            }
            if (!string.IsNullOrEmpty(adate))
            {
                paraDir.Add("date", adate);
            }
            paraDir.Add("rows", row > 0 ? row : 5);
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
 