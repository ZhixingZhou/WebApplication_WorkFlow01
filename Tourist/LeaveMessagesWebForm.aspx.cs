using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using WebApplication_WorkFlow01.Common;

namespace WebApplication_WorkFlow01.Tourist
{
    public partial class LeaveMessagesWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断是否登录，减少异常
            if (Session.IsNewSession)
            {
                Session["UserFlag"] = false;
                Response.Redirect("../Account/LoginWebForm.aspx");
                return;
            }
            if (Session["UserFlag"].ToString() == "false")
            {
                Response.Redirect("../Account/LoginWebForm.aspx");
                return;
            }
            if (Session["UserType"].ToString() != "游客")
            {
                Response.Redirect("../Index.aspx");
                return;
            }
            //查询用户的留言,要考虑到如果用户的留言太多，应该分页
            //还应考虑到回复的问题
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            string param_username = Session["UserName"].ToString();
            string sql1 = "SELECT 留言ID,留言内容,留言时间,部门名称 FROM LeaveMessagesInfo t1,Department t2 WHERE 留言用户='" 
                + param_username + "' AND t1.留言部门ID=t2.部门ID ORDER BY t1.留言时间 DESC";
            SqlDataAdapter adapter = new SqlDataAdapter(sql1, conn);
            //SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            //adapter.DeleteCommand = builder.GetDeleteCommand();//对于多个基表不支持动态 SQL 生成
            DataTable table_MessagesInfo = new DataTable();
            adapter.Fill(table_MessagesInfo);
            int rowNum = table_MessagesInfo.Rows.Count;
            string strHtml = string.Empty;
            for (int i = 0; i < rowNum; i++)
            {
                DataRow row = table_MessagesInfo.Rows[i];
                strHtml += "<div class=\"info_item\">";
                strHtml += "<div class=\"message_content\">" + row[2] + "我向" + row[3] + "留言:" + row[1] + "</div>";
                string sql2 = "SELECT * FROM ReplyInfo WHERE 留言ID=" + row[0] + " AND 所针对的回复的ID='-1' ORDER BY 回复时间 DESC";
                SqlDataAdapter adapter2 = new SqlDataAdapter(sql2, conn);
                DataTable table_Reply = new DataTable();
                adapter2.Fill(table_Reply);
                if (table_Reply.Rows.Count != 0)
                {
                    strHtml += "<div class=\"reply_content\">";
                    for (int j = 0; j < table_Reply.Rows.Count; j++)
                    {
                        DataRow row2 = table_Reply.Rows[j];
                        strHtml += row2[1] + ":" + row2[4] + "  " + row2[5] + "<br/>";
                        strHtml += SearchReply.DoSearch((int)row2[2], (int)row2[0], Session["UserName"].ToString());
                    }
                    strHtml += "</div></div>";
                }
            }
            if (rowNum == 0)
            {
                strHtml = "您还没有留言！";
            }
            content_right_message.InnerHtml = strHtml;
            Label1.Text = "第1页/共1页";
        }
    }
}