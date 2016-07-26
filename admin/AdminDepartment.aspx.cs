using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using WebApplication_WorkFlow01.WebService;

namespace WebApplication_WorkFlow01.Admin
{
    public partial class AdminDepartment : System.Web.UI.Page
    {
        static int rowsCount;
        static string editName;
        //static string editPlace;
        static int editRow;
        static string updateName;
        //static string updatePlace;
        static string deleteStr;
        static int deleteInt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.GridViewDepartment.DataBind();
            }
        }
        /// <summary>
        /// 初始化ddlCurrentPage
        /// </summary>
        private void ddlCurrentPageInit()
        {
            this.ddlCurrentPage.Items.Clear();
            for (int i = 1; i <= this.GridViewDepartment.PageCount; i++)
            {
                this.ddlCurrentPage.Items.Add(i.ToString());
            }
            if (this.GridViewDepartment.PageCount > 0)
            {
                this.ddlCurrentPage.SelectedIndex = this.GridViewDepartment.PageIndex;
            }
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbtnFrist_Click(object sender, EventArgs e)
        {
            this.GridViewDepartment.PageIndex = 0;
            ddlCurrentPageInit();
        }
        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbtnPre_Click(object sender, EventArgs e)
        {
            if (this.GridViewDepartment.PageIndex > 0)
            {
                this.GridViewDepartment.PageIndex = this.GridViewDepartment.PageIndex - 1;
                ddlCurrentPageInit();
            }
        }
        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbtnNext_Click(object sender, EventArgs e)
        {
            if (this.GridViewDepartment.PageIndex < this.GridViewDepartment.PageCount - 1)
            {
                this.GridViewDepartment.PageIndex = this.GridViewDepartment.PageIndex + 1;
                ddlCurrentPageInit();
            }
        }
        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbtnLast_Click(object sender, EventArgs e)
        {
            this.GridViewDepartment.PageIndex = this.GridViewDepartment.PageCount - 1;
            ddlCurrentPageInit();
        }
        /// <summary>
        /// 跳转到某页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GridViewDepartment.PageIndex = this.ddlCurrentPage.SelectedIndex;
            ddlCurrentPageInit();
        }
        /// <summary>
        /// gridview绑定时初始化ddlCurrentPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewDepartment_DataBound(object sender, EventArgs e)
        {
            ddlCurrentPageInit();
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddDepartment_Click(object sender, EventArgs e)
        {
            if (this.txtDepartmentName.Text.Equals(""))
            {
                Js_ShowMessage("部门名称不能为空！");
            }
            else
            {
                EFModel.Database1Entities context = new EFModel.Database1Entities();
                var q = from t in context.Department
                        select t.部门名称;
                foreach (var i in q)
                {
                    if (i.Equals(this.txtDepartmentName.Text))
                    {
                        Js_ShowMessage("该部门已经存在了！");
                        this.txtDepartmentName.Text = "";
                        return;
                    }
                }
                string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into Department(部门名称,部门地点) values('" + txtDepartmentName.Text + "','" + txtDepartmentPlace.Text + "');insert into LoginInfo values('" + txtDepartmentName.Text + "','0123456789','部门')";
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Js_ShowMessage("添加成功！");
                    this.txtDepartmentName.Text = "";
                    GridViewDepartment.DataBind();
                    this.DropDownList_Search.DataBind();
                    GridViewDepartment.PageIndex = GridViewDepartment.PageCount - 1;   //  将当前显示页的索引转到最后一页    
                    GridViewDepartment.DataBind();         //重新绑定数据，这是十分重要，这样才能到达最后一页   
                    int lastSize = GridViewDepartment.Rows.Count;
                    this.GridViewDepartment.Rows[lastSize - 1].BackColor = Color.Red;
                }
                catch (Exception)
                {
                    Js_ShowMessage("对不起，添加失败！");
                    throw;
                }
            }
        }
        private void Js_ShowMessage(string message)
        {
            Response.Write("<script language=javascript>alert(\"" + message.Replace("\r\n", "\\n") + "\")</script>");
        }
        private void Js_Redirect(string url)
        {
            Response.Write("<script language=javascript>window.location.href='" + url + "'</script>");
        }
        /// <summary>
        /// 修改部门信息后更新部门登记用户名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void GridViewDepartment_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        //{
        //    string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(strConn);
        //    conn.Open();
        //    SqlCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = "update LoginInfo set 用户名='" + updateName + "' where 用户名='" + editName + "'";
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //    Js_ShowMessage("修改成功！");
        //}
        /// <summary>
        /// 统计gridview总共有多少行
        /// </summary>
        private void RowCount()
        {
            if (this.GridViewDepartment.PageCount > 0)     //  如果页数大于0   
            {
                GridViewDepartment.PageIndex = GridViewDepartment.PageCount - 1;   //  将当前显示页的索引转到最后一页    
                GridViewDepartment.DataBind();         //重新绑定数据，这是十分重要，这样才能到达最后一页   
                int lastSize = GridViewDepartment.Rows.Count;           //  然后获得最后一页的行数   
                if (GridViewDepartment.PageCount > 1)     //  如果页数大于1页，则计算出   
                {                                                       //  总行数=（总页数-1）* 每页行数 +  最后一页的行数   
                    rowsCount = GridViewDepartment.PageSize * (GridViewDepartment.PageCount - 1) + lastSize;
                }
                else
                    rowsCount = lastSize;
                //GridViewDepartment.PageIndex = 0;
            }
            else rowsCount = 0;     //  如果无记录，页显示0  
        }
        /// <summary>
        /// 搜索部门
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if (this.txtDepartmentSearch.Text.Equals(""))
            //{
            //    Js_ShowMessage("请输入要搜索的部门名称！");
            //    return;
            //}
            RowCount();
            GridViewDepartment.PageIndex = 0;
            GridViewDepartment.DataBind();
            //bool flag = false;
            for (int i = 0; i < rowsCount; ++i)
            {
                if (i % GridViewDepartment.PageSize == 0 && i != 0)//翻页
                {
                    GridViewDepartment.PageIndex = GridViewDepartment.PageIndex + 1;
                    GridViewDepartment.DataBind();
                }
                if (this.DropDownList_Search.Text.Equals(this.GridViewDepartment.Rows[i % GridViewDepartment.PageSize].Cells[1].Text))
                {
                    //flag = true;
                    this.GridViewDepartment.Rows[i % GridViewDepartment.PageSize].BackColor = Color.Red;//标记红色
                    return;
                }
            }
            //if (!flag)
            //{
            //    Js_ShowMessage("该部门不存在！");
            //    this.txtDepartmentSearch.Text = "";
            //}
        }
        //编辑之前记下原先的部门名称和地点
        protected void GridViewDepartment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            editRow = e.NewEditIndex;
            editName = GridViewDepartment.Rows[editRow].Cells[1].Text;
            //editPlace = GridViewDepartment.Rows[editRow].Cells[2].Text;
        }
        //更新之前
        protected void GridViewDepartment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //updateName = GridViewDepartment.Rows[editRow].Cells[1].Text;
            //updatePlace = GridViewDepartment.Rows[editRow].Cells[2].Text;
            updateName = ((TextBox)(GridViewDepartment.Rows[e.RowIndex].Cells[1].Controls[0])).Text;
            //updatePlace = ((TextBox)(GridViewDepartment.Rows[e.RowIndex].Cells[2].Controls[0])).Text;
            if (updateName.Equals(""))
            {
                Js_ShowMessage("部门名称不能为空！");
                e.Cancel = true;
                GridViewDepartment.DataBind();
                return;
                //updateName = editName;
                //GridViewDepartment.Rows[editRow].Cells[1].Text = editName;
            }
            string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update LoginInfo set 用户名='" + updateName + "' where 用户名='" + editName + "'";
                cmd.ExecuteNonQuery();
                conn.Close();
                Js_ShowMessage("修改成功！");
            }
            catch (Exception)
            {
                Js_ShowMessage("对不起，修改失败！");
                throw;
            }
        }
        ///// <summary>
        ///// 删除部门之前获取部门名称和部门ID
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void GridViewDepartment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    deleteStr = GridViewDepartment.Rows[e.RowIndex].Cells[1].Text;
        //    deleteInt = int.Parse(GridViewDepartment.Rows[e.RowIndex].Cells[0].Text);
        //    string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(strConn);
        //    try
        //    {
        //        conn.Open();
        //        SqlCommand cmd = conn.CreateCommand();
        //        cmd.CommandText = "delete LoginInfo where 用户名='" + deleteStr + "';delete MatterRelatedDepartment where 部门ID='" + deleteInt + "'";
        //        cmd.ExecuteNonQuery();
        //        conn.Close();
        //        Js_ShowMessage("删除成功！");
        //        this.GridViewDepartment.DataBind();
        //    }
        //    catch (Exception)
        //    {
        //        Js_ShowMessage("对不起，删除失败！");
        //        throw;
        //    }
        //}

        protected void GridViewDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //鼠标经过时，行背景色变 
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#2589d3'");
                //鼠标移出时，行背景色变 
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");

            }
        }

        protected void GridViewDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Button btn = (Button)e.CommandSource;
                GridViewRow row = (GridViewRow)btn.Parent.Parent;
                //int deleteInt = int.Parse(row.Cells[0].Text);//部门ID
                DeleteDepartmentClass.departmentId = int.Parse(row.Cells[0].Text);
                DeleteDepartmentClass.departmentName = row.Cells[1].Text;
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "scrillo", "<script>IsDeleteDepartment();</script>");
            }
        }
        /// <summary>
        /// 删除该部门的登录信息和与该部门有关的事项步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void GridViewDepartment_RowDeleted(object sender, GridViewDeletedEventArgs e)
        //{
        //    string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(strConn);
        //    conn.Open();
        //    SqlCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = "delete LoginInfo where 用户名='" + deleteStr + "';delete MatterRelatedDepartment where 部门ID='" + deleteInt + "'";
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //    Js_ShowMessage("删除成功！");
        //}

    }
}