<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ThanhToan.aspx.cs" Inherits="TestUserSQL.WebForm15" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <link href="css/ThanhToan_CSS.css" rel="stylesheet" type="text/css" />
    <style>
        #divall {
            text-align:center;
        }
        #form1 {
            min-height:500px;
            background-color:white;
            
            
        }
        .ItemIMG {
            width:100px;
            min-height:100px;
            max-height:150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divall">
    <form id="form1" runat="server">
        <div id="snackbar">Thanh toán thất bại</div>
        <div id="snackbar2">Đặt hàng thành công</div>
        <div style="text-align:center;">
        <h3 runat="server" id="title"></h3>
        <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
        <br />
        <asp:Label runat="server" ID="lblTenkhachhang" ></asp:Label>
        <br />
        <asp:Label runat="server" ID="lblTennguoinhan" ></asp:Label>
        <asp:Label runat="server" ID="lblSDT"></asp:Label>
        <br />
        <asp:Label runat="server" ID="lblDiachi"></asp:Label>
        <br />
        <asp:Label runat="server" ID="lblTongtien"></asp:Label>
        <div class="grid_div" >
        <asp:GridView ID="grvChiTiet"   CssClass="gridview"  runat="server"  DataKeyNames="PK_iCT_HoaDonID"  AutoGenerateColumns="false"
                                OnRowDataBound="grvChiTiet_RowDataBound" >
                            <Columns>
                            <asp:TemplateField HeaderText="Sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30%" >
                               <ItemTemplate>
                                     <asp:Image ID="Image1" CssClass="ItemIMG"  runat="server" ImageUrl='<%# Eval("sNguonhinhanh") %>' />
                                </ItemTemplate> 
                               
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Tên sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" >
                               <ItemTemplate>
                                    <%#Eval("sTensanpham") %>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Số lượng" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:label id="lblSoluong" runat="server" Text=' <%#Eval("iSoluong") %> '></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Đơn giá" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:label id="lbltDonGia" runat="server" Text=' <%#Eval("iDonGia") %> '></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Thành tiền" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:label id="lblThanhtien"  runat="server"></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                            
                            </Columns>
           </asp:GridView>
            </div>
            <div class="hinhthucthanhtoan" >
            <h3>Hình thức thanh toán</h3>
            <table border="1" class="gridview" >
                <tr>
                    <th style="width:50%;">Thanh toán sau</th>
                    <th style="width:50%;">Thanh toán qua MoMo</th>
                </tr>
                <tr>
                    <td><asp:Button ID="backbtn" Text="Quay lại" style="width:70%;" runat="server" OnClick="backbtn_Click" /></td>
                    <td> <asp:LinkButton ID="thanhtoanlinkbtn" runat="server" OnClick="thanhtoanlinkbtn_Click">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/MoMo_Logo.png" Height="100"  />
                    </asp:LinkButton>

                    </td>
                </tr>
            </table>
            </div>
           
           </div>

    </form>
    </div>

</asp:Content>