﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ProBusiness.Manage;
using ProDAL.UserAttrs;
using ProEntity;

namespace ProBusiness.UserAttrs
{
    public class UserReplyBusiness 
    {
        public static UserReplyBusiness BaseBusiness = new UserReplyBusiness();

        #region 查询 

        public static List<UserReply> GetUserReplys(string guid, string userid, int type, int pageSize, int pageIndex, ref int totalCount, ref int pageCount)
        {
            string tablename = "UserReply  a left join M_Users b  on a.Guid =b.UserID left join M_Users c  on a.CreateUserID =c.UserID ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(guid))
            {
                sqlwhere += " and a.guid='" + guid + "' ";
            }
            if (type>-1)
            {
                sqlwhere += " and a.Type=" + type;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                sqlwhere += " and a.CreateUserID='" + userid + "' ";
            }
            DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.Name as UserName,b.Avatar as UserAvatar,c.Name as FromName,c.Avatar as FromAvatar ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserReply> list = new List<UserReply>();
            foreach (DataRow dr in dt.Rows)
            {
                UserReply model = new UserReply();
                model.FillData(dr);
                list.Add(model);
            }
            return list; 
        }
        public static UserReply GetReplyDetail(string replyid)
        {
            DataTable dt = UserReplyDAL.BaseProvider.GetReplyDetail(replyid);
            UserReply model = null;
            if (dt.Rows.Count == 1)
            {
                model = new UserReply();
                model.FillData(dt.Rows[0]);
            }
            return model;
        }
        #endregion

        #region 添加.删除

        public static bool CreateUserReply(string guid, string content, string userID, string fromReplyID, string fromReplyUserID,int type)
        {
            return UserReplyDAL.BaseProvider.CreateUserReply(guid, content, userID, fromReplyID, fromReplyUserID, type);
        }  
        public static bool DeleteReply(string replyid)
        {
            bool bl = CommonBusiness.Update("UserReply", "Status", 9, "ReplyID='" + replyid + "'");
            return bl;
        }
        public static bool UpdateReplyStatus(string replyid,int status)
        {
            bool bl = CommonBusiness.Update("UserReply", "Status", status, "ReplyID='" + replyid + "' and Status<>9");
            return bl;
        }
        #endregion 
    }
}
