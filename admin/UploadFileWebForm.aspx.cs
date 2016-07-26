using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using System.Data.SqlClient;


namespace WebApplication_WorkFlow01.Admin
{
    public partial class UploadFileWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count > 0)
            {
                HiddenField1.Value = Request.QueryString["matterid"];
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!FileUpload1.HasFile)
            {
                Label1.Visible = true;
                Label1.Text = "您还没有选择文件！";
                return;
            }
            string fileName = FileUpload1.FileName;
            //if (File.Exists(MapPath(@"..\UploadFile\Content\" + fileName)))
            //{
            //    //从数据库中读取swf所在的绝对路径，并将该swf文件删除即可
            //}
            //将附件的绝对路劲存入数据库
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = @"UPDATE Matter SET 附件名='" + fileName + "',附件绝对路径='" + MapPath(@"..\UploadFile\Content\" + fileName) + "' WHERE 事项ID=" + HiddenField1.Value;
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch(Exception error)
            {
                Console.WriteLine(error.Message);
            }
            finally
            {
                conn.Close();
            }
            HttpPostedFile postFile = FileUpload1.PostedFile;//Upload上传的控件
            postFile.SaveAs(MapPath(@"..\UploadFile\Content\" + fileName));//保存文件
            if (File.Exists(MapPath(@"..\UploadFile\Content\" + fileName)))
            {
                Label1.Text = "上传成功！";
            }

            PathString ps = new PathString();
            ps.sourceFile = MapPath(@"..\UploadFile\Content\" + fileName);
            ps.swfFile = MapPath(@"..\UploadFile\swf\swf");
            ps.matterId = HiddenField1.Value;
            Thread myThread = new Thread(DoConvert);
            myThread.Start(ps);
        }
        protected void DoConvert(object pathString)
        {
            PathString ps = (PathString)pathString;
            DocConverter converter = new DocConverter();
            converter.init(ps.sourceFile, ps.swfFile);
            bool isSuccess = false;
            int transferTimes = 5;
            do
            {
                isSuccess = converter.convertFunc();
            } while (transferTimes > 0 && !isSuccess);
            if (isSuccess)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);
                string sql = @"UPDATE Matter SET 附件swf相对路径='" + converter.swfShowName + "' WHERE 事项ID=" + ps.matterId;
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception error)
                {
                    Console.WriteLine(error.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}