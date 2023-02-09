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
    public partial class PhanQuyen : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int current_quyen;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated == false)
                    Response.Redirect("DangNhap.aspx");
                if (!checkadmin())
                    Response.Redirect("Default.aspx");
                KhoiTaoDuLieu();   
            }
        }
        protected void KhoiTaoDuLieu()
        {
            string sql = "select PK_iMataikhoan,FK_iMaquyen,sTentaikhoan,sHovaten from NGUOIDUNG where FK_iMaquyen != 0";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvTaiKhoan.DataSource = dt;
            grvTaiKhoan.DataBind();
            current_quyen = 0;
            txtTentaikhoan.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            ddlQuyen.SelectedIndex = 0;
            lblThongBaoLoi.Text = string.Empty;
        }

        protected bool checkadmin()
        {
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
            if (memb.idquyen != 0)
                return false;
            else
                return true;
        }

        protected void grvTaiKhoan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvTaiKhoan, "Select$" + e.Row.RowIndex));
                e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                Label lbl = e.Row.FindControl("quyen") as Label;
                if (lbl.Text.Equals("2"))
                {
                    lbl.Text = "Khách hàng";

                }
                if (lbl.Text.Equals("1"))
                {
                    lbl.Text = "Nhân viên";
                }
            }
            
        }

        protected void grvTaiKhoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grvTaiKhoan.Rows)
            {
                if (row.RowIndex == grvTaiKhoan.SelectedIndex)
                {
                    string sql = "select PK_iMataikhoan,FK_iMaquyen,sTentaikhoan,sHovaten from NGUOIDUNG where PK_iMataikhoan = " + grvTaiKhoan.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Read();
                        this.txtTentaikhoan.Text = data["sTentaikhoan"].ToString();
                        this.txtHoTen.Text = data["sHovaten"].ToString();
                        this.ddlQuyen.SelectedIndex = Int32.Parse(data["FK_iMaquyen"].ToString());
                        current_quyen = this.ddlQuyen.SelectedIndex;
                        lblThongBaoLoi.Text = string.Empty;
                    }
                    cnn.Close();
                }
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            if (!this.ddlQuyen.SelectedValue.ToString().Equals(current_quyen.ToString()))
            {
                string sql = "update NGUOIDUNG set FK_iMaquyen = " + this.ddlQuyen.SelectedValue + " where PK_iMataikhoan = " + grvTaiKhoan.SelectedDataKey.Value.ToString();
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

        protected void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiem.Text.Trim().Length >= 2)
            {
                string search = txtTimKiem.Text;
                string sql = "select PK_iMataikhoan,FK_iMaquyen,sTentaikhoan,sHovaten from NGUOIDUNG where FK_iMaquyen != 0 and sHovaten  Like N'%" + search + "%'";
                SqlConnection Cnn = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grvTaiKhoan.DataSource = dt;
                grvTaiKhoan.DataBind();
                current_quyen = 0;
                txtTentaikhoan.Text = string.Empty;
                txtHoTen.Text = string.Empty;
                ddlQuyen.SelectedIndex = 0;
                lblThongBaoLoi.Text = string.Empty;
            }
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

        protected void grvTaiKhoan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvTaiKhoan.PageIndex = e.NewPageIndex;
            KhoiTaoDuLieu();
        }
    }
}