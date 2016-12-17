using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class LotteryResultDAL:BaseDAL
    {
        public static LotteryResultDAL BaseProvider = new LotteryResultDAL();



        public void InsertLotteryResultAll()
        {
            SqlParameter[] paras = {};
            ExecuteNonQuery("InsertLotteryResultAll", paras, CommandType.StoredProcedure);
        }

        public bool UpdateLotteryResult(string issuenum, string cpcode, int status)
        {
            SqlParameter[] paras = {new SqlParameter("@IssueNum",issuenum),
                                       new SqlParameter("@Status",status),
                                       new SqlParameter("@CPCode",cpcode)
                                   };
            return ExecuteNonQuery("Update LotteryResult set Status=@Status  where IssueNum=@IssueNum and CPCode=@CPCode", paras, CommandType.Text) > 0;
        }
        public bool UpdateLotteryStatus(string cpcode, int status)
        {
            SqlParameter[] paras = {
                                       new SqlParameter("@Status",status),
                                       new SqlParameter("@CPCode",cpcode)
                                   };
            return ExecuteNonQuery("UpdateLotteryStatus", paras, CommandType.StoredProcedure) > 0;
        }
        public bool UpdateSD11X5Result(string result, string issnum, string cpcode)
        {
            SqlParameter[] paras = {
                                       new SqlParameter("@Content",result),
                                        new SqlParameter("@IssueName",issnum),
                                       new SqlParameter("@CPCode",cpcode)
                                   };
            return ExecuteNonQuery("UpdateSD11X5Result", paras, CommandType.StoredProcedure) > 0;
        }
    }
}
