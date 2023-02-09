<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChiTietDonHang.aspx.cs" EnableEventValidation="false" Inherits="TestUserSQL.ChiTietDonHang" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <link rel="stylesheet" href="css/mastercss.css" />
    <style>
        table {
            margin:auto;
        }
        body{
            background-color:white;
        }
    </style>
    <script type="text/javascript">
        function Check() {
            var sanpham = document.getElementById('ddlSanpham').value;
            if (sanpham == "0") {
                alert("Chưa chọn sản phẩm");
                return false;
            }
            var soluong = document.getElementById('txtSoluong').value;
            var dongia = document.getElementById('txtDongia').value;
            if (isNaN(dongia.trim()) || dongia.trim() <= 0) {
                return false;
            }
            if (isNaN(soluong.trim()) || soluong.trim() <= 0) {
                alert("Số lượng phải là số nguyên lớn hơn 0");
                return false;
            }
            var conf = confirm("Bạn có muốn thay đổi thông tin đơn hàng này?");
            if (!conf)
                return false;
            return true;
        }
        function timkiemcheck() {
            var search = document.getElementById('txtTimkiem').value;
            if (search.trim() == "")
                return false;
            return true;

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1"
             runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
    <div style="text-align:center;">
        <b><asp:Label ID="lbltitle" runat="server" ></asp:Label></b>
        
        <table class="input_table">
        <div style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></div>
                                <tr>
                                    <td colspan="2" style="padding-bottom: 8px;">
                                        <span style="color:red"><asp:Literal  runat="server" ID="Literal1"></asp:Literal></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tên sản phẩm</td>
                                    <td><asp:DropDownList runat="server" ID="ddlSanpham" AutoPostBack="true" OnSelectedIndexChanged="ddlSanpham_SelectedIndexChanged" >
                                    </asp:DropDownList> </td>
                                </tr>
                                <tr>
                                    <td>Số lượng</td>
                                    <td><asp:TextBox  runat="server" ID="txtSoluong" TextMode="Number"  min="1"  step="1" ></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Đơn giá</td>
                                    <td><asp:TextBox  runat="server" ID="txtDongia" Enabled="false" ></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align:center; display:none;">
                                        <asp:Button ID="addBtn" runat="server" OnClientClick="return Check();" OnClick="addBtn_Click" Text="Thêm sản phẩm" />
                                        <asp:Button ID="saveBtn" runat="server" OnClientClick="return Check();" OnClick="saveBtn_Click" Text="Lưu" />
                                        <asp:Button ID="delBtn" runat="server" OnClick="delBtn_Click" Text="Xóa sản phẩm" OnClientClick="return confirm('Bạn có thật sự muốn xóa dòng đơn hàng này?');" />
                                    </td>
                                </tr>
                            </table>
        <asp:GridView ID="grvChiTiet" runat="server"  CssClass="gridview" DataKeyNames="PK_iCT_HoaDonID" AutoGenerateColumns="false"
                       OnRowDataBound="grvChiTiet_RowDataBound" OnSelectedIndexChanged="grvChiTiet_SelectedIndexChanged" >
                       <Columns>
                           <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Tên sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%" >
                                <ItemTemplate>
                                    <%#Eval("sTensanpham") %>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Số lượng" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:label id="lblSoluong" runat="server" Text=' <%#Eval("iSoluong") %> '></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Đơn giá" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:label id="lbltDonGia" runat="server" Text=' <%#Eval("iDonGia") %> '></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Thành tiền" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:label id="lblThanhtien" runat="server"></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField   >
                                <ItemTemplate>
                                    <asp:label style="display:none;" id="idsp_saveCheck" Text=' <%#Eval("FK_iMasanpham") %> '  runat="server"></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                       </Columns>
                   </asp:GridView>
    </div>
            <div style="text-align:right;">
            <asp:Label ID="lblphuphi" runat="server" ></asp:Label>
            </div>
                <div style="text-align:right;">
            <asp:Label ID="lblTongTien" runat="server" ></asp:Label>
                    </div>
       </ContentTemplate>
       </asp:UpdatePanel>
    </form>
</body>
</html>
