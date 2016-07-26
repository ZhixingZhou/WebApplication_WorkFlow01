using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using WebApplication_WorkFlow01.WebService;

namespace WebApplication_WorkFlow01.Admin
{
    public partial class AdminMatter : System.Web.UI.Page
    {
        static int rowsCount;
        static int deleteInt;
        static string editName;
        static int editRow;
        static string updateName;
        static int matterInt;
        EFModel.Database1Entities context = new EFModel.Database1Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                div1.Visible = false;
            }
        }
        /// <summary>
        /// 初始化ddlCurrentPage中的项
        /// </summary>
        private void ddlCurrentPageInit()
        {
            this.ddlCurrentPage.Items.Clear();
            for (int i = 1; i <= this.GridViewMatter.PageCount; i++)
            {
                this.ddlCurrentPage.Items.Add(i.ToString());
            }
            this.ddlCurrentPage.SelectedIndex = this.GridViewMatter.PageIndex;
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbtnFrist_Click(object sender, EventArgs e)
        {
            this.GridViewMatter.PageIndex = 0;
            ddlCurrentPageInit();
        }
        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbtnPre_Click(object sender, EventArgs e)
        {
            if (this.GridViewMatter.PageIndex > 0)
            {
                this.GridViewMatter.PageIndex = this.GridViewMatter.PageIndex - 1;
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
            if (this.GridViewMatter.PageIndex < this.GridViewMatter.PageCount - 1)
            {
                this.GridViewMatter.PageIndex = this.GridViewMatter.PageIndex + 1;
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
            this.GridViewMatter.PageIndex = this.GridViewMatter.PageCount - 1;
            ddlCurrentPageInit();
        }
        /// <summary>
        /// 跳转到第几页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GridViewMatter.PageIndex = this.ddlCurrentPage.SelectedIndex;
            ddlCurrentPageInit();
        }
        /// <summary>
        /// 绑定时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewMatter_DataBound(object sender, EventArgs e)
        {
            ddlCurrentPageInit();
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
        /// 获取GridView的总行数
        /// </summary>
        private void RowCount()
        {
            if (this.GridViewMatter.PageCount > 0)     //  如果页数大于0   
            {
                GridViewMatter.PageIndex = GridViewMatter.PageCount - 1;   //  将当前显示页的索引转到最后一页    
                GridViewMatter.DataBind();         //重新绑定数据，这是十分重要，这样才能到达最后一页   
                int lastSize = GridViewMatter.Rows.Count;           //  然后获得最后一页的行数   
                if (GridViewMatter.PageCount > 1)     //  如果页数大于1页，则计算出   
                {                                                       //  总行数=（总页数-1）* 每页行数 +  最后一页的行数   
                    rowsCount = GridViewMatter.PageSize * (GridViewMatter.PageCount - 1) + lastSize;
                }
                else
                    rowsCount = lastSize;
                GridViewMatter.PageIndex = 0;
            }
            else rowsCount = 0;     //  如果无记录，页显示0  
        }
        /// <summary>
        /// 搜索事项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if (this.txtMatterSearch.Text.Equals(""))
            //{
            //    Js_ShowMessage("请输入要搜索的事项名称！");
            //    return;
            //}
            RowCount();
            GridViewMatter.PageIndex = 0;
            GridViewMatter.DataBind();
            //bool flag = false;
            for (int i = 0; i < rowsCount; ++i)
            {
                if (i % GridViewMatter.PageSize == 0 && i != 0)
                {
                    GridViewMatter.PageIndex = GridViewMatter.PageIndex + 1;
                    GridViewMatter.DataBind();
                }
                if (this.DropDownList_Search.Text.Equals(this.GridViewMatter.Rows[i % GridViewMatter.PageSize].Cells[1].Text))
                {
                    //flag = true;
                    this.GridViewMatter.Rows[i % GridViewMatter.PageSize].BackColor = Color.Red;
                    return;
                }
            }
            //if (!flag)
            //{
            //    Js_ShowMessage("该事项不存在！");
            //    this.txtMatterSearch.Text = "";
            //}
        }

        //protected void GridViewMatter_RowDeleted(object sender, GridViewDeletedEventArgs e)
        //{
        //    string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(strConn);
        //    conn.Open();
        //    SqlCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = "delete MatterRelatedDepartment where 事项ID='" + deleteInt + "'";
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //    Js_ShowMessage("删除成功！");
        //    this.GridViewMatter.DataBind();
        //}
        /// <summary>
        /// 捕获用户点击详情事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewMatter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "detailed")
            {
                div1.Visible = true;
                Button btn = (Button)e.CommandSource;
                GridViewRow row = (GridViewRow)btn.Parent.Parent;
                int a = int.Parse(row.Cells[0].Text);
                string sss = row.Cells[1].Text;
                //以下这行代码添加人：张俊玲
                HiddenField1.Value = string.Format("{0}",a);

                var q = from t in context.MatterRelatedDepartment where t.事项ID == a orderby t.步骤号 ascending select t.部门ID;
                string strArr1 = string.Empty;
                int x = 0;
                foreach (var i in q)
                {
                    strArr1 += i.ToString() + ";";
                    x++;
                }
                GetHangNO.flowDepartment = strArr1;
                GetHangNO.stepNum = x;
                GetHangNO.matterId = a;
                GetHangNO.matterName = sss;
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "scripthello", "<script>DescriptFlow();</script>");
            }
        }
        /// <summary>
        /// 删除事项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewMatter_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            deleteInt = int.Parse(GridViewMatter.Rows[e.RowIndex].Cells[0].Text);
            string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "delete MatterRelatedDepartment where 事项ID='" + deleteInt + "'";
                cmd.ExecuteNonQuery();
                conn.Close();
                Js_ShowMessage("删除成功！");
                this.GridViewMatter.DataBind();
                this.DropDownList_Search.DataBind();
                Response.Write("<script language=javascript>window.location.reload( );</script>");
            }
            catch (Exception)
            {
                Js_ShowMessage("删除失败！");
                throw;
            }
        }
        /// <summary>
        /// 添加事项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.txtMatterAdd.Text.Equals(""))
            {
                Js_ShowMessage("事项名称不能为空！");
            }
            else
            {

                var q = from t in context.Matter
                        select t.事项名称;
                foreach (var i in q)
                {
                    if (i.Equals(this.txtMatterAdd.Text))
                    {
                        Js_ShowMessage("该事项已经存在了！");
                        this.txtMatterAdd.Text = "";
                        return;
                    }
                }
                string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into Matter(事项名称) values('" + txtMatterAdd.Text + "')";
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Js_ShowMessage("添加成功！");
                    this.txtMatterAdd.Text = "";
                    GridViewMatter.DataBind();
                    this.DropDownList_Search.DataBind();
                    GridViewMatter.PageIndex = GridViewMatter.PageCount - 1;   //  将当前显示页的索引转到最后一页    
                    GridViewMatter.DataBind();         //重新绑定数据，这是十分重要，这样才能到达最后一页   
                    int lastSize = GridViewMatter.Rows.Count;
                    this.GridViewMatter.Rows[lastSize - 1].BackColor = Color.Red;
                }
                catch (Exception)
                {
                    Js_ShowMessage("对不起，添加失败！");
                    throw;
                }
            }
        }
        /// <summary>
        /// 编辑之前获取要编辑的事项ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewMatter_RowEditing(object sender, GridViewEditEventArgs e)
        {
            editRow = e.NewEditIndex;
            editName = GridViewMatter.Rows[editRow].Cells[1].Text;
            matterInt = int.Parse(GridViewMatter.Rows[editRow].Cells[0].Text);
        }
        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewMatter_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            updateName = ((TextBox)(GridViewMatter.Rows[e.RowIndex].Cells[1].Controls[0])).Text;
            //updatePlace = ((TextBox)(GridViewDepartment.Rows[e.RowIndex].Cells[2].Controls[0])).Text;
            if (updateName.Equals(""))
            {
                Js_ShowMessage("部门名称不能为空！");
                e.Cancel = true;
                GridViewMatter.DataBind();
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
                cmd.CommandText = "update MatterRelatedDepartment set 事项名称='" + updateName + "' where 事项ID='" + matterInt + "'";
                cmd.ExecuteNonQuery();
                conn.Close();
                Js_ShowMessage("修改成功！");
                this.GridViewMatter.DataBind();
                this.DropDownList_Search.DataBind();
            }
            catch (Exception)
            {
                Js_ShowMessage("对不起，修改失败！");
                throw;
            }
        }

        protected void GridViewMatter_RowDataBound(object sender, GridViewRowEventArgs e)
        {   
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //鼠标经过时，行背景色变 
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#2589d3'");
                //鼠标移出时，行背景色变 
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
                
            }
        }
        /// <summary>
        /// 点击“上传附件”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("UploadFileWebForm.aspx?matterid={0}", Server.UrlEncode(HiddenField1.Value)));
        }
    }
}