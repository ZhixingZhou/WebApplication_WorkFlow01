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

namespace WebApplication_WorkFlow01.Department
{
    public partial class EditProcess : System.Web.UI.Page
    {
        static int rowsCount;
        static string name;
        static int departmentId;
        static int a;
        static int officeId;
        static string strName;
        static string s = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
                //div1.Visible = false;
                //this.btnSave.Enabled = false;
                DropDownList_Bind();
            }
        }
        //代码绑定gridview
        public void Bind()
        {
            EFModel.Database1Entities context = new EFModel.Database1Entities();
            string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            name = HttpContext.Current.Session["UserName"].ToString();
            var q = from t in context.Department where t.部门名称 == name select t.部门ID;
            foreach (var v in q)
            {
                departmentId = v;
            }
            conn.Open();
            string sqlStr = "select 步骤号,事项名称 from MatterRelatedDepartment where 部门ID='" + departmentId + "'";
            SqlDataAdapter myda = new SqlDataAdapter(sqlStr, conn);
            DataSet myds = new DataSet();
            myda.Fill(myds, name);
            GridView1.DataSource = myds;
            GridView1.DataBind();
            conn.Close();
            
            //MatterRelatedThis.DepartmentId = departmentId;
            //ClientScript.RegisterStartupScript(ClientScript.GetType(), "sclo", "<script>DropDownList_Bind();</script>");
        }
        //绑定Dropdownlist
        public void DropDownList_Bind()
        {
            EFModel.Database1Entities context = new EFModel.Database1Entities();
            DropDownList_Search.Items.Clear();
            DropDownList_Search.Items.Add(new ListItem("请选择您要搜素的事项名称", "0"));
            var query = from t in context.MatterRelatedDepartment orderby t.事项ID where t.部门ID == departmentId select t.事项名称;
            int y = 1;
            foreach (var q1 in query)
            {
                DropDownList_Search.Items.Add(new ListItem(q1.ToString(),y.ToString()));
                y++;
            }
            DropDownList_office.Items.Clear();
            DropDownList_office.Items.Add(new ListItem("请选择科室名称", "0"));
            var query1 = from t1 in context.Office where t1.所属部门ID == departmentId select t1.科室名称;
            int x = 1;
            foreach (var q1 in query1)
            {
                DropDownList_office.Items.Add(new ListItem(q1.ToString(), x.ToString()));
                x++;
            }
        }
        //初始化ddlCurrentPageInit
        private void ddlCurrentPageInit()
        {
            this.ddlCurrentPage.Items.Clear();
            for (int i = 1; i <= this.GridView1.PageCount; i++)
            {
                this.ddlCurrentPage.Items.Add(i.ToString());
            }
            if(this.GridView1.PageCount>0)
                this.ddlCurrentPage.SelectedIndex = this.GridView1.PageIndex;
        }
        //首页
        protected void lkbtnFrist_Click(object sender, EventArgs e)
        {
            this.GridView1.PageIndex = 0;
            ddlCurrentPageInit();
        }
        //上一页
        protected void lkbtnPre_Click(object sender, EventArgs e)
        {
            if (this.GridView1.PageIndex > 0)
            {
                this.GridView1.PageIndex = this.GridView1.PageIndex - 1;
                ddlCurrentPageInit();
            }
        }
        //下一页
        protected void lkbtnNext_Click(object sender, EventArgs e)
        {
            if (this.GridView1.PageIndex < this.GridView1.PageCount - 1)
            {
                this.GridView1.PageIndex = this.GridView1.PageIndex + 1;
                ddlCurrentPageInit();
            }
        }
        //尾页
        protected void lkbtnLast_Click(object sender, EventArgs e)
        {
            this.GridView1.PageIndex = this.GridView1.PageCount - 1;
            ddlCurrentPageInit();
        }
        //跳转到第几页
        protected void ddlCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GridView1.PageIndex = this.ddlCurrentPage.SelectedIndex;
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
        //gridview的总行数
        private void RowCount()
        {
            if (this.GridView1.PageCount > 0)     //  如果页数大于0   
            {
                GridView1.PageIndex = GridView1.PageCount - 1;   //  将当前显示页的索引转到最后一页    
                Bind();         //重新绑定数据，这是十分重要，这样才能到达最后一页   
                int lastSize = GridView1.Rows.Count;           //  然后获得最后一页的行数   
                if (GridView1.PageCount > 1)     //  如果页数大于1页，则计算出   
                {                                                       //  总行数=（总页数-1）* 每页行数 +  最后一页的行数   
                    rowsCount = GridView1.PageSize * (GridView1.PageCount - 1) + lastSize;
                }
                else
                    rowsCount = lastSize;
                GridView1.PageIndex = 0;
            }
            else rowsCount = 0;     //  如果无记录，页显示0  
        }
        //搜素事项
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if (this.txtMatterSearch.Text.Equals(""))
            //{
            //    Js_ShowMessage("请输入要搜索的事项名称！");
            //    return;
            //}
            RowCount();
            GridView1.PageIndex = 0;
            Bind();
            //bool flag = false;
            for (int i = 0; i < rowsCount; ++i)
            {
                if (i % GridView1.PageSize == 0 && i != 0)
                {
                    GridView1.PageIndex = GridView1.PageIndex + 1;
                    Bind();
                }
                if (this.DropDownList_Search.SelectedItem.Text.Equals(this.GridView1.Rows[i % GridView1.PageSize].Cells[1].Text))
                {
                    //flag = true;
                    this.GridView1.Rows[i % GridView1.PageSize].BackColor = Color.Red;
                    return;
                }
            }
            //if (!flag)
            //{
            //    Js_ShowMessage("该事项不存在！");
            //}
        }
        //gridview绑定时初始化ddlCurrentPageInit
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            ddlCurrentPageInit();
        }
        //事项详情
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "detailed")
            {
                this.address.Text = "";
                this.person.Text = "";
                this.remark.Text = "";
                this.DropDownList_office.SelectedIndex = 0;
                this.btnEdit.Enabled = true;
                this.btnSave.Enabled = false;
                this.DropDownList_office.Enabled = false;
                this.address.Enabled = false;
                this.person.Enabled = false;
                this.remark.Enabled = false;
                //this.btnEdit.Enabled = true;
                //Button btn = (Button)e.CommandSource;
                //GridViewRow row = (GridViewRow)btn.Parent.Parent;
                //a = int.Parse(row.Cells[0].Text);
                //strName = row.Cells[1].Text;
                //Label1.Text = strName + "第" + a + "步>>步骤描述";
                //div1.Visible = true;
                //EFModel.Database1Entities context = new EFModel.Database1Entities();
                //var q = from t in context.MatterRelatedDepartment where t.步骤号 == a where t.事项名称 == strName select t.步骤描述;
                //foreach (var x in q)
                //{
                //    this.TextArea1.Value = x;
                //}
                //if (this.TextArea1.InnerText == "")
                //{
                //    this.TextArea1.Value = "尚无描述，请您描述该步骤的要求！";
                //}
                //this.TextArea1.Disabled = true; 
                Button btn = (Button)e.CommandSource;
                GridViewRow row = (GridViewRow)btn.Parent.Parent;
                a = int.Parse(row.Cells[0].Text);
                strName = row.Cells[1].Text;
                HiddenField1.Value = strName;
                Label1.Text = strName + "第" + a + "步>>步骤描述";
                EFModel.Database1Entities context = new EFModel.Database1Entities();
                var query = from t in context.MatterRelatedDepartment where t.步骤号 == a where t.事项名称 == strName select t.步骤描述;
                foreach (var q in query)
                {
                    if (q == null)
                    {
                        return;
                    }
                    else
                    {
                        s = q.ToString();
                    }
                }
                string[] ss = new string[] { };
                ss = s.Split('.');
                for (int i = 0; i < this.DropDownList_office.Items.Count; ++i)
                {
                    if (this.DropDownList_office.Items[i].Text == ss[0])
                    {
                        //this.DropDownList_office.Items[i].Selected = true;
                        this.DropDownList_office.SelectedIndex = i;
                    }
                    //else
                    //{
                    //    this.DropDownList_office.Items[i].Selected = false;
                    //}
                }
                this.address.Text = ss[1];
                this.person.Text = ss[2];
                this.remark.Text = ss[3];
                
            }
        }
        ////编辑
        //protected void btnEdit_Click(object sender, EventArgs e)
        //{
        //    if (this.TextArea1.Value.Equals("尚无描述，请您描述该步骤的要求！"))
        //    {
        //        this.TextArea1.Value = "";
        //    }
        //    this.TextArea1.Disabled = false;
        //    this.btnEdit.Enabled = false;
        //    this.btnSave.Enabled = true;
        //}
        ////保存
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(strConn);
        //    SqlCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = "update MatterRelatedDepartment set 步骤描述='" + this.TextArea1.Value + "' where 步骤号='" + a + "' AND 事项名称='" + strName + "'";
        //    try
        //    {
        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //        conn.Close();
        //        this.TextArea1.Disabled = true;
        //        this.btnEdit.Enabled = true;
        //        this.btnSave.Enabled = false;
        //        Js_ShowMessage("保存成功！");
        //    }
        //    catch (Exception)
        //    {
        //        Js_ShowMessage("对不起，保存失败！");
        //        throw;
        //    }
        //}
        //鼠标滑过变色
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //鼠标经过时，行背景色变 
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#2589d3'");
                //鼠标移出时，行背景色变 
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");

            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            this.DropDownList_office.Enabled = true;
            this.address.Enabled = true;
            this.person.Enabled = true;
            this.remark.Enabled = true;
            this.btnSave.Enabled = true;
            this.btnEdit.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string officeName = "";
            if (this.DropDownList_office.SelectedIndex == 0)
            {
                officeId = 0;
            }
            else
            {
                officeName = this.DropDownList_office.SelectedItem.Text;
                EFModel.Database1Entities context = new EFModel.Database1Entities();
                var query = from t in context.Office where t.科室名称 == officeName select t.科室ID;
                foreach (var q in query)
                {
                    officeId = q;
                }
            }
            string stepDescript = officeName + "." + this.address.Text + "." + this.person.Text + "." + this.remark.Text;

            string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                if (officeId == 0)
                {
                    cmd.CommandText = "update MatterRelatedDepartment set 科室ID=null,步骤描述='" + stepDescript + "' where 步骤号='" + a + "' AND 事项名称='" + strName + "'";
                }
                else
                {
                    cmd.CommandText = "update MatterRelatedDepartment set 科室ID='" + officeId + "',步骤描述='" + stepDescript + "' where 步骤号='" + a + "' AND 事项名称='" + strName + "'";
                }
                cmd.ExecuteNonQuery();
                conn.Close();
                Js_ShowMessage("保存成功！");
                this.DropDownList_office.Enabled = false;
                this.address.Enabled = false;
                this.person.Enabled = false;
                this.remark.Enabled = false;
                this.btnSave.Enabled = false;
                this.btnEdit.Enabled = true;
            }
            catch (Exception)
            {
                Js_ShowMessage("对不起，保存失败了！");
                throw;
            }
        }
    }
}