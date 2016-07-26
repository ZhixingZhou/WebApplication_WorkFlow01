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
    public partial class TouristAccountManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SubmitButton.Click += new EventHandler(SubmitButton_Click);
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(OldPassword.Text) || string.IsNullOrEmpty(NewPassword.Text)||string.IsNullOrEmpty(ConfirmNewPassword.Text))
            {
                Label4.Text = "请输入内容！";
                return;
            }
            if (OldPassword.Text.Length != 10 || NewPassword.Text.Length != 10 || ConfirmNewPassword.Text.Length != 10)
            {
                Label4.Text = "密码必须为10位！";
                return;
            }
            if (NewPassword.Text != ConfirmNewPassword.Text)
            {
                Label4.Text = "两次输入的新密码不同！";
                return;
            }
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            string param_username = Session["UserName"].ToString();
            string sql = "select 密码 from LoginInfo where 用户名='" + param_username + "'";
            //string sql = @"update LoginInfo set 密码='" + NewPassword.Text + "' where 用户名='" + Session["UserName"].ToString() + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                //cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Session["UserName"].ToString();
                SqlDataReader r = cmd.ExecuteReader();
                r.Read();
                if (r[0].ToString() != OldPassword.Text)
                {
                    Label4.Text = "您输入的旧密码不正确！";
                    r.Close();
                    return;
                }
                else
                {
                    r.Close();
                    string sql01 = @"update LoginInfo set 密码='" + NewPassword.Text + "' where 用户名='" + Session["UserName"].ToString() + "'";
                    cmd.CommandText = sql01;
                    int num = cmd.ExecuteNonQuery();
                    if (num == 1)
                    {
                        Label4.Text = "修改密码成功！";
                    }
                }
            }
            catch
            {
                Label4.Text = "修改密码失败";
            }
            finally
            {
                conn.Close();
            }
        }
    }
}