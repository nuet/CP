using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProDAL.UserAttrs
{
    public class UserBanksDAL : BaseDAL
    {
        public static UserBanksDAL BaseProvider = new UserBanksDAL();
        public bool Create(string userID, string cardcode, string bankname,string bankchild,string truename,string bankpre,string bankcity)
        {
            string sql = @"declare @rerrormsg nvarchar(50) if(exists( select cardcode from userBanks where  Cardcode=@Cardcode ) ) begin  insert into UserImgs(UserID,Cardcode,BankName,Status,CreateTime,TrueName,BankChild,BankPre,BankCity)" +
                         "values(@UserID,@Cardcode,@BankName,0,getdate(),@TrueName,@BankChild,@BankPre,@BankCity)  ";
            SqlParameter[] paras = { 
                                    new SqlParameter("@ErrorMsg",userID),
                                    new SqlParameter("@UserID",userID), 
                                    new SqlParameter("@Cardcode",cardcode),
                                    new SqlParameter("@TrueName",truename),
                                    new SqlParameter("@BankChild",bankchild),
                                    new SqlParameter("@BankPre",bankpre),
                                    new SqlParameter("@BankCity",bankcity),
                                    new SqlParameter("@BankName",bankname) 
                                   };
            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }

        public bool UpdateStatus(string autoids, int status)
        {
            SqlParameter[] paras =
           {
               new SqlParameter("@AutoID", autoids),
               new SqlParameter("@Status", status)
           };
            return ExecuteNonQuery("update UserBanks set Status=@Status where AutoID=@AutoID", paras, CommandType.StoredProcedure) > 0;

        }
    }
}
