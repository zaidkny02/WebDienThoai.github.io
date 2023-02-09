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
    public partial class WebForm14 : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int taikhoanid = 0;
        static int khuyenmaiid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated == false)
                    Response.Redirect("DangNhap.aspx");
                if (!checkadmin())
                    Response.Redirect("TrangItem.aspx");
                KhoiTaoSanPham();
                KhoiTaoDuLieu();
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
            string sql = " select PK_iMakhuyenmai,sTensanpham,iGiaban,FK_iMasanpham,iTilekhuyenmai,dNgaybatdau,dNgayketthuc,KHUYENMAI.iTrangthai,sGhichu ";
            sql = sql + " from SANPHAM,KHUYENMAI where KHUYENMAI.FK_iMasanpham = SANPHAM.PK_iMasanpham";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvKhuyenMai.DataSource = dt;
            grvKhuyenMai.DataBind();
            khuyenmaiid = 0;
            txtTileKhuyenMai.Text = string.Empty;
            ddlSanpham.SelectedIndex = 0;
            ddlSanpham.Enabled = true;
            txtDenNgay.Text = string.Empty;
            txtTuNgay.Text = string.Empty;
            txtGhichu.Text = string.Empty;
            lblThongBaoLoi.Text = string.Empty;
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

        protected void grvKhuyenMai_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.DataRow:
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvKhuyenMai, "Select$" + e.Row.RowIndex));
                    e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                    Label lbl = e.Row.FindControl("trangthai") as Label;
                    if (lbl.Text.Equals("1"))
                    {
                        lbl.Text = "Còn hạn";

                    }
                    if (lbl.Text.Equals("0"))
                    {
                        lbl.Text = "Hết hạn";
                    }
                    break;
            }
        }

        protected void grvKhuyenMai_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in this.grvKhuyenMai.Rows)
                {
                    if (row.RowIndex == grvKhuyenMai.SelectedIndex)
                    {
                        // Tongtien = 0;
                        int Id = Convert.ToInt32(grvKhuyenMai.SelectedDataKey.Value.ToString());
                        khuyenmaiid = Id;
                        string sql = "select PK_iMakhuyenmai,sTensanpham,iGiaban,FK_iMasanpham,iTilekhuyenmai,dNgaybatdau,dNgayketthuc,KHUYENMAI.iTrangthai,sGhichu";
                        sql = sql + " from SANPHAM,KHUYENMAI where KHUYENMAI.FK_iMasanpham = SANPHAM.PK_iMasanpham and PK_iMakhuyenmai = " + Id;
                        SqlConnection cnn = new SqlConnection(constr);
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        lblThongBaoLoi.Text = string.Empty;
                        SqlDataReader data = cmd.ExecuteReader();
                        if (data.HasRows)
                        {
                            data.Read();
                            txtTileKhuyenMai.Text = data["iTilekhuyenmai"].ToString();
                            string idsp = data["FK_iMasanpham"].ToString();
                            foreach (ListItem item in ddlSanpham.Items)
                            {
                                if (item.Value.Equals(idsp))
                                {
                                    ddlSanpham.ClearSelection();
                                    item.Selected = true;
                                    
                                    break;
                                }
                            }
                            txtGhichu.Text = data["sGhichu"].ToString();
                            if (Int32.Parse(data["iTrangthai"].ToString()) == 1)
                            {
                                this.ddlTrangThai.SelectedIndex = 0;
                            }
                            else
                                this.ddlTrangThai.SelectedIndex = 1;
                            if (!data["dNgaybatdau"].ToString().Equals(""))
                            {
                                DateTime a = new DateTime();
                                a = DateTime.Parse(data["dNgaybatdau"].ToString());
                                txtTuNgay.Text = a.ToString("yyyy-MM-dd");
                            }
                            else
                                txtTuNgay.Text = null; 
                            if (!data["dNgayketthuc"].ToString().Equals(""))
                            {
                                DateTime b = new DateTime();
                                b = DateTime.Parse(data["dNgayketthuc"].ToString());
                                txtDenNgay.Text = b.ToString("yyyy-MM-dd");
                            }
                            else
                                txtDenNgay.Text = null;
                            ddlSanpham.Enabled = false;
                            lblThongBaoLoi.Text = string.Empty;
                        }
                        cnn.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Có lỗi xảy ra");
                lblThongBaoLoi.Text = ex.Message; 
            }
        }

        protected bool KiemTraThem(string id_sp, DateTime TuNgay, DateTime DenNgay,string makhuyenmai)
        {
            if (DateTime.Compare(TuNgay, DenNgay) <= 0)
            {
                //Tên của procedure
                string sql = "checkaddKhuyenmai";
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FK_iMasanpham", id_sp);
                cmd.Parameters.AddWithValue("@dNgaybatdau", TuNgay.ToShortDateString());
                cmd.Parameters.AddWithValue("@dNgayketthuc", DenNgay.ToShortDateString());
                cmd.Parameters.AddWithValue("@iMakhuyenmai", makhuyenmai);
                int check = 0;
                cnn.Open();
                SqlDataReader data = cmd.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    check = Int32.Parse(data["ADD_CHECK"].ToString());
                }
                cnn.Close();
                if (check == 0)
                    return true;
                else
                {
                    lblThongBaoLoi.Text = "Sản phẩm này đã có khuyến mại trong khoảng thời gian này";
                    return false;
                }
            }
            else
            {
                lblThongBaoLoi.Text = "Ngày bắt đầu không được lớn hơn ngày kết thúc";
                return false;
            }
        }

        protected void btnThem_Click(object sender, EventArgs e)
        {
            string TuNgay = txtTuNgay.Text;
            string DenNgay = txtDenNgay.Text;
            string tilekhuyenmai = txtTileKhuyenMai.Text.Trim();
            DateTime dt_TuNgay, dt_DenNgay;
           // lblThongBaoLoi.Text = TuNgay + "||" + DenNgay;
            string id_sp = ddlSanpham.SelectedValue;
            if(TuNgay == null || TuNgay == string.Empty)
                dt_TuNgay = new DateTime(2000, 01, 01); 
            else
                dt_TuNgay = DateTime.Parse(TuNgay);
            if (DenNgay == null || DenNgay == string.Empty)
                dt_DenNgay = new DateTime(2100, 12, 12);
            else
                dt_DenNgay = DateTime.Parse(DenNgay);
        //    DateTime dt_DenNgay = DateTime.Parse(DenNgay);
            
            if (KiemTraThem(id_sp, dt_TuNgay, dt_DenNgay,"0"))
            {
                string sql = "insert into KHUYENMAI values (" + id_sp + "," + tilekhuyenmai + ",'" + dt_TuNgay.ToShortDateString() + "','" + dt_DenNgay.ToShortDateString() + "'," + "1,N'" + txtGhichu.Text.Trim() + "')";
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                KhoiTaoDuLieu();
                lblThongBaoLoi.Text = "Thêm thành công";
            }
        }

        protected void btnSua_Click(object sender, EventArgs e)
        {
            if (grvKhuyenMai.SelectedDataKey != null && khuyenmaiid != 0)
            {
                string TuNgay = txtTuNgay.Text;
                string DenNgay = txtDenNgay.Text;
                string tilekhuyenmai = txtTileKhuyenMai.Text.Trim();
                DateTime dt_TuNgay, dt_DenNgay;
                // lblThongBaoLoi.Text = TuNgay + "||" + DenNgay;
                string id_sp = ddlSanpham.SelectedValue;
                if (TuNgay == null || TuNgay == string.Empty)
                    dt_TuNgay = new DateTime(2000, 01, 01);
                else
                    dt_TuNgay = DateTime.Parse(TuNgay);
                if (DenNgay == null || DenNgay == string.Empty)
                    dt_DenNgay = new DateTime(2100, 12, 12);
                else
                    dt_DenNgay = DateTime.Parse(DenNgay);
                //    DateTime dt_DenNgay = DateTime.Parse(DenNgay);

                if (KiemTraThem(id_sp, dt_TuNgay, dt_DenNgay, grvKhuyenMai.SelectedDataKey.Value.ToString()))
                {
                    string sql = "UPDATE KHUYENMAI set iTilekhuyenmai = " + tilekhuyenmai + ", dNgaybatdau = '" + dt_TuNgay.ToShortDateString() + "', dNgayketthuc = '" + dt_DenNgay.ToShortDateString() + "',sGhichu = N'" + txtGhichu.Text.Trim() + "' where PK_iMakhuyenmai =" + grvKhuyenMai.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    KhoiTaoDuLieu();
                    lblThongBaoLoi.Text = "Cập nhật thành công";
                }
                
            }
            else
                    lblThongBaoLoi.Text = "Chưa chọn bản ghi";
        }

        protected void btnXoa_Click(object sender, EventArgs e)
        {
            if (grvKhuyenMai.SelectedDataKey != null && khuyenmaiid != 0)
            {
                string sql = "delete from KHUYENMAI where PK_iMakhuyenmai =" + grvKhuyenMai.SelectedDataKey.Value.ToString();
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                KhoiTaoDuLieu();
                lblThongBaoLoi.Text = "Xóa thành công";
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn bản ghi";
        }

        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            KhoiTaoDuLieu();
        }

        

        protected void grvKhuyenMai_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvKhuyenMai.PageIndex = e.NewPageIndex;
            KhoiTaoDuLieu();
        }
        protected void quaylaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }



    }
}