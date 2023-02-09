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
    public partial class WebForm9 : System.Web.UI.Page
    {
        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int taikhoanid = 0;
        static int dong_chitiet = 0;
        static int mahoadon = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated == false)
                    Response.Redirect("TrangItem.aspx");
                checkuser();
                Session["SortedView"] = null;
                KhoiTaoSanPham();
                KhoiTaoDuLieu();
                btnHuyKhiGiaoHang.Visible = false;
                lblLydo.Visible = false;
                txtLydoHuy.Visible = false;
                ddlBoNho.Visible = false;
                ddlMausac.Visible = false;
                btnThayDoiKhiGiaoHang.Visible = false;
                btnHuyYeuCauThayDoi.Visible = false;
                btnTraHang.Visible = false;
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

        private void setChiTietBtn(bool a)
        {
            saveBtn.Enabled = a;
            addBtn.Enabled = a;
            delBtn.Enabled = a;
            //thanhtoanBtn.Enabled = a;
        }
        private void setCoBanBtn(bool a)
        {
            btnLuuCoBan.Enabled = a;
            btnHuy.Enabled = a;
        }
        protected void ddlChedoxem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SortedView"] = null;
            KhoiTaoDuLieu(null);
        }

        protected void KhoiTaoDuLieu(string sortExpression = null)
        {
            string timkiem = "";
            string TuNgay = txtTuNgay.Text;
            string DenNgay = txtDenNgay.Text;
            DateTime dt_TuNgay, dt_DenNgay;
            string chedoxem = ddlChedoxem.SelectedValue;
            if (TuNgay == null || TuNgay == string.Empty)
                dt_TuNgay = new DateTime(2000, 01, 01);
            else
                dt_TuNgay = DateTime.Parse(TuNgay);
            if (DenNgay == null || DenNgay == string.Empty)
                dt_DenNgay = new DateTime(2100, 12, 12);
            else
                dt_DenNgay = DateTime.Parse(DenNgay);
            if (txtTimkiem.Text.Trim() != "")
                timkiem = txtTimkiem.Text.Trim();
            string sql = "select PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,";
            sql = sql + "HOADON.iTrangthai,sum(iSoluong*CAST(iDongia as bigint)+isnull(iPhuphi,0)) as 'iTongtien' ";
            sql = sql + " from HOADON left join NGUOIDUNG on HOADON.FK_iManhanvien = NGUOIDUNG.PK_iMataikhoan";
            sql = sql + " left join CT_HOADON on HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon where FK_iMataikhoan = "+ taikhoanid;
            if (timkiem != "")
                sql = sql + " and PK_iMahoadon like '%" + timkiem + "%'";
            switch (chedoxem)
            {
                case "0":
                    break;
                case "1":
                    sql = sql + " and HOADON.iTrangthai = 0";
                    break;
                case "2":
                    sql = sql + " and ( HOADON.iTrangthai = 1 or HOADON.iTrangthai = -3)";
                    break;
                case "3":
                    sql = sql + " and ( HOADON.iTrangthai = -1 or HOADON.iTrangthai = -2)";
                    break;
                case "4":
                    sql = sql + " and HOADON.iTrangthai = 3";
                    break;
                case "5":
                    sql = sql + " and ( HOADON.iTrangthai = 4 or HOADON.iTrangthai = -4)";
                    break;
            }
            sql = sql + " and dNgayLap >= '" + dt_TuNgay.ToString("MM-dd-yyyy") + "' and dNgayLap <= DATEADD(DAY, 1,'" + dt_DenNgay.ToString("MM-dd-yyyy") + "')";
            sql = sql + " group by PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,HOADON.iTrangthai";
            sql = sql + " order by dNgayLap DESC";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (sortExpression != null)
            {
                DataView dv = dt.AsDataView();
                this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
                dv.Sort = sortExpression + " " + this.SortDirection;
                Session["SortedView"] = dv;
                grvHoaDon.DataSource = dv;
            }
            else
                grvHoaDon.DataSource = dt;
            grvHoaDon.DataBind();
            dong_chitiet = 0;
            setChiTietBtn(true);
            ddlSanpham.ClearSelection();
            txtDongia.Text = string.Empty;
            txtSoluong.Text = string.Empty;
            tblChiTiet.Visible = false ;
            tblCoBan.Visible = false;
            mahoadon = 0;
         //   lblThongBaoLoi.Text = string.Empty;
        }

        protected void grvHoaDon_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.DataRow:
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvHoaDon, "Select$" + e.Row.RowIndex));
                    e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                    Label lbl = e.Row.FindControl("lblTrangthai") as Label;
                    switch (lbl.Text)
                    {
                        case "-1":
                            lbl.Text = "Đã bị hủy";
                            break;
                        case "0":
                            lbl.Text = "Chưa xác nhận";
                            break;
                        case "1":
                            lbl.Text = "Đang giao hàng";
                            break;
                        case "3":
                            lbl.Text = "Đã thanh toán";
                            break;
                        case "4":
                            lbl.Text = "Trả lại hàng";
                            break;
                        case "-2":
                            lbl.Text = "Yêu cầu hủy";
                            break;
                        case "-3":
                            lbl.Text = "Yêu cầu thay đổi";
                            break;
                        case "-4":
                            lbl.Text = "Yêu cầu trả hàng";
                            break;
                    }
                  /*  if (lbl.Text.Equals("-1"))
                        lbl.Text = "Đã bị hủy";
                    if (lbl.Text.Equals("0"))
                        lbl.Text = "Chưa xác nhận";
                    if (lbl.Text.Equals("1"))
                    {
                        lbl.Text = "Đang giao hàng";
                    }
                    if (lbl.Text.Equals("3"))
                        lbl.Text = "Đã thanh toán";
                    if (lbl.Text.Equals("4"))
                        lbl.Text = "Trả lại hàng";*/
                    break;
            }
        }

        private void BindChiTiet(int id)
        {
            string sql = "select PK_iCT_HoaDonID,CT_HOADON.FK_iMasanpham,sTensanpham,sNguonhinhanh,CT_HOADON.iSoluong,CT_HOADON.iDonGia,CT_HOADON.sBonho,CT_HOADON.sMausac";
            sql = sql + " from SANPHAM,CT_HOADON,HINHANHSP where FK_iMahoadon = " + id + " and CT_HOADON.FK_iMasanpham = PK_iMasanpham";
            sql = sql + " and PK_iMasanpham = HINHANHSP.FK_iMasanpham";
            sql = sql + " and HINHANHSP.iHienthi = 1";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvChiTiet.DataSource = dt;
            grvChiTiet.DataBind();
        }

        protected void BindCoBan(int id_hoadon)
        {
            string sql = "select PK_iMahoadon,HOADON.sSDT,HOADON.sDiachi,iTrangthai,isnull(iPhuphi,0) as [iPhuphi],isnull(sGhichu,'') as [sGhichu],sTennguoinhan";
            sql = sql + "   from NGUOIDUNG,HOADON";
            sql = sql + "   where NGUOIDUNG.PK_iMataikhoan = HOADON.FK_iMataikhoan and PK_iMahoadon = " + id_hoadon;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                txtSDT.Text = data["sSDT"].ToString();
                txtDiaChi.Text = data["sDiachi"].ToString();
                txtNguoiNhan.Text = data["sTennguoinhan"].ToString();
                txtPhuPhi.Text = data["iPhuphi"].ToString();
                txtGhiChu.Text = data["sGhichu"].ToString(); 
                
            }
            cnn.Close();
        }

        protected void grvHoaDon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in this.grvHoaDon.Rows)
                {
                    if (row.RowIndex == grvHoaDon.SelectedIndex)
                    {
                        int Id = Convert.ToInt32(grvHoaDon.SelectedDataKey.Value.ToString());
                        mahoadon = Id;
                        BindCoBan(Id);
                       // Label lbl = row.FindControl("lblTrangthai") as Label;
                        if (kiemtratrangthai(mahoadon.ToString()))
                            setCoBanBtn(true);
                        else
                            setCoBanBtn(false);
                        tblCoBan.Visible = true;
                        tblChiTiet.Visible = false;
                        lblThongBaoLoi.Text = string.Empty;

                        
                    }
                }
            }
            catch (Exception ex)
            {
               // Response.Write("Có lỗi xảy ra");
                lblThongBaoLoi.Text = ex.Message; 
            }
        }

        protected void checkuser()
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
                memb.idquyen = Int32.Parse(data["FK_iMaquyen"].ToString());

            }
            cnn.Close();
            #endregion
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
                int soluong = int.Parse(lblsoluong.Text);
                long thanhtien = dongia * soluong;
                lblthanhtien.Text = thanhtien.ToString();
                
            }
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
                        string bonho = data["sBonho"].ToString();
                        string mausac = data["sMausac"].ToString();
                        foreach (ListItem item in ddlSanpham.Items)
                        {
                            if (item.Value.Equals(idsp))
                            {
                                ddlSanpham.ClearSelection();
                                item.Selected = true;
                                KhoiTaoBoNho(idsp,bonho);
                                KhoiTaoMausac(idsp, mausac);
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
        protected void KhoiTaoMausac(string id, string mausac = null)
        {

            string sql = " select distinct sMausac from THONGSOSP where FK_iMasanpham = " + id;
            SqlConnection cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
                ddlMausac.Visible = true;
            else
                ddlMausac.Visible = false;
            ddlMausac.DataSource = dt;
            ddlMausac.DataTextField = "sMausac";
            ddlMausac.DataValueField = "sMausac";
            ddlMausac.DataBind();
            foreach (ListItem item in ddlMausac.Items)
            {
                if (item.Text.Equals(mausac))
                {
                    ddlMausac.ClearSelection();
                    item.Selected = true;
                    break;
                }
            }
        }

        protected void KhoiTaoBoNho(string id, string bonho = null)
        {
            string sql = " select distinct sBonho,iBonho_price from THONGSOSP where FK_iMasanpham = " + id;
            SqlConnection cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0)
                ddlBoNho.Visible = true;
            else
                ddlBoNho.Visible = false;
            ddlBoNho.DataSource = dt;
            ddlBoNho.DataTextField = "sBonho";
            ddlBoNho.DataValueField = "sBonho";
            ddlBoNho.DataBind();
            foreach (ListItem item in ddlBoNho.Items)
            {
                if (item.Text.Equals(bonho))
                {
                    ddlBoNho.ClearSelection();
                    item.Selected = true;
                    break;
                }
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            if (grvChiTiet.SelectedDataKey != null && dong_chitiet != 0 && kiemtratrangthai(mahoadon.ToString()))
            {
                Label lbl =  grvChiTiet.SelectedRow.FindControl("idsp_saveCheck") as Label;
                string sp_id = lbl.Text.Trim();
                // KiemTraThem(ddlSanpham.SelectedValue) || sp_id.Equals(ddlSanpham.SelectedValue)
                if (KiemTraBoNho(mahoadon.ToString(),ddlSanpham.SelectedValue,"SAVE",sp_id))
                {
                   
                    string dongia = LayDongiaSP(ddlSanpham.SelectedValue);
                    string adjust_price = updateDonGia(ddlSanpham.SelectedValue, ddlBoNho.SelectedValue, ddlMausac.SelectedValue);
                    long price = Int64.Parse(dongia);
                    if (!adjust_price.Equals("err"))
                    {
                        int adjust = int.Parse(adjust_price);
                        price = price + adjust;
                    }
                    string soluong = txtSoluong.Text.Trim();
                    string sql = "update CT_HOADON set iSoluong = " + soluong + ",iDongia = " + price + ",FK_iMasanpham = " + ddlSanpham.SelectedValue;
                    sql = sql + " ,sBonho = '" + ddlBoNho.SelectedValue + "',sMausac = N'"+ ddlMausac.SelectedValue+"'";
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
            else
                lblThongBaoLoi.Text = "Chưa chọn dòng sản phẩm";
        }
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

        protected bool KiemTraThem(string idSP)
        {
            string sql = "select * from CT_HOADON where FK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString() + " and FK_iMasanpham = " + idSP;
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

        protected bool  kiemtratrangthai(string idHoadon)
        {
            string sql = "select iTrangthai from HOADON where PK_iMahoadon = " + idHoadon;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                string trangthai = data["iTrangthai"].ToString();
                cnn.Close();
                bool re=false;
                switch (trangthai)
                {
                    case "0":
                        thanhtoanBtn.Enabled = true;
                        btnHuy.Visible = true;
                        btnHuyKhiGiaoHang.Visible = false;
                        btnThayDoiKhiGiaoHang.Visible = false;
                      //  lblLydo.Visible = false;
                     //   txtLydoHuy.Visible = false;
                        btnHuyYeuCauThayDoi.Visible = false;
                        btnLuuCoBan.Visible = true;
                        btnTraHang.Visible = false;
                        re = true;
                        break;
                    case "1":
                        thanhtoanBtn.Enabled = true;
                        btnHuy.Visible = false;
                        btnHuyKhiGiaoHang.Visible = true;
                        btnThayDoiKhiGiaoHang.Visible = true;
                        btnHuyYeuCauThayDoi.Visible = false;
                        btnLuuCoBan.Visible = false;
                        btnTraHang.Visible = false;
                      //  lblLydo.Visible = true;
                     //   txtLydoHuy.Visible = true;
                        re = false;
                        break;
                    case "3":
                        thanhtoanBtn.Enabled = false;
                        btnHuy.Visible = true;
                        btnHuyKhiGiaoHang.Visible = false;
                        btnThayDoiKhiGiaoHang.Visible = false;
                        btnHuyYeuCauThayDoi.Visible = false;
                        btnLuuCoBan.Visible = false;
                        btnTraHang.Visible = true;
                        re = false;
                        break;
                    case "-1":
                    case "4":
                        thanhtoanBtn.Enabled = false;
                        btnHuy.Visible = true;
                        btnHuyKhiGiaoHang.Visible = false;
                        btnThayDoiKhiGiaoHang.Visible = false;
                        btnHuyYeuCauThayDoi.Visible = false;
                        btnLuuCoBan.Visible = true;
                        btnTraHang.Visible = false;
                      //  lblLydo.Visible = false;
                       // txtLydoHuy.Visible = false;
                        re = false;
                        break;
                    case "-2":
                        thanhtoanBtn.Enabled = false;
                        btnHuy.Visible = false;
                        btnHuyKhiGiaoHang.Visible = false;
                        btnThayDoiKhiGiaoHang.Visible = false;
                        btnHuyYeuCauThayDoi.Visible = true;
                        btnLuuCoBan.Visible = true;
                        btnTraHang.Visible = false;
                        re = false;
                        break;
                    case "-3":
                        thanhtoanBtn.Enabled = false;
                        btnHuy.Visible = false;
                        btnHuyKhiGiaoHang.Visible = false;
                        btnThayDoiKhiGiaoHang.Visible = false;
                        btnHuyYeuCauThayDoi.Visible = false;
                        btnLuuCoBan.Visible = true;
                        btnTraHang.Visible = false;
                        re = true;
                        break;
                    case "-4":
                        thanhtoanBtn.Enabled = false;
                        btnHuy.Visible = false;
                        btnHuyKhiGiaoHang.Visible = false;
                        btnThayDoiKhiGiaoHang.Visible = false;
                        btnHuyYeuCauThayDoi.Visible = true;
                        btnLuuCoBan.Visible = true;
                        btnTraHang.Visible = false;
                        re = false;
                        break;
                }
                return re;
                
            }
            else
            {
                cnn.Close();
                return false;
            }
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
                KhoiTaoBoNho(value);
                KhoiTaoMausac(value);
            }
            else
            {
                txtDongia.Text = string.Empty;
                ddlMausac.Visible = false;
                ddlBoNho.Visible = false;
            }
        }

        protected string updateDonGia(string idsp, string bonho, string mausac)
        {
            string adjust_price="err";
            string sql = "select iBonho_price from THONGSOSP where FK_iMasanpham = " + idsp + "and sBonho = '" + bonho + "'";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                adjust_price = data["iBonho_price"].ToString();
            }
            cnn.Close();
            return adjust_price;
        }

        protected bool KiemTraBoNho(string hoadonid, string sanphamid, string loai,string idsave = null)
        {
            //LOẠI : ADD HOẶC SAVE
            string sql = "select sBonho from CT_HOADON where FK_iMahoadon = " + hoadonid + " and FK_iMasanpham = " + sanphamid;
            sql =  sql + " and (sBonho = '"+ ddlBoNho.SelectedValue +"' or sBonho is null)";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                if (sanphamid.Equals(idsave))
                {
                    data.Read();
                    string bonho = data["sBonho"].ToString();
                    cnn.Close();
                    Label lbl = grvChiTiet.SelectedRow.FindControl("bonho_saveCheck") as Label;
                    string bonho_check = lbl.Text.Trim();
                    if (loai.Equals("SAVE") && bonho_check.Equals(bonho))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                cnn.Close();
                return true;
            }
        }

        protected void addBtn_Click(object sender, EventArgs e)
        {
            if (mahoadon != 0 && kiemtratrangthai(mahoadon.ToString()))
            {
                
                
                //   string ghichu = txtGhichu.Text.Trim();
                string idSP = ddlSanpham.SelectedValue;
                
                    if (KiemTraBoNho(mahoadon.ToString(), idSP, "ADD"))
                    {
                        string Dongia = LayDongiaSP(idSP);
                        string adjust_price = updateDonGia(idSP, ddlBoNho.SelectedValue, ddlMausac.SelectedValue);
                        long price = Int64.Parse(Dongia);
                        if (!adjust_price.Equals("err"))
                        {
                            int adjust = int.Parse(adjust_price);
                            price = price + adjust;
                        }
                        int Soluong = Int32.Parse(txtSoluong.Text.Trim());
                        string sql = "insert into CT_HOADON(FK_iMahoadon,FK_iMasanpham,iSoluong,iDongia,sBonho,sMausac) values (" + mahoadon + "," + idSP + "," + Soluong + "," + price + ",'" + ddlBoNho.SelectedValue + "',N'" + ddlMausac.SelectedValue + "')";
                        SqlConnection cnn = new SqlConnection(constr);
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                        KhoiTaoDuLieu();
                        lblThongBaoLoi.Text = "Thêm thành công";
                    }
                    else
                        lblThongBaoLoi.Text = "Đã tồn tại sản phẩm này trong đơn hàng này";
                
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            if (grvChiTiet.SelectedDataKey != null && dong_chitiet != 0 && kiemtratrangthai(mahoadon.ToString()))
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
            else
                lblThongBaoLoi.Text = "Chưa chọn dòng sản phẩm";
        }

        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            grvHoaDon.PageIndex = 0;
            txtTimkiem.Text = string.Empty;
            txtDenNgay.Text = string.Empty;
            txtTuNgay.Text = string.Empty;
            Session["SortedView"] = null;
            KhoiTaoDuLieu();
            lblThongBaoLoi.Text = string.Empty;
            
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            string search = txtTimkiem.Text.Trim();
            if (search == "")
                KhoiTaoDuLieu();
            else
            {
                string TuNgay = txtTuNgay.Text;
                string DenNgay = txtDenNgay.Text;
                DateTime dt_TuNgay, dt_DenNgay;
                if (TuNgay == null || TuNgay == string.Empty)
                    dt_TuNgay = new DateTime(2000, 01, 01);
                else
                    dt_TuNgay = DateTime.Parse(TuNgay);
                if (DenNgay == null || DenNgay == string.Empty)
                    dt_DenNgay = new DateTime(2100, 12, 12);
                else
                    dt_DenNgay = DateTime.Parse(DenNgay);

                string sql = "select PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,";
                sql = sql + "HOADON.iTrangthai,sum(iSoluong*iDongia)+isnull(iPhuphi,0) as 'iTongtien' ";
                sql = sql + " from HOADON left join NGUOIDUNG on HOADON.FK_iManhanvien = NGUOIDUNG.PK_iMataikhoan";
                sql = sql + " inner join CT_HOADON on HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon where FK_iMataikhoan = " + taikhoanid;
                sql = sql + " and PK_iMahoadon like '%"+ search+"%'";
                sql = sql + " and dNgayLap >= '" + dt_TuNgay.ToString("MM-dd-yyyy") + "' and dNgayLap <= DATEADD(DAY, 1,'" + dt_DenNgay.ToString("MM-dd-yyyy") + "')";
                sql = sql + " group by PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,HOADON.iTrangthai";

                SqlConnection Cnn = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grvHoaDon.DataSource = dt;
                grvHoaDon.DataBind();
              //  repeaterPaging.Visible = false;
              //  current_page = 1;
                dong_chitiet = 0;
                setChiTietBtn(true);
                ddlSanpham.ClearSelection();
                txtDongia.Text = string.Empty;
                txtSoluong.Text = string.Empty;
                tblChiTiet.Visible = false;
                mahoadon = 0;
                
                lblThongBaoLoi.Text = string.Empty;
            }
        }

        protected void grvHoaDon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvHoaDon.PageIndex = e.NewPageIndex;
            if (Session["SortedView"] != null)
            {
                grvHoaDon.DataSource = Session["SortedView"];
                grvHoaDon.DataBind();
                lblThongBaoLoi.Text = string.Empty;
            }
            else
                KhoiTaoDuLieu();
        }

        protected void txtTuNgay_TextChanged(object sender, EventArgs e)
        {
            string TuNgay = txtTuNgay.Text;

            if (TuNgay == null || TuNgay == string.Empty)
            { }
            else
            {
                Session["SortedView"] = null;
                KhoiTaoDuLieu();
            }
        }

        protected void txtDenNgay_TextChanged(object sender, EventArgs e)
        {
            string DenNgay = txtDenNgay.Text;

            if (DenNgay == null || DenNgay == string.Empty)
            { }
            else
            {
                Session["SortedView"] = null;
                KhoiTaoDuLieu();
            }
        }

        protected void thanhtoanBtn_Click(object sender, EventArgs e)
        {
            if (mahoadon != 0)
            {
                Response.Redirect("ThanhToan.aspx?id=" + mahoadon);
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn hóa đơn";
            
        }

        protected void grvHoaDon_Sorting(object sender, GridViewSortEventArgs e)
        {
            KhoiTaoDuLieu(e.SortExpression);
        }

        protected void btnLuuCoBan_Click(object sender, EventArgs e)
        {
            if (grvHoaDon.SelectedDataKey != null && mahoadon != 0 &&  kiemtratrangthai(mahoadon.ToString()))
            {
                string sdt = txtSDT.Text.Trim();
                string diachi = txtDiaChi.Text.Trim();
                int phuphi;
                if (txtPhuPhi.Text.Trim() == "")
                    phuphi = 0;
                else
                    phuphi = Int32.Parse(txtPhuPhi.Text.Trim());
                string ghichu = txtGhiChu.Text.Trim();
                string sql = "update HOADON set sSDT = '" + sdt + "',sDiachi = N'" + diachi +"',sGhichu = N'" + ghichu + "',sTennguoinhan = N'" + txtNguoiNhan.Text.Trim() + "' where PK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString();
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                GhiLogHoaDon(grvHoaDon.SelectedDataKey.Value.ToString());
                Session["SortedView"] = null;
                KhoiTaoDuLieu();
                grvChiTiet.DataSource = null;
                grvChiTiet.DataBind();
                lblThongBaoLoi.Text = "Lưu thành công";
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }

        protected string laytrangthaidonhang(int id, string kieu,string trangthaiboqua = null)
        {
            // kieu co 2 loai: hientai va cu
            string trangthai = "";
            string sql = "";
            if (kieu.ToLower().Equals("hientai"))
                sql = "select iTrangthai from HOADON where PK_iMahoadon = " + id;
            else
            {
                if(trangthaiboqua == null)
                    sql = "select iTrangthai from HOADON_LOG where iMahoadon = " + id + " and iTrangthai != -1  order by Ngaycapnhat DESC";
                else
                    sql = "select iTrangthai from HOADON_LOG where iMahoadon = " + id + " and iTrangthai != "+ trangthaiboqua +"  order by Ngaycapnhat DESC";
            }
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

        protected void btnHuy_Click(object sender, EventArgs e)
        {
            if (grvHoaDon.SelectedDataKey != null && mahoadon != 0)
            {
                string trangthai = laytrangthaidonhang(mahoadon, "hientai");
                if (trangthai.Equals("0"))
                {
                    string sql = "update HOADON set iTrangthai = -1  where PK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    GhiLogHoaDon(grvHoaDon.SelectedDataKey.Value.ToString());
                    Session["SortedView"] = null;
                    KhoiTaoDuLieu();
                    lblThongBaoLoi.Text = "Đã hủy đơn hàng";
                }
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }

        protected void GhiLogHoaDon(string hoadonid)
        {

            //Tên của procedure
            string sql = "GHI_HOADON_LOG";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@hoadonid", hoadonid);
            cmd.Parameters.AddWithValue("@Ngaycapnhat", DateTime.Now);
            cmd.Parameters.AddWithValue("@iMataikhoancapnhat", taikhoanid);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        protected void lkbtnChiTiet_Click(object sender, EventArgs e)
        {
            ddlSanpham.ClearSelection();
            txtDongia.Text = string.Empty;
            txtSoluong.Text = string.Empty;
            LinkButton lkbtn = (LinkButton)sender;
            int Id = Int32.Parse(lkbtn.CommandArgument);
            mahoadon = Id;
            BindChiTiet(Id);
            ddlMausac.Visible = false;
            ddlBoNho.Visible = false;
            if (kiemtratrangthai(mahoadon.ToString()))
                setChiTietBtn(true);
            else
                setChiTietBtn(false);
            dong_chitiet = 0;
            tblChiTiet.Visible = true;
            tblCoBan.Visible = false;
            lblThongBaoLoi.Text = string.Empty;
        }

        protected void btnHuyKhiGiaoHang_Click(object sender, EventArgs e)
        {
            if (grvHoaDon.SelectedDataKey != null && mahoadon != 0)
            {
                string trangthai = laytrangthaidonhang(mahoadon, "hientai");
                if (trangthai.Equals("1"))
                {
                   // string sql = "insert into HOADON_YEUCAUHUY(FK_iMahoadon,dNgayyeucau,sLydo,iTrangthai)";
                   // sql = sql + " values(" + grvHoaDon.SelectedDataKey.Value.ToString() + ",'" + DateTime.Now + "',N'" + txtLydoHuy.Text.Trim() + "',0";
                    string sql = "update HOADON set iTrangthai = -2  where PK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    GhiLogHoaDon(grvHoaDon.SelectedDataKey.Value.ToString());
                    Session["SortedView"] = null;
                    KhoiTaoDuLieu();
                    lblThongBaoLoi.Text = "Đã gửi yêu cầu";
                }
                if(trangthai.Equals("-2"))
                    lblThongBaoLoi.Text = "Đã gửi yêu cầu";
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }

        protected void ddlBoNho_SelectedIndexChanged(object sender, EventArgs e)
        {

            string bonho = ddlBoNho.SelectedValue;
            string dongia = LayDongiaSP(ddlSanpham.SelectedValue);
            string adjust_price = updateDonGia(ddlSanpham.SelectedValue, bonho, ddlMausac.SelectedValue);
            long price = Int64.Parse(dongia);
            if (!adjust_price.Equals("err"))
            {
                int adjust = int.Parse(adjust_price);
                price = price + adjust;
            }
            txtDongia.Text = price.ToString();
        }

        protected void ddlMausac_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnThayDoiKhiGiaoHang_Click(object sender, EventArgs e)
        {
            if (grvHoaDon.SelectedDataKey != null && mahoadon != 0)
            {
                string trangthai = laytrangthaidonhang(mahoadon, "hientai");
                if (trangthai.Equals("1"))
                {
                    // string sql = "insert into HOADON_YEUCAUHUY(FK_iMahoadon,dNgayyeucau,sLydo,iTrangthai)";
                    // sql = sql + " values(" + grvHoaDon.SelectedDataKey.Value.ToString() + ",'" + DateTime.Now + "',N'" + txtLydoHuy.Text.Trim() + "',0";
                    string sql = "update HOADON set iTrangthai = -3  where PK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    GhiLogHoaDon(grvHoaDon.SelectedDataKey.Value.ToString());
                    Session["SortedView"] = null;
                    KhoiTaoDuLieu();
                    lblThongBaoLoi.Text = "Đã gửi yêu cầu";
                }
                if (trangthai.Equals("-3"))
                    lblThongBaoLoi.Text = "Đã gửi yêu cầu";
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }

        protected void btnHuyYeuCauThayDoi_Click(object sender, EventArgs e)
        {
            if (grvHoaDon.SelectedDataKey != null && mahoadon != 0)
            {
                string trangthai = laytrangthaidonhang(mahoadon, "hientai");
                if (trangthai.Equals("-2") || trangthai.Equals("-3") || trangthai.Equals("-4"))
                {
                    string trangthaicu = laytrangthaidonhang(mahoadon, "cu", trangthai);
                    if (trangthaicu.Equals("") && trangthai.Equals("-4"))
                        trangthaicu = "3";
                    // string sql = "insert into HOADON_YEUCAUHUY(FK_iMahoadon,dNgayyeucau,sLydo,iTrangthai)";
                    // sql = sql + " values(" + grvHoaDon.SelectedDataKey.Value.ToString() + ",'" + DateTime.Now + "',N'" + txtLydoHuy.Text.Trim() + "',0";
                    string sql = "update HOADON set iTrangthai = " + trangthaicu + "  where PK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    GhiLogHoaDon(grvHoaDon.SelectedDataKey.Value.ToString());
                    Session["SortedView"] = null;
                    KhoiTaoDuLieu();
                    lblThongBaoLoi.Text = "Đã hủy yêu cầu";
                }
                else
                    lblThongBaoLoi.Text = "Trạng thái đơn hàng không hợp lệ";
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }

        protected void btnTraHang_Click(object sender, EventArgs e)
        {
            if (grvHoaDon.SelectedDataKey != null && mahoadon != 0)
            {
                string trangthai = laytrangthaidonhang(mahoadon, "hientai");
                if (trangthai.Equals("3"))
                {
                    // string sql = "insert into HOADON_YEUCAUHUY(FK_iMahoadon,dNgayyeucau,sLydo,iTrangthai)";
                    // sql = sql + " values(" + grvHoaDon.SelectedDataKey.Value.ToString() + ",'" + DateTime.Now + "',N'" + txtLydoHuy.Text.Trim() + "',0";
                    string sql = "update HOADON set iTrangthai = -4 where PK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    GhiLogHoaDon(grvHoaDon.SelectedDataKey.Value.ToString());
                    Session["SortedView"] = null;
                    KhoiTaoDuLieu();
                    lblThongBaoLoi.Text = "Đã gửi yêu cầu";
                }
                else
                    lblThongBaoLoi.Text = "Trạng thái đơn hàng không hợp lệ";
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }
    }
}