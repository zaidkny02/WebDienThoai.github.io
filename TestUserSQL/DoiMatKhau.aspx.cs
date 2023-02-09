using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestUserSQL
{
    public partial class WebForm16 : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int idnguoidung = 0;
        static string currentpass;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                Response.Redirect("TrangItem.aspx");
            }
            checkmember();
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            if (idnguoidung != 0)
            {
                string oldpass = txtOldPass.Value.Trim();
                string newpass = txtNewPass.Value.Trim();
                string confirm = txtConfirm.Value.Trim();
                if (checkdata(oldpass, newpass, confirm))
                {
                    string sql = "Update NGUOIDUNG set sMatkhau= '" + newpass + "' where PK_iMataikhoan = " + idnguoidung;
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    lblthongbao.Text = "Cập nhật thành công";
                }
            }
            


        }

        protected bool checkdata(string oldpass,string newpass,string confirm)
        {
            if (!newpass.Equals(confirm))
            {
                lblthongbao.Text = "Nhập lại mật khẩu phải trùng";
                return false;
            }
            if (newpass.Equals(oldpass))
            {
                lblthongbao.Text = "Mật khẩu mới phải khác mật khẩu cũ";
                return false;
            }

            if (!oldpass.Equals(currentpass.Trim()))
            {
                lblthongbao.Text = "Mật khẩu cũ không chính xác";
                return false;
            }
            
            return true;
        }



        protected bool checkmember()
        {
            if (User.Identity.IsAuthenticated != false)
            {
                #region checkmember
                Member memb = new Member();
                string sql = "select * from NGUOIDUNG where sTentaikhoan = '" + User.Identity.Name + "'";
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                SqlDataReader data = cmd.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    memb.idnguoidung = Int32.Parse(data["PK_iMataikhoan"].ToString());
                    memb.tentaikhoan = data["sTentaikhoan"].ToString();
                    memb.name = data["sHovaten"].ToString();
                    memb.idquyen = Int32.Parse(data["FK_iMaquyen"].ToString());
                    currentpass = data["sMatkhau"].ToString();
                    idnguoidung = Int32.Parse(memb.idnguoidung.ToString());
                    cnn.Close();
                    return true;
                }
                #endregion
            }
            return false;
        }
    }
}