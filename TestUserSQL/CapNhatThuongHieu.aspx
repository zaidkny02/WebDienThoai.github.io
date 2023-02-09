<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CapNhatThuongHieu.aspx.cs" Inherits="TestUserSQL.WebForm13" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        #form1 {
            min-height:450px;
        }
    </style>
    <script type="text/javascript">
        function Check() {
            var tenth = document.getElementById('ContentPlaceHolder1_txtTenthuonghieu').value;
            if (tenth.trim() == "") {
                alert("Tên thương hiệu không được rỗng");
                return false;
            }
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
            <table style="width:100%;" class="input_grid_table">
            <tr>
                <td style="width:40%;vertical-align:top" class="classtd">
                    <table class="input_table">
                        <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                        <tr>
                            <td>Mã thương hiệu</td>
                            <td><asp:TextBox ID="txtMathuonghieu" runat="server"  Enabled="false"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Tên thương hiệu</td>
                            <td><asp:TextBox ID="txtTenthuonghieu" runat="server" ></asp:TextBox></td>
                        </tr>
                        
                        <tr>
                            <td colspan="2" style="text-align:center;">
                                <asp:Button ID="btnThem" runat="server" Text="Thêm mới" OnClientClick="return Check();" OnClick="btnThem_Click" />
                                <asp:Button ID="btnSua" runat="server" Text="Sửa" OnClientClick="return Check();" OnClick="btnSua_Click" />
                                <asp:Button ID="btnXoa" runat="server" Text="Xóa" OnClientClick="return confirm('Bạn có muốn xóa thương hiệu này?!');" OnClick="btnXoa_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:60%;vertical-align:top" class="classtd">
                    <div style="overflow-x:auto;">
                    <asp:gridview runat="server"  CssClass="gridview" ID="grvThuongHieu" DataKeyNames="PK_iMathuonghieu" AutoGenerateColumns="false" OnPageIndexChanging="grvThuongHieu_PageIndexChanging"
                      ShowHeaderWhenEmpty="true"  OnRowDataBound="grvThuongHieu_RowDataBound" OnSelectedIndexChanged="grvThuongHieu_SelectedIndexChanged"
                        AllowPaging="true" PageSize="10" >
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Mã thương hiệu" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30%" >
                            <ItemTemplate>
                                <%#Eval("PK_iMathuonghieu") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên thương hiệu" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70%" >
                            <ItemTemplate>
                                <%#Eval("sTenthuonghieu") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                        
                    </Columns>
                    
                </asp:gridview>
                </div>
                </td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
