using ClosedXML.Excel;
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
    public partial class WebForm10 : System.Web.UI.Page
    {
        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        long Tongtien = 0;
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int taikhoanid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated == false)
                    Response.Redirect("DangNhap.aspx");
                if (!checkadmin())
                    Response.Redirect("TrangItem.aspx");
                txtTuNgay.Text = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                txtDenNgay.Text = DateTime.Now.ToString("yyyy-MM-dd");
                Session["SortedView"] = null;
                KhoiTaoDuLieu();
                grvBanChay.Visible = false;
                grvHangTon.Visible = false;
                KhoiTaoThangNam();
            }
            
        }

        protected void KhoiTaoThangNam()
        {
            
            for (int i = 1; i <= 12; i++)
            {
                ListItem item = new ListItem(i.ToString(), i.ToString());
                ddlThang.Items.Insert(i - 1, item);
            }
            for (int i = 2010; i <= 2050; i++)
            {
                ListItem item = new ListItem(i.ToString(), i.ToString());
                ddlNam.Items.Insert(i - 2010, item);
            }
        }

        protected void KhoiTaoDuLieu(string sortExpression = null)
        {
            DateTime tungay_temp = DateTime.Parse(this.txtTuNgay.Text.Trim());
            DateTime denngay_temp = DateTime.Parse(this.txtDenNgay.Text.Trim());
            if (tungay_temp > denngay_temp)
            {
                lblThongBaoLoi.Text = "Từ ngày không được lớn hơn đến ngày";
                txtTuNgay.Focus();
                Chart1.Visible = false;

            }
            else
            {
                string chedoxem = ddlChedoxem.SelectedValue;
                // string sql = "select PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,isnull(HOADON.iPhuphi,0) as [iPhuphi],";
                //  sql = sql + " dNgayLap,sGhichu,HOADON.iTrangthai,sum(iSoluong*iDongia)+isnull(iPhuphi,0) as 'iTongtien' ";
                //  sql = sql + " from HOADON left join NGUOIDUNG on HOADON.FK_iManhanvien = NGUOIDUNG.PK_iMataikhoan ";
                //  sql = sql + " inner join CT_HOADON on HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon where HOADON.iTrangthai = 3 ";
                //  sql = sql + " group by PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,HOADON.iTrangthai";
                switch (chedoxem)
                {
                    case "0":
                    string sql = "select PK_iMahoadon,b.sHovaten as [tenkhachhang],NGUOIDUNG.sHovaten as [tennhanvien],HOADON.sSDT,HOADON.sDiachi,isnull(HOADON.iPhuphi,0) as [iPhuphi],";
                    sql = sql + " dNgayLap,sGhichu,HOADON.iTrangthai,sum(iSoluong*CAST(iDongia as bigint)+isnull(iPhuphi,0)) as 'iTongtien' ";
                    sql = sql + " from HOADON left join NGUOIDUNG on HOADON.FK_iManhanvien = NGUOIDUNG.PK_iMataikhoan left join NGUOIDUNG b on b.PK_iMataikhoan = HOADON.FK_iMataikhoan ";
                    sql = sql + " inner join CT_HOADON on HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon where HOADON.iTrangthai = 3 ";
                    sql = sql + " and dNgayLap >= '" + tungay_temp.ToString("MM-dd-yyyy") + "' and dNgayLap <= DATEADD(DAY, 1,'" + denngay_temp.ToString("MM-dd-yyyy") + "')";
                    sql = sql + " group by PK_iMahoadon,b.sHovaten,NGUOIDUNG.sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,HOADON.iTrangthai";
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
                    {
                        grvHoaDon.DataSource = dt;
                    }
                    Chart1.Visible = false;
                    lblThongBaoLoi.Text = string.Empty;
                    grvHoaDon.DataBind();

                    LayTongTien();
                    
                    break;
                    case "1":
                    KhoiTaoBanChay(null);
                    break;
                    case "2":
                    KhoiTaoHangTon(null);
                    break;
                }
            }
            
            
        }
        
        protected void LayTongTien()
        {
            DateTime tungay_temp = DateTime.Parse(this.txtTuNgay.Text.Trim());
            DateTime denngay_temp = DateTime.Parse(this.txtDenNgay.Text.Trim());
            string sql = "select isnull(sum(iSoluong*cast(iDongia as bigint)+isnull(iPhuphi,0)),0) as [tong]";
            sql = sql + "   from HOADON,CT_HOADON where HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon";
            sql = sql + "   and HOADON.iTrangthai = 3  and dNgayLap >= '" + tungay_temp.ToString("MM-dd-yyyy") + "' and dNgayLap <= DATEADD(DAY, 1,'" + denngay_temp.ToString("MM-dd-yyyy") + "')";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                if (data["tong"].ToString() != "0")
                {
                    Tongtien = Int64.Parse(data["tong"].ToString());
                }
            }
            cnn.Close();
            string b = Convert.ToInt64(Tongtien) > 10 ? String.Format("{0:0,0}", Tongtien).Replace(',', '.') : Convert.ToInt64(Tongtien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
            if (Tongtien > 0)
                lbltongtien.Text = "Tổng: " + b + " VND";
            else
                lbltongtien.Text = "";
        }

        protected void KhoiTaoHangTon(string sortExpression = null)
        {
            string sql = "select PK_iMasanpham,sTensanpham,iGiaban,iSoluong from SANPHAM";
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
                grvHangTon.DataSource = dv;
            }
            else
            {
                grvHangTon.DataSource = dt;
            }
            Chart1.Visible = false;
            lblThongBaoLoi.Text = string.Empty;
            grvHangTon.DataBind();
        }

        protected void KhoiTaoBanChay(string sortExpression = null)
        {
            DateTime tungay_temp = DateTime.Parse(this.txtTuNgay.Text.Trim());
            DateTime denngay_temp = DateTime.Parse(this.txtDenNgay.Text.Trim());
            string sql = "select isnull(sum(CT_HOADON.iSoluong),0) as [soluong],SANPHAM.sTensanpham,SANPHAM.PK_iMasanpham,";
            sql = sql + " isnull(SUM(CAST(CT_HOADON.iDongia as bigint)*CT_HOADON.iSoluong),0) as [tongtien] ";
            sql = sql + " from HOADON left join CT_HOADON on CT_HOADON.FK_iMahoadon = HOADON.PK_iMahoadon and HOADON.iTrangthai = 3 ";
            sql = sql + " and HOADON.dNgayLap >= '" + tungay_temp.ToString("MM-dd-yyyy") + "' and HOADON.dNgayLap <= DATEADD(DAY, 1, '" + denngay_temp.ToString("MM-dd-yyyy") + "') ";
            sql = sql + " right join SANPHAM on CT_HOADON.FK_iMasanpham = SANPHAM.PK_iMasanpham";
            sql = sql + " group by SANPHAM.sTensanpham,SANPHAM.PK_iMasanpham order by sum(CT_HOADON.iSoluong) DESC";
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
                grvBanChay.DataSource = dv;
            }
            else
            {
                grvBanChay.DataSource = dt;
            }
            Chart1.Visible = false;
            lblThongBaoLoi.Text = string.Empty;
            grvBanChay.DataBind();
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
                    Label lbltien = e.Row.FindControl("lbltien") as Label;
                    Int64 tien = Int64.Parse(lbltien.Text);
                    Tongtien = Tongtien + tien;
                    string b = Convert.ToInt64(Tongtien) > 10 ? String.Format("{0:0,0}", Tongtien).Replace(',', '.') : Convert.ToInt64(Tongtien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                    
                    string a = Convert.ToInt64(tien) > 10 ? String.Format("{0:0,0}", tien).Replace(',', '.') : Convert.ToInt64(tien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                    lbltien.Text = a;
                    //<%# Convert.ToInt32(Eval("iTongtien")) > 10 ? String.Format("{0:0,0}", Eval("iTongtien")).Replace(',', '.') : Convert.ToInt32(Eval("iTongtien")).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.')%>
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
                       // Tongtien = 0;
                      /*  int Id = Convert.ToInt32(grvHoaDon.SelectedDataKey.Value.ToString());
                        string sql = "select PK_iMahoadon,HOADON.sSDT,HOADON.sDiachi,iTrangthai,isnull(iPhuphi,0) as [iPhuphi],isnull(sGhichu,'') as [sGhichu]";
                        sql = sql + "   from NGUOIDUNG,HOADON";
                        sql = sql + "   where NGUOIDUNG.PK_iMataikhoan = HOADON.FK_iMataikhoan and PK_iMahoadon = " + Id;
                        SqlConnection cnn = new SqlConnection(constr);
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        SqlDataReader data = cmd.ExecuteReader();
                        if (data.HasRows)
                        {
                           // data.Read();
                          //  this.txtSDT.Text = data["sSDT"].ToString();
                           // this.txtDiaChi.Text = data["sDiachi"].ToString();
                           // this.txtPhuPhi.Text = data["iPhuphi"].ToString();
                           // Tongtien = Tongtien + Int64.Parse(data["iPhuphi"].ToString());
                           // this.txtGhiChu.Text = data["sGhichu"].ToString();

                        }
                        cnn.Close();*/
                     //   BindData(Id);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Có lỗi xảy ra");
                //lblThongBaoLoi.Text = ex.Message; 
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
                memb.idquyen = Int32.Parse(data["FK_iMaquyen"].ToString());

            }
            cnn.Close();
            #endregion
            if (memb.idquyen == 0 || memb.idquyen == 1)
                return true;
            else
                return false;
        }

        protected void btnBaocao_Click(object sender, EventArgs e)
        {
            string sql = "BaoCaoHoaDonTrongThang";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@thang", ddlThang.SelectedValue);
            cmd.Parameters.AddWithValue("@nam", ddlNam.SelectedValue);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            cnn.Open();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cnn.Close();
            Chart1.Width = 800;
            Chart1.Height = 300;
            if (dt.Rows.Count > 12)
            {
                //Chart1.Attributes["style"] = "width: 35%;";
                Chart1.Width = 1100;
                Chart1.Height = 300;
            }
            Chart1.DataSource = dt;
            Chart1.DataBind();
            Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
          //  Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8f);
           // Chart1.Series["Series1"]["PixelPointWidth"] = "60";
            Chart1.Visible = true;
            
            grvBanChay.Visible = false;
            grvHangTon.Visible = false;
            grvHoaDon.Visible = false;
            lbltongtien.Visible = false;
            Chart1.Titles[0].Text = "Báo cáo doanh thu tháng " + ddlThang.SelectedValue + "/" + ddlNam.SelectedValue;
        }

        protected void btnXemBaoCao_Click(object sender, EventArgs e)
        {
           /* DateTime tungay_temp = DateTime.Parse(this.txtTuNgay.Text.Trim());
            DateTime denngay_temp = DateTime.Parse(this.txtDenNgay.Text.Trim());
            if (tungay_temp > denngay_temp)
            {
                lblThongBaoLoi.Text = "Từ ngày không được lớn hơn đến ngày";
                txtTuNgay.Focus();
                Chart1.Visible = false;

            }
            else
            {
                string sql = "select PK_iMahoadon,b.sHovaten as [tenkhachhang],NGUOIDUNG.sHovaten as [tennhanvien],HOADON.sSDT,HOADON.sDiachi,isnull(HOADON.iPhuphi,0) as [iPhuphi],";
                sql = sql + " dNgayLap,sGhichu,HOADON.iTrangthai,sum(iSoluong*iDongia)+isnull(iPhuphi,0) as 'iTongtien' ";
                sql = sql + " from HOADON left join NGUOIDUNG on HOADON.FK_iManhanvien = NGUOIDUNG.PK_iMataikhoan left join NGUOIDUNG b on b.PK_iMataikhoan = HOADON.FK_iMataikhoan ";
                sql = sql + " inner join CT_HOADON on HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon where HOADON.iTrangthai = 3 ";
                sql = sql + " and dNgayLap >= '" + tungay_temp.ToString("MM-dd-yyyy") + "' and dNgayLap <= DATEADD(DAY, 1,'" + denngay_temp.ToString("MM-dd-yyyy") +"')";
                sql = sql + " group by PK_iMahoadon,b.sHovaten,NGUOIDUNG.sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,HOADON.iTrangthai";
                SqlConnection Cnn = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
           //     Session["SortedView"] = dt;
                grvHoaDon.DataSource = dt;
                Chart1.Visible = false;
                grvHoaDon.DataBind();
                lblThongBaoLoi.Text = string.Empty;
            }*/
            string chedoxem = ddlChedoxem.SelectedValue;
            switch (chedoxem)
            {
                case "0":
                    Tongtien = 0;
                    grvHoaDon.Visible = true;
                    grvBanChay.Visible = false;
                    grvHangTon.Visible = false;
                    btnExcel.Visible = true;
                    lbltongtien.Visible = true;
                    break;
                case "1":
                    Tongtien = 0;
                    grvHoaDon.Visible = false;
                    grvBanChay.Visible = true;
                    grvHangTon.Visible = false;
                    btnExcel.Visible = false;
                    lbltongtien.Visible = false;
                    break;
                case "2":
                    Tongtien = 0;
                    grvHoaDon.Visible = false;
                    grvBanChay.Visible = false;
                    grvHangTon.Visible = true;
                    btnExcel.Visible = false;
                    lbltongtien.Visible = false;
                    break;
            }
            KhoiTaoDuLieu();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DateTime tungay_temp = DateTime.Parse(this.txtTuNgay.Text.Trim());
            DateTime denngay_temp = DateTime.Parse(this.txtDenNgay.Text.Trim());
            if (tungay_temp > denngay_temp)
            {
                lblThongBaoLoi.Text = "Từ ngày không được lớn hơn đến ngày";
                txtTuNgay.Focus();
                Chart1.Visible = false;

            }
            else
            {
                ghiExcel();

            }
        }

        protected void ghiExcel()
        {
            DateTime tungay_temp = DateTime.Parse(this.txtTuNgay.Text.Trim());
            DateTime denngay_temp = DateTime.Parse(this.txtDenNgay.Text.Trim());
           /* Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];
            
            xlWorkSheet.Cells[1][1] = "Mã hóa đơn";
            xlWorkSheet.Cells[2][1] = "Tên khách hàng";
            xlWorkSheet.Cells[3][1] = "Số điện thoại";
            xlWorkSheet.Cells[4][1] = "Địa chỉ";
            xlWorkSheet.Cells[5][1] = "Phụ phí";
            xlWorkSheet.Cells[6][1] = "Ngày lập";
            xlWorkSheet.Cells[7][1] = "Ghi chú";
            xlWorkSheet.Cells[8][1] = "Tổng tiền";
            string sql = "select PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,";
            sql = sql + " dNgayLap,sGhichu,HOADON.iTrangthai,sum(iSoluong*iDongia)+isnull(iPhuphi,0) as 'iTongtien' ";
            sql = sql + " from HOADON left join NGUOIDUNG on HOADON.FK_iMataikhoan = NGUOIDUNG.PK_iMataikhoan ";
            sql = sql + " inner join CT_HOADON on HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon where HOADON.iTrangthai = 3 ";
            sql = sql + " and dNgayLap >= '" + tungay_temp.ToString("MM-dd-yyyy") + "' and dNgayLap <= DATEADD(DAY, 1,'" + denngay_temp.ToString("MM-dd-yyyy") + "')";
            sql = sql + " group by PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,HOADON.iTrangthai";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int dong = 2;
            foreach (DataRow dr in dt.Rows)
            {
                
                xlWorkSheet.Cells[1][dong] = dr["PK_iMahoadon"].ToString();
                xlWorkSheet.Cells[2][dong] = dr["sHovaten"].ToString();
                xlWorkSheet.Cells[3][dong] = dr["sSDT"].ToString();
                xlWorkSheet.Cells[4][dong] = dr["sDiachi"].ToString();
                xlWorkSheet.Cells[5][dong] = dr["iPhuphi"].ToString();
                xlWorkSheet.Cells[6][dong] = "'" + dr["dNgayLap"].ToString();
                xlWorkSheet.Cells[7][dong] = dr["sGhichu"].ToString();
                xlWorkSheet.Cells[8][dong] = dr["iTongtien"].ToString();
                dong++;
            }
            xlWorkSheet.Range["A1:"+"H"+dong].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
           // xlWorkSheet.Range["F2:" + "F" + dong].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            xlWorkSheet.Columns.AutoFit();
           // wb.SaveCopyAs("Data.xlsx");
            wb.SaveAs("Data.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
            wb.Close(false, Type.Missing, Type.Missing);
            app.Quit();
            */
            //CLOSEDXML
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Sheet1");
                //ws.Cell(1, 1).Value = "Tên cũ";
                ws.Cell(1, 1).Value = "Mã hóa đơn";
                ws.Cell(1, 2).Value = "Tên khách hàng";
                ws.Cell(1, 3).Value = "Số điện thoại";
                ws.Cell(1, 4).Value = "Địa chỉ";
                ws.Cell(1, 5).Value = "Phụ phí";
                ws.Cell(1, 6).Value = "Ngày lập";
                ws.Cell(1, 7).Value = "Ghi chú";
                ws.Cell(1, 8).Value = "Tổng tiền";
                // Dòng + cột
                //ws.Cell(i, 1).Value = i;
                string sql = "select PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,";
                sql = sql + " dNgayLap,sGhichu,HOADON.iTrangthai,sum(iSoluong*CAST(iDongia as bigint)+isnull(iPhuphi,0)) as 'iTongtien' ";
                sql = sql + " from HOADON left join NGUOIDUNG on HOADON.FK_iMataikhoan = NGUOIDUNG.PK_iMataikhoan ";
                sql = sql + " inner join CT_HOADON on HOADON.PK_iMahoadon = CT_HOADON.FK_iMahoadon where HOADON.iTrangthai = 3 ";
                sql = sql + " and dNgayLap >= '" + tungay_temp.ToString("MM-dd-yyyy") + "' and dNgayLap <= DATEADD(DAY, 1,'" + denngay_temp.ToString("MM-dd-yyyy") + "')";
                sql = sql + " group by PK_iMahoadon,sHovaten,HOADON.sSDT,HOADON.sDiachi,HOADON.iPhuphi,dNgayLap,sGhichu,HOADON.iTrangthai";
                SqlConnection Cnn = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int dong = 2;
                foreach (DataRow dr in dt.Rows)
                {
                    ws.Cell( dong,1).Value = dr["PK_iMahoadon"].ToString();
                    ws.Cell( dong,2).Value = dr["sHovaten"].ToString();
                    ws.Cell( dong,3).Value = dr["sSDT"].ToString();
                    ws.Cell( dong,4).Value = dr["sDiachi"].ToString();
                    ws.Cell( dong,5).Value = dr["iPhuphi"].ToString();
                    ws.Cell( dong,6).Value = "'" + dr["dNgayLap"].ToString();
                    ws.Cell( dong,7).Value = dr["sGhichu"].ToString();
                    ws.Cell( dong,8).Value = dr["iTongtien"].ToString();
                    dong++;
                }


                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Columns().AdjustToContents();
                // wb.SaveAs("D:\\styled.xlsx");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + "Data" + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            lblThongBaoLoi.Text = "Đã lưu vào Data.xlsx";
            Chart1.Visible = false;
        
        }

        protected void grvHoaDon_Sorting(object sender, GridViewSortEventArgs e)
        {
            KhoiTaoDuLieu(e.SortExpression);
            lblThongBaoLoi.Text = string.Empty;
        }

        protected void grvHoaDon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvHoaDon.PageIndex = e.NewPageIndex;
            //  grvSanPham.DataBind();    
            //  grvSanPham.DataSource = ViewState["Paging"];    
            if (Session["SortedView"] != null)
            {
                grvHoaDon.DataSource = Session["SortedView"];
                grvHoaDon.DataBind();
                Chart1.Visible = false;
                lblThongBaoLoi.Text = string.Empty;
            }
            else
                KhoiTaoDuLieu();
        }
        protected void quaylaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnAnBaocao_Click(object sender, EventArgs e)
        {
            Chart1.Visible = false;
        }

        protected void grvBanChay_Sorting(object sender, GridViewSortEventArgs e)
        {
            KhoiTaoBanChay(e.SortExpression);
            lblThongBaoLoi.Text = string.Empty;
        }

        protected void grvBanChay_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvBanChay.PageIndex = e.NewPageIndex;
            //  grvSanPham.DataBind();    
            //  grvSanPham.DataSource = ViewState["Paging"];    
            if (Session["SortedView"] != null)
            {
                grvBanChay.DataSource = Session["SortedView"];
                grvBanChay.DataBind();
                Chart1.Visible = false;
                lblThongBaoLoi.Text = string.Empty;
            }
            else
                KhoiTaoBanChay();
        }

        protected void grvBanChay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.DataRow:
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvBanChay, "Select$" + e.Row.RowIndex));
                    e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                    break;
            }
        }

        protected void grvBanChay_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ddlChedoxem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SortedView"] = null;
            KhoiTaoDuLieu(null);
            string chedoxem = ddlChedoxem.SelectedValue;
            switch (chedoxem)
            {
                case "0":
                    Tongtien = 0;
                    grvHoaDon.Visible = true;
                    grvBanChay.Visible = false;
                    grvHangTon.Visible = false;
                    btnExcel.Visible = true;
                    lbltongtien.Visible = true;
                    break;
                case "1":
                    Tongtien = 0;
                    grvHoaDon.Visible = false;
                    grvBanChay.Visible = true;
                    grvHangTon.Visible = false;
                    btnExcel.Visible = false;
                    lbltongtien.Visible = false;
                    break;
                case "2":
                    Tongtien = 0;
                    grvHoaDon.Visible = false;
                    grvBanChay.Visible = false;
                    grvHangTon.Visible = true;
                    btnExcel.Visible = false;
                    lbltongtien.Visible = false;
                    break;
            }
        }

        protected void grvHangTon_Sorting(object sender, GridViewSortEventArgs e)
        {
            KhoiTaoHangTon(e.SortExpression);
            lblThongBaoLoi.Text = string.Empty;
        }

        protected void grvHangTon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvHangTon.PageIndex = e.NewPageIndex;
            //  grvSanPham.DataBind();    
            //  grvSanPham.DataSource = ViewState["Paging"];    
            if (Session["SortedView"] != null)
            {
                grvHangTon.DataSource = Session["SortedView"];
                grvHangTon.DataBind();
                Chart1.Visible = false;
                lblThongBaoLoi.Text = string.Empty;
            }
            else
                KhoiTaoHangTon();
        }

        protected void grvHangTon_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.DataRow:
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvHangTon, "Select$" + e.Row.RowIndex));
                    e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                    break;
            }
        }
    }
}