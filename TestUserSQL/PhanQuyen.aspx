<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PhanQuyen.aspx.cs" Inherits="TestUserSQL.PhanQuyen"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        #form1 {
             
            min-height:450px;
        }
        @media only screen and (max-width: 768px) {
            .gridview {
                width:100%;
            }
        }
    </style>
    <script type="text/javascript">
        function Check() {
            var tentaikhoan = document.getElementById('ContentPlaceHolder1_txtTentaikhoan').value;
             if (tentaikhoan.trim() == "")
                 return false;
             //console.log(tentaikhoan.trim());
            var e = document.getElementById('ContentPlaceHolder1_ddlQuyen').value;
            if (e == "0")
                return false;
            var conf = confirm("Bạn có muốn thay đổi quyền của người dùng này?");
            if(!conf)
                return false;
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1"
             runat="server">
        </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>
        <div><asp:Button ID="quaylaibtn" runat="server" Text="Quay về trang quản trị" OnClick="quaylaibtn_Click" /></div>
        <br />
        <div>
            <asp:TextBox  ID="txtTimKiem" runat="server" placeholder="Nhập tên người dùng"  ></asp:TextBox>
            <asp:Button ID="btnTimKiem" runat="server" Text="Tìm kiếm" OnClick="txtTimKiem_TextChanged" />
            <asp:Button ID="btnrefesh" runat="server" Text="Tạo lại dữ liệu" OnClick="btnrefesh_Click" />
        </div>
        <table style="width:100%" class="input_grid_table">
        <tr>
            <td style="width:40%" class="classtd">
                <table cellpadding="0" cellspacing="1" style="width:100%" class="input_table">
                    <tr>
                        <td colspan="2" style="padding-bottom: 8px;">
                            <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Label ID="lblTentaikhoan" runat="server" Text="Label">Tên tài khoản</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox  runat="server" ID="txtTentaikhoan" Enabled="false"
                                Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblHoten" runat="server" Text="Label">Họ và tên</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox  runat="server" ID="txtHoTen" Enabled="false"
                                Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <asp:Label ID="lblQuyen" runat="server" Text="Label">Quyền</asp:Label>
                        </td>
                        <td width="55%">
                            <asp:DropDownList runat="server" ID="ddlQuyen" Width="90%">
                                <asp:ListItem Value="0" Text="Chọn"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Nhân viên"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Khách hàng"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Button runat="server" ID="saveBtn" Text="Lưu" OnClick="saveBtn_Click"
                         OnClientClick="return Check();"/></td>
                    </tr>
                </table>
            </td>
            <td style="width:60% ; " class="classtd">
                <div style="overflow-x:auto;">
                <asp:gridview runat="server"  CssClass="gridview" ID="grvTaiKhoan" DataKeyNames="PK_iMataikhoan" AutoGenerateColumns="false"
                     OnRowDataBound="grvTaiKhoan_RowDataBound" OnSelectedIndexChanged="grvTaiKhoan_SelectedIndexChanged"
                     ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10" OnPageIndexChanging="grvTaiKhoan_PageIndexChanging">
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Tên tài khoản" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%" >
                            <ItemTemplate>
                                <%#Eval("sTentaikhoan") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên người dùng" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%" >
                            <ItemTemplate>
                                <%#Eval("sHovaten") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quyền hiện tại" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >
                            <ItemTemplate>
                                <asp:label id="quyen" runat="server" Text=' <%#Eval("FK_iMaquyen") %> '></asp:label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>Không tìm thấy dữ liệu</EmptyDataTemplate>
                </asp:gridview>
                </div>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</asp:Content>
