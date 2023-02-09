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
    public partial class WebForm8 : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int idnguoidung = 0;
        static int idphieunhap = 0;
        static int dong_chitiet = 0;
        static Int64 tongtien = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!checkuser())
                    Response.Redirect("TrangItem.aspx");
                KhoiTaoSanPham();

                bool try_Parse;
                if (Request.QueryString["id"] != null)
                    try_Parse = int.TryParse(Request.QueryString["id"].ToString(), out idphieunhap);
                else
                    try_Parse = false;
                if (!try_Parse)
                    Response.Redirect("Default.aspx");
                
                KhoiTaoDuLieu();
            }


        }

        protected void LayTrangThai()
        {
            string sql = "select iTrangthai from PHIEUNHAP where PK_iMaphieunhap = '" + idphieunhap + "'";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            int trangthai = 1;
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                trangthai = Int32.Parse(data["iTrangthai"].ToString());
            }
            cnn.Close();
            if (trangthai == 0)
            {
                addBtn.Enabled = false;
                saveBtn.Enabled = false;
                delBtn.Enabled = false;
                khoaphieuBtn.Enabled = false;
            }
        }

        #region LayDanhSachSP
        protected void KhoiTaoSanPham()
        {
            string sql = " select PK_iMasanpham,sTensanpham from SANPHAM  ";
            SqlConnection cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ddlSanpham.DataSource = dt;
            ddlSanpham.DataTextField = "sTensanpham";
            ddlSanpham.DataValueField = "PK_iMasanpham";
            ddlSanpham.DataBind();
            ListItem item = new ListItem("Chọn", "0");
            ddlSanpham.Items.Insert(0, item);   
        }
        #endregion
        protected void KhoiTaoDuLieu()
        {
            tongtien = 0;
            lblTongtien.Text = string.Empty;
            string sql = "select PK_iCT_PhieuNhapID,FK_iMaphieunhap,sTensanpham,CT_PHIEUNHAP.iDongia,CT_PHIEUNHAP.iSoluong  ";
            sql = sql + " from CT_PHIEUNHAP,SANPHAM,PHIEUNHAP where CT_PHIEUNHAP.FK_iMaphieunhap = PHIEUNHAP.PK_iMaphieunhap ";
            sql = sql + " and CT_PHIEUNHAP.FK_iMasanpham = SANPHAM.PK_iMasanpham and FK_iMaphieunhap = " + idphieunhap;
            SqlConnection cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvChiTietNhap.DataSource = dt;
            grvChiTietNhap.DataBind();
            txtMaPhieu.Text = idphieunhap.ToString();
            ddlSanpham.Enabled = true;
            ddlSanpham.SelectedIndex = 0;
            txtDongia.Text = string.Empty;
            txtSoluong.Text = string.Empty;
            dong_chitiet = 0;
            lblThongBaoLoi.Text = string.Empty;
            LayTrangThai();
        }


        protected bool checkuser()
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
                idnguoidung = memb.idnguoidung;
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

        protected void addBtn_Click(object sender, EventArgs e)
        {
            if (idphieunhap != 0 && idnguoidung != 0)
            {
                
                int Soluong = Int32.Parse(txtSoluong.Text.Trim());
                int Dongia = Int32.Parse(txtDongia.Text.Trim());
                //   string ghichu = txtGhichu.Text.Trim();
                int idSP = Int32.Parse(ddlSanpham.SelectedValue);
                if (KiemTraThem(idSP))
                {
                    string sql = "insert into CT_PHIEUNHAP values (" + idphieunhap + "," + idSP + "," + Soluong + "," + Dongia + ")";
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    KhoiTaoDuLieu();
                }
            }
        }

        protected bool KiemTraThem(int idSP)
        {
            string sql = "select * from CT_PHIEUNHAP where FK_iMaphieunhap = " + idphieunhap + " and FK_iMasanpham = " + idSP;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                cnn.Close();
                lblThongBaoLoi.Text = "Sản phẩm này đã có trong danh sách nhập";
                return false;
                
            }
            else
            {
                cnn.Close();
                return true;
            }
        }


        protected void grvChiTietNhap_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.DataRow:
                    e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvChiTietNhap, "Select$" + e.Row.RowIndex));
                    Label lbldongia = e.Row.FindControl("lbldongia") as Label;
                    Int64 dongia = Int64.Parse(lbldongia.Text.ToString());
                    lbldongia.Text = Convert.ToInt64(dongia) > 10 ? String.Format("{0:0,0}", dongia).Replace(',', '.') : Convert.ToInt64(dongia).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                    Label lblsoluong = e.Row.FindControl("lblsoluong") as Label;
                    int soluong = Int32.Parse(lblsoluong.Text.ToString());
                    Int64 thanhtien = dongia * soluong;
                    tongtien = tongtien + thanhtien;
                    Label lblthanhtien = e.Row.FindControl("lblThanhtien") as Label;
                    lblthanhtien.Text = Convert.ToInt64(thanhtien) > 10 ? String.Format("{0:0,0}", thanhtien).Replace(',', '.') : Convert.ToInt64(thanhtien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                  //  lblTongtien.Text = "Tổng tiền :" + tongtien + " VND";
                    string x= Convert.ToInt64(tongtien) > 10 ? String.Format("{0:0,0}", tongtien).Replace(',', '.') : Convert.ToInt64(tongtien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                    lblTongtien.Text = "Tổng tiền :" + x + " VND";
                    break;
            }
        }

        protected void grvChiTietNhap_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grvChiTietNhap.Rows)
            {
                if (row.RowIndex == grvChiTietNhap.SelectedIndex)
                {
                    string sql = "select PK_iCT_PhieuNhapID,FK_iMasanpham,iDongia,iSoluong ";
                    sql = sql + " from CT_PHIEUNHAP ";
                    sql = sql + " where PK_iCT_PhieuNhapID = " + grvChiTietNhap.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Read();
                        dong_chitiet = Int32.Parse(data["PK_iCT_PhieuNhapID"].ToString());
                        txtDongia.Text = data["iDongia"].ToString();
                        txtSoluong.Text = data["iSoluong"].ToString();
                        string idsp = data["FK_iMasanpham"].ToString();
                        foreach (ListItem item in ddlSanpham.Items)
                        {
                            if (item.Value.Equals(idsp))
                            {
                                ddlSanpham.ClearSelection();
                                item.Selected = true;
                                ddlSanpham.Enabled = false;
                                break;
                            }
                        }
                    }
                    cnn.Close();
                    lblThongBaoLoi.Text = string.Empty;
                }
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            if (grvChiTietNhap.SelectedDataKey != null && dong_chitiet != 0)
            {
                string dongia = txtDongia.Text.Trim();
                string soluong = txtSoluong.Text.Trim();
                string sql = "update CT_PHIEUNHAP set iSoluong = " + soluong + ",iDongia = " + dongia ;
                sql = sql + " where PK_iCT_PhieuNhapID = " + grvChiTietNhap.SelectedDataKey.Value.ToString();
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                
                KhoiTaoDuLieu();
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn dòng sản phẩm";
            
            
        }

        protected void newBtn_Click(object sender, EventArgs e)
        {
            KhoiTaoDuLieu();
        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            if (grvChiTietNhap.SelectedDataKey != null && dong_chitiet != 0)
            {

                string sql = "delete from CT_PHIEUNHAP where PK_iCT_PhieuNhapID = " + grvChiTietNhap.SelectedDataKey.Value.ToString();
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                KhoiTaoDuLieu();
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn dòng sản phẩm";
        }

        protected void khoaphieuBtn_Click(object sender, EventArgs e)
        {
            if (idphieunhap != 0 && grvChiTietNhap.Rows.Count > 0)
            {
                string sql = "update PHIEUNHAP set iTrangthai = 0 where PK_iMaphieunhap = "+ idphieunhap;
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();

                KhoiTaoDuLieu();
                lblThongBaoLoi.Text = "Khóa phiếu thành công";
            }
            else
                lblThongBaoLoi.Text = "Phiếu nhập không có dữ liệu";
        }
        protected void quaylaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ThemPhieuNhap.aspx");
        }

        protected void grvChiTietNhap_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvChiTietNhap.PageIndex = e.NewPageIndex;
            KhoiTaoDuLieu();
        }


    }
}