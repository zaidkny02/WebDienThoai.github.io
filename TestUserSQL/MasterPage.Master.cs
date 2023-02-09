using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace TestUserSQL
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public string UserNamePropertyOnMasterPage
        {
            get
            {
                // Get value of control on master page  
                return giohang_text.InnerText;
            }
            set
            {
                // Set new value for control on master page  
                giohang_text.InnerText = value;
            }
        } 
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            string masterpageidmember = "";
            if (HttpContext.Current.User.Identity.IsAuthenticated != false)
            {
                #region checkmember
                Member memb = new Member();
                string sql = "select * from NGUOIDUNG where sTentaikhoan = '" + HttpContext.Current.User.Identity.Name + "'";
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
                    masterpageidmember = memb.idnguoidung.ToString();
                    cnn.Close();
                }
                #endregion
                #region createhyperlink
                hyperlinktaikhoan.Text = "Tài khoản";
                //
                HyperLink link = new HyperLink();
                link.Text = "Thông tin tài khoản";
                link.NavigateUrl = "ThayDoiTT_TaiKhoan.aspx";
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Controls.Add(link);
                taikhoanul.Controls.Add(li);
                //
                HyperLink donhang_link = new HyperLink();
                donhang_link.Text = "Lịch sử mua hàng";
                donhang_link.NavigateUrl = "DanhSachMuaHang.aspx";
                HtmlGenericControl donhang_li = new HtmlGenericControl("li");
                donhang_li.Controls.Add(donhang_link);
                taikhoanul.Controls.Add(donhang_li);

                if (memb.idquyen == 0 || memb.idquyen == 1)
                {
                    HyperLink link2 = new HyperLink();
                    link2.Text = "Trang quản trị";
                    link2.NavigateUrl = "Default.aspx";
                    HtmlGenericControl li2 = new HtmlGenericControl("li");
                    li2.Controls.Add(link2);
                    taikhoanul.Controls.Add(li2);
                }
                //
                HyperLink link4 = new HyperLink();
                link4.Text = "Đổi mật khẩu";
                link4.NavigateUrl = "DoiMatKhau.aspx";
                HtmlGenericControl li4 = new HtmlGenericControl("li");
                li4.Controls.Add(link4);
                taikhoanul.Controls.Add(li4);
                //
                HyperLink link3 = new HyperLink();
                link3.Text = "Đăng xuất";
                link3.NavigateUrl = "../DangXuat.aspx";
                HtmlGenericControl li3 = new HtmlGenericControl("li");
                li3.Controls.Add(link3);
                taikhoanul.Controls.Add(li3);
                #endregion
            }
            else
            {
                hyperlinktaikhoan.Text = "Đăng nhập";
                hyperlinktaikhoan.NavigateUrl = "DangNhap.aspx";
            }


            string sql2 = "select top 5 * from THUONGHIEU ";
            SqlConnection cnn2 = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql2, cnn2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                HyperLink link = new HyperLink();
                link.Text = dr["sTenthuonghieu"].ToString();
                if (!dr["sTenthuonghieu"].ToString().Equals("Apple"))
                    link.NavigateUrl = "TimKiem_SP.aspx?kw=" + dr["sTenthuonghieu"].ToString();
                else
                    link.NavigateUrl = "TimKiem_SP.aspx?kw=" + "Iphone";
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Controls.Add(link);
                ulhang.Controls.Add(li);
            }
            TestUserSQL.Class.GioHang giohang = (TestUserSQL.Class.GioHang)Session["GioHang"];
            int soluong = giohang.arrsp.Count;
            giohang_text.InnerText = "Giỏ hàng (" + soluong + ")";
        }

        

       

        
    }
}