using Newtonsoft.Json.Linq;
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
using TestUserSQL.Class;

namespace TestUserSQL
{
    public partial class WebForm15 : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string constr = ConfigurationManager.ConnectionStrings["CnnStr"].ToString();
        static int idhoadon;
        static int phuphi = 0;
        static int taikhoanid = 0;
        static int hoadon_taikhoanid = 0;
        static long tongtien = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated == false)
                    Response.Redirect("TrangItem.aspx");
                checkuser();
                bool try_Parse;
                if (Request.QueryString["id"] != null)
                    try_Parse = int.TryParse(Request.QueryString["id"].ToString(), out idhoadon);
                else
                    try_Parse = false;
                if (!try_Parse)
                    Response.Redirect("TrangItem.aspx");
                string message;
                if (Request.QueryString["message"] != null)
                    message = Request.QueryString["message"].ToString();
                else
                    message = "";
                if (message.Equals("Transaction denied by user."))
                {
                   // snackbar.InnerText = "Thanh toán thất bại";
                   // Response.Write("Thanh toán thất bại");
                    string snackbarScript;
                    var sb = new StringBuilder();
                    sb.AppendLine("var x = document.getElementById('snackbar');");
                    sb.AppendLine("x.className = 'show';");
                    sb.AppendLine("setTimeout(function(){ x.className = x.className.replace('show', ''); }, 3000);");
                    snackbarScript = sb.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "snackbar", snackbarScript, true);
                }

                title.InnerText = "Đơn hàng số " + idhoadon;
                int trangthai = KhoiTaoThongTin(idhoadon);
                if (trangthai == 0 || trangthai == 1 || trangthai == 2)
                {
                    KhoiTaoDuLieu(idhoadon);
                    if (Request.QueryString["ck"] == "success")
                    {
                        string snackbarScript;
                        var sb = new StringBuilder();
                        sb.AppendLine("var x = document.getElementById('snackbar2');");
                        sb.AppendLine("x.className = 'show';");
                        sb.AppendLine("setTimeout(function(){ x.className = x.className.replace('show', ''); }, 3000);");
                        snackbarScript = sb.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "snackbar2", snackbarScript, true);
                    }
                }
                else
                {
                    lblThongBaoLoi.Text = "Đơn hàng đã bị hủy hoặc đã được thanh toán";
                    thanhtoanlinkbtn.Enabled = false;
                }
                if(hoadon_taikhoanid != taikhoanid)
                    Response.Redirect("DanhSachMuaHang.aspx");
            }
        }

        protected int KhoiTaoThongTin(int idhoadon)
        {
            int trangthai = 0;
            string sql = "select PK_iMataikhoan,sTentaikhoan,sHovaten,HOADON.sSDT,HOADON.sDiachi,iPhuphi,iTrangthai,HOADON.sTennguoinhan";
            sql = sql + " from NGUOIDUNG,HOADON";
            sql = sql + " where FK_iMataikhoan = PK_iMataikhoan and PK_iMahoadon = " + idhoadon;
            tongtien = 0;
            SqlConnection cnn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = CommandType.Text;
            cnn.Open();
            SqlDataReader data = cmd.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                hoadon_taikhoanid = Int32.Parse(data["PK_iMataikhoan"].ToString());
                if (!data["sTennguoinhan"].ToString().Equals(""))
                    lblTennguoinhan.Text = "Tên người nhận: " + data["sTennguoinhan"].ToString() + "<br />";
                lblTenkhachhang.Text = "Tên khách hàng: "+ data["sHovaten"].ToString();
                lblSDT.Text = "Số điện thoại liên hệ: " + data["sSDT"].ToString();
                lblDiachi.Text = "Địa chỉ nhận hàng: " + data["sDiachi"].ToString();
                if (!data["iPhuphi"].ToString().Equals(""))
                    phuphi = Int32.Parse(data["iPhuphi"].ToString());
                else
                    phuphi = 0;
                tongtien = tongtien + phuphi;
                trangthai = Int32.Parse(data["iTrangthai"].ToString());
            }
            cnn.Close();

            return trangthai;
        }

        protected void KhoiTaoDuLieu(int idhoadon)
        {
            string sql = "select PK_iCT_HoaDonID,CT_HOADON.FK_iMasanpham,sTensanpham,sNguonhinhanh,CT_HOADON.iSoluong,CT_HOADON.iDonGia";
            sql = sql + " from SANPHAM,CT_HOADON,HINHANHSP where FK_iMahoadon = " + idhoadon + " and CT_HOADON.FK_iMasanpham = PK_iMasanpham";
            sql = sql + " and PK_iMasanpham = HINHANHSP.FK_iMasanpham";
            sql = sql + " and HINHANHSP.iHienthi = 1";
            SqlConnection Cnn = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter(sql, Cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            grvChiTiet.DataSource = dt;
            grvChiTiet.DataBind();


        }

        protected void grvChiTiet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "self.MouseOverOldColor=this.style.backgroundColor;this.style.backgroundColor='#C0C0C0'; this.style.cursor='pointer'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=self.MouseOverOldColor");
                Label lblsoluong = e.Row.FindControl("lblSoluong") as Label;
                Label lbldongia = e.Row.FindControl("lbltDonGia") as Label;
                Label lblthanhtien = e.Row.FindControl("lblThanhtien") as Label;
                Int64 dongia = Int64.Parse(lbldongia.Text);
                int soluong = int.Parse(lblsoluong.Text);
                long thanhtien = dongia * soluong;
                lblthanhtien.Text = thanhtien.ToString();
                tongtien = tongtien + thanhtien;
                lblTongtien.Text = "Tổng tiền: " + tongtien + " VND";
            }
        }

        protected void checkuser()
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
        }

        protected void backbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("DanhSachMuaHang.aspx");
        }

        protected void thanhtoanlinkbtn_Click(object sender, EventArgs e)
        {
            if (taikhoanid == hoadon_taikhoanid && taikhoanid != 0)
            {
                //request params need to request to MoMo system
                string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
                string partnerCode = "MOMO5RGX20191128";
                string accessKey = "M8brj9K6E22vXoDB";

                string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
                string orderInfo = "Đơn hàng số "+ idhoadon;
                // địa chỉ trở về
                string redirectUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                string ipnUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                string requestType = "captureWallet";
                // giá tiền
                string amount = tongtien.ToString();
                string orderId = Guid.NewGuid().ToString();
                string requestId = Guid.NewGuid().ToString();
                string extraData = "";

                //Before sign HMAC SHA256 signature
                string rawHash = "accessKey=" + accessKey +
                    "&amount=" + amount +
                    "&extraData=" + extraData +
                    "&ipnUrl=" + ipnUrl +
                    "&orderId=" + orderId +
                    "&orderInfo=" + orderInfo +
                    "&partnerCode=" + partnerCode +
                    "&redirectUrl=" + redirectUrl +
                    "&requestId=" + requestId +
                    "&requestType=" + requestType
                    ;

                log.Debug("rawHash = " + rawHash);

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);
                log.Debug("Signature = " + signature);

                //build body json request
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
                
                log.Debug("Json request to MoMo: " + message.ToString());
                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                JObject jmessage = JObject.Parse(responseFromMomo);
                



              //  log.Debug("Return from MoMo: " + jmessage.ToString());
            //    btnThanhToan.Text = "Return from MoMo: " + jmessage.ToString();
                //        DialogResult result = MessageBox.Show(responseFromMomo, "Open in browser", MessageBoxButtons.OKCancel);
                //       if (result == DialogResult.OK)
                //        {
                //yes...
                System.Diagnostics.Process.Start(jmessage.GetValue("payUrl").ToString());
                //      }
                //       else if (result == DialogResult.Cancel)
                //      {
                //no...
                //       }
            }
        }

    }
}