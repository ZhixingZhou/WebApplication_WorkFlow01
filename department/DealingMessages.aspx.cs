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
    public partial class DealingMessages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strHtml = string.Empty;
            //查询用户的留言,要考虑到如果用户的留言太多，应该分页
            //还应考虑到回复的问题
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT 留言ID,留言用户,留言内容,留言时间 FROM LeaveMessagesInfo WHERE 留言部门ID IN (SELECT 部门ID FROM Department WHERE 部门名称='" + Session["UserName"] + "') ORDER BY 留言时间";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable table_MessagesInfo = new DataTable();
            adapter.Fill(table_MessagesInfo);
            int rowNum = table_MessagesInfo.Rows.Count;
            for (int i = 0; i < rowNum; i++)
            {
                DataRow row = table_MessagesInfo.Rows[i];
                string sql2 = "SELECT COUNT(*) FROM ReplyInfo WHERE 留言ID='" + row[0] + "' AND 所针对的回复的ID='-1' AND 回复用户='" + Session["UserName"] + "'";
                SqlCommand cmd = new SqlCommand(sql2, conn);
                try
                {
                    conn.Open();
                    int record = (int)cmd.ExecuteScalar();
                    if (record == 0)
                    {
                        strHtml += "<div class=\"info_item\">";
                        strHtml += "<div class=\"message_content\">" + row[3] + row[1] + "向我留言：" + row[2] + "</div>";
                        strHtml += "<textarea style=\"height:20px;width:400px;\">请在此输入回复内容</textarea>" 
                            + "<input class=\"replyButton\" type=\"button\" value=\"发表\" data-messageid=\"" 
                            + row[0] + "\" data-replyuser=\"" + Session["UserName"] + "\" data-replyobject=\"" 
                            + row[1] + "\" data-objectid=\"-1\"/>";
                        strHtml += "</div>";
                    }
                }
                catch
                {
                    strHtml = "读取信息失败！";
                }
                finally
                {
                    conn.Close();
                }
            }
            if (string.IsNullOrEmpty(strHtml))
            {
                strHtml = "您没有待处理的留言!";
            }
            content_right_message.InnerHtml = strHtml;
            Label1.Text = "第1页/共1页";
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FirstPage_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PrevPage_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NextPage_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LastPage_Click(object sender, EventArgs e)
        {

        }
    }
}