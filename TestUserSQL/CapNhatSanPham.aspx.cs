using System;
using System.Collections;
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
    public partial class WebForm6 : System.Web.UI.Page
    {
        private string SortDirection
        {
            get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        int page_size = 3;
        int countItem;
        static int current_page;
        static int id_Sp = 0;
        static string ten_sp;
        static string OldImageURL = "";
       // string order;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!checkadmin())
                    Response.Redirect("TrangItem.aspx");
             /*   SqlConnection Cnn = new SqlConnection(constr);
                string sql;
                // data for Paging
                SqlCommand cmd = new SqlCommand("select count(PK_iMasanpham) as soluong from SANPHAM", Cnn);
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
                int current_item = current_page * page_size;*/
                Session["SortedView"] = null;
                KhoiTaoDuLieu();
               /* if (countPage > 1)
                    CreatePaging();*/

            }
        }


        protected bool checkadmin()
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

        protected void KhoiTaoDuLieu( string sortExpression = null)
        {
            int except = (current_page - 1) * page_size < 0 ? 0 : (current_page - 1) * page_size;
            //string sql = "select top " + page_size + "  * from SANPHAM WHERE PK_iMasanpham not in (select top " + except + " PK_iMasanpham From SANPHAM ) ";
            string sql = "select * from SANPHAM";
            if (txtTimkiem.Text.Trim() != "")
                sql = sql + " WHERE sTensanpham LIKE N'%" + txtTimkiem.Text.Trim() + "%'";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (sortExpression != null)
            {
                DataView dv = dt.AsDataView();
                this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";

                dv.Sort = sortExpression + " " + this.SortDirection;
                grvSanPham.DataSource = dv;
            }
            else
            {
                grvSanPham.DataSource = dt;
            }
            
            grvSanPham.DataBind();
            txtGiaban.Text = string.Empty;
            txtSoluong.Text = string.Empty;
            txtTensanpham.Text = string.Empty;
            txtMota.Text = string.Empty;
            id_Sp = 0;
            ten_sp = string.Empty;
            chitiet.Visible = false;
            binhthuong.Visible = true;
            //lblThongBaoLoi.Text = string.Empty;
            OldImageURL = string.Empty;
           
        }

        protected void grvSanPham_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this.grvSanPham, "Select$" + e.Row.RowIndex));
                e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                Label lbl = e.Row.FindControl("trangthai") as Label;
                if (lbl.Text.Equals("1"))
                {
                    lbl.Text = "Còn hàng";

                }
                if (lbl.Text.Equals("0"))
                {
                    lbl.Text = "Hết hàng";
                }
            }
        }

        protected void grvSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblGiaban.Text = "sadaa";
            foreach (GridViewRow row in this.grvSanPham.Rows)
            {
                if (row.RowIndex == grvSanPham.SelectedIndex)
                {
                    string sql = "select * from SANPHAM where PK_iMasanpham = " + grvSanPham.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Read();
                        this.txtTensanpham.Text = data["sTensanpham"].ToString();
                        ten_sp = txtTensanpham.Text.Trim();
                        id_Sp = Int32.Parse(data["PK_iMasanpham"].ToString());
                        this.txtGiaban.Text = data["iGiaban"].ToString();
                        this.txtSoluong.Text = data["iSoluong"].ToString();
                        this.txtMota.Text = data["sMota"].ToString();
                        if (Int32.Parse(data["iTrangthai"].ToString()) == 1)
                        {
                            this.ddlTrangThai.SelectedIndex = 0;
                        }
                        else
                            this.ddlTrangThai.SelectedIndex = 1;
                        
                    }
                    cnn.Close();
                    
                    chitiet.Visible = false;
                    binhthuong.Visible = true;
                    lblThongBaoLoi.Text = string.Empty;
                }
            }
        }
        protected bool KiemTraThem(string tensanpham)
        {
            string sql = "select * from SANPHAM where sTensanpham = N'" + tensanpham + "'";
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                cnn.Close();
                if (!ten_sp.Equals(tensanpham))
                {
                    lblThongBaoLoi.Text = "Đã có tên sản phẩm này!!";
                    return false;
                }
                else
                    return true;

            }
            else
            {
                cnn.Close();
                return true;
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            if (grvSanPham.SelectedDataKey != null && id_Sp != 0)
            {
                string tensanpham = txtTensanpham.Text.Trim();
                if (KiemTraThem(tensanpham))
                {
                    string mota = txtMota.Text.Trim();
                    int soluong, giaban;
                    soluong = Int32.Parse(txtSoluong.Text.Trim().ToString());
                    giaban = Int32.Parse(txtGiaban.Text.Trim().ToString());
                    string sql = "update SANPHAM set sTensanpham = N'" + tensanpham + "',sMota = N'" + mota + "',iGiaban =" + giaban + ",iSoluong = '" + soluong + "',iTrangthai = "+ ddlTrangThai.SelectedValue+ "  where PK_iMasanpham = " + grvSanPham.SelectedDataKey.Value.ToString();
                    SqlConnection cnn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    btnTimkiem_Click(btnTimkiem, e);
                    lblThongBaoLoi.Text = "Cập nhật thành công";
                }
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn sản phẩm";
        }

        protected void repeaterPaging_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // item click event
            int nextPage = Convert.ToInt32(e.CommandArgument);
           // string a = DropDownList1.SelectedItem.Value;
            Response.Redirect("CapNhatSanPham.aspx?page=" + nextPage );
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

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            string search = txtTimkiem.Text.Trim();
            if (search == "")
                KhoiTaoDuLieu();
            else
            {
                string sql = "select * from SANPHAM WHERE sTensanpham LIKE N'%" + search + "%'";
                SqlConnection Cnn = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grvSanPham.DataSource = dt;
                grvSanPham.DataBind();
                txtTensanpham.Text = string.Empty;
                txtGiaban.Text = string.Empty;
                txtSoluong.Text = string.Empty;
                txtMota.Text = string.Empty;
                repeaterPaging.Visible = false;
                current_page = 1;
                id_Sp = 0;
                ten_sp = string.Empty;
                OldImageURL = string.Empty;
                chitiet.Visible = false;
                binhthuong.Visible = true;
              //  lblThongBaoLoi.Text = string.Empty;
            }
        }

        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            grvSanPham.PageIndex = 0;
            txtTimkiem.Text = string.Empty;
            lblThongBaoLoi.Text = string.Empty;
            Session["SortedView"] = null;
            KhoiTaoDuLieu();
            repeaterPaging.Visible = true;
            
            
        }

        protected void lkbtnChiTiet_Click(object sender, EventArgs e)
        {
            
            string sql = "select * from THUONGHIEU ";
            SqlConnection cnn = new SqlConnection(constr);
            #region LayThuongHieu
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ddlThuongHieu.DataSource = dt;
            ddlThuongHieu.DataTextField = "sTenthuonghieu";
            ddlThuongHieu.DataValueField = "PK_iMathuonghieu";
            ddlThuongHieu.DataBind();
            #endregion
            
            // sql lúc này = tên procedure
            sql = "LaythongtinSP";
            LinkButton lkbtn = (LinkButton)sender;
            int id = Int32.Parse(lkbtn.CommandArgument);
            grvSanPham.SelectedIndex = Int32.Parse(lkbtn.CommandName);
            grvSanPham_SelectedIndexChanged(grvSanPham,e);
            id_Sp = id;
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
           
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                Image1.ImageUrl = data["sNguonhinhanh"].ToString();
                OldImageURL = Image1.ImageUrl.Substring(9);
                txtRAM.Text = data["sRAM"].ToString();
                txtBonho.Text = data["sBonho"].ToString();
                txtManhinh.Text = data["sManhinh"].ToString();
                txtDungluongpin.Text = data["sDungluongpin"].ToString();
                txtMausac.Text = data["sMausac"].ToString();
                txtGhichu.Text = data["sGhichu"].ToString();
                foreach (ListItem item in ddlThuongHieu.Items)
                {
                    if (item.Value.Equals(data["FK_iMathuonghieu"].ToString()))
                    {
                        item.Selected = true;
                        break;
                    }
                }
                
            }
            cnn.Close();
            binhthuong.Visible = false;
            chitiet.Visible = true;
        }

        protected bool kiemtradinhdang(string dinhdanganh)
        {
            if (dinhdanganh.ToLower().Equals(".png") || dinhdanganh.ToLower().Equals(".jpg") || dinhdanganh.ToLower().Equals(".jpeg"))
                return true;
            else
            {
                
                lblThongBaoLoi.Text = "Định dạng ảnh không phù hợp!!";
                return false;
            }
        }

        protected void btnLuuChiTiet_Click(object sender, EventArgs e)
        {
            if (id_Sp != 0)
            {
                
                string ram, bonho, manhinh, dungluongpin, mausac, ghichu, thuonghieu, imageurl;
                ram = txtRAM.Text.Trim();
                bonho = txtBonho.Text.Trim();
                manhinh = txtManhinh.Text.Trim();
                dungluongpin = txtDungluongpin.Text.Trim();
                ghichu = txtGhichu.Text.Trim();
                mausac = txtMausac.Text.Trim();
                thuonghieu = ddlThuongHieu.SelectedValue;
                SqlConnection cnn = new SqlConnection(constr);
                // Ten Procedure
                string sql = "CapNhatChiTietSP";

                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id_Sp);
                cmd.Parameters.AddWithValue("@idthuonghieu", thuonghieu);
                cmd.Parameters.AddWithValue("@ram", ram);
                cmd.Parameters.AddWithValue("@bonho", bonho);
                cmd.Parameters.AddWithValue("@manhinh", manhinh);
                cmd.Parameters.AddWithValue("@dungluongpin", dungluongpin);
                cmd.Parameters.AddWithValue("@mausac", mausac);
                cmd.Parameters.AddWithValue("@ghichu", ghichu);
                if (fileImport.HasFile && fileImport.FileContent.Length > 0 && kiemtradinhdang(Path.GetExtension(fileImport.FileName)))
                {
                   /*FileInfo file = new FileInfo(Server.MapPath("~/Images/") + OldImageURL);
                   if (file.Exists)
                   {
                       file.Delete();
                   } */
                   string filename = Path.GetFileNameWithoutExtension(fileImport.FileName);
                   filename = filename + DateTime.Now.ToBinary().ToString();
                   filename = filename + Path.GetExtension(fileImport.FileName);
                   string sFilePath = Server.MapPath("~/Images/") + filename;
                   fileImport.SaveAs(sFilePath);
                   cmd.Parameters.AddWithValue("@nguonhinhanh", "~/images/" + filename);
                   
                }
                else
                {
                    cmd.Parameters.AddWithValue("@nguonhinhanh", "");
                }
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                id_Sp = 0;
                chitiet.Visible = false;
                binhthuong.Visible = true;
                OldImageURL = string.Empty;
                lblThongBaoLoi.Text = "Cập nhật thành công";
                
            }
            

        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            if (grvSanPham.SelectedDataKey != null && id_Sp != 0)
            {
                string id = grvSanPham.SelectedDataKey.Value.ToString();
                int check = 0;
                SqlConnection cnn = new SqlConnection(constr);
                // Ten Procedure
                string sql = "checkDelSP";

                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaSP", id);

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
                    lblThongBaoLoi.Text = "Sản phẩm này có hóa đơn và phiếu nhập, không xóa được!";
                }
                else
                {
                   /* FileInfo file = new FileInfo(Server.MapPath("~/Images/") + OldImageURL);
                    if (file.Exists)
                    {
                        file.Delete();*/
                        string sql2 = "delete from SANPHAM where PK_iMasanpham = " + id;

                        SqlCommand cmd2 = new SqlCommand(sql2, cnn);
                        cmd2.CommandType = CommandType.Text;
                        cnn.Open();
                        cmd2.ExecuteNonQuery();
                        cnn.Close();

                        btnTimkiem_Click(btnTimkiem, e);
                        lblThongBaoLoi.Text = "Xóa thành công";
                   /* }
                    else
                    {
                        lblThongBaoLoi.Text = "Hãy nhấn vào chi tiết sản phẩm rồi xóa";
                    }*/
                }
               
            }
            else
                lblThongBaoLoi.Text = "Chưa chọn sản phẩm";
        }
        protected void quaylaibtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void grvSanPham_Sorting(object sender, GridViewSortEventArgs e)
        {
            KhoiTaoDuLieu(e.SortExpression);
        }

        protected void grvSanPham_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSanPham.PageIndex = e.NewPageIndex;
            if (Session["SortedView"] != null)
            {
                grvSanPham.DataSource = Session["SortedView"];
                grvSanPham.DataBind();
                lblThongBaoLoi.Text = string.Empty;
            }
            else
                KhoiTaoDuLieu();
        }

        protected void btnThemChiTiet_Click(object sender, EventArgs e)
        {
            if (id_Sp != 0)
            {

                string ram, bonho, manhinh, dungluongpin, mausac, ghichu, thuonghieu, imageurl;
                bonho = txtBonho.Text.Trim();
                mausac = txtMausac.Text.Trim();
                SqlConnection cnn = new SqlConnection(constr);
                // Ten Procedure
                string sql = "ThemChiTietSP";

                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id_Sp);
                cmd.Parameters.AddWithValue("@bonho", bonho);
                cmd.Parameters.AddWithValue("@mausac", mausac);
                if (fileImport.HasFile && fileImport.FileContent.Length > 0 && kiemtradinhdang(Path.GetExtension(fileImport.FileName)))
                {
                    /*FileInfo file = new FileInfo(Server.MapPath("~/Images/") + OldImageURL);
                    if (file.Exists)
                    {
                        file.Delete();
                    } */
                    string filename = Path.GetFileNameWithoutExtension(fileImport.FileName);
                    filename = filename + DateTime.Now.ToBinary().ToString();
                    filename = filename + Path.GetExtension(fileImport.FileName);
                    lblThongBaoLoi.Text = filename;
                    string sFilePath = Server.MapPath("~/Images/") + filename;
                    fileImport.SaveAs(sFilePath);
                    cmd.Parameters.AddWithValue("@nguonhinhanh", "~/images/" + filename);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@nguonhinhanh", "");
                }
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                id_Sp = 0;
                chitiet.Visible = false;
                binhthuong.Visible = true;
                OldImageURL = string.Empty;
                lblThongBaoLoi.Text = "Thêm thành công";

            }
        }

    /*    protected void grvSanPham_Sorting(object sender, GridViewSortEventArgs e)
        {
            KhoiTaoDuLieu(current_page, e.SortExpression);
        }*/

        


    }
}