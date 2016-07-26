using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication_WorkFlow01.Account
{
    public partial class LoginWebForm : System.Web.UI.Page
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
            /*
            if (string.IsNullOrEmpty(username.Text))
            {
                Label1.Text = "请输入用户名！";
                return;
            }
            if (string.IsNullOrEmpty(password.Text))
            {
                Label1.Text = "请输入密码";
                return;
            }
             * */
            Label1.Text = "正在验证……";
            EFModel.Database1Entities context = new EFModel.Database1Entities();
            var q = from t in context.LoginInfo
                    where t.用户名 == Username.Text
                    select t;
            if (q.Count() == 0)
            {
                Label1.Text = "用户名不存在！";
            }
            else
            {
                if (Password.Text == q.First().密码)
                {
                    Label1.Text = "登录成功！即将为您跳转……";
                    Session["UserFlag"] = true;
                    Session["UserName"] = Username.Text;
                    Session["UserType"] = q.First().用户类型;
                    //Response.Redirect(string.Format("WebForm1.aspx?userName={0}", Server.UrlEncode(q.First().用户名)));
                    if (q.First().用户类型 == "超级管理员")
                    {
                        Response.Redirect("../Admin/AdminWebForm.aspx");
                    }
                    else if (q.First().用户类型 == "部门")
                    {
                        Response.Redirect("../Department/DepartmentWebForm.aspx");
                    }
                    else
                    {
                        Response.Redirect("../Tourist/TouristWebForm.aspx");
                    }
                }
                else
                {
                    Label1.Text = "密码不正确！";
                }
            }
        }
    }
}