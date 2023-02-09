<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ThayDoiTT_TaiKhoan.aspx.cs" 
    Inherits="TestUserSQL.WebForm3"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        #form1 {
            
            
             text-align:left;
        }
        #content {
             width:100%;
             display:inline-block;
             text-align:center;
        }
    </style>
    <script type="text/javascript">
        function changetaikhoan_check() {
            var name = document.getElementById('txtName').value;
            var cmt = document.getElementById('txtCMT').value;
            console.log("sda");
            if (name.trim() == "") {
                alert("Họ tên không được trống");
                return false;
            }
       /*     if (cmt.trim() != "") {
                for(var i = 0;i< cmt.length; i ++){
                    if (cmt.charAt(i) > 31 && (cmt.charAt(i) < 48 || cmt.charAt(i) > 57)) {
                        alert("Căn cước công dân phải nhập ký tự số");
                        return false;
                    }
                }
            }*/

            return true;
        }
        function kiemtra() {
            var name = document.getElementById('ContentPlaceHolder1_txtName').value;
            var cmt = document.getElementById('ContentPlaceHolder1_txtCMT').value;
            console.log("sda");
            if (name.trim() == "") {
                alert("Họ tên không được trống");
                return false;
            }
                 if (cmt.trim() != "") {
                     for(var i = 0;i< cmt.length; i ++){
                         if (cmt.charCodeAt(i) > 31 && (cmt.charCodeAt(i) < 48 || cmt.charCodeAt(i) > 57)) {
                             alert("Căn cước công dân phải nhập ký tự số");
                             return false;
                         }
                     }
                 }

            return true;


        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content">
    <form id="form1" runat="server" style="display: inline-block; background-color:white;  border: 2px solid; padding:10px;">
        <div style="text-align:center;"><img src="Images/logo.png" /></div>
        <h3 style="text-align:center;">Thay đổi thông tin tài khoản</h3>
        <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
        <table>

            <tr>
                <td><p>Họ và tên</p></td>
                <td><asp:TextBox ID="txtName" runat="server" ></asp:TextBox></td>
            </tr>
            <tr>
                <td><p>Số điện thoại</p></td>
                <td><asp:TextBox ID="txtSDT" runat="server" ></asp:TextBox></td>
            </tr>
            <tr>
                <td><p>Ngày sinh</p></td>
                <td><asp:TextBox   Type="Date" ID="txtNgaysinh" runat="server" ></asp:TextBox></td>
            </tr>
            <tr>
                <td><p>Số căn cước</p></td>
                <td><asp:TextBox ID="txtCMT" runat="server" ></asp:TextBox></td>
                
            </tr>
            <tr>
                <td><p>Địa chỉ</p></td>
                <td><asp:TextBox ID="txtDiachi" runat="server" ></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center"><asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClientClick="return kiemtra();" OnClick="btnUpdate_Click"  /></td>
            </tr>
        </table>
    </form>
    </div>
</asp:Content>
