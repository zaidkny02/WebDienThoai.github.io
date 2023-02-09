<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="DangNhap.aspx.cs" Inherits="TestUserSQL.DangNhap" 
    MasterPageFile="~/MasterPage.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        
        #Login1 td{
           
            padding:10px;
        
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
        .title {
            
    font-size: 1.17em;
    
    font-weight: bold;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content">
    <form id="form1" runat="server" style="display:inline-block; background-color:white;" >
        <img src="Images/logo.png" />
        <br />
        <asp:Label ID="lblthongbao" style="color:red;" runat="server" Text="" ></asp:Label>
        <asp:Login ID="Login1" runat="server" DisplayRememberMe="false" UserNameLabelText="Tên tài khoản"
             PasswordLabelText="Mật khẩu" TitleText="Đăng nhập" LoginButtonText="Đăng nhập" 
                OnLoggedIn="Check"
                OnLoginError="LoginErr"
                OnAuthenticate="Login1_Authenticate" FailureText="" 
             TitleTextStyle-CssClass="title"
                BorderStyle="Solid">
        
        </asp:Login>
        <div><p>Chưa có tài khoản? Đăng ký ngay <a href="DangKy.aspx">Đăng ký</a></p></div>
    
    
    </form>
    </div>
</asp:Content>
