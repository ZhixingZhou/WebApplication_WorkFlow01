using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using WebApplication_WorkFlow01.WebService;
using System.Data;

namespace WebApplication_WorkFlow01.Department
{
    public partial class DepartmentOffice : System.Web.UI.Page
    {
        string departmentName;
        int departmentId;
        int rowsCount;
        int editOfficeRow;
        string editOfficeName;
        string editOfficePlace;
        string updateOfficeName;
        string updateOfficePlace;
        int officeId;
        EFModel.Database1Entities context = new EFModel.Database1Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            departmentName = Session["UserName"].ToString();
            var query = from t in context.Department where t.部门名称 == departmentName select t.部门ID;
            foreach (var q in query)
            {
                departmentId = q;    
            }
            if (!IsPostBack)
            {
                Bind();
                DropDownList_Bind();
            }
        }
        //代码绑定gridview
        public void Bind()
        {
            EFModel.Database1Entities context = new EFModel.Database1Entities();
            string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            //departmentName = HttpContext.Current.Session["UserName"].ToString();
            var q = from t in context.Department where t.部门名称 == departmentName select t.部门ID;
            foreach (var v in q)
            {
                departmentId = v;
            }
            conn.Open();
            string sqlStr = "select 科室ID,科室名称,科室地点 from Office where 所属部门ID='" + departmentId + "'";
            SqlDataAdapter myda = new SqlDataAdapter(sqlStr, conn);
            DataSet myds = new DataSet();
            myda.Fill(myds, departmentName);
            GridViewOffice.DataSource = myds;
            GridViewOffice.DataBind();
            conn.Close();

            //MatterRelatedThis.DepartmentId = departmentId;
            //ClientScript.RegisterStartupScript(ClientScript.GetType(), "sclo", "<script>DropDownList_Bind();</script>");
        }
        //绑定Dropdownlist
        public void DropDownList_Bind()
        {
            EFModel.Database1Entities context = new EFModel.Database1Entities();
            DropDownList_Search.Items.Clear();
            DropDownList_Search.Items.Add(new ListItem("请选择您要搜素的科室名称", "0"));
            var query = from t in context.Office orderby t.科室ID where t.所属部门ID == departmentId select t.科室名称;
            int y = 1;
            foreach (var q in query)
            {
                DropDownList_Search.Items.Add(new ListItem(q.ToString(), y.ToString()));
                y++;
            }
        }
        /// <summary>
        /// 初始化ddlCurrentPage
        /// </summary>
        private void ddlCurrentPageInit()
        {
            this.ddlCurrentPage.Items.Clear();
            for (int i = 1; i <= this.GridViewOffice.PageCount; i++)
            {
                this.ddlCurrentPage.Items.Add(i.ToString());
            }
            if (this.GridViewOffice.PageCount > 0)
            {
                this.ddlCurrentPage.SelectedIndex = this.GridViewOffice.PageIndex;
            }
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbtnFrist_Click(object sender, EventArgs e)
        {
            this.GridViewOffice.PageIndex = 0;
            ddlCurrentPageInit();
        }
        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbtnPre_Click(object sender, EventArgs e)
        {
            if (this.GridViewOffice.PageIndex > 0)
            {
                this.GridViewOffice.PageIndex = this.GridViewOffice.PageIndex - 1;
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
            if (this.GridViewOffice.PageIndex < this.GridViewOffice.PageCount - 1)
            {
                this.GridViewOffice.PageIndex = this.GridViewOffice.PageIndex + 1;
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
            this.GridViewOffice.PageIndex = this.GridViewOffice.PageCount - 1;
            ddlCurrentPageInit();
        }
        /// <summary>
        /// 跳转到某页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GridViewOffice.PageIndex = this.ddlCurrentPage.SelectedIndex;
            ddlCurrentPageInit();
        }
        /// <summary>
        /// gridview绑定时初始化ddlCurrentPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewOffice_DataBound(object sender, EventArgs e)
        {
            ddlCurrentPageInit();
        }
        /// <summary>
        /// 添加科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddOffice_Click(object sender, EventArgs e)
        {
            if (this.txtOfficeName.Text == "")
            {
                Js_ShowMessage("科室名称不能为空！");
            }
            else if (this.txtOfficePlace.Text == "")
            {
                Js_ShowMessage("请指明科室地点！");
            }
            else
            {
                string officeName = this.txtOfficeName.Text;
                string officePlace = this.txtOfficePlace.Text;
                var query = from t in context.Office select t.科室名称;
                foreach(var q in query)
                {
                    if (q.Equals(officeName))
                    {
                        Js_ShowMessage("该科室已经存在了！");
                        this.txtOfficeName.Text = "";
                        this.txtOfficePlace.Text = "";
                        return;
                    }
                }
                string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(strConn);
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into Office(科室名称,科室地点,所属部门ID) values('" + officeName + "','" + officePlace + "','"+departmentId+"')";
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Js_ShowMessage("添加成功！");
                    this.txtOfficeName.Text = "";
                    this.txtOfficePlace.Text = "";
                    Bind();
                    DropDownList_Bind();
                    GridViewOffice.PageIndex = GridViewOffice.PageCount - 1;   //  将当前显示页的索引转到最后一页    
                    Bind();         //重新绑定数据，这是十分重要，这样才能到达最后一页   
                    int lastSize = GridViewOffice.Rows.Count;
                    this.GridViewOffice.Rows[lastSize - 1].BackColor = Color.Red;
                }
                catch (Exception)
                {
                    Js_ShowMessage("对不起，添加失败！");
                    throw;
                }
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        private void RowCount()
        {
            if (this.GridViewOffice.PageCount > 0)     //  如果页数大于0   
            {
                GridViewOffice.PageIndex = GridViewOffice.PageCount - 1;   //  将当前显示页的索引转到最后一页    
                Bind();         //重新绑定数据，这是十分重要，这样才能到达最后一页   
                int lastSize = GridViewOffice.Rows.Count;           //  然后获得最后一页的行数   
                if (GridViewOffice.PageCount > 1)     //  如果页数大于1页，则计算出   
                {                                                       //  总行数=（总页数-1）* 每页行数 +  最后一页的行数   
                    rowsCount = GridViewOffice.PageSize * (GridViewOffice.PageCount - 1) + lastSize;
                }
                else
                    rowsCount = lastSize;
                //GridViewOffice.PageIndex = 0;
            }
            else rowsCount = 0;     //  如果无记录，页显示0  
        }
        /// <summary>
        /// 搜索科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            RowCount();
            GridViewOffice.PageIndex = 0;
            Bind();
            for (int i = 0; i < rowsCount; ++i)
            {
                if (i % GridViewOffice.PageSize == 0 && i != 0)//翻页
                {
                    GridViewOffice.PageIndex = GridViewOffice.PageIndex + 1;
                    Bind();
                }
                if (this.DropDownList_Search.SelectedItem.Text.Equals(this.GridViewOffice.Rows[i % GridViewOffice.PageSize].Cells[1].Text))
                {
                    this.GridViewOffice.Rows[i % GridViewOffice.PageSize].BackColor = Color.Red;//标记红色
                    return;
                }
            }
        }
        //编辑之前记下原先的科室名称和地点
        protected void GridViewOffice_RowEditing(object sender, GridViewEditEventArgs e)
        {
            editOfficeRow = e.NewEditIndex;
            editOfficeName = GridViewOffice.Rows[editOfficeRow].Cells[1].Text;
            editOfficePlace = GridViewOffice.Rows[editOfficeRow].Cells[2].Text;
            officeId =int.Parse(GridViewOffice.Rows[editOfficeRow].Cells[0].Text);
        }
        //更新之前
        protected void GridViewOffice_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            updateOfficeName = ((TextBox)(GridViewOffice.Rows[e.RowIndex].Cells[1].Controls[0])).Text;
            updateOfficePlace = ((TextBox)(GridViewOffice.Rows[e.RowIndex].Cells[2].Controls[0])).Text;
            if (updateOfficeName.Equals(""))
            {
                Js_ShowMessage("科室名称不能为空！");
                e.Cancel = true;
                Bind();
                return;
            }
            else if (updateOfficePlace.Equals(""))
            {
                Js_ShowMessage("科室地点不能为空！");
                e.Cancel = true;
                Bind();
                return;
            }
            string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update Office set 科室名称='" + updateOfficeName + "',科室地点='"+updateOfficePlace+"' where 科室ID='" + officeId + "'";
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

        protected void GridViewOffice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //鼠标经过时，行背景色变 
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#2589d3'");
                //鼠标移出时，行背景色变 
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");

            }
        }
        protected void GridViewOffice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Button btn = (Button)e.CommandSource;
                GridViewRow row = (GridViewRow)btn.Parent.Parent;
                int deleteInt = int.Parse(row.Cells[0].Text);//科室ID
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "scrillo", "<script>IsDeleteOffice(" + deleteInt + ","+departmentId+");</script>");
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

        
    }
}