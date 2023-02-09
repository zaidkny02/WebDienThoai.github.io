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
    public partial class test : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(User.Identity.IsAuthenticated == false)
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
                memb.tentaikhoan = data["sTentaikhoan"].ToString();
                memb.name = data["sHovaten"].ToString();
                memb.idquyen = Int32.Parse(data["FK_iMaquyen"].ToString());

            }
            cnn.Close();
            #endregion
            if (memb.idquyen != 0 && memb.idquyen != 1)
            {
                Response.Redirect("TrangItem.aspx");
            }
            if (memb.idquyen != 0)
                backupbtn.Visible = false;
            lbl.Text = "Xin chào  " + memb.name;

            LayDonHang();


        }

        protected void LayDonHang()
        {
            string sql = "select count(PK_iMahoadon) as [soluong] from HOADON where iTrangthai = 0";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                int soluong = 0;
                soluong = Int32.Parse(data["soluong"].ToString());
                if (soluong > 0)
                    lbldonhang.Text = "Có " + soluong + " đơn hàng đang chờ xác nhận";
            }
            cnn.Close();
        }

        protected void logoutbtn_Click(object sender, EventArgs e)
        {
            
            //Session.Abandon();
            
            FormsAuthentication.SignOut();
            Response.Redirect("TrangItem.aspx");
        }

        protected void phanquyenbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("PhanQuyen.aspx");
        }

        protected void xacnhandonhangbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("XacNhanDonHang.aspx");
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated == false)
                Response.Redirect("TrangItem.aspx");
        }

        protected void capnhatspbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CapNhatSanPham.aspx");
        }

        protected void phieunhapbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ThemPhieuNhap.aspx");
        }

        protected void baocaohoadonbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaoCaoHoaDon.aspx");
        }

        protected void themmoibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ThemMoiSP.aspx");
        }

        protected void nccbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CapNhatNCC.aspx");
        }

        protected void thuonghieubtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CapNhatThuongHieu.aspx");
        }

        protected void khuyenmaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CapNhatKhuyenMai.aspx");
        }

        protected void dshoadon_dathanhtoan_Click(object sender, EventArgs e)
        {
            Response.Redirect("HoaDon_DaThanhToan.aspx");
        }

        protected void backupbtn_Click(object sender, EventArgs e)
        {
            try
            {
                string backuplocation = Server.MapPath("~/BackUp/");
                string sql = "backup database TestDA_test1 to disk ='" + backuplocation + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak'";
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                lbl.Text = lbl.Text + "</br>Tạo file backup thành công";
            }
            catch (Exception ex)
            {
                lbl.Text = lbl.Text + "</br>Có lỗi xảy ra khi backup: "+ ex.ToString();
            }
        }
    }
}