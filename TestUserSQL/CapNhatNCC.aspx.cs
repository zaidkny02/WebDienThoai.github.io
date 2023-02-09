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
    public partial class WebForm12 : System.Web.UI.Page
    {
        
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int taikhoanid = 0;
        static int nhacungcapid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated == false)
                    Response.Redirect("DangNhap.aspx");
                if (!checkadmin())
                    Response.Redirect("TrangItem.aspx");
                KhoiTaoDuLieu();
            }
        }

        protected void KhoiTaoDuLieu()
        {
            string sql = " select * from NHACUNGCAP ";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvNCC.DataSource = dt;
            grvNCC.DataBind();
            nhacungcapid = 0;
            txtEmail.Text = string.Empty;
            txtSDT.Text = string.Empty;
            txtDiachi.Text = string.Empty;
            txtTenNCC.Text = string.Empty;
            lblThongBaoLoi.Text = string.Empty;
        }

        protected void grvNCC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.DataRow:
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvNCC, "Select$" + e.Row.RowIndex));
                    e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                    break;
            }
        }

        protected void grvNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in this.grvNCC.Rows)
                {
                    if (row.RowIndex == grvNCC.SelectedIndex)
                    {
                        // Tongtien = 0;
                        int Id = Convert.ToInt32(grvNCC.SelectedDataKey.Value.ToString());
                        nhacungcapid = Id;
                        string sql = "select * from NHACUNGCAP where PK_iMaNCC = " + Id;
                        SqlConnection cnn = new SqlConnection(constr);
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        lblThongBaoLoi.Text = string.Empty ;
                        SqlDataReader data = cmd.ExecuteReader();
                        if (data.HasRows)
                        {
                            data.Read();
                            txtTenNCC.Text = data["sTenNCC"].ToString();
                            txtDiachi.Text = data["sDiachi"].ToString();
                            txtSDT.Text = data["sSDT"].ToString();
                            txtEmail.Text = data["sEmail"].ToString();
                            //  this.txtSDT.Text = data["sSDT"].ToString();
                            // this.txtDiaChi.Text = data["sDiachi"].ToString();
                            // this.txtPhuPhi.Text = data["iPhuphi"].ToString();
                            // Tongtien = Tongtien + Int64.Parse(data["iPhuphi"].ToString());
                            // this.txtGhiChu.Text = data["sGhichu"].ToString();

                        }
                        cnn.Close();
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

        protected void grvNCC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvNCC.PageIndex = e.NewPageIndex;
            KhoiTaoDuLieu();
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

        protected void btnThem_Click(object sender, EventArgs e)
        {
            string tennhacungcap = txtTenNCC.Text.Trim();
            if (KiemTraThem(tennhacungcap))
            {
                string sdt = txtSDT.Text.Trim();
                string diachi = txtDiachi.Text.Trim();
                string email = txtEmail.Text.Trim();
                string insertsql = "  insert into NHACUNGCAP values (" + "N'" + tennhacungcap + "','" + sdt + "',N'"+ diachi  +"','"+email+"')  ";
                //  SET IDENTITY_INSERT tbl_taikhoan ON;
                using (SqlConnection cnn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(insertsql, cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                    }
                }
                
                KhoiTaoDuLieu();
                lblThongBaoLoi.Text = "Thêm thành công";
            }
        }
        protected bool KiemTraThem(string tennhacungcap)
        {

            string sql = "select * from NHACUNGCAP where sTenNCC = N'" + tennhacungcap + "' and PK_iMaNCC != " + grvNCC.SelectedValue.ToString();
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                cnn.Close();
                lblThongBaoLoi.Text = "Đã có nhà cung cấp này!!";
                return false;

            }
            else
            {
                cnn.Close();
                return true;
            }
        }

        protected void btnSua_Click(object sender, EventArgs e)
        {
            if (grvNCC.SelectedDataKey != null && nhacungcapid != 0)
            {
                string id = grvNCC.SelectedDataKey.Value.ToString();
               // int check = 0;
                string tennhacungcap = txtTenNCC.Text.Trim();
                if (KiemTraThem(tennhacungcap))
                {
                    string sdt = txtSDT.Text.Trim();
                    string diachi = txtDiachi.Text.Trim();
                    string email = txtEmail.Text.Trim();
                    string sql = "update NHACUNGCAP set sTenNCC = N'" + tennhacungcap + "',sSDT = N'" + sdt + "',sDiachi = N'" + diachi + "',sEmail = '" + email + "'  where PK_iMaNCC = " + id;
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
                lblThongBaoLoi.Text = "Chưa chọn nhà cung cấp";
        }

        protected void btnXoa_Click(object sender, EventArgs e)
        {
            if (grvNCC.SelectedDataKey != null && nhacungcapid != 0)
            {
                string id = grvNCC.SelectedDataKey.Value.ToString();
                int check = 0;
                SqlConnection cnn = new SqlConnection(constr);
                // Ten Procedure
                string sql = "checkDelNCC";

                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNCC", id);

                cnn.Open();
                SqlDataReader data = cmd.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    check = Int32.Parse(data["DEL_CHECK"].ToString());
                }
                cnn.Close();
                if (check != 0)
                {
                    lblThongBaoLoi.Text = "Nhà cung cấp này có phiếu nhập, không xóa được!";
                }
                else
                {
                    string sql2 = "delete from NHACUNGCAP where PK_iMaNCC = " + id;

                    SqlCommand cmd2 = new SqlCommand(sql2, cnn);
                    cmd2.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd2.ExecuteNonQuery();
                    cnn.Close();
                    KhoiTaoDuLieu();
                    lblThongBaoLoi.Text = "Xóa thành công";
                }
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn nhà cung cấp";
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            string search = txtTimKiem.Text;
            string sql = "select * from NHACUNGCAP where sTenNCC Like N'%" + search + "%'";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvNCC.DataSource = dt;
            grvNCC.DataBind();
            nhacungcapid = 0;
            txtEmail.Text = string.Empty;
            txtSDT.Text = string.Empty;
            txtDiachi.Text = string.Empty;
            txtTenNCC.Text = string.Empty;
            lblThongBaoLoi.Text = string.Empty;
        }

        protected void btnrefesh_Click(object sender, EventArgs e)
        {
            KhoiTaoDuLieu();
            txtTimKiem.Text = string.Empty;
        }
        protected void quaylaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}