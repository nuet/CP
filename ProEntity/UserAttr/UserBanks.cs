using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProEntity
{
    public class UserBanks
    {
        public int AutoID { get; set; }
        public string UserID { get; set; }
        public string TrueName { get; set; }
        public string BankName { get; set; }
        public string BankPre { get; set; }
        public string BankCity { get; set; }
        public int BankChild { get; set; }
        public int CardCode { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
