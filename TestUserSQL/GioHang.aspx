<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"  AutoEventWireup="true" CodeBehind="GioHang.aspx.cs" Inherits="TestUserSQL.WebForm1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <link href="css/GioHang.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Check() {
            var sdt = document.getElementById("ContentPlaceHolder1_txtHoTen").value;
            if (sdt.trim() == "") {
                alert("Chưa nhập họ tên người nhận");
                return false;
            }
            var diachi = document.getElementById("ContentPlaceHolder1_txtDiaChi").value;
            if(diachi.trim() == ""){
                alert("Chưa nhập địa chỉ nhận hàng");
                return false;
            }
            var sdt = document.getElementById("ContentPlaceHolder1_txtSDT").value;
            if (sdt.trim() == "") {
                alert("Chưa nhập số điện thoại liên lạc");
                return false;
            }
            return true;
        }
        function thongbaoloi() {
            alert("Không tìm thấy sản phẩm trong giỏ hàng hoặc bạn chưa đăng nhập hoặc không đủ số lượng hàng");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div id="snackbar">Đặt hàng thành công</div>
        <asp:ScriptManager ID="ScriptManager1"
             runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
        
       <table style="width:100%;">
        <tr>
        <td style="vertical-align:top; width:70%;">
        <div class ="lstitem">
                <div class="giohang_title">
                    GIỎ HÀNG
                </div>
                <asp:Repeater ID="grvData" runat="server" OnItemDataBound="grvData_ItemDataBound" >                   
                        <HeaderTemplate>                                             
                         </HeaderTemplate>                  
                        <ItemTemplate>                     
                            <div class ="item">
                                
                                <asp:Image ID="Image1" CSSclass="itemIMG" runat="server" ImageUrl='<%# Eval("sNguonhinhanh") %>' />
                                
                                <table class="thongso" style="width:100%;">
                                <tr>
                                 <td>
                                <asp:Label ID="Label1" CSSclass="itemName" Text=' <%# Eval("sTensanpham") %> ' runat="server" />   
                                </td>
                                 <td class="itemThongTin">
                                     <asp:Label ID="lblthongtin"  runat="server"></asp:Label>
                                 </td>
                                </tr>
                                
                                <tr>
                                    <td colspan="2">
                                        <asp:label ID="Label3" runat="server" ></asp:label>
                                    </td>
                                </tr>
                                <tr>
                                 <td>
                                <asp:Label ID="Label2" Cssclass="itemPrice"  Text = ' <%# Eval("iGiaban") + " VND"%> ' runat="server"></asp:Label>
                                </td>
                                <td class="td_btndel">
                                    
                                    <asp:Button CssClass="btnxoasp"  ID="Button1"  CommandArgument='<%# Container.ItemIndex %>' runat="server" Text="Xóa" OnClientClick="return confirm('Bạn có xóa sản phẩm này?');" OnClick="delsp_Click"/>

                                </td>
                                </tr>
                                <tr>
                                 <td colspan="2">
                                <asp:TextBox ID="TextBox1" AutoPostBack="true"  Text='<%# Container.ItemIndex %>' OnTextChanged="TextBox1_TextChanged" TextMode="Number" runat="server" min="1" max="20" step="1"/>
                                </td>
                                </tr>
                                </table>
                                
                                
                            </div>                                                           
                        </ItemTemplate>                  
                </asp:Repeater>    
                <asp:Button ID="hiddenBtn" runat="server"  style="display:none" OnClick="hiddenBtn_Click" />
            </div>
            </td>
            <td style="vertical-align:top; width:30%; ">
            <table class="input_table">
                <tr>
                    <td colspan="2"><b><asp:Label ID="lbltongtien" runat="server" Text="Tổng tiền:"></asp:Label></b></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblHoTen"  Text="Tên người nhận" runat="server" /> </td>  
                    <td><asp:TextBox CssClass="lblthongtin" ID="txtHoTen" runat="server"></asp:TextBox> </td>  
                </tr>
                <tr>
                    <td><asp:Label ID="lblDiaChi"  Text="Địa chỉ nhận hàng" runat="server" /> </td>  
                    <td><asp:TextBox  CssClass="lblthongtin" ID="txtDiaChi" runat="server"></asp:TextBox> </td>  
                </tr>
                <tr>
                    <td><asp:Label ID="lblSDT"  Text="Số điện thoại" runat="server" /></td>  
                    <td><asp:TextBox CssClass="lblthongtin" ID="txtSDT" runat="server"></asp:TextBox></td> 
                </tr>
                <tr>
               <td colspan="2" style="text-align:center;"><asp:Button CssClass="btnDatHang"   runat="server" ID="btnDatHang" Text="Đặt hàng" OnClientClick="return Check();" OnClick="btnDatHang_Click" /></td>
                </tr>
              </table>
           </td>
        </tr>
        </table>
        </ContentTemplate>
         <Triggers>
             <asp:PostBackTrigger ControlID="hiddenBtn" />
         </Triggers>
        </asp:UpdatePanel>
     </form>
</asp:Content>
