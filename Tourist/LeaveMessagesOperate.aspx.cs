using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication_WorkFlow01.Tourist
{
    public partial class LeaveMessagesOperate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT 部门ID,部门名称 FROM Department";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable table_Department = new DataTable();
            adapter.Fill(table_Department);
            int rowNum = table_Department.Rows.Count;
            string strHtml = "<select id=\"select_department\" data-user=\"" + Session["UserName"] + "\">";
            for (int i = 0; i < rowNum; i++)
            {
                DataRow row = table_Department.Rows[i];
                if (i == 0)
                {
                    strHtml += "<option selected=\"selected\" value=\"" + row[0] +"\">" + row[1] + "</option>";
                }
                else
                {
                    strHtml += "<option value=\"" + row[0] + "\">" + row[1] + "</option>";
                }
            }
            strHtml += "</select>";
            departmentSelection.InnerHtml = strHtml;
        }
    }
}