<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CapNhatKhuyenMai.aspx.cs" Inherits="TestUserSQL.WebForm14" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        #form1 {
            min-height:450px;
        }
        select {
            width:100%;
        }
    </style>
    <script type="text/javascript">
        function Check() {
            var sanpham = document.getElementById('ContentPlaceHolder1_ddlSanpham').value;
            if (sanpham == "0") {
                alert("Chưa chọn sản phẩm");
                return false;
            }
            var tile = document.getElementById('ContentPlaceHolder1_txtTileKhuyenMai').value;
            if (isNaN(tile.trim()) || tile.trim() <= 0 || tile.trim() > 100) {
                alert("tỉ lệ khuyến mại phải trong khoảng");
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
                <td style="width:30%;vertical-align:top" class="classtd">
                    <table class="input_table">
                        <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                        <tr>
                            <td>Tên sản phẩm</td>
                            <td><asp:DropDownList runat="server" ID="ddlSanpham"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Tỉ lệ khuyến mại (từ 0 đến 100)</td>
                            <td><asp:TextBox ID="txtTileKhuyenMai" runat="server" TextMode="Number"  min="0"  step="1" max="100" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Ngày bắt đầu</td>
                            <td><asp:TextBox ID="txtTuNgay" runat="server" Type="Date" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Ngày kết thúc</td>
                            <td><asp:TextBox Type="Date" ID="txtDenNgay" runat="server" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Trạng thái</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTrangThai" Enabled="false">
                                    <asp:ListItem Value="1" Text="Còn hạn"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Hết hạn"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Ghi chú</td>
                            <td><asp:TextBox  ID="txtGhichu" runat="server" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:center;">
                                <asp:Button ID="btnThem" runat="server" OnClick="btnThem_Click" Text="Thêm mới" OnClientClick="return Check();" />
                                <asp:Button ID="btnSua" runat="server" OnClick="btnSua_Click" Text="Cập nhật" OnClientClick="return Check();"  />
                                <asp:Button ID="btnXoa" runat="server" OnClick="btnXoa_Click" Text="Xóa" OnClientClick="return confirm('Bạn có muốn xóa khuyến mãi này?!');"  />
                                <asp:Button ID="btnRefesh" runat="server"  Text="Làm mới" OnClick="btnRefesh_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:70%;vertical-align:top" class="classtd">
                    <div style="overflow-x:auto;">
                    <asp:gridview CssClass="gridview" runat="server" ID="grvKhuyenMai" DataKeyNames="PK_iMakhuyenmai" AutoGenerateColumns="false"
                      ShowHeaderWhenEmpty="true"  OnRowDataBound="grvKhuyenMai_RowDataBound" OnSelectedIndexChanged="grvKhuyenMai_SelectedIndexChanged"
                        AllowPaging="true" PageSize="10" OnPageIndexChanging="grvKhuyenMai_PageIndexChanging" >
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Tên sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >
                            <ItemTemplate>
                                <%#Eval("sTensanpham") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá ban đầu" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <%#Eval("iGiaban") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tỉ lệ khuyến mại" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <%#Eval("iTilekhuyenmai") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày bắt đầu" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >
                            <ItemTemplate>
                                <%#Eval("dNgaybatdau") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày kết thúc" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >
                            <ItemTemplate>
                                <%#Eval("dNgayketthuc") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trạng thái" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >
                            <ItemTemplate>
                                <asp:label id="trangthai" runat="server" Text=' <%#Eval("iTrangthai") %> '></asp:label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ghi chú" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%#Eval("sGhichu") %>
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
