using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class LotteryOrderDAL : BaseDAL
    {
        public static LotteryOrderDAL BaseProvider = new LotteryOrderDAL();
        public bool CreateLotteryOrder(string ordercode, string orderid, string issueNum, int type, string cpcode, string cpname, string content, string typename, int num,
           decimal payfee, string userID, int pmuch, decimal rpoint, string operatip,int usedisFee,ref string errormsg)
        {
//            string sql = @"INSERT INTO [LotteryOrder]([OrderCode],[OrderID],[IssueNum],[IP],[CreateTime],[Status],[UserID],[CPCode],[CPName],[PayFee],[Content],[TypeName],[PMuch],RPoint,Type,Num)
//                    VALUES (@OrderCode,@OrderID,@IssueNum,@IP,getdate(), 0,@UserID, @CPCode, @CPName, @PayFee, @Content, @TypeName,@PMuch,@RPoint,@Type,@Num)";
            SqlParameter[] paras = { 
                                    new SqlParameter("@ErrorMsg" , SqlDbType.VarChar,300),
                                    new SqlParameter("@Result",SqlDbType.Int),
                                    new SqlParameter("@OrderCode",ordercode),
                                    new SqlParameter("@OrderID",orderid),
                                    new SqlParameter("@UserID",userID),
                                    new SqlParameter("@IssueNum",issueNum),
                                    new SqlParameter("@IP",operatip), 
                                    new SqlParameter("@CPCode",cpcode),
                                    new SqlParameter("@CPName",cpname),
                                    new SqlParameter("@PayFee",payfee),
                                    new SqlParameter("@Content",content),
                                    new SqlParameter("@TypeName",typename),
                                    new SqlParameter("@PMuch",pmuch),
                                    new SqlParameter("@RPoint",rpoint),
                                    new SqlParameter("@Type",type),
                                    new SqlParameter("@UsedisFee",usedisFee), 
                                    new SqlParameter("@Num",num)  
                                   };
            paras[0].Direction = ParameterDirection.Output;
            paras[1].Direction = ParameterDirection.Output;
            ExecuteNonQuery("InsertLotteryOrder", paras, CommandType.StoredProcedure);
            var result = Convert.ToInt32(paras[1].Value);
            errormsg = paras[0].Value.ToString();
            return result > 0;
        }

        public DataTable GetLotteryOrderDetail(string lcode)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@LCode",lcode)
                                   };

            return GetDataTable("select * from LotteryOrder where LCode=@LCode", paras, CommandType.Text);
        }


        

    }
}
