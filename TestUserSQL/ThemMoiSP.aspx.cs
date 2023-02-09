using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestUserSQL
{
    public partial class WebForm11 : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!checkadmin())
                    Response.Redirect("TrangItem.aspx");
                KhoiTaoDuLieu();
            }
        }

        protected void KhoiTaoDuLieu()
        {
            string sql = "select * from THUONGHIEU ";
            SqlConnection cnn = new SqlConnection(constr);
            #region LayThuongHieu
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ddlThuongHieu.DataSource = dt;
            ddlThuongHieu.DataTextField = "sTenthuonghieu";
            ddlThuongHieu.DataValueField = "PK_iMathuonghieu";
            ddlThuongHieu.DataBind();
            #endregion
        }

        protected void clearinput()
        {
           // lblThongBaoLoi.Text = string.Empty;
            txtTenSP.Text = string.Empty;
            txtRAM.Text = string.Empty;
            txtSoluong.Text = string.Empty;
            txtGiaban.Text = string.Empty;
            txtMota.Text = string.Empty;
            txtBonho.Text = string.Empty;
            txtManhinh.Text = string.Empty;
            txtDungluongpin.Text = string.Empty;
            txtMausac.Text = string.Empty;
            txtGhichu.Text = string.Empty;
        }

        protected void btnThem_Click(object sender, EventArgs e)
        {
            string tensanpham = txtTenSP.Text.Trim();
            if (KiemTraThem(tensanpham))
            {
                string thuonghieu = ddlThuongHieu.SelectedValue;
                string soluong = txtSoluong.Text.Trim();
                string giaban = txtGiaban.Text.Trim();
                string mota = txtMota.Text.Trim();
                string TS_Ram = txtRAM.Text.Trim();
                string TS_Bonho = txtBonho.Text.Trim();
                string TS_dungluongpin = txtDungluongpin.Text.Trim();
                string TS_manhinh = txtManhinh.Text.Trim();
                string TS_mausac = txtMausac.Text.Trim();
                string TS_ghichu = txtGhichu.Text.Trim();
                ThemSP(tensanpham,thuonghieu,soluong,giaban,mota,TS_Ram,TS_Bonho,TS_dungluongpin,TS_manhinh,TS_mausac,TS_ghichu);
                lblThongBaoLoi.Text = "Thêm thành công";
            }


        }

        protected void ThemSP(string tensanpham, string thuonghieu, string soluong, string giaban, string mota, string TS_Ram, string TS_Bonho, string TS_dungluongpin, string TS_manhinh, string TS_mausac, string TS_ghichu)
        {
            SqlConnection cnn = new SqlConnection(constr);
            // Ten Procedure
            string sql = "ThemSP";

            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            //Thông tin cơ bản SP
            cmd.Parameters.AddWithValue("@tensanpham", tensanpham);
            cmd.Parameters.AddWithValue("@mathuonghieu", thuonghieu);
            cmd.Parameters.AddWithValue("@mota", mota);
            cmd.Parameters.AddWithValue("@giaban", giaban);
            cmd.Parameters.AddWithValue("@soluong", soluong);
            //Thông số SP
            cmd.Parameters.AddWithValue("@ram", TS_Ram);
            cmd.Parameters.AddWithValue("@bonho", TS_Bonho);
            cmd.Parameters.AddWithValue("@manhinh", TS_manhinh);
            cmd.Parameters.AddWithValue("@dungluongpin", TS_dungluongpin);
            cmd.Parameters.AddWithValue("@mausac", TS_mausac);
            cmd.Parameters.AddWithValue("@ghichu", TS_ghichu);
            //Hình ảnh SP
            if (fileImport.HasFile && fileImport.FileContent.Length > 0 && kiemtradinhdang(Path.GetExtension(fileImport.FileName)))
            {
                string filename = Path.GetFileNameWithoutExtension(fileImport.FileName);
                filename = filename + DateTime.Now.ToBinary().ToString();
                filename = filename + Path.GetExtension(fileImport.FileName);
                string sFilePath = Server.MapPath("~/Images/") + filename;
                fileImport.SaveAs(sFilePath);
                cmd.Parameters.AddWithValue("@nguonhinhanh", "~/images/" + filename);
            }
            else
            {
                lblThongBaoLoi.Text = "Định dạng ảnh không phù hợp!!";
                cmd.Parameters.AddWithValue("@nguonhinhanh", "");
            }
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            clearinput();
        }

        protected bool kiemtradinhdang(string dinhdanganh)
        {
            if (dinhdanganh.ToLower().Equals(".png") || dinhdanganh.ToLower().Equals(".jpg") || dinhdanganh.ToLower().Equals(".jpeg"))
                return true;
            else
            {
                lblThongBaoLoi.Text = "Định dạng ảnh không phù hợp!!";
                return false;
            }
        }

        protected bool KiemTraThem(string tensanpham)
        {
            string sql = "select * from SANPHAM where sTensanpham = N'" + tensanpham + "'" ;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                cnn.Close();
                lblThongBaoLoi.Text = "Đã có tên sản phẩm này!!";
                return false;

            }
            else
            {
                cnn.Close();
                return true;
            }
        }

        protected void quaylaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected bool checkadmin()
        {
            if (!User.Identity.IsAuthenticated)
                return false;
            #region checkmember
            Member memb = new Member();
            string sql = "select PK_iMataikhoan,FK_iMaquyen,sTentaikhoan,sHovaten from NGUOIDUNG where sTentaikhoan = '" + User.Identity.Name + "'";
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
                return false;
            else
                return true;
        }
    }
}