<%@ Page Language="C#"  MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestUserSQL.test" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        #logoutbtn {
            
        }
        #form1 {
            width:100%;
            
            min-height:450px;
        }
        .listBtn {
            
            
            
        }
        .modulebtn {
            width:100%;
            height:40px;
            
        }
        @media only screen and (max-width: 768px) {
            .listBtn {
                 width:100%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1"
             runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upd" runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Enabled="true"
                    Interval="1000" OnTick ="Timer1_Tick">
                </asp:Timer>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lbl" runat="server" ></asp:Label>
        <br />
        <table class="listBtn">
            <tr><td>
        <asp:Button CssClass="modulebtn" ID="phanquyenbtn" runat="server" OnClick="phanquyenbtn_Click" Text="Danh sách tài khoản"/>
            </td></tr>
            <tr><td>
        <asp:Button CssClass="modulebtn" ID="nccbtn" runat="server" OnClick="nccbtn_Click" Text="Danh sách nhà cung cấp"/>
            </td></tr>
            <tr><td>
        <asp:Button CssClass="modulebtn" ID="xacnhandonhangbtn" runat="server" OnClick="xacnhandonhangbtn_Click" Text="Danh sách đơn hàng" />
            </td>
                <td><asp:Label ID="lbldonhang" style="color:red;" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Button CssClass="modulebtn" ID="dshoadon_dathanhtoan" runat="server" OnClick="dshoadon_dathanhtoan_Click" Text="Danh sách đã thanh toán"/></td>
            </tr>
            <tr><td>
        <asp:Button CssClass="modulebtn" ID="themmoibtn" runat="server" OnClick="themmoibtn_Click" Text="Thêm sản phẩm"/>
            </td></tr>
            <tr><td>
        <asp:Button CssClass="modulebtn" ID="capnhatspbtn" runat="server" OnClick="capnhatspbtn_Click" Text="Danh sách sản phẩm"/>
            </td></tr>
            <tr><td>
        <asp:Button CssClass="modulebtn" ID="khuyenmaibtn" runat="server" OnClick="khuyenmaibtn_Click" Text="Danh sách khuyến mại"/>
            </td></tr>
            <tr><td>
        <asp:Button CssClass="modulebtn" ID="phieunhapbtn" runat="server" OnClick="phieunhapbtn_Click" Text="Danh sách phiếu nhập"/>
            </td></tr>
            <tr><td>
            <asp:Button CssClass="modulebtn" ID="thuonghieubtn" runat="server" OnClick="thuonghieubtn_Click" Text="Danh sách thương hiệu"/>
                
            </td></tr>
            <tr><td>
        <asp:Button CssClass="modulebtn" ID="baocaohoadonbtn" runat="server" OnClick="baocaohoadonbtn_Click" Text="Báo cáo hóa đơn"/>
            </td></tr>
            <tr><td>
        <asp:Button CssClass="modulebtn" ID="backupbtn" runat="server" OnClick="backupbtn_Click" Text="Sao lưu"/>
            </td></tr>
                </table>
      <!--  <asp:Button ID="logoutbtn" runat="server" OnClick="logoutbtn_Click" Text="Đăng xuất" /> -->

    
    </form>
</asp:Content>
