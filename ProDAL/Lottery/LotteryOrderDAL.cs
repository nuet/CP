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
        public bool CreateLotteryOrder(string ordercode, int paytype, string spname, string bankinfo, string sku, string content, decimal totalfee, string othercode, int type, decimal num, decimal paytfee, string userID, string createid, string operatip)
        {
            string sql = @"INSERT INTO [UserOrders]([OrderCode],[BankName],[SPName],[Sku],[Content],[CreateTime],[Status],[UserID],[PayType],[TotalFee],[OtherCode],[Type],[Num],[PayFee],CreateUserID,IP)
                    VALUES (@OrderCode,@BankName,@SPName,@Sku,@Content,getdate(), 0,@UserID, @PayType, @TotalFee, @OtherCode, @Type, @Num,@PayFee,@CreateUserID,@IP)";
            SqlParameter[] paras = { 
                                    new SqlParameter("@SPName",spname),
                                    new SqlParameter("@BankName",bankinfo),
                                    new SqlParameter("@UserID",userID),
                                    new SqlParameter("@CreateUserID",createid),
                                    new SqlParameter("@IP",operatip), 
                                    new SqlParameter("@Sku",sku),
                                    new SqlParameter("@PayType",paytype),
                                    new SqlParameter("@PayFee",paytfee),
                                    new SqlParameter("@Content",content),
                                    new SqlParameter("@TotalFee",totalfee),
                                    new SqlParameter("@OtherCode",othercode),
                                    new SqlParameter("@Type",type),
                                    new SqlParameter("@Num",num),
                                    new SqlParameter("@OrderCode",ordercode),    
                                   };
            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0; 
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
