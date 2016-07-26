using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication_WorkFlow01.Tourist
{
    public partial class TouristWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
        }
    }
}