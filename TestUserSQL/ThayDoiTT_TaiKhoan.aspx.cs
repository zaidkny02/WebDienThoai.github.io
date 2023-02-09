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
    public partial class WebForm3 : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int idmember = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                    Response.Redirect("TrangItem.aspx");
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
                    idmember = memb.idnguoidung;
                   // Response.Write(idmember);
                    memb.tentaikhoan = data["sTentaikhoan"].ToString();
                    memb.name = data["sHovaten"].ToString();
                    memb.idquyen = Int32.Parse(data["FK_iMaquyen"].ToString());
                    txtName.Text = memb.name;
                    txtSDT.Text = data["sSDT"].ToString();
                    if (!data["dNgaysinh"].ToString().Equals(""))
                    {
                        txtNgaysinh.Text = data["dNgaysinh"].ToString();
                        DateTime a = new DateTime();
                        a = DateTime.Parse(data["dNgaysinh"].ToString());
                        txtNgaysinh.Text = a.ToString("yyyy-MM-dd");
                    }
              //      Response.Write(a.ToShortDateString());
               //     txtNgaysinh.Text = DateTime.Now.ToShortDateString();

                    txtCMT.Text = data["sCMT"].ToString();
                    txtDiachi.Text = data["sDiachi"].ToString();
                    
                }
                cnn.Close();
                #endregion
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
           // txtNgaysinh.Text = DateTime.Today.ToString("yyyy-MM-dd");
            string Name = txtName.Text.Trim();
            string SDT = txtSDT.Text.Trim();
            string Ngaysinh = txtNgaysinh.Text;
            
         
            string CMT = txtCMT.Text.Trim();
            string Diachi = txtDiachi.Text.Trim();
           // DateTime Ns = txtNgaysinh.Text.ToString("yyyy-MM-dd");
            string sql;
            if (!Ngaysinh.Equals(""))
            {
                DateTime dt = DateTime.Parse(txtNgaysinh.Text);
                sql = "update NGUOIDUNG set sHovaten = N'" + Name + "', sSDT = '" + SDT + "', dNgaysinh = '" + dt.ToShortDateString() + "', ";
                sql = sql + " sCMT = '" + CMT + "', sDiachi = N'" + Diachi + "' where PK_iMataikhoan = " + idmember;
            }
            else
            {
                sql = "update NGUOIDUNG set sHovaten = N'" + Name + "', sSDT = '" + SDT + "', ";
                sql = sql + " sCMT = '" + CMT + "', sDiachi = N'" + Diachi + "' where PK_iMataikhoan = " + idmember;
            }
         //   Response.Write(sql);
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            lblThongBaoLoi.Text = "Cập nhật thành công";
        }
    }
}