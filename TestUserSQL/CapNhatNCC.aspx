<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CapNhatNCC.aspx.cs" Inherits="TestUserSQL.WebForm12" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <link rel="stylesheet" href="css/tablecss.css" />
    <style>
        #form1 {
            min-height:450px;
        }
    </style>
    <script type="text/javascript">
        function Check() {
            var tenncc = document.getElementById('ContentPlaceHolder1_txtTenNCC').value;
            var sdt = document.getElementById('ContentPlaceHolder1_txtSDT').value;
            var diachi = document.getElementById('ContentPlaceHolder1_txtDiachi').value;
            if (tenncc.trim() == "") {
                alert("Tên nhà cung cấp không được rỗng");
                return false;
            }
            if (sdt.trim() == "") {
                alert("Số điện thoại nhà cung cấp không được rỗng");
                return false;
            }
            if (diachi.trim() == "") {
                alert("Địa chỉ nhà cung cấp không được rỗng");
                return false;
            }

            return true;
        }
        function TimkiemCheck() {
            var search = document.getElementById('ContentPlaceHolder1_txtTimKiem').value;
            if (search.trim() == "")
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
            <asp:TextBox  ID="txtTimKiem" runat="server" placeholder="Nhập tên nhà cung cấp"  ></asp:TextBox>
            <asp:Button ID="btnTimKiem" runat="server" Text="Tìm kiếm" OnClick="btnTimKiem_Click" OnClientClick="return TimkiemCheck();" />
            <asp:Button ID="btnrefesh" runat="server" Text="Tạo lại dữ liệu" OnClick="btnrefesh_Click" />
        </div>
        <div style="overflow-x:auto;">
        <table class="input_grid_table"  style="width:100%;">
            <tr>
                <td  class="classtd" style="width:40%;vertical-align:top">
                    
                    <table class="input_table">
                        <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                        <tr>
                            <td>Tên nhà cung cấp</td>
                            <td><asp:TextBox ID="txtTenNCC" runat="server" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Số điện thoại</td>
                            <td><asp:TextBox ID="txtSDT" runat="server" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Địa chỉ</td>
                            <td><asp:TextBox ID="txtDiachi" runat="server" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Email</td>
                            <td><asp:TextBox ID="txtEmail" runat="server" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:center;">
                                <asp:Button ID="btnThem" runat="server" Text="Thêm mới" OnClientClick="return Check();" OnClick="btnThem_Click" />
                                <asp:Button ID="btnSua" runat="server" Text="Sửa" OnClientClick="return Check();" OnClick="btnSua_Click" />
                                <asp:Button ID="btnXoa" runat="server" Text="Xóa" OnClientClick="return confirm('Bạn có muốn xóa nhà cung cấp này?!');" OnClick="btnXoa_Click" />
                            </td>
                        </tr>
                    </table>
                    
                </td>
                <td  class="classtd" style="width:60%;vertical-align:top">
                    <div style="overflow-x:auto;">
                    <asp:gridview runat="server" style="width:100%;"  CssClass="gridview" ID="grvNCC" DataKeyNames="PK_iMaNCC" AutoGenerateColumns="false"
                      ShowHeaderWhenEmpty="true"  OnRowDataBound="grvNCC_RowDataBound" OnSelectedIndexChanged="grvNCC_SelectedIndexChanged"
                        AllowPaging="true" PageSize="10"  OnPageIndexChanging="grvNCC_PageIndexChanging">
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Tên nhà cung cấp" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30%" >
                            <ItemTemplate>
                                <%#Eval("sTenNCC") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SĐT" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%#Eval("sSDT") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%" >
                            <ItemTemplate>
                                <%#Eval("sDiachi") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%#Eval("sEmail") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                    
                </asp:gridview>
                </div>
                </td>
            </tr>
        </table>
        </div>
       </ContentTemplate>
       </asp:UpdatePanel>
    </form>
</asp:Content>
