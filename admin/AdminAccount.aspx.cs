using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace WebApplication_WorkFlow01.Admin
{
    public partial class AdminAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void Js_ShowMessage(string message)
        {
            Response.Write("<script language=javascript>alert(\"" + message.Replace("\r\n", "\\n") + "\")</script>");
        }
        private void Js_Redirect(string url)
        {
            Response.Write("<script language=javascript>window.location.href='" + url + "'</script>");
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string password=string.Empty;
            string str = this.yanZhengMa.Text.ToString();
            string ss = str[0].ToString() + str[1].ToString() + str[2].ToString().ToUpper() + str[3].ToString();
            if (Session["validate"].Equals(ss))
            {
                string name=Session["UserName"].ToString();
                EFModel.Database1Entities context = new EFModel.Database1Entities();
                var query = from t in context.LoginInfo where t.用户名 ==name select t.密码;
                foreach (var q in query)
                {
                    password = q;
                }
                if (password.Equals(this.oldPassword.Text.ToString()))
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
                    SqlConnection conn = new SqlConnection(connectionString);
                    string sql = "update LoginInfo set 密码='" + NewPassword.Text + "' where 用户名='" + name + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        Js_ShowMessage("恭喜您，密码修改成功！请重新登录！");
                        Session["UserFlag"] = false;
                        Js_Redirect("../Index.aspx");
                        Response.End();
                    }
                    catch (Exception)
                    {
                        Js_ShowMessage("对不起，密码修改失败！");
                        Js_Redirect("AdminAccount.aspx");
                        Response.End();
                        throw;
                    }
                }
                else
                {
                    Response.Write("<script>alert('对不起，您输入的原始密码错误！');</script>");
                    Js_Redirect("AdminAccount.aspx");
                }
            }
            else
            {
                Response.Write("<script>alert('验证码错误！');</script>");
                Js_Redirect("AdminAccount.aspx");
            }
        }
    }
}