using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using WebApplication_WorkFlow01.Common;

namespace WebApplication_WorkFlow01.Department
{
    public partial class LookMessagesReplied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strHtml = string.Empty;
            //查询用户的留言,要考虑到如果用户的留言太多，应该分页
            //还应考虑到回复的问题
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT 留言ID,留言用户,部门名称,留言内容,留言时间 " 
                + "FROM LeaveMessagesInfo t1,Department t2 " 
                + "WHERE t1.留言部门ID=t2.部门ID AND 留言ID IN(SELECT 留言ID FROM ReplyInfo WHERE 回复用户='" 
                + Session["UserName"] + "') ORDER BY 留言时间 DESC";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable table_MessagesReplied = new DataTable();
            adapter.Fill(table_MessagesReplied);
            int rowNum = table_MessagesReplied.Rows.Count;
            for (int i = 0; i < rowNum; i++)
            {
                DataRow row = table_MessagesReplied.Rows[i];
                strHtml += "<div class=\"info_item\">";
                strHtml += "<div class=\"message_content\">" + row[1] + "向" + row[2] + "留言:" + row[3] + "  " + row[4] + "</div>";
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
                        strHtml += row2[1] + ":" + row2[4] + "  " + row2[5];
                        strHtml += "<span class=\"reply_span\">回复</span>";
                        strHtml += "<div class=\"reply_div\">";
                        strHtml += "<textarea style=\"height:60px;width:400px;\">请在此输入回复内容</textarea>"
                            + "<input class=\"replyButton\" type=\"button\" value=\"发表\" data-messageid=\""
                            + row2[2] + "\" data-replyuser=\"" + Session["UserName"] + "\" data-replyobject=\""
                            + row2[1] + "\" data-objectid=\"" + row2[0] + "\"/>";
                        strHtml += "</div><br/>";
                        strHtml += SearchReply.DoSearch((int)row2[2], (int)row2[0], Session["UserName"].ToString());
                    }
                    strHtml += "</div>";
                }
                strHtml += "<textarea style=\"height:20px;width:400px;\">请在此输入回复内容</textarea>"
                    + "<input class=\"replyButton\" type=\"button\" value=\"发表\" data-messageid=\""
                    + row[2] + "\" data-replyuser=\"" + Session["UserName"] + "\" data-replyobject=\""
                    + row[1] + "\" data-objectid=\"" + row[0] + "\"/>";
                strHtml += "</div>";
            }
            if (string.IsNullOrEmpty(strHtml))
            {
                strHtml = "您没有回复过的留言";
            }
            content_right_message.InnerHtml = strHtml;
            Label1.Text = "第1页/共1页";
        }
    }
}