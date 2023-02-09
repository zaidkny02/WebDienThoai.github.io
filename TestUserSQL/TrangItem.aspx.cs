using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TestUserSQL
{
    public partial class TrangItem : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        int page_size = 20;
        int countItem;
        static int current_page;
        string order;
        string idnguoidung;
        private int binhluan_size = 4;  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                SqlConnection Cnn = new SqlConnection(constr);
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
                DropDownList1.SelectedIndex = 0;
                int countPage = countItem / page_size;
                if (countItem % page_size > 0 && countPage > 0)
                    countPage++;
                //  countPage = countPage >= 1 && countItem % page_size > 0 ? countPage++ : countPage;
                bool try_Parse;
                if (Request.QueryString["page"] != null)
                    try_Parse = int.TryParse(Request.QueryString["page"].ToString(), out current_page);             
                else
                    try_Parse = false;
                if (Request.QueryString["order"] != null)
                    order = Request.QueryString["order"].ToString();
                else
                    order = "";

                //set current page from 1 to final page (1<=page<=countpage)
                if (!try_Parse || current_page < 1 || countPage < 2)
                    current_page = 1;
                if (current_page > countPage && countPage > 1)
                    current_page = countPage;
                int except = (current_page - 1) * page_size < 0 ? 0 : (current_page - 1) * page_size;
                int current_item = current_page * page_size;
                //Lay danh sach Hang
                KhoiTaoHang();
                // data for item
                if (order.Equals("ASC") || order.Equals("DESC") || order.Equals("SELL_DESC"))
                {
                     switch(order)
                     {
                         case "ASC":
                             DropDownList1.SelectedIndex = 1;
                             DropDownList1_SelectedIndexChanged(DropDownList1, e);
                             break;
                         case "DESC":
                             DropDownList1.SelectedIndex = 2;
                             DropDownList1_SelectedIndexChanged(DropDownList1, e);
                             break;
                         case "SELL_DESC":
                             DropDownList1.SelectedIndex = 3;
                             DropDownList1_SelectedIndexChanged(DropDownList1, e);
                             break;
                    

                    
                    }
                }
                else
                {
                    sql = "select top " + page_size + " PK_iMasanpham,sNguonhinhanh,sTensanpham,FK_iMathuonghieu,iGiaban,iTilekhuyenmai from HINHANHSP,SANPHAM left join KHUYENMAI on SANPHAM.PK_iMasanpham = KHUYENMAI.FK_iMasanpham";
                    sql = sql + " and isnull(KHUYENMAI.dNgaybatdau,'1/1/2000') <= GETDATE() and isnull(KHUYENMAI.dNgayketthuc,'12/12/2100') >= GETDATE() ";
                    sql = sql + " WHERE PK_iMasanpham not in (select top " + except + " PK_iMasanpham From SANPHAM order by PK_iMasanpham DESC ";
                    sql = sql + " )";
                    sql = sql + " and SANPHAM.PK_iMasanpham = HINHANHSP.FK_iMasanpham";
                    sql = sql + " and HINHANHSP.iHienthi = 1";
                    sql = sql + " order by PK_iMasanpham DESC";
                    SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    grvData.DataSource = dt;
                    grvData.DataBind();
                }
                if (countPage > 1)
                    CreatePaging();
                getdanhsachbinhluannhieu();
            }
        }

        protected void getdanhsachbinhluannhieu()
        {
            SqlConnection Cnn = new SqlConnection(constr);
            string sql;
            sql = "select top 8 PK_iMasanpham,sNguonhinhanh,sTensanpham,FK_iMathuonghieu,iGiaban,iTilekhuyenmai";
            sql = sql + " from HINHANHSP,SANPHAM left join KHUYENMAI on SANPHAM.PK_iMasanpham = KHUYENMAI.FK_iMasanpham";
            sql = sql + " and isnull(KHUYENMAI.dNgaybatdau,'1/1/2000') <= GETDATE() and isnull(KHUYENMAI.dNgayketthuc,'12/12/2100') >= GETDATE() ";
            sql = sql + " left join BINHLUANSP on SANPHAM.PK_iMasanpham = BINHLUANSP.FK_iMasanpham where SANPHAM.PK_iMasanpham = HINHANHSP.FK_iMasanpham";
            sql = sql + " and HINHANHSP.iHienthi = 1";
            sql = sql + " group by PK_iMasanpham,sNguonhinhanh,sTensanpham,FK_iMathuonghieu,iGiaban,iTilekhuyenmai ";
            sql = sql + " order by count(BINHLUANSP.FK_iMasanpham) DESC";
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            PagedDataSource pdsData = new PagedDataSource();
            DataView dv = new DataView(dt);
            pdsData.DataSource = dv;
            pdsData.AllowPaging = true;
            pdsData.PageSize = binhluan_size;
            if (ViewState["PageNumber"] != null)
                pdsData.CurrentPageIndex = Convert.ToInt32(ViewState["PageNumber"]);
            else
                pdsData.CurrentPageIndex = 0;
            if (pdsData.PageCount > 1)
            {
                repeater_binhluannhieu.Visible = true;
                ArrayList alPages = new ArrayList();
                for (int i = 1; i <= pdsData.PageCount; i++)
                    alPages.Add((i).ToString());
                repeater_binhluannhieu.DataSource = alPages;
                repeater_binhluannhieu.DataBind();

                foreach (RepeaterItem i in repeater_binhluannhieu.Items)
                {
                    System.Web.UI.WebControls.LinkButton btn = i.FindControl("btnPage") as System.Web.UI.WebControls.LinkButton;

                    if (btn.Text.Equals((pdsData.CurrentPageIndex + 1).ToString()))
                    {
                        btn.Enabled = false;
                        break;
                    }
                }
            }
            else
            {
                repeater_binhluannhieu.Visible = false;
            }
            grvData_Binhluan.DataSource = pdsData;
            grvData_Binhluan.DataBind(); 
        }

        protected void KhoiTaoHang()
        {
            string sql = " select PK_iMathuonghieu,sTenthuonghieu from THUONGHIEU  ";
            SqlConnection cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ddlHang.DataSource = dt;
            ddlHang.DataTextField = "sTenthuonghieu";
            ddlHang.DataValueField = "PK_iMathuonghieu";
            ddlHang.DataBind();
            ListItem item = new ListItem("Thương hiệu", "0");
            ddlHang.Items.Insert(0, item);
        }

        protected bool checkmember()
        {
            if (User.Identity.IsAuthenticated != false)
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
                    memb.tentaikhoan = data["sTentaikhoan"].ToString();
                    memb.name = data["sHovaten"].ToString();
                    memb.idquyen = Int32.Parse(data["FK_iMaquyen"].ToString());
                    idnguoidung = memb.idnguoidung.ToString();
                    cnn.Close();
                    return true;
                }
                #endregion
            }
            return false;
        }
        protected void repeaterPaging_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // item click event
            int nextPage = Convert.ToInt32(e.CommandArgument);
            string a = DropDownList1.SelectedItem.Value;
            Response.Redirect("TrangItem.aspx?page=" + nextPage + "&order=" + a);
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

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            switch (DropDownList1.SelectedItem.Value)
            {
                
                case "0":
                    break;
                case "ASC":
                case "DESC":
                    {
                    SqlConnection Cnn = new SqlConnection(constr);
                    string sql;
                    order = DropDownList1.SelectedItem.Value.ToString();
                    int except = (current_page - 1) * page_size < 0 ? 0 : (current_page - 1) * page_size;
                    /* VD: lấy page_size (10) sản phẩm từ tbldienthoai khi ở trang thứ 2(bỏ qua 10 sản phẩm đầu) sắp xếp theo DESC
                     * với điều kiện where thêm ở cả 2 lệnh select 
                     * VD: where FK_iNhansanxuatID = 3 and PK_iDienthoaiID not in (SELECT TOP 10  PK_iDienthoaiID From tblDienthoai where FK_iNhansanxuatID = 3)
                     * VD khi có order: sql = SELECT TOP 10  * FROM tblDienthoai  
                            WHERE  PK_iDienthoaiID  not in (SELECT TOP 10  PK_iDienthoaiID From tblDienthoai ORDER BY mGiaban DESC)
                            ORDER BY mGiaban DESC   */
                    sql = "select top " + page_size + " PK_iMasanpham,sNguonhinhanh,sTensanpham,FK_iMathuonghieu,iGiaban,iTilekhuyenmai from HINHANHSP,SANPHAM left join KHUYENMAI on SANPHAM.PK_iMasanpham = KHUYENMAI.FK_iMasanpham";
                    sql = sql + " and isnull(KHUYENMAI.dNgaybatdau,'1/1/2000') <= GETDATE() and isnull(KHUYENMAI.dNgayketthuc,'12/12/2100') >= GETDATE() ";
                    sql = sql + " WHERE PK_iMasanpham not in (select top " + except + " PK_iMasanpham From SANPHAM ";
                    sql = sql + " ORDER BY iGiaban-iGiaban*isnull(iTilekhuyenmai,0)/100 " + order + ") and SANPHAM.PK_iMasanpham = HINHANHSP.FK_iMasanpham";
                    sql = sql + " and HINHANHSP.iHienthi = 1";
                    sql = sql + "    ORDER BY iGiaban-iGiaban*isnull(iTilekhuyenmai,0)/100 " + order;
                    SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    grvData.DataSource = dt;
                    grvData.DataBind();
                    break;
                    }
                case "SELL_DESC":
                    {
                    SqlConnection Cnn = new SqlConnection(constr);
                    string sql;
                    int except = (current_page - 1) * page_size < 0 ? 0 : (current_page - 1) * page_size;
                    sql = "select PK_iMasanpham,sNguonhinhanh,sTensanpham,FK_iMathuonghieu,SANPHAM.iGiaban,iTilekhuyenmai,isnull(sum(CT_HOADON.iSoluong),0)";
                    sql = sql + " from HINHANHSP,SANPHAM left join CT_HOADON on SANPHAM.PK_iMasanpham = CT_HOADON.FK_iMasanpham";
                    sql = sql + " left join KHUYENMAI on SANPHAM.PK_iMasanpham = KHUYENMAI.FK_iMasanpham";
                    sql = sql + " and isnull(KHUYENMAI.dNgaybatdau,'1/1/2000') <= GETDATE() and isnull(KHUYENMAI.dNgayketthuc,'12/12/2100') >= GETDATE()"; 
                    sql = sql + " where SANPHAM.PK_iMasanpham = HINHANHSP.FK_iMasanpham";
                    sql = sql + " and HINHANHSP.iHienthi = 1";
                    sql = sql + " group by PK_iMasanpham,sNguonhinhanh,sTensanpham,FK_iMathuonghieu,SANPHAM.iGiaban,iTilekhuyenmai";
                    sql = sql + " order by sum(CT_HOADON.iSoluong) DESC OFFSET " +except+ " ROWS FETCH FIRST "+ page_size +" ROWS ONLY";
                    SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    grvData.DataSource = dt;
                    grvData.DataBind();
                    break;
                    }
            }
          /*  if (!DropDownList1.SelectedItem.Value.Equals("0"))
            {
                order = DropDownList1.SelectedItem.Value.ToString();
                SqlConnection Cnn = new SqlConnection(constr);
                string sql;
                int except = (current_page - 1) * page_size < 0 ? 0 : (current_page - 1) * page_size;
                /* VD: lấy page_size (10) sản phẩm từ tbldienthoai khi ở trang thứ 2(bỏ qua 10 sản phẩm đầu) sắp xếp theo DESC
                 * với điều kiện where thêm ở cả 2 lệnh select 
                 * VD: where FK_iNhansanxuatID = 3 and PK_iDienthoaiID not in (SELECT TOP 10  PK_iDienthoaiID From tblDienthoai where FK_iNhansanxuatID = 3)
                 * VD khi có order: sql = SELECT TOP 10  * FROM tblDienthoai  
                        WHERE  PK_iDienthoaiID  not in (SELECT TOP 10  PK_iDienthoaiID From tblDienthoai ORDER BY mGiaban DESC)
                        ORDER BY mGiaban DESC   */
            /*    sql = "select top " + page_size + " PK_iMasanpham,sNguonhinhanh,sTensanpham,FK_iMathuonghieu,iGiaban,iTilekhuyenmai from HINHANHSP,SANPHAM left join KHUYENMAI on SANPHAM.PK_iMasanpham = KHUYENMAI.FK_iMasanpham";
                sql = sql + " and isnull(KHUYENMAI.dNgaybatdau,'1/1/2000') <= GETDATE() and isnull(KHUYENMAI.dNgayketthuc,'12/12/2100') >= GETDATE() ";
                sql = sql + " WHERE PK_iMasanpham not in (select top " + except + " PK_iMasanpham From SANPHAM ";
                sql = sql + " ORDER BY iGiaban-iGiaban*isnull(iTilekhuyenmai,0)/100 " + order + ") and SANPHAM.PK_iMasanpham = HINHANHSP.FK_iMasanpham ORDER BY iGiaban-iGiaban*isnull(iTilekhuyenmai,0)/100 " + order;
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grvData.DataSource = dt;
                grvData.DataBind();

            }*/
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
            TestUserSQL.Class.SanPham sp = new TestUserSQL.Class.SanPham();
            Button clickedButton = (Button)sender;
            sp.idsanpham = Int32.Parse(clickedButton.CommandArgument);
            sp.soluong = 1;
            sp.dongia = Int64.Parse(clickedButton.CommandName);
            int check=0;
            giohang.username = "";
            if(giohang.arrsp != null  && giohang.arrsp.Count > 0)
                foreach (TestUserSQL.Class.SanPham a in giohang.arrsp)
                {
                    if (a.idsanpham == sp.idsanpham)
                    {
                        a.soluong++;
                        check = 1;
                        break;
                    }
                }

            if (check == 0)
            {
                giohang.arrsp.Add(sp);
            }
        //    giohang.arrsp.Sort((x, y) => y.idsanpham.CompareTo(x.idsanpham));
            giohang.arrsp = giohang.arrsp.OrderBy(o=>o.idsanpham).ToList();
            string snackbarScript;
            var sb = new StringBuilder();
            sb.AppendLine("var x = document.getElementById('snackbar');");
            sb.AppendLine("x.className = 'show';");
            sb.AppendLine("setTimeout(function(){ x.className = x.className.replace('show', ''); }, 3000);");
            snackbarScript = sb.ToString();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "snackbar", snackbarScript, true);

        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (txtTimKiem.Text.Trim().Length >= 1)
            {
                Response.Redirect("Timkiem_SP.aspx?kw=" + txtTimKiem.Text.Trim());
            }
        }

        protected void grvData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Label newpricelbl = e.Item.FindControl("Label2") as Label;
                Label olpricelbl = e.Item.FindControl("oldprice") as Label;
                string price = newpricelbl.Text.Remove(newpricelbl.Text.Length - 4);
                Label giamgia = e.Item.FindControl("giamgia") as Label;
                int tilegiam = 0;
                bool try_parse = Int32.TryParse(giamgia.Text,out tilegiam);
                if (!try_parse)
                    tilegiam = 0;
                int gia = Int32.Parse(price);
               // int tilegiam = Int32.TryParse(giamgia.Text);
                int final_price = gia - gia * tilegiam / 100;
                string convertold = Convert.ToInt32(gia) > 10 ? String.Format("{0:0,0}", gia).Replace(',', '.') : Convert.ToInt32(gia).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                string convert = Convert.ToInt32(final_price) > 10 ? String.Format("{0:0,0}", final_price).Replace(',', '.') : Convert.ToInt32(final_price).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                newpricelbl.Text = convert + " VND";
                if (final_price != Int32.Parse(price))
                {
                    olpricelbl.Text = convertold + " VND";
                }
                if (tilegiam > 0)
                {
                    giamgia.Text = "-" + tilegiam + "%";
                }
            
            }
        }

        protected void ddlHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddlHang.SelectedValue;
            if (!value.Equals("0"))
            {
                switch (ddlHang.SelectedItem.Text)
                {
                    case "Apple":
                        Response.Redirect("Timkiem_SP.aspx?kw=Iphone");
                        break;
                    default:
                        Response.Redirect("Timkiem_SP.aspx?kw=" + ddlHang.SelectedItem.Text);
                        break;
                }
            }
        }

        protected void repeater_binhluannhieu_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ViewState["PageNumber"] = Convert.ToInt32(e.CommandArgument) - 1;
            getdanhsachbinhluannhieu(); 
        }

        protected void grvData_Binhluan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Label newpricelbl = e.Item.FindControl("Label2") as Label;
                Label olpricelbl = e.Item.FindControl("oldprice") as Label;
                string price = newpricelbl.Text.Remove(newpricelbl.Text.Length - 4);
                Label giamgia = e.Item.FindControl("giamgia") as Label;
                int tilegiam = 0;
                bool try_parse = Int32.TryParse(giamgia.Text, out tilegiam);
                if (!try_parse)
                    tilegiam = 0;
                int gia = Int32.Parse(price);
                // int tilegiam = Int32.TryParse(giamgia.Text);
                int final_price = gia - gia * tilegiam / 100;
                string convertold = Convert.ToInt32(gia) > 10 ? String.Format("{0:0,0}", gia).Replace(',', '.') : Convert.ToInt32(gia).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                string convert = Convert.ToInt32(final_price) > 10 ? String.Format("{0:0,0}", final_price).Replace(',', '.') : Convert.ToInt32(final_price).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                newpricelbl.Text = convert + " VND";
                if (final_price != Int32.Parse(price))
                {
                    olpricelbl.Text = convertold + " VND";
                }
                if (tilegiam > 0)
                {
                    giamgia.Text = "-" + tilegiam + "%";
                }

            }
        }


    }
}