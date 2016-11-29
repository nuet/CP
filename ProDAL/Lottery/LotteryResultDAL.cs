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
 

        public bool UpdateLotteryResult(string issuenum, string cpcode, int status)
        {
            SqlParameter[] paras = {new SqlParameter("@IssueNum",issuenum),
                                       new SqlParameter("@Status",status),
                                       new SqlParameter("@CPCode",cpcode)
                                   };
            return ExecuteNonQuery("Update LotteryResult set Status=@Status  where IssueNum=@IssueNum and CPCode=@CPCode", paras, CommandType.Text) > 0;
        }

    }
}
