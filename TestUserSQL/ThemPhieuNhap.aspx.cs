using System;
using System.Collections;
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
    public partial class WebForm7 : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int idnguoidung = 0;
        static int current_page;
        int page_size = 8;
        int countItem;
        static int id_Phieunhap = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated == false)
                    Response.Redirect("TrangItem.aspx");
                if (!checkadmin())
                    Response.Redirect("Default.aspx");
                KhoiTaoNhaCungCap();
                SqlConnection Cnn = new SqlConnection(constr);
                string sql;
                // data for Paging
                SqlCommand cmd = new SqlCommand("select count(PK_iMaphieunhap) as soluong from PHIEUNHAP", Cnn);
                cmd.CommandType = CommandType.Text;
                Cnn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    countItem = int.Parse(dr["soluong"].ToString());
                }
                Cnn.Close();
                int countPage = countItem / page_size;
                if (countItem % page_size > 0 && countPage > 0)
                    countPage++;
                //  countPage = countPage >= 1 && countItem % page_size > 0 ? countPage++ : countPage;
                bool try_Parse;
                if (Request.QueryString["page"] != null)
                    try_Parse = int.TryParse(Request.QueryString["page"].ToString(), out current_page);
                else
                    try_Parse = false;
                //set current page from 1 to final page (1<=page<=countpage)
                if (!try_Parse || current_page < 1 || countPage < 2)
                    current_page = 1;
                if (current_page > countPage && countPage > 1)
                    current_page = countPage;
                int except = (current_page - 1) * page_size < 0 ? 0 : (current_page - 1) * page_size;
                int current_item = current_page * page_size;
                KhoiTaoDuLieu(current_page);
                if (countPage > 1)
                    CreatePaging();
            }
        }
        #region LayNhaCungCap
        protected void KhoiTaoNhaCungCap()
        {
            string sql = "select PK_iMaNCC,sTenNCC from NHACUNGCAP ";
            SqlConnection cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ddlNhaCungCap.DataSource = dt;
            ddlNhaCungCap.DataTextField = "sTenNCC";
            ddlNhaCungCap.DataValueField = "PK_iMaNCC";
            ddlNhaCungCap.DataBind();
            ListItem item = new ListItem("Chọn", "0");
            ddlNhaCungCap.Items.Insert(0, item);    
            
        }
        #endregion
        protected void KhoiTaoDuLieu(int current_page)
        {
            int except = (current_page - 1) * page_size < 0 ? 0 : (current_page - 1) * page_size;
            string sql = "select top " + page_size + " PK_iMaphieunhap,sTenNCC,sTennguoigiao,dNgayLap,sGhichu,NGUOIDUNG.sHovaten, PHIEUNHAP.iTrangthai";
            sql = sql + " from PHIEUNHAP,NGUOIDUNG,NHACUNGCAP ";
            sql = sql + " where PK_iMaphieunhap not in (select top " + except + " PK_iMaphieunhap from PHIEUNHAP) and PHIEUNHAP.FK_iMaNCC = NHACUNGCAP.PK_iMaNCC and PHIEUNHAP.FK_iMataikhoan = NGUOIDUNG.PK_iMataikhoan ";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvPhieunhap.DataSource = dt;
            grvPhieunhap.DataBind();
            txtTrangThai.Text = string.Empty;
            txtNgaylap.Text = null;
            txtTennguoigiao.Text = string.Empty;
            txtGhichu.Text = string.Empty;
            ddlNhaCungCap.SelectedIndex = 0;
            lblThongBaoLoi.Text = string.Empty;
            addBtn.Enabled = true;
            delBtn.Enabled = true;
            saveBtn.Enabled = true;
            id_Phieunhap = 0;
        }


        protected void grvPhieunhap_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.DataRow:
                    e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvPhieunhap, "Select$" + e.Row.RowIndex));
                    Label lbl = e.Row.FindControl("trangthai") as Label;
                    if (lbl.Text.Equals("1"))
                    {
                        lbl.Text = "Chưa khóa phiếu";

                    }
                    if (lbl.Text.Equals("0"))
                    {
                        lbl.Text = "Đã khóa phiếu";
                        
                    }
                    Label lbl2 = e.Row.FindControl("lblnguoilap") as Label;
                    if (lbl2.Text.Equals(""))
                        lbl2.Text = "Unname";
                    break;
            }
        }

        protected void repeaterPaging_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // item click event
            int nextPage = Convert.ToInt32(e.CommandArgument);
            // string a = DropDownList1.SelectedItem.Value;
            Response.Redirect("ThemPhieuNhap.aspx?page=" + nextPage);
        }

        protected void grvPhieunhap_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grvPhieunhap.Rows)
            {
                if (row.RowIndex == grvPhieunhap.SelectedIndex)
                {
                    string sql = "select PK_iMaphieunhap,PK_iMaNCC,sTennguoigiao,dNgayLap,sGhichu,NGUOIDUNG.sHovaten, PHIEUNHAP.iTrangthai";
                    sql = sql + " from PHIEUNHAP,NGUOIDUNG,NHACUNGCAP ";
                    sql = sql + " where PHIEUNHAP.FK_iMaNCC = NHACUNGCAP.PK_iMaNCC and PHIEUNHAP.FK_iMataikhoan = NGUOIDUNG.PK_iMataikhoan ";
                    sql = sql + " and PK_iMaphieunhap = " + grvPhieunhap.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Read();
                        id_Phieunhap = Int32.Parse(data["PK_iMaphieunhap"].ToString());
                        this.txtTennguoigiao.Text = data["sTennguoigiao"].ToString();
                        DateTime a = new DateTime();
                        a = DateTime.Parse(data["dNgayLap"].ToString());
                        txtNgaylap.Text = a.ToString("yyyy-MM-dd");
                        string MaNCC = data["PK_iMaNCC"].ToString();
                        this.txtGhichu.Text = data["sGhichu"].ToString();
                        foreach (ListItem item in ddlNhaCungCap.Items)
                        {
                            if (item.Value.Equals(MaNCC))
                            {
                                ddlNhaCungCap.ClearSelection();
                                item.Selected = true;
                                break;
                            }
                        }
                        if (Int32.Parse(data["iTrangthai"].ToString()) == 1)
                        {
                            txtTrangThai.Text = "Chưa khóa phiếu";
                            addBtn.Enabled = true;
                            delBtn.Enabled = true;
                            saveBtn.Enabled = true;
                        }

                        else
                        {
                            txtTrangThai.Text = "Đã khóa phiếu";
                            addBtn.Enabled = false;
                            delBtn.Enabled = false;
                            saveBtn.Enabled = false;
                        }

                    }
                    cnn.Close();
                    lblThongBaoLoi.Text = string.Empty;
                  //  id_Sp = 0;
                 //   chitiet.Visible = false;
                }
            }
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

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            string search = txtTimKiem.Text.Trim();
            if (search == "")
                KhoiTaoDuLieu(current_page);
            else
            {
                string sql = "select PK_iMaphieunhap,sTenNCC,sTennguoigiao,dNgayLap,sGhichu,NGUOIDUNG.sHovaten, PHIEUNHAP.iTrangthai";
                sql = sql + " from PHIEUNHAP,NGUOIDUNG,NHACUNGCAP ";
                sql = sql + " where PHIEUNHAP.FK_iMaNCC = NHACUNGCAP.PK_iMaNCC and PHIEUNHAP.FK_iMataikhoan = NGUOIDUNG.PK_iMataikhoan ";
                sql = sql + " and  (sTenNCC LIKE N'%" + search + "%' or NGUOIDUNG.sHovaten LIKE N'%" + search + "%')";
                SqlConnection Cnn = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grvPhieunhap.DataSource = dt;
                grvPhieunhap.DataBind();
                txtTrangThai.Text = string.Empty;
                txtNgaylap.Text = null;
                txtTennguoigiao.Text = string.Empty;
                txtGhichu.Text = string.Empty;
                ddlNhaCungCap.SelectedIndex = 0;
                repeaterPaging.Visible = false;
                id_Phieunhap = 0;
                current_page = 1;
                lblThongBaoLoi.Text = string.Empty;
            }
        }

        protected void btnrefesh_Click(object sender, EventArgs e)
        {
            KhoiTaoDuLieu(1);
            txtTimKiem.Text = string.Empty;
            
            repeaterPaging.Visible = true;
        }

        protected void addBtn_Click(object sender, EventArgs e)
        {
            string nguoigiaohang = txtTennguoigiao.Text.Trim();
            string ghichu = txtGhichu.Text.Trim();
            int idNCC = Int32.Parse(ddlNhaCungCap.SelectedValue);
            DateTime ngaylap = DateTime.Now;
            string dt = ngaylap.ToString("MM-dd-yyyy hh:mm");
            string sql = "insert into PHIEUNHAP values (" + idNCC + "," + idnguoidung + ",N'" + nguoigiaohang + "','" + dt + "',N'" + ghichu + "',1 )";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
            KhoiTaoDuLieu(1);
        }


        protected void CreatePaging()
        {
            repeaterPaging.Visible = true;
            int countPage = countItem / page_size;
            if (countItem % page_size > 0)
                countPage++;

            // max page display = max_pre_and_next_display*2 + max_start_and_end_display*2 + 1
            int max_pre_and_next_display = 4; //must > 0
            int max_start_and_end_display = 3; // must > 0
            ArrayList pages = new ArrayList();
            // create from start to current page
            if (current_page <= max_pre_and_next_display + max_start_and_end_display)
            {
                for (int i = 0; i <= current_page - 1; i++)
                {
                    pages.Add((i + 1).ToString());
                }
            }
            else
            {
                for (int i = 0; i <= max_start_and_end_display - 1; i++)
                {
                    pages.Add((i + 1).ToString());
                }
                for (int i = current_page - max_pre_and_next_display; i <= current_page; i++)
                {
                    pages.Add((i).ToString());
                }

            }
            // create from current to end page
            if (current_page > countPage)
                current_page = countPage;
            if (current_page + max_pre_and_next_display >= countPage)
            {
                for (int i = current_page; i < countPage; i++)
                {
                    pages.Add((i + 1).ToString());
                }
            }
            else
            {
                for (int i = current_page; i < current_page + max_pre_and_next_display; i++)
                {
                    pages.Add((i + 1).ToString());
                }

                if (current_page + max_pre_and_next_display > countPage - max_start_and_end_display)
                {
                    for (int i = current_page + max_pre_and_next_display; i < countPage; i++)
                    {
                        pages.Add((i + 1).ToString());
                    }
                }
                else
                {
                    for (int i = countPage - max_start_and_end_display; i < countPage; i++)
                    {
                        pages.Add((i + 1).ToString());
                    }
                }
            }
            repeaterPaging.DataSource = pages;
            repeaterPaging.DataBind();
            // disable current page link button
            foreach (RepeaterItem i in repeaterPaging.Items)
            {
                System.Web.UI.WebControls.LinkButton btn = i.FindControl("btnPage") as System.Web.UI.WebControls.LinkButton;

                if (btn.Text.Equals(current_page.ToString()))
                {
                    btn.Enabled = false;
                    break;
                }
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            if (grvPhieunhap.SelectedDataKey != null && id_Phieunhap != 0)
            {
                string nguoigiaohang = txtTennguoigiao.Text.Trim();
                string ghichu = txtGhichu.Text.Trim();
                int idNCC = Int32.Parse(ddlNhaCungCap.SelectedValue);
                //    DateTime ngaylap = DateTime.Now;
                //    string dt = ngaylap.ToString("MM-dd-yyyy hh:mm");
                string sql = "update PHIEUNHAP set FK_iMaNCC = " + idNCC + ",sTennguoigiao = N'" + nguoigiaohang + "',sGhichu = N'" + ghichu + "' ";
                sql = sql + " where PK_iMaphieunhap = " + grvPhieunhap.SelectedDataKey.Value.ToString();
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                btnTimKiem_Click(btnTimKiem, e);
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn phiếu nhập";
        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            if (grvPhieunhap.SelectedDataKey != null && id_Phieunhap != 0)
            {
                
                string sql = "delete from PHIEUNHAP where PK_iMaphieunhap = " + grvPhieunhap.SelectedDataKey.Value.ToString();
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                btnTimKiem_Click(btnTimKiem, e);
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn phiếu nhập";
        }

        protected void lkbtnChiTiet_Click(object sender, EventArgs e)
        {
            LinkButton lkbtn = (LinkButton)sender;
            int id = Int32.Parse(lkbtn.CommandArgument);
            Response.Redirect("ChiTietPhieuNhap.aspx?id=" + id);
        }
        protected void quaylaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}