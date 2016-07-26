using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication_WorkFlow01.Admin
{
    public partial class AdminSite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Session.IsNewSession) && Session["UserFlag"].ToString() == "True")
            {
                string strHtml = "<div id=\"log_out\"><a href=\"../Account/LogoutWebForm.aspx\">退出</a></div>";
                strHtml += "<div id=\"logined_user\">欢迎你！亲爱的<span class=\"userName_span\">" + Session["UserName"] + "</span></div>";
                header.InnerHtml = strHtml;
            }
            else
            {
                if (Session.IsNewSession)
                {
                    Session["UserFlag"] = false;
                }
                Response.Redirect("../Index.aspx");
            }
        }
    }
}