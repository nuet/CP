﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class LotteryOrder
    {
        public long AutoID { get; set; }
        public string LCode { get; set; }
        public string LID { get; set; }
        public string CPCode { get; set; }
        public string CPName { get; set; } 
        /// <summary>
        /// 期号
        /// </summary>
        public string IssueNum { get; set; }
        /// <summary>
        /// 开奖结果
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 状态 0 未开奖 1 开奖中 2 已开奖    --9 开奖失败  轮训处理再次开奖 
        /// </summary>
        public int Status { get; set; }
        public int Type { get; set; }
        public int TypeName { get; set; } 
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 中奖总注数
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int PMuch { get; set; } 
        /// <summary>
        /// 奖金
        /// </summary>
        public decimal WinFee { get; set; }
        /// <summary>
        /// 和值 开奖结果的和值
        /// </summary>
        public decimal PayFee { get; set; }
        /// <summary>
        /// 返点级别
        /// </summary>
        public decimal RPoint { get; set; }
        /// <summary>
        /// 用户ＩＤ
        /// </summary>
        public string  UserID  { get; set; }

        public string UserName { get; set; }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}