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
    public partial class WebForm2 : System.Web.UI.Page
    {
        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        long Tongtien = 0;
        static int taikhoanid = 0;
        static string tennguoidung = "";
        static int donhangid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated == false)
                    Response.Redirect("DangNhap.aspx");
                if (!checkadmin())
                    Response.Redirect("TrangItem.aspx");
                Session["SortedView"] = null;
                KhoiTaoDuLieu();
                TuChoiHuy.Visible = false;
            }



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

        protected void KhoiTaoDuLieu(string sortExpression = null)
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
            string chedoxem = ddlChedoxem.SelectedValue;
            string sql = "select PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,dNgayLap,isnull(iPhuphi,0) as [iPhuphi],isnull(sGhichu,'') as [sGhichu],iTrangthai";
            sql = sql + ",sum(iSoluong*CAST(iDongia as bigint)+isnull(iPhuphi,0)) as 'iTongtien' ";
            sql = sql + "   from NGUOIDUNG,HOADON,CT_HOADON";
            sql = sql + "   where  HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon and NGUOIDUNG.PK_iMataikhoan = HOADON.FK_iMataikhoan ";
            sql = sql + " and dNgayLap >= '" + dt_TuNgay.ToString("MM-dd-yyyy") + "' and dNgayLap <= DATEADD(DAY, 1,'" + dt_DenNgay.ToString("MM-dd-yyyy") + "')";
            if (txtTimkiem.Text.Trim() != "")
                sql = sql + " and PK_iMahoadon like '%" + txtTimkiem.Text.Trim() + "%'";
            switch(chedoxem)
            {
                case "0":
                    sql = sql + " and HOADON.iTrangthai = 0";
                    break;
                case "1":
                    sql = sql + " and HOADON.iTrangthai = 1";
                    break;
                case "2":
                    sql = sql + " and HOADON.iTrangthai = -1";
                    break;
                case "3":
                    sql = sql + " and HOADON.iTrangthai = -2";
                    break;
                case "4":
                    sql = sql + " and HOADON.iTrangthai = -3";
                    break;
                case "5":
                    break;
            }
            
            sql = sql + " group by PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,HOADON.iTrangthai";
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
            donhangid = 0;
            lblThongBaoLoi.Text = string.Empty;
         //   txtTentaikhoan.Text = string.Empty;
         //   txtHoTen.Text = string.Empty;
         //   ddlQuyen.SelectedIndex = 0;
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
                    Button btn = e.Row.FindControl("xacnhanbtn") as Button;
                    Label hoadonid = e.Row.FindControl("lblhd_mahoadon") as Label;

                    switch(btn.Text)
                    {
                        case "-1":
                            btn.Text = "Đã hủy";
                            break;
                        case "0":
                            if (checkslsp(Int32.Parse(hoadonid.Text)))
                                btn.Text = "Chờ xác nhận";
                            else
                            {
                                btn.Text = "Không đủ số lượng";
                                btn.Enabled = false;
                                lblThongBaoLoi.Text = "";
                            }
                            break;
                        case "1":
                            btn.Text = "Đang giao hàng";
                            break;
                        case "3":
                            btn.Text = "Đã thanh toán";
                            btn.Enabled = false;
                            break;
                        case "-2":
                            btn.Text = "Xác nhận hủy";
                            break;
                        case "-3":
                            btn.Text = "Xác nhận thay đổi";
                            break;
                        case "4":
                            btn.Text = "Đã hoàn trả";
                            btn.Enabled = false;
                            break;
                        case "-4":
                            btn.Text = "Yêu cầu trả";
                            btn.Enabled = false;
                            break;
                    }
                  /*  if (btn.Text.Equals("-1"))
                        btn.Text = "Đã hủy";
                    if (btn.Text.Equals("0"))
                        btn.Text = "Chờ xác nhận";
                    if (btn.Text.Equals("1"))
                    {
                        btn.Text = "Đang giao hàng";
                       // btn.Enabled = false;
                    }
                    if (btn.Text.Equals("3"))
                    {
                        btn.Text = "Đã thanh toán";
                        btn.Enabled = false;
                    }*/
                    break;
            }
        }

        protected void grvHoaDon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in this.grvHoaDon.Rows)
                {
                    if (row.RowIndex == grvHoaDon.SelectedIndex)
                    {
                        Tongtien = 0;
                        int Id = Convert.ToInt32(grvHoaDon.SelectedDataKey.Value.ToString());
                        donhangid = Id;
                        string sql = "select PK_iMahoadon,HOADON.sSDT,HOADON.sDiachi,iTrangthai,isnull(iPhuphi,0) as [iPhuphi],isnull(sGhichu,'') as [sGhichu],sTennguoinhan";
                        sql = sql + "   from NGUOIDUNG,HOADON";
                        sql = sql + "   where NGUOIDUNG.PK_iMataikhoan = HOADON.FK_iMataikhoan and PK_iMahoadon = " + Id;
                        SqlConnection cnn = new SqlConnection(constr);
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        SqlDataReader data = cmd.ExecuteReader();
                        if (data.HasRows)
                        {
                            data.Read();
                            this.txtSDT.Text = data["sSDT"].ToString();
                            this.txtDiaChi.Text = data["sDiachi"].ToString();
                            this.txtPhuPhi.Text = data["iPhuphi"].ToString();
                            this.txtNguoiNhan.Text = data["sTennguoinhan"].ToString();
                            if (data["iPhuphi"].ToString() != "0")
                            {
                                int phuphi = Int32.Parse(txtPhuPhi.Text);
                                lblphuphi.Text = "Phụ phí: " + (Convert.ToInt32(phuphi) > 10 ? String.Format("{0:0,0}", phuphi).Replace(',', '.') : Convert.ToInt32(phuphi).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.'));
                            }
                            Tongtien = Tongtien + Int64.Parse(data["iPhuphi"].ToString());
                            this.txtGhiChu.Text = data["sGhichu"].ToString();
                            
                        }
                        cnn.Close();
                        BindData(Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Có lỗi xảy ra");
                //lblThongBaoLoi.Text = ex.Message; 
            }
        }
        private void BindData(int id)
        {
            string sql = "select PK_iCT_HoaDonID,sTensanpham,CT_HOADON.iSoluong,CT_HOADON.iDonGia";
            sql = sql + " from SANPHAM,CT_HOADON where FK_iMahoadon = " + id + " and FK_iMasanpham = PK_iMasanpham";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvChiTiet.DataSource = dt;
            grvChiTiet.DataBind();
        }

        protected void grvChiTiet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
        }

        protected bool checkslsp(int iddonhang)
        {
            
            int ck = 0;
            SqlConnection cnn = new SqlConnection(constr);
            DataTable dataTable = new DataTable();
            cnn.Open();
            
                //string price = "";
                // string sql = "insert into CT_HOADON values (" + hoadonid + "," + sp.idsanpham + "," + sp.soluong + "," + sp.dongia + ")";
            string sql = "select SANPHAM.sTensanpham,SANPHAM.iSoluong as [soluongco],CT_HOADON.iSoluong as [soluongban] ";
            sql = sql + " from SANPHAM ,HOADON,CT_HOADON";
            sql = sql + " where CT_HOADON.FK_iMasanpham = SANPHAM.PK_iMasanpham and HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon and HOADON.PK_iMahoadon = " + iddonhang; 
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            cnn.Close();
            string tensp = "";
            int soluongco, soluongban;
            if(dataTable.Rows.Count > 0)
                foreach (DataRow row in dataTable.Rows)
                {
                    tensp = row["sTensanpham"].ToString();
                    soluongco = Int32.Parse(row["soluongco"].ToString());
                    soluongban = Int32.Parse(row["soluongban"].ToString());
                    if (soluongco < soluongban)
                    {
                        ck = 1;
                        break;
                    }
                }
            if (ck == 0)
                return true;
            else
            {
                lblThongBaoLoi.Text = "Sản phẩm " + tensp + " vượt quá số lượng có hiện tại";
                return false;
            }
        }

        protected string laytrangthaidonhang(int id, string kieu, string trangthaiboqua = null)
        {
            // kieu co 2 loai: hientai va cu
            string trangthai = "";
            string sql = "";
            if (kieu.ToLower().Equals("hientai"))
                sql = "select iTrangthai from HOADON where PK_iMahoadon = " + id;
            else
            {
                if (trangthaiboqua == null)
                    sql = "select iTrangthai from HOADON_LOG where iMahoadon = " + id + " and iTrangthai != -1  order by Ngaycapnhat DESC";
                else
                    sql = "select iTrangthai from HOADON_LOG where iMahoadon = " + id + " and iTrangthai != " + trangthaiboqua + "  order by Ngaycapnhat DESC";
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

        protected void xacnhanbtn_Click(object sender, EventArgs e)
        {
            if (taikhoanid != 0)
            {
                
                Button btn = (Button)sender;
                string btntext = btn.Text;
                int id = int.Parse(btn.CommandArgument);
                    string sql = "";
                    //get trang thai
                    string trangthai = laytrangthaidonhang(id,"hientai");
                    switch (trangthai)
                    {
                        case "":
                            break;
                        case "0":
                            if (checkslsp(id))
                                sql = "update HOADON set iTrangthai = 1,FK_iManhanvien =" + taikhoanid + "  where PK_iMahoadon = " + id;
                            break;
                        case "1":
                            sql = "update HOADON set iTrangthai = 3 where PK_iMahoadon = " + id;
                            break;
                        case "-1":
                            string trangthaicu = laytrangthaidonhang(id, "cu");
                            if (trangthaicu.Equals(""))
                                trangthaicu = "0";
                            sql = "update HOADON set iTrangthai = " + trangthaicu + "   where PK_iMahoadon = " + id;
                            break;
                        case "-2":
                            sql = "update HOADON set iTrangthai = -1  where PK_iMahoadon = " + id;
                            break;
                        case "-3":
                            sql = "update HOADON set iTrangthai = 1  where PK_iMahoadon = " + id;
                            break;
                        default:
                            break;
                    }
                    //
                    if (!sql.Equals(""))
                    {
                        SqlConnection cnn = new SqlConnection(constr);
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                        GhiLogHoaDon(id.ToString());
                        Session["SortedView"] = null;
                        KhoiTaoDuLieu();
                    }
                
                
            }

        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            if (grvHoaDon.SelectedDataKey != null && donhangid != 0)
            {
                string sdt = txtSDT.Text.Trim();
                string diachi = txtDiaChi.Text.Trim();
                int phuphi;
                if (txtPhuPhi.Text.Trim() == "")
                    phuphi = 0;
                else
                    phuphi = Int32.Parse(txtPhuPhi.Text.Trim());
                string ghichu = txtGhiChu.Text.Trim();
              //  string sql = "update HOADON set sSDT = '" + sdt + "',sDiachi = N'" + diachi + "',iPhuphi =" + phuphi + ",sGhichu = N'" + ghichu + "',sTennguoinhan = N'"+ txtNguoiNhan.Text.Trim() +  "' where PK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString();
                string sql;
                string trangthai = laytrangthaidonhang(donhangid, "hientai");
                if(trangthai.Equals("0"))
                    sql = "update HOADON set iPhuphi =" + phuphi + ",sGhichu = N'" + ghichu + "' where PK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString();
                else
                    sql = "update HOADON set sGhichu = N'" + ghichu + "' where PK_iMahoadon = " + grvHoaDon.SelectedDataKey.Value.ToString();
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
                lblTongTien.Text = "";
                lblThongBaoLoi.Text = string.Empty;
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            if(grvHoaDon.SelectedDataKey != null && donhangid != 0)
            {
                string trangthai = laytrangthaidonhang(donhangid, "hientai");
                if (!trangthai.Equals("-1"))
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
                }
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn đơn hàng";
        }
        protected void quaylaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
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

        protected void grvHoaDon_In_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblsoluong = e.Row.FindControl("lblSoluong") as Label;
                Label lbldongia = e.Row.FindControl("lbltDonGia") as Label;
                Label lblthanhtien = e.Row.FindControl("lblThanhtien") as Label;
                Int64 dongia = Int64.Parse(lbldongia.Text);
                string dongialbl = Convert.ToInt64(dongia) > 10 ? String.Format("{0:0,0}", dongia).Replace(',', '.') : Convert.ToInt64(dongia).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                lbldongia.Text = dongialbl;
                int soluong = int.Parse(lblsoluong.Text);
                long thanhtien = dongia * soluong;
                lblthanhtien.Text = Convert.ToInt64(thanhtien) > 10 ? String.Format("{0:0,0}", thanhtien).Replace(',', '.') : Convert.ToInt64(thanhtien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (grvHoaDon.SelectedDataKey != null && donhangid != 0)
            {
                string id = grvHoaDon.SelectedDataKey.Value.ToString();
                string sql = "select PK_iCT_HoaDonID,sTensanpham,CT_HOADON.iSoluong,CT_HOADON.iDonGia";
                sql = sql + " from SANPHAM,CT_HOADON where FK_iMahoadon = " + id + " and FK_iMasanpham = PK_iMasanpham";
                SqlConnection Cnn = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grvHoaDon_In.DataSource = dt;
                grvHoaDon_In.DataBind();
                lbltongtienin.Text = lblTongTien.Text;
                lblngayin.Text = "Ngày lập: "+ DateTime.Now.ToString("dd/MM/yyyy");
                lbltennhanvien.Text = "Người lập: "+ tennguoidung;
                //this.ClientScript.RegisterStartupScript(this.GetType(), "myFunc", "Print("+id+")", true);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "PrintPanel()", true);
                //  btnPrint.Text = "đã in";
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

        protected void grvHoaDon_Sorting(object sender, GridViewSortEventArgs e)
        {
            KhoiTaoDuLieu(e.SortExpression);
            
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            string search = txtTimkiem.Text.Trim();
            if (search == "")
                KhoiTaoDuLieu();
            else
            {
                KhoiTaoDuLieu();
               /* string TuNgay = txtTuNgay.Text;
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

                string sql = "select PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,dNgayLap,isnull(iPhuphi,0) as [iPhuphi],isnull(sGhichu,'') as [sGhichu],iTrangthai";
                sql = sql + ",sum(iSoluong*iDongia)+isnull(iPhuphi,0) as 'iTongtien' ";
                sql = sql + "   from NGUOIDUNG,HOADON,CT_HOADON";
                sql = sql + "   where  HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon and NGUOIDUNG.PK_iMataikhoan = HOADON.FK_iMataikhoan and iTrangthai != 3 and iTrangthai != -1";
                sql = sql + " and PK_iMahoadon like '%" + search + "%'";
                sql = sql + " and dNgayLap >= '" + dt_TuNgay.ToString("MM-dd-yyyy") + "' and dNgayLap <= DATEADD(DAY, 1,'" + dt_DenNgay.ToString("MM-dd-yyyy") + "')";
                sql = sql + " group by PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,HOADON.iTrangthai";

                

                SqlConnection Cnn = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grvHoaDon.DataSource = dt;
                grvHoaDon.DataBind();
                lblThongBaoLoi.Text = string.Empty;
                donhangid = 0; */
            }
        }

        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            grvHoaDon.PageIndex = 0;
            txtTimkiem.Text = string.Empty;
            txtDenNgay.Text = string.Empty;
            txtTuNgay.Text = string.Empty;
            Session["SortedView"] = null;
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

        protected void ddlChedoxem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SortedView"] = null;
            string chedoxem = ddlChedoxem.SelectedValue;
            switch (chedoxem)
            {
                case "2":
                    delBtn.Enabled = false;
                    delBtn.Visible = true;
                    TuChoiHuy.Visible = false;
                    btnPrint.Visible = true;
                    break;
                case "3":
                case "4":
                    delBtn.Enabled = true;
                    TuChoiHuy.Visible = true;
                    btnPrint.Visible = false;
                    break;
                default:
                    delBtn.Enabled = true;
                    TuChoiHuy.Visible = false;
                    btnPrint.Visible = true;
                    break;
            }
         /*   if (ddlChedoxem.SelectedValue.Equals("2") || ddlChedoxem.SelectedValue.Equals("3") || ddlChedoxem.SelectedValue.Equals("4"))
            {
                
                delBtn.Enabled = false;
                delBtn.Visible = false;
            }
            else
            {
                delBtn.Enabled = true;
                delBtn.Visible = true;
                
            } */
            KhoiTaoDuLieu(null);
        }

        protected void TuChoiHuy_Click(object sender, EventArgs e)
        {
            if (grvHoaDon.SelectedDataKey != null && donhangid != 0)
            {
                string trangthai = laytrangthaidonhang(donhangid, "hientai");
                if (trangthai.Equals("-2") || trangthai.Equals("-3") || trangthai.Equals("-4"))
                {
                    string trangthaicu = laytrangthaidonhang(donhangid, "cu", trangthai);
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
    }
}