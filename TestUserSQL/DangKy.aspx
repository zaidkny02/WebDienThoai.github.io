<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DangKy.aspx.cs" Inherits="TestUserSQL.DangKy" 
    MasterPageFile="~/MasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <title></title>
    <style>
        
        
        
        table tr {
            margin-top:10px;
        }
        #content {
            
            text-align:center;
            min-height:450px;
            width:100%;
            display:inline-block;
             
        }
        form {
        
          
        
    }
        input {
            border-width:thin;
        }
    </style>
    <script type="text/javascript">
        function re() {
            document.getElementById('ContentPlaceHolder1_txtTaiKhoan').value = "";
            document.getElementById('ContentPlaceHolder1_txtPass').value = "";
        }
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content">
    <form id="form1" runat="server" style="display: inline-block; background-color:white; border: 2px solid; padding:10px;">
        <img src="Images/logo.png" />
        <h3 style="text-align:center;">Đăng ký tài khoản</h3>
        <asp:Label ID="lblthongbao" style="color:red;" runat="server" Text="" ></asp:Label>
        <table >
            <tr>
                    <td style="float:left;">Tên tài khoản:</td>
                    <td><input  style="float:right;"  type="text" runat="server" required="required" id="txtTaiKhoan"/></td>
               </tr>
               <tr>
                    <td style="float:left;">Mật khẩu:</td>
                    <td><input style="float:right;"  type="password" runat="server" required="required" id="txtPass"/></td>             
               </tr>
               <tr>
			        <td colspan="2" align="center">
				        
                        <asp:Button  id="btn" runat="server" Text="Đăng ký" OnClick="dangkytaikhoan_Click" />
                         
                        <input type="button" id="reset" name="reset"  value="Hủy" onclick="re();"  />
			        </td>
		       </tr>
               <tr>
                    <td colspan="2" align="right">
				        <p>Đã có tài khoản? <a href="DangNhap.aspx">Đăng nhập</a></p>
			        </td>
               </tr>
        </table>
    
    </form>
    </div>
</asp:Content>
