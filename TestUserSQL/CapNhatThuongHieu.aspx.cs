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
    
    public partial class WebForm13 : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int taikhoanid = 0;
        static int thuonghieuid = 0;
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
            string sql = " select * from THUONGHIEU ";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvThuongHieu.DataSource = dt;
            grvThuongHieu.DataBind();
            thuonghieuid = 0;
            txtMathuonghieu.Text = string.Empty;
            txtTenthuonghieu.Text = string.Empty;
            lblThongBaoLoi.Text = string.Empty;
        }

        protected void grvThuongHieu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.DataRow:
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvThuongHieu, "Select$" + e.Row.RowIndex));
                    e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                    break;
            }
        }

        protected void grvThuongHieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in this.grvThuongHieu.Rows)
                {
                    if (row.RowIndex == grvThuongHieu.SelectedIndex)
                    {
                        // Tongtien = 0;
                        int Id = Convert.ToInt32(grvThuongHieu.SelectedDataKey.Value.ToString());
                        thuonghieuid = Id;
                        string sql = "select * from THUONGHIEU where PK_iMathuonghieu = " + Id;
                        SqlConnection cnn = new SqlConnection(constr);
                        SqlCommand cmd = new SqlCommand(sql, cnn);
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        lblThongBaoLoi.Text = string.Empty;
                        SqlDataReader data = cmd.ExecuteReader();
                        if (data.HasRows)
                        {
                            data.Read();
                            txtTenthuonghieu.Text = data["sTenthuonghieu"].ToString();
                            txtMathuonghieu.Text = data["PK_iMathuonghieu"].ToString();
                           

                        }
                        cnn.Close();
                        
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

        protected void btnThem_Click(object sender, EventArgs e)
        {
            string tenthuonghieu = txtTenthuonghieu.Text.Trim();
            if (KiemTraThem(tenthuonghieu))
            {

                string insertsql = "  insert into THUONGHIEU values (" + "N'" + tenthuonghieu + "')  ";
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

        protected bool KiemTraThem(string tenthuonghieu)
        {

            string sql = "select * from THUONGHIEU where sTenthuonghieu = N'" + tenthuonghieu + "'";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                cnn.Close();
                lblThongBaoLoi.Text = "Đã có thương hiệu này!!";
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
            if (grvThuongHieu.SelectedDataKey != null && thuonghieuid != 0)
            {
                string id = grvThuongHieu.SelectedDataKey.Value.ToString();
                // int check = 0;
                string tenthuonghieu = txtTenthuonghieu.Text.Trim();
                if (KiemTraThem(tenthuonghieu))
                {
                    string sql = "update THUONGHIEU set sTenthuonghieu = N'" + tenthuonghieu + "'  where PK_iMathuonghieu = " + id;
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
                lblThongBaoLoi.Text = "Chưa chọn thương hiệu";
        }

        protected void btnXoa_Click(object sender, EventArgs e)
        {
            if (grvThuongHieu.SelectedDataKey != null && thuonghieuid != 0)
            {
                string id = grvThuongHieu.SelectedDataKey.Value.ToString();
                int check = 0;
                SqlConnection cnn = new SqlConnection(constr);
                // Ten Procedure
                string sql = "checkDelThuongHieu";

                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaThuongHieu", id);

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
                    lblThongBaoLoi.Text = "Thương hiệu này đã có sản phẩm, không xóa được!";
                }
                else
                {
                    string sql2 = "delete from THUONGHIEU where PK_iMathuonghieu = " + id;

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
                lblThongBaoLoi.Text = "Chưa chọn thương hiệu";
        }
        protected void quaylaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void grvThuongHieu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvThuongHieu.PageIndex = e.NewPageIndex;
            KhoiTaoDuLieu();
        }



    }
}