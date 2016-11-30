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
    }
}
