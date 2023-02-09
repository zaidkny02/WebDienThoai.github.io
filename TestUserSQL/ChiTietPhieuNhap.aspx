<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false"  AutoEventWireup="true" CodeBehind="ChiTietPhieuNhap.aspx.cs" Inherits="TestUserSQL.WebForm8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        #form1 {
            min-height:450px;
        }
    </style>
    <script type="text/javascript">
        function Check() {
            var sanpham = document.getElementById('ContentPlaceHolder1_ddlSanpham').value;
            if (sanpham == "0") {
                alert("Chưa chọn sản phẩm");
                return false;
            }
            var soluong = document.getElementById('ContentPlaceHolder1_txtSoluong').value;
            var dongia = document.getElementById('ContentPlaceHolder1_txtDongia').value;
            if (isNaN(dongia.trim()) || dongia.trim() <= 0) {
                alert("Giá nhập phải là số nguyên lớn hơn 0");
                return false;
            }
            if (isNaN(soluong.trim()) || soluong.trim() <= 0) {
                alert("Số lượng phải là số nguyên lớn hơn 0");
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
            <div><asp:Button ID="quaylaibtn" runat="server" Text="Quay về danh sách phiếu nhập" OnClick="quaylaibtn_Click" /></div>
            <table style="width:100%" class="input_grid_table">
                <tr>
                    <td style="width:35%;  vertical-align:top;" class="classtd">
                        <table cellpadding="0" cellspacing="1" style="width:100%" class="input_table">
                        <tr>
                            <td colspan="2" style="padding-bottom: 8px;">
                                <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                            </td>
                        </tr>
                        <tr>
                            <td>Mã phiếu nhập</td>
                            <td>
                                <asp:TextBox  runat="server" ID="txtMaPhieu" Enabled="false" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:Label ID="lblTensanpham" runat="server" Text="Label">Tên sản phẩm</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSanpham" >
                                
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDongia" runat="server" Text="Label">Giá nhập</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox  runat="server" ID="txtDongia" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSoluong" runat="server" Text="Label">Số lượng</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox  runat="server" ID="txtSoluong" TextMode="Number"  min="1"  step="1" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button runat="server" ID="addBtn" Text="Thêm mới" OnClick="addBtn_Click"  OnClientClick="return Check();"/>
                                <asp:Button runat="server" ID="saveBtn" Text="Lưu" OnClick="saveBtn_Click"  OnClientClick="return Check();"/>
                                <asp:Button runat="server" ID="delBtn" Text="Xóa" OnClick="delBtn_Click"  OnClientClick="return confirm('Bạn có thật sự muốn xóa dòng phiếu nhập này?');"/>
                                <asp:Button runat="server" ID="khoaphieuBtn" Text="Khóa phiếu"  OnClientClick="return confirm('Xác nhận khóa phiếu nhập này?');" OnClick="khoaphieuBtn_Click" />
                                <asp:Button runat="server" ID="newBtn" Text="Làm mới"  OnClick="newBtn_Click" />
                            </td>
                        </tr>
                        </table>
                    </td>
                    <td style="width:65%;  vertical-align:top; " class="classtd" >
                        <div style="overflow-x:auto;">
                        <asp:gridview runat="server" ID="grvChiTietNhap"  CssClass="gridview" DataKeyNames="PK_iCT_PhieuNhapID" AutoGenerateColumns="false"
                      OnRowDataBound="grvChiTietNhap_RowDataBound" OnSelectedIndexChanged="grvChiTietNhap_SelectedIndexChanged"
                         ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10" OnPageIndexChanging="grvChiTietNhap_PageIndexChanging">
                    
                        <Columns>
                            <asp:TemplateField HeaderText="Tên sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >
                                <ItemTemplate>
                                    <%#Eval("sTensanpham") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Giá nhập" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                                <ItemTemplate>
                                    <asp:Label ID="lbldongia" runat="server" Text='<%#Eval("iDongia") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Số lượng" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                                <ItemTemplate>
                                    <asp:Label ID="lblsoluong" runat="server" Text='<%#Eval("iSoluong") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Thành tiền" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                                <ItemTemplate>
                                    <asp:Label ID="lblThanhtien" runat="server" ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                            
                        </asp:gridview>
                        </div>
                        <div style="text-align:center; background-color:white;">
                            <asp:Label runat="server" ID="lblTongtien" ></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
