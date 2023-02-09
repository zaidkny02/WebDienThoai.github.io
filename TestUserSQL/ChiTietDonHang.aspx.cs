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
    public partial class ChiTietDonHang : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        long Tongtien = 0;
        static int taikhoanid = 0;
        static string tennguoidung = "";
        static int donhangid = 0;
        static int dong_chitiet = 0;
        static string trangthai = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated == false)
                    Response.Redirect("DangNhap.aspx");
                if (!checkadmin())
                { }
                else
                {
                    bool try_Parse;
                    if (Request.QueryString["id"] != null)
                        try_Parse = int.TryParse(Request.QueryString["id"].ToString(), out donhangid);
                    else
                        try_Parse = false;
                    if (try_Parse)
                    {
                        KhoiTaoSanPham();
                        KhoiTaoDuLieu();
                        lbltitle.Text = "CHI TIẾT ĐƠN HÀNG " + donhangid;
                    }
                }
            }
        }

        private void setChiTietBtn(bool a)
        {
            saveBtn.Enabled = a;
            addBtn.Enabled = a;
            delBtn.Enabled = a;
        }


        protected void ddlSanpham_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddlSanpham.SelectedValue;
            if (!value.Equals("0"))
            {
                string sql = "select iGiaban-(iGiaban*isnull(iTilekhuyenmai,0)/100) as 'i_Final_price' ";
                sql = sql + " from SANPHAM left join KHUYENMAI on SANPHAM.PK_iMasanpham = KHUYENMAI.FK_iMasanpham";
                sql = sql + " and isnull(KHUYENMAI.dNgaybatdau,'1/1/2000') <= GETDATE() and isnull(KHUYENMAI.dNgayketthuc,'12/12/2100') >= GETDATE()";
                sql = sql + " where PK_iMasanpham =" + value;
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                SqlDataReader data = cmd.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    txtDongia.Text = data["i_Final_price"].ToString();
                }
                cnn.Close();
            }
            else
                txtDongia.Text = string.Empty;
        }

        #region LatTrangThaiDonHang
        protected string laytrangthaidonhang(int id, string kieu)
        {
            // kieu co 2 loai: hientai va cu
            string trangthai = "";
            string sql = "";
            if (kieu.ToLower().Equals("hientai"))
                sql = "select iTrangthai from HOADON where PK_iMahoadon = " + id;
            else
                sql = "select iTrangthai from HOADON_LOG where iMahoadon = " + id + " and iTrangthai != -1  order by Ngaycapnhat DESC";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                trangthai = data["iTrangthai"].ToString();
            }
            cnn.Close();
            return trangthai;
        }
        #endregion

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

        #region LayDonGiaSP
        protected string LayDongiaSP(string idSP)
        {
            string dulieu = "";
            string sql = "select iGiaban-(iGiaban*isnull(iTilekhuyenmai,0)/100) as 'i_Final_price' ";
            sql = sql + " from SANPHAM left join KHUYENMAI on SANPHAM.PK_iMasanpham = KHUYENMAI.FK_iMasanpham";
            sql = sql + " and isnull(KHUYENMAI.dNgaybatdau,'1/1/2000') <= GETDATE() and isnull(KHUYENMAI.dNgayketthuc,'12/12/2100') >= GETDATE()";
            sql = sql + " where PK_iMasanpham =" + idSP;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                dulieu = data["i_Final_price"].ToString();
            }
            cnn.Close();
            return dulieu;
        }
        #endregion

        protected void KhoiTaoDuLieu()
        {

            string sql = "select isnull(iPhuphi,0) as [iPhuphi],HOADON.iTrangthai";
            sql = sql + "   from NGUOIDUNG,HOADON";
            sql = sql + "   where NGUOIDUNG.PK_iMataikhoan = HOADON.FK_iMataikhoan and PK_iMahoadon = " + donhangid;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                if (data["iPhuphi"].ToString() != "0")
                {
                    int phuphi = Int32.Parse(data["iPhuphi"].ToString());
                    lblphuphi.Text = "Phụ phí: " + (Convert.ToInt32(phuphi) > 10 ? String.Format("{0:0,0}", phuphi).Replace(',', '.') : Convert.ToInt32(phuphi).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.')) + " VND";
                }
                Tongtien = Tongtien + Int64.Parse(data["iPhuphi"].ToString());
                trangthai = data["iTrangthai"].ToString();
            }
            
            
            cnn.Close();
            if (trangthai.Equals("-1") || trangthai.Equals("3") || trangthai.Equals("4") || trangthai.Equals("-2"))
                setChiTietBtn(false);
            sql = "select PK_iCT_HoaDonID,CT_HOADON.FK_iMasanpham,sTensanpham,CT_HOADON.iSoluong,CT_HOADON.iDonGia";
            sql = sql + " from SANPHAM,CT_HOADON where FK_iMahoadon = " + donhangid + " and FK_iMasanpham = PK_iMasanpham";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvChiTiet.DataSource = dt;
            grvChiTiet.DataBind();
           // lblThongBaoLoi.Text = string.Empty;
        }


        protected void addBtn_Click(object sender, EventArgs e)
        {
            if (donhangid != 0)
            {
                string trangthai = laytrangthaidonhang(donhangid, "hientai");
                if (trangthai.Equals("-1") || trangthai.Equals("3") || trangthai.Equals("4") || trangthai.Equals("-2"))
                    lblThongBaoLoi.Text = "Đơn hàng không thể thêm mới sản phẩm!";
                else
                {
                    //   string ghichu = txtGhichu.Text.Trim();
                    string idSP = ddlSanpham.SelectedValue;
                    if (KiemTraThem(idSP))
                    {
                        int Soluong = Int32.Parse(txtSoluong.Text.Trim());
                        string Dongia = LayDongiaSP(idSP);
                        string sql = "insert into CT_HOADON(FK_iMahoadon,FK_iMasanpham,iSoluong,iDongia) values (" + donhangid + "," + idSP + "," + Soluong + "," + Dongia + ")";
                        SqlConnection cnn = new SqlConnection(constr);
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                        KhoiTaoDuLieu();
                        lblThongBaoLoi.Text = "Thêm thành công!";
                    }
                    else
                        lblThongBaoLoi.Text = "Đã tồn tại sản phẩm này trong đơn hàng này";
                }
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }


        protected bool KiemTraThem(string idSP)
        {
            string sql = "select * from CT_HOADON where FK_iMahoadon = " + donhangid + " and FK_iMasanpham = " + idSP;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                cnn.Close();
                return false;
            }
            else
            {
                cnn.Close();
                return true;
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            if (grvChiTiet.SelectedDataKey != null && dong_chitiet != 0)
            {
                string trangthai = laytrangthaidonhang(donhangid, "hientai");
                if (trangthai.Equals("-1") || trangthai.Equals("3") || trangthai.Equals("4"))
                    lblThongBaoLoi.Text = "Đơn hàng không thể cập nhật sản phẩm!";
                else
                {
                    Label lbl = grvChiTiet.SelectedRow.FindControl("idsp_saveCheck") as Label;
                    string sp_id = lbl.Text.Trim();
                    if (KiemTraThem(ddlSanpham.SelectedValue) || sp_id.Equals(ddlSanpham.SelectedValue))
                    {
                        string dongia = LayDongiaSP(ddlSanpham.SelectedValue);
                        string soluong = txtSoluong.Text.Trim();
                        string sql = "update CT_HOADON set iSoluong = " + soluong + ",iDongia = " + dongia + ",FK_iMasanpham = " + ddlSanpham.SelectedValue;
                        sql = sql + " where PK_iCT_HoaDonID = " + grvChiTiet.SelectedDataKey.Value.ToString();
                        SqlConnection cnn = new SqlConnection(constr);
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                        KhoiTaoDuLieu();
                        lblThongBaoLoi.Text = "Sửa thành công";
                    }
                    else
                        lblThongBaoLoi.Text = "Đã tồn tại sản phẩm này trong đơn hàng này";
                }


            }
            else
                lblThongBaoLoi.Text = "Chưa chọn dòng sản phẩm";
        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            if (grvChiTiet.SelectedDataKey != null && dong_chitiet != 0)
            {
                string trangthai = laytrangthaidonhang(donhangid, "hientai");
                if (trangthai.Equals("-1") || trangthai.Equals("3") || trangthai.Equals("4") || trangthai.Equals("-2"))
                    lblThongBaoLoi.Text = "Đơn hàng không thể xóa sản phẩm!";
                else
                {
                    string sql = "delete from CT_HOADON where PK_iCT_HoaDonID = " + grvChiTiet.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    KhoiTaoDuLieu();
                    lblThongBaoLoi.Text = "Xóa thành công";
                }
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn dòng sản phẩm";
        }

        protected void grvChiTiet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvChiTiet, "Select$" + e.Row.RowIndex));
                e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                Label lblsoluong = e.Row.FindControl("lblSoluong") as Label;
                Label lbldongia = e.Row.FindControl("lbltDonGia") as Label;
                Label lblthanhtien = e.Row.FindControl("lblThanhtien") as Label;
                Int64 dongia = Int64.Parse(lbldongia.Text);
                string dongialbl = Convert.ToInt64(dongia) > 10 ? String.Format("{0:0,0}", dongia).Replace(',', '.') : Convert.ToInt64(dongia).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                lbldongia.Text = dongialbl;
                int soluong = int.Parse(lblsoluong.Text);
                long thanhtien = dongia * soluong;
                lblthanhtien.Text = Convert.ToInt64(thanhtien) > 10 ? String.Format("{0:0,0}", thanhtien).Replace(',', '.') : Convert.ToInt64(thanhtien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                Tongtien = Tongtien + thanhtien;
                string a = Convert.ToInt64(Tongtien) > 10 ? String.Format("{0:0,0}", Tongtien).Replace(',', '.') : Convert.ToInt64(Tongtien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                lblTongTien.Text = "Tổng tiền: " + a + " VND";
            }
            e.Row.Cells[4].Visible = false;
        }







        protected bool checkadmin()
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
                taikhoanid = memb.idnguoidung;
                memb.tentaikhoan = data["sTentaikhoan"].ToString();
                memb.name = data["sHovaten"].ToString();
                tennguoidung = memb.name;
                memb.idquyen = Int32.Parse(data["FK_iMaquyen"].ToString());

            }
            cnn.Close();
            #endregion
            if (memb.idquyen == 0 || memb.idquyen == 1)
                return true;
            else
                return false;
        }

        protected void grvChiTiet_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grvChiTiet.Rows)
            {
                if (row.RowIndex == grvChiTiet.SelectedIndex)
                {
                    string sql = "select * from  CT_HOADON ";
                    sql = sql + " where PK_iCT_HoaDonID = " + grvChiTiet.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Read();
                        dong_chitiet = Int32.Parse(data["PK_iCT_HoaDonID"].ToString());
                        txtDongia.Text = data["iDongia"].ToString();
                        txtSoluong.Text = data["iSoluong"].ToString();
                        string idsp = data["FK_iMasanpham"].ToString();
                        foreach (ListItem item in ddlSanpham.Items)
                        {
                            if (item.Value.Equals(idsp))
                            {
                                ddlSanpham.ClearSelection();
                                item.Selected = true;
                                //ddlSanpham.Enabled = false;
                                break;
                            }
                        }
                    }
                    cnn.Close();
                    lblThongBaoLoi.Text = string.Empty;
                }
            }
        }
    }
}