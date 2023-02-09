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
    public partial class WebForm1 : System.Web.UI.Page
    {
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        long uoctinh = 0;
        string idnguoidung;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                KhoiTaoDuLieu();
                
            }
        }
        protected void KhoiTaoDuLieu()
        {
            TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
            string tagnamesql = "";
            DataTable table_giohang = new DataTable();
            table_giohang.Columns.Add("PK_iMasanpham",typeof(int));
            table_giohang.Columns.Add("sTensanpham", typeof(string));
            table_giohang.Columns.Add("iGiaban", typeof(long));
            table_giohang.Columns.Add("sNguonhinhanh", typeof(string));
            foreach (TestUserSQL.Class.SanPham sp in giohang.arrsp)
            {
                tagnamesql += sp.idsanpham + ",";
                DataRow dr = table_giohang.NewRow();
                dr[0] = sp.idsanpham;
                dr[2] = sp.dongia;
                table_giohang.Rows.Add(dr);

            }
            if (tagnamesql != "") tagnamesql = tagnamesql.Remove(tagnamesql.Length - 1);
            SqlConnection Cnn = new SqlConnection(constr);
            if (tagnamesql != "")
            {
                /*string sql = "select PK_iMasanpham,sTensanpham,iGiaban-(iGiaban*isnull(iTilekhuyenmai,0)/100) as 'iGiaban',sNguonhinhanh";
                sql = sql + " from HINHANHSP,SANPHAM left join KHUYENMAI on SANPHAM.PK_iMasanpham = KHUYENMAI.FK_iMasanpham ";
                sql = sql + " and isnull(KHUYENMAI.dNgaybatdau,'1/1/2000') <= GETDATE() and isnull(KHUYENMAI.dNgayketthuc,'12/12/2100') >= GETDATE()";
                
                sql = sql + " where SANPHAM.PK_iMasanpham = HINHANHSP.FK_iMasanpham and SANPHAM.PK_iMasanpham in (" + tagnamesql + ")";
                sql = sql + " and HINHANHSP.iHienthi = 1 order by PK_iMasanpham";*/
                string sql = "select PK_iMasanpham,sTensanpham,sNguonhinhanh from HINHANHSP,SANPHAM ";
                sql = sql + " where SANPHAM.PK_iMasanpham = HINHANHSP.FK_iMasanpham and SANPHAM.PK_iMasanpham in (" + tagnamesql + ")";
                sql = sql + " and HINHANHSP.iHienthi = 1 order by PK_iMasanpham";
                SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataRow giohang_dr in table_giohang.Rows)
                    {
                        if (dr["PK_iMasanpham"].ToString().Equals(giohang_dr[0].ToString()))
                        {
                            giohang_dr[1] = dr["sTensanpham"].ToString();
                            giohang_dr[3] = dr["sNguonhinhanh"].ToString();
                        }
                        
                    }
                }
                grvData.DataSource = table_giohang;
                grvData.DataBind();
            }
            else
            {
                grvData.DataSource = null;
                grvData.DataBind();
                lbltongtien.Text = "Tổng tiền: ";
                txtDiaChi.Text = string.Empty;
                txtSDT.Text = string.Empty;
                txtHoTen.Text = string.Empty;
            }
        }

        protected void grvData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
            
         // TextBox txt = e.Item.FindControl("ContentPlaceHolder1_grvData_TextBox1_") as TextBox;
           
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {

                
                Label lbl = e.Item.FindControl("Label2") as Label;
                TextBox txt = e.Item.FindControl("TextBox1") as TextBox;
                Label lblthongtin = e.Item.FindControl("lblthongtin") as Label;
              //  string price = lbl.Text.Remove(lbl.Text.Length - 4);
                string price = giohang.arrsp[Int32.Parse(txt.Text)].dongia.ToString();
                lbl.Text = price + " VND";
                lblthongtin.Text = "(" + giohang.arrsp[Int32.Parse(txt.Text)].stringBoNho.ToString()+ "/" + giohang.arrsp[Int32.Parse(txt.Text)].stringMau.ToString() + ")";


                txt.Text = giohang.arrsp[Int32.Parse(txt.Text)].soluong.ToString();
               // int tien = Int32.Parse(price);
               // lbl.Text = Convert.ToInt32(tien) > 10 ? String.Format("{0:0,0}", tien).Replace(',', '.') : Convert.ToInt32(tien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.') ;
                uoctinh = uoctinh + (Int32.Parse(txt.Text) * Int64.Parse(price));
                string x = Convert.ToInt64(uoctinh) > 10 ? String.Format("{0:0,0}", uoctinh).Replace(',', '.') : Convert.ToInt64(uoctinh).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
                lbltongtien.Text = "Tổng tiền: " + x + " VND";
            }
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
        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            long tongtien = 0;
            TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
            
            foreach (RepeaterItem item in grvData.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    TextBox txt = item.FindControl("TextBox1") as TextBox;
                    Label lbl = item.FindControl("Label2") as Label;
                    string price = lbl.Text.Remove(lbl.Text.Length - 4);
                    
                    tongtien = tongtien + (Int32.Parse(txt.Text) * Int64.Parse(price));
                    //save giohang
                    giohang.arrsp[item.ItemIndex].soluong = Int32.Parse(txt.Text);
                }
            }
            string x = Convert.ToInt64(tongtien) > 10 ? String.Format("{0:0,0}", tongtien).Replace(',', '.') : Convert.ToInt64(tongtien).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.');
            lbltongtien.Text = "Tổng tiền: " + x + " VND";
            
            

        }

        protected void delsp_Click(object sender, EventArgs e)
        {
            TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
           
            Button btn = (Button)sender;
            giohang.arrsp.RemoveAt(Int32.Parse(btn.CommandArgument));
            KhoiTaoDuLieu();
         //   ClientScript.RegisterStartupScript(GetType(), "Do-click", "$('#hiddenBtn').click();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Do-click", "var x = document.getElementById('ContentPlaceHolder1_hiddenBtn'); x.click();", true);
            //lbltongtien.Text = soluong.ToString();
            
        }
    /*    protected bool checkinput()
        {
            if (txtDiaChi.Text.Trim() == "" || txtDiaChi.Text == null)
                return false;
            if (txtSDT.Text.Trim() == "" || txtSDT.Text == null)
                return false;
            return true;
        }*/
        protected bool checkslsp()
        {
            TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
            int ck = 0;
            SqlConnection cnn = new SqlConnection(constr);
            cnn.Open();
            foreach (TestUserSQL.Class.SanPham sp in giohang.arrsp)
            {
                //string price = "";
               // string sql = "insert into CT_HOADON values (" + hoadonid + "," + sp.idsanpham + "," + sp.soluong + "," + sp.dongia + ")";
                string sql = "select iSoluong from SANPHAM where PK_iMasanpham = "+sp.idsanpham;
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader data = cmd.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    int soluong = Int32.Parse(data["iSoluong"].ToString());
                    if (soluong < sp.soluong)
                    {
                        ck = 1;
                        break;
                    }
                }
            }
            cnn.Close();
            if (ck == 0)
                return true;
            else
                return false;
        }
        protected void btnDatHang_Click(object sender, EventArgs e)
        {
            TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
            int hoadonid = 0;
            if ( checkmember() && giohang.arrsp.Count > 0 && giohang != null )
            {
                //Tên của procedure
                string sql = "themhoadon";
                SqlConnection cnn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SDT", txtSDT.Text.Trim());
                cmd.Parameters.AddWithValue("@sDiachi", txtDiaChi.Text.Trim());
                cmd.Parameters.AddWithValue("@sTennguoinhan", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@dNgaylap", DateTime.Now);
                cmd.Parameters.AddWithValue("@FK_iMataikhoan", Int32.Parse(idnguoidung));
                cnn.Open();
                SqlDataReader data = cmd.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    hoadonid = Int32.Parse(data["ID"].ToString());
                }
                cnn.Close();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "thongbaoloi();", true);
            }
            if (hoadonid != 0)
            {
                SqlConnection cnn = new SqlConnection(constr);
                cnn.Open();
                foreach (TestUserSQL.Class.SanPham sp in giohang.arrsp)
                {
                    //string price = "";
                    string sql = "insert into CT_HOADON values (" + hoadonid + "," + sp.idsanpham + "," + sp.soluong + "," + sp.dongia + ",'" + sp.stringBoNho + "',N'" + sp.stringMau + "')";
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                cnn.Close();
                giohang.arrsp.Clear();
                GhiLogHoaDon(hoadonid.ToString());
                string snackbarScript;
                var sb = new StringBuilder();
                sb.AppendLine("var x = document.getElementById('snackbar');");
                sb.AppendLine("x.className = 'show';");
                sb.AppendLine("setTimeout(function(){ x.className = x.className.replace('show', ''); }, 3000);");
                snackbarScript = sb.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "snackbar", snackbarScript, true);
                Response.Redirect("ThanhToan.aspx?id=" + hoadonid+"&ck=success");
               /* KhoiTaoDuLieu();
              */
            }




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
            cmd.Parameters.AddWithValue("@iMataikhoancapnhat", idnguoidung);
            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        protected void hiddenBtn_Click(object sender, EventArgs e)
        {
            TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
            int soluong = giohang.arrsp.Count;
            
            MasterPage myMasterPage = Page.Master as MasterPage;
            myMasterPage.UserNamePropertyOnMasterPage = "Giỏ hàng (" + soluong + ")";
        }
    }
}