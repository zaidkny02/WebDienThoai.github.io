using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestUserSQL
{
    public partial class DangKy : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormsAuthentication.SignOut();
            }
        }
        protected bool Kiemtra()
        {
            if (txtTaiKhoan.Value.Length < 6)
            {
                lblthongbao.Text = "Tên tài khoản quá ngắn";
                txtTaiKhoan.Focus();
                return false;
            }
            if (txtPass.Value.Length < 8)
            {
                lblthongbao.Text = "Mật khẩu quá ngắn";
                txtPass.Focus();
                return false;
            }
            
            return true;
            
        }
        protected void dangkytaikhoan_Click(object sender, EventArgs e)
        {
            if (Kiemtra())
            {
                string tentaikhoan = txtTaiKhoan.Value.Trim();
                string pass = txtPass.Value.Trim();
                int check = 0;
                string sql = "select * from NGUOIDUNG";
                using (SqlConnection cnn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        SqlDataReader data = cmd.ExecuteReader();
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                if (data["sTentaikhoan"].ToString().Equals(tentaikhoan))
                                {
                                    lblthongbao.Text = "Đã có tài khoản này";
                                    txtTaiKhoan.Focus();
                                    check = 1;
                                    break;
                                }
                            }
                        }
                        else
                            check = 0;
                        cnn.Close();
                    }

                }

                if (check == 0)
                {
                    string insertsql = "  insert into NGUOIDUNG(sTentaikhoan,sMatkhau,FK_iMaquyen) values (" + "N'" + tentaikhoan + "',N'" + pass + "',2);  ";
                    //  SET IDENTITY_INSERT tbl_taikhoan ON;
                    using (SqlConnection cnn = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(insertsql, cnn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cnn.Open();
                            cmd.ExecuteNonQuery();
                            cnn.Close();
                        }
                    }
                    lblthongbao.Text = "Đăng ký thành công";
                }

            }
        }




    }

}