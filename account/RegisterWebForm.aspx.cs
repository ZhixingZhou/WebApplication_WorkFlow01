using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace WebApplication_WorkFlow01.Account
{
    public partial class RegisterWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Session["UserFlag"] = false;
            }
            SubmitButton.Click += new EventHandler(SubmitButton_Click);
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (Password.Text != ConfirmPassword.Text)
            {
                Label1.Text = "两次输入的密码不一致！";
                return;
            }
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = @"insert into LoginInfo values('"+Username.Text+"','"+Password.Text+"','游客')";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                int number = cmd.ExecuteNonQuery();
                if (number == 1)
                {
                    Label1.Text = "注册成功！请点击登录~";
                }
            }
            catch
            {
                Label1.Text = "注册失败！该名称已被注册";
            }
            finally
            {
                conn.Close();
            }
        }
    }
}