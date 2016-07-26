using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication_WorkFlow01.Department
{
    public partial class DepartmentUpdatedInfo : System.Web.UI.Page
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
            if (Session["UserType"].ToString() != "部门")
            {
                Response.Redirect("../Index.aspx");
                return;
            }
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM ReplyInfo WHERE 回复对象='" + Session["UserName"] + "' AND isRead='否' ORDER BY 回复时间 DESC";
            SqlDataAdapter adpter = new SqlDataAdapter(sql, conn);
            DataTable table_UnRead = new DataTable();
            adpter.Fill(table_UnRead);
            string strHtml = string.Empty;
            EFModel.Database1Entities entity = new EFModel.Database1Entities();
            for (int i = 0; i < table_UnRead.Rows.Count; i++)
            {
                DataRow row = table_UnRead.Rows[i];
                strHtml += "<div class=\"info_item\">";
                strHtml += row[1] + ":" + row[4] + "  " + row[5];
                strHtml += "<span class=\"reply_span\">回复</span>";
                strHtml += "<div class=\"reply_div\">";
                strHtml += "<textarea style=\"height:20px;width:400px;\">请在此输入回复内容</textarea>"
                    + "<input class=\"replyButton\" type=\"button\" value=\"发表\" data-messageid=\""
                    + row[2] + "\" data-replyuser=\"" + Session["UserName"] + "\" data-replyobject=\""
                    + row[1] + "\" data-objectid=\"" + row[0] + "\"/>";
                strHtml += "</div><br/>";
                var q = from t in entity.LeaveMessagesInfo
                        where t.留言ID == (int)row[2]
                        select t;
                var message = q.First();
                strHtml += "<div class=\"message_content\">" + message.留言用户 + "向"
                    + message.留言部门ID + "留言:" + message.留言内容 + "  " + message.留言时间 + "</div>";
                if ((int)row[7] != -1)
                {
                    var q2 = from t in entity.ReplyInfo
                             where t.回复ID == (int)row[7]
                             select t;
                    var reply = q2.First();
                    strHtml += "<div class=\"reply_content\">" + reply.回复用户 + "回复"
                        + reply.回复对象 + ":" + reply.回复内容 + "  " + reply.回复时间 + "</div>";
                }
                strHtml += "</div>";
                //将“否”改为“是”
                string sql2 = "UPDATE ReplyInfo SET isRead='是' WHERE 回复ID='" + row[0] + "'";
                SqlCommand cmd = new SqlCommand(sql2, conn);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch { }
                finally
                {
                    conn.Close();
                }
            }

            if (strHtml == string.Empty)
            {
                strHtml += "您没有最新动态！";
            }
            content_right_message.InnerHtml = strHtml;
            Label1.Text = "第1页/共1页";
        }
    }
}