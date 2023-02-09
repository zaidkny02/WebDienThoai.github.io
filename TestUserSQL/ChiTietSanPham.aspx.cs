using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestUserSQL
{
    
    public partial class WebForm5 : System.Web.UI.Page
    {
        static int id_sp = 0;
        static int id_tk = 0;
        static string trangthai_donhang = "1";
        static Int64 dongia = 0;
        static Int64 dongia_final = 0;
        string stringBoNho = "";
        string stringMau = "";
        static string Mau, BoNho;
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool try_Parse;
                if (Request.QueryString["id"] != null)
                    try_Parse = int.TryParse(Request.QueryString["id"].ToString(), out id_sp);
                else
                    try_Parse = false;
                if (try_Parse && id_sp > 0)
                {
                    CheckTaiKhoan();
                    LayThongTinCoBan(id_sp);
                    LayThongSo(id_sp);
                    LayBinhLuan(id_sp);
                    
                }
               /* if (Request.QueryString["RAM"] != null)
                {
                    stringRam = Request.QueryString["RAM"].ToString();
                    stringRam = stringRam.Replace("%20", " ");
                    this.sRam.Text = stringRam;

                }*/
                LayDanhSachBonho(id_sp);
                if (stringBoNho.Equals(""))
                {
                    RepeaterItem i = grvBonho.Items[0];
                    LinkButton lkbtn = i.FindControl("chuyendoiBonho") as LinkButton;
                    lkbtn.Attributes["class"] = "choosebtn";
                    Label lbl = i.FindControl("lblBonho") as Label;
                    stringBoNho = lbl.Text.Trim();
                    BoNho = stringBoNho;
                }
                LayMauSac(id_sp, stringBoNho);
                if (grvMausac.Items.Count > 0)
                {
                    RepeaterItem itemMau = grvMausac.Items[0];
                    LinkButton lkbtnMau = itemMau.FindControl("chuyendoimau") as LinkButton;
                    Label lblMau = itemMau.FindControl("lblMau") as Label;
                    lkbtnMau.Attributes["class"] = "itemColor_choose";
                    stringMau = lblMau.Text.Trim();
                    Mau = stringMau;
                }
                //Response.Write(stringRam +"||"+ stringMau);
            }
        }
        protected void CheckTaiKhoan()
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
                    id_tk = memb.idnguoidung;
                    cnn.Close();
                    
                }
                #endregion
            }
        }

        protected void LayMauSac(int id, string ram)
        {
            string sql = "select distinct sMausac from THONGSOSP where FK_iMasanpham = " + id;
           // sql = sql + " and sRam = '"+ ram +"'";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvMausac.DataSource = dt;
            grvMausac.DataBind();
        }

        protected void LayDanhSachBonho(int id)
        {
            string sql = "select distinct sBonho from THONGSOSP where FK_iMasanpham = " + id;
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvBonho.DataSource = dt;
            grvBonho.DataBind();
        }

        protected void LayThongSo(int id)
        {
            string sql = "select * from THONGSOSP where  iTinhTrang = 1 and FK_iMasanpham = " + id;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                this.sRam.Text = data["sRAM"].ToString();
                this.sBonho.Text = data["sBonho"].ToString();
                this.sManhinh.Text = data["sManhinh"].ToString();
                this.sDungluong.Text = data["sDungluongpin"].ToString();
                sGhichu.Text = data["sGhichu"].ToString();
              /*  DataTable dt = new DataTable();
                DataColumn dc = new DataColumn("sMausac", typeof(String));
                dt.Columns.Add(dc);
                dt.Rows.Add(data["sMausac"].ToString());
                grvMausac.DataSource = dt;
                grvMausac.DataBind();*/
                //color
                //ghi chu
            }
            cnn.Close();
            
        }

        protected void LayThongTinCoBan(int id)
        {
            string sql = "select PK_iMasanpham,sTenthuonghieu,sTensanpham,SANPHAM.sMota,iGiaban,";
            sql = sql + " iGiaban-(iGiaban*isnull(iTilekhuyenmai,0)/100) as 'i_Final_price',iSoluong,SANPHAM.iTrangthai,HINHANHSP.sNguonhinhanh";
            sql = sql + " from HINHANHSP,THUONGHIEU,SANPHAM left join KHUYENMAI on SANPHAM.PK_iMasanpham = KHUYENMAI.FK_iMasanpham";
            sql = sql + " and isnull(KHUYENMAI.dNgaybatdau,'1/1/2000') <= GETDATE() and isnull(KHUYENMAI.dNgayketthuc,'12/12/2100') >= GETDATE()";
            sql = sql + " where SANPHAM.PK_iMasanpham = HINHANHSP.FK_iMasanpham and SANPHAM.FK_iMathuonghieu = THUONGHIEU.PK_iMathuonghieu";
            sql = sql + " and PK_iMasanpham = " + id;
            sql = sql + " and HINHANHSP.iHienthi = 1";
            
            
            
            
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                this.Image1.ImageUrl = data["sNguonhinhanh"].ToString();
                this.Label1.Text = data["sTensanpham"].ToString();
                this.lblThuongHieu.Text = "Thương hiệu: <b>"+ data["sTenthuonghieu"].ToString() + "</b>";
                string trangthai = data["iTrangthai"].ToString();
                trangthai_donhang = trangthai;
                if (trangthai.Equals("1"))
                    lblTrangThai.Text = "Còn hàng";
                if (trangthai.Equals("0"))
                    lblTrangThai.Text = "Hết hàng";
                int giacu = Int32.Parse(data["iGiaban"].ToString());
                dongia = Int32.Parse(data["i_Final_price"].ToString());
                dongia_final = dongia;
                string convertold = Convert.ToInt32(giacu) > 10 ? String.Format("{0:0,0}", giacu).Replace(',', '.') : Convert.ToInt32(giacu).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                string convert = Convert.ToInt32(dongia) > 10 ? String.Format("{0:0,0}", dongia).Replace(',', '.') : Convert.ToInt32(dongia).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                this.Label2.Text = "Giá bán: " + convert + " VND";
                if (dongia != giacu)
                {
                    this.oldprice.Text = convertold + " VND";
                }
                this.lblMota.Text = data["sMota"].ToString();
              //  this.Label2.Text =  Convert.ToInt32(dongia) > 10 ? String.Format("{0:0,0}", dongia).Replace(',', '.') : Convert.ToInt32(dongia).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
            }
            cnn.Close();
        }

        protected void LayBinhLuan(int id)
        {
            string sql = "select PK_iMabinhluan,FK_iMasanpham,sTentaikhoan,sHovaten,sNoidung,dNgaygio ";
            sql = sql + " from BINHLUANSP,NGUOIDUNG where BINHLUANSP.FK_iMasanpham = " + id;
            sql = sql + " and BINHLUANSP.FK_iMataikhoan = NGUOIDUNG.PK_iMataikhoan order by dNgaygio DESC ";
           // Response.Write(sql);
            SqlConnection cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                comment_empty.Visible = true;
            }
            else
                comment_empty.Visible = false;
            grvBinhluan.DataSource = dt;
            grvBinhluan.DataBind();
            
    
        }

        protected void btnbinhluan_Click(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                string binhluan = txtbinhluan.Text.Trim();
                DateTime datetime = DateTime.Now;
                //   txtbinhluan.Text = datetime.ToString("MM/dd/yyyy hh:mm:ss");
                string insertsql = "  insert into BINHLUANSP(FK_iMasanpham,FK_iMataikhoan,sNoidung,dNgaygio) values (" + id_sp + "," + id_tk + ",N'" + binhluan + "','" + datetime.ToString("MM/dd/yyyy hh:mm:ss") + "');  ";
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
                txtbinhluan.Text = string.Empty;
                LayBinhLuan(id_sp);
            }
            else
            {
               // Response.Write("<script>alert('Cần phải đăng nhập trước');</script>");
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }

        }

        protected void grvBinhluan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl = e.Item.FindControl("lblhoten") as Label;
                if (lbl.Text.Equals(""))
                {
                    lbl.Text = "Unname";
                }
            }
        }

        protected void addBtn_Click(object sender, EventArgs e)
        {
           // Response.Write(stringMau + stringRam);
            if (trangthai_donhang.Equals("1"))
            {
                TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
                TestUserSQL.Class.SanPham sp = new TestUserSQL.Class.SanPham();

                sp.idsanpham = id_sp;
                sp.soluong = 1;
                sp.dongia = dongia_final;
                sp.stringMau = Mau;
                sp.stringBoNho = BoNho;

                int check = 0;
                giohang.username = "";
                if (giohang.arrsp != null && giohang.arrsp.Count > 0)
                    foreach (TestUserSQL.Class.SanPham a in giohang.arrsp)
                    {
                        if (a.idsanpham == sp.idsanpham && a.stringMau == sp.stringMau && a.stringBoNho == sp.stringBoNho)
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
                giohang.arrsp = giohang.arrsp.OrderBy(o => o.idsanpham).ToList();
                string snackbarScript;
                var sb = new StringBuilder();
                sb.AppendLine("var x = document.getElementById('snackbar');");
                sb.AppendLine("x.className = 'show';");
                sb.AppendLine("setTimeout(function(){ x.className = x.className.replace('show', ''); }, 3000);");
                snackbarScript = sb.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "snackbar", snackbarScript, true);
                int soluong = giohang.arrsp.Count;
                MasterPage myMasterPage = Page.Master as MasterPage;
                myMasterPage.UserNamePropertyOnMasterPage = "Giỏ hàng (" + soluong + ")";
                //  giohang_text.InnerText = "Giỏ hàng (" + soluong + ")";
            }
            else
            { }
        }

        protected int LayGiaBoNho(int id, string stringBoNho)
        {
            string sql = "select iBonho_price from THONGSOSP where  sBonho = '" + stringBoNho + "' and FK_iMasanpham = " + id;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            int bonhoprice = 0;
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                bonhoprice = Int32.Parse(data["iBonho_price"].ToString());
            }
            cnn.Close();
            return bonhoprice;
        }

        protected void chuyendoibonho_Click(object sender, EventArgs e)
        {
            LinkButton lkbtn = (LinkButton)sender;
            string bonho = lkbtn.CommandArgument;
          //  Response.Redirect("ChiTietSanPham.aspx?id=" + id_sp + "&RAM=" + ram);
            stringBoNho = bonho;
            int gia = LayGiaBoNho(id_sp, stringBoNho);
            dongia_final = dongia + gia;
            BoNho = stringBoNho;
            LayDanhSachBonho(id_sp);
            this.sBonho.Text = stringBoNho;
            string convert = Convert.ToInt32(dongia_final) > 10 ? String.Format("{0:0,0}", dongia_final).Replace(',', '.') : Convert.ToInt32(dongia_final).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
            this.Label2.Text = "Giá bán: " + convert + " VND";
        }

        protected void grvBonho_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {

                LinkButton lkbtn = e.Item.FindControl("chuyendoiBonho") as LinkButton;
                if (stringBoNho.Equals("") )
                {
                    lkbtn.Attributes["class"] = "fakebtn";
                    /*
                    if (k == 0)
                    {
                        
                        
                        lkbtn.Attributes["class"] = "choosebtn";
                        k = 1;
                    }
                    else
                        lkbtn.Attributes["class"] = "fakebtn";
                    */
                    
                }
                else
                {
                    Label lbl = e.Item.FindControl("lblBonho") as Label;
                    if (lbl.Text.Equals(stringBoNho))
                    {
                        
                        
                        lkbtn.Attributes["class"] = "choosebtn";
                    }
                    else
                        lkbtn.Attributes["class"] = "fakebtn";
                }
            }
        }

        protected void chuyendoimau_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem i in grvMausac.Items)
            {
                LinkButton lbktn_i = i.FindControl("chuyendoimau") as LinkButton;
                lbktn_i.Attributes["class"] = "itemColor";
               
            }
            LinkButton lkbtn = (LinkButton)sender;
            lkbtn.Attributes["class"] = "itemColor_choose";
            stringMau = lkbtn.CommandArgument;
            Mau = stringMau;
            //Response.Write(stringMau);
        }

        protected void grvMausac_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                LinkButton lkbtn = e.Item.FindControl("chuyendoimau") as LinkButton;
                lkbtn.Attributes["class"] = "itemColor";
            }
        }



    }
}