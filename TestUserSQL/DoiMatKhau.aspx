<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="DoiMatKhau.aspx.cs" Inherits="TestUserSQL.WebForm16" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        #content {
             width:100%;
             display:inline-block;
             text-align:center;
        }
        input {
            border-width:thin;
        }
        #form1 {
             
        }
    </style>
    <script type="text/javascript">
        function re() {
            document.getElementById('ContentPlaceHolder1_txtOldPass').value = "";
            document.getElementById('ContentPlaceHolder1_txtNewPass').value = "";
            document.getElementById('ContentPlaceHolder1_txtConfirm').value = "";
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content">
    <form id="form1" runat="server" style="display: inline-block; background-color:white;  border: 2px solid; padding:10px;">
        <img src="Images/logo.png" />
        <h3 style="text-align:center;">Đổi mật khẩu</h3>
        
        <asp:Label ID="lblthongbao" style="color:red;" runat="server" Text="" ></asp:Label>
        <table >
            <tr>
                    <td style="float:left;">Mật khẩu cũ:</td>
                    <td><input style="float:right;"  type="password" runat="server" required="required" id="txtOldPass"/></td>
               </tr>
               <tr>
                    <td style="float:left;">Mật khẩu mới:</td>
                    <td><input style="float:right;"  type="password" runat="server" required="required" id="txtNewPass"/></td>             
               </tr>
               <tr>
                    <td style="float:left;">Nhập lại mật khẩu mới:</td>
                    <td><input style="float:right;"  type="password" runat="server" required="required" id="txtConfirm"/></td>             
               </tr>
                <tr></tr>
               <tr>
			        <td colspan="2" align="right">
				        
                        <asp:Button id="btn" runat="server" Text="Đổi mật khẩu" OnClick="btn_Click" />
                         &nbsp;
                        <input type="button" id="reset" name="reset"  value="Hủy" onclick="re();"  />
			        </td>
		       </tr>
               
        </table>
    
    </form>
    </div>
</asp:Content>
