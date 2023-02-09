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
    public partial class DangNhap : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                Response.Redirect("Default.aspx");
            }
        }

        protected void Check(object sender, EventArgs e)
        {
            //trước khi xác thực
            //OnloggedIn : sau khi xác thực
            
        }
        protected void LoginErr(object sender, EventArgs e)
        {
            //khi đăng nhập thất bại
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            //Xác thực
            string tentaikhoan = Login1.UserName.Trim();
            string matkhau = Login1.Password.Trim();
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
                                if (data["sMatkhau"].ToString().Equals(matkhau))
                                {
                                    
                                   
                                    FormsAuthentication.RedirectFromLoginPage(Login1.UserName, true,"Default.aspx");
                                   // Response.Redirect("test.aspx");
                                    check = 2;
                                    break;                                  
                                }
                                else
                                {
                                    lblthongbao.Text = "Sai mật khẩu, vui lòng nhập lại";
                                    check = 1;
                                    break;
                                    
                                }
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
                lblthongbao.Text = "Không tìm thấy tài khoản";
                e.Authenticated = false;
            }


        }

    }
}