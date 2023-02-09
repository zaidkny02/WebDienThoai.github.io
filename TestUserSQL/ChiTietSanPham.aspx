<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ChiTietSanPham.aspx.cs" Inherits="TestUserSQL.WebForm5" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <link href="css/ChiTietSP_CSS.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Check() {
            var binhluan = document.getElementById('ContentPlaceHolder1_txtbinhluan').value;
            if (binhluan.trim() == "") {
                return false;
            }
            return true;
        }
        function Showalert() {
            alert("Cần đăng nhập trước");
        }
        
    </script>
    <style>
        #form1 {
            min-height:450px;
        }
        .fakebtn {
  font-weight:400;
  border-radius:0.5rem;
  text-decoration: none;
  background-color: white;
  color: #333333;
  padding: 0.5rem;
  border-top: 1px solid #080df3;
  border-right: 1px solid #080df3;
  border-bottom: 1px solid #080df3;
  border-left: 1px solid #080df3;
        }
        .choosebtn {
            font-weight:400;
  border-radius:0.5rem;
  text-decoration: none;
  background-color: white;
  color: #333333;
  padding: 0.5rem;
  border-top: 1px solid red;
  border-right: 1px solid red;
  border-bottom: 1px solid red;
  border-left: 1px solid red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1"
             runat="server">
        </asp:ScriptManager>
        <div id="snackbar">Thêm hàng vào giỏ thành công</div>
        <div class ="item">
          
          <div class="image" >
          <asp:Image ID="Image1" CSSclass="itemIMG" runat="server" ImageUrl="#" />
          </div>
          <div class="ts">
          <h3>Thông tin sản phẩm</h3>
          <br />
          <asp:Label ID="Label1" class="itemName" Text="" runat="server" />
          <br />
          <br />
          <asp:Label ID="lblThuongHieu" class="itemThuongHieu" Text = "" runat="server"></asp:Label>
          <br />
          <br />
          <asp:Repeater ID="grvMausac" runat="server" OnItemDataBound="grvMausac_ItemDataBound" >
              <HeaderTemplate>                                             
              </HeaderTemplate>
              <ItemTemplate>
                  <asp:LinkButton  ID="chuyendoimau" CommandArgument=' <%# Eval("sMausac") %> ' runat="server" OnClick="chuyendoimau_Click"  >
                        <asp:Label  ID="lblMau"  Text=' <%# Eval("sMausac") %> '  runat="server"></asp:Label>
                  </asp:LinkButton>
              </ItemTemplate> 
          </asp:Repeater>
          <br />
          <br />
          <asp:Repeater ID="grvBonho" runat="server" OnItemDataBound="grvBonho_ItemDataBound">
              <HeaderTemplate></HeaderTemplate>
              <ItemTemplate>
                  <asp:LinkButton  ID="chuyendoiBonho" CommandArgument=' <%# Eval("sBonho") %> ' runat="server"  OnClick="chuyendoibonho_Click"  >
                     <asp:label id="lblBonho" Text=' <%# Eval("sBonho") %> ' runat="server"></asp:label>
                  </asp:LinkButton>
              </ItemTemplate>
          </asp:Repeater>
          <br />
          <br />
          <div class="div_price">
          <asp:Label ID="Label2" class="itemPrice" Text = "" runat="server"></asp:Label>
          <asp:Label ID="oldprice" CssClass="old" Text="" runat="server"></asp:Label> 
          </div> 
          <br />
          <div class="div_tuvan">
              <img src="/images/service_1.jpg" width="64px;" />
              <p class="text_tuvan">Gọi ngay 0879455495 để có giá tốt nhất</p>
          </div>
          <br />  
          <asp:Label ID="lblMota" CssClass="itemDetail" Text="" runat="server"></asp:Label>
          <br />  
          <br />
          <ul class="baohanh">
              <li class="text_li">Nguyên hộp, đầy đủ phụ kiện từ nhà sản xuất</li>
              <li class="text_li">Bảo hành 12 tháng tại trung tâm bảo hành Chính hãng. 1 đổi 1 trong 30 ngày nếu có lỗi phần cứng từ nhà sản xuất.</li>
              <li class="text_li">Thu cũ đổi mới: Giá thu cao - Thủ tục nhanh chóng - Trợ giá tốt nhất</li>
              <li><asp:Label ID="lblTrangThai" CssClass="itemDetail" Text="" runat="server"></asp:Label></li>
          </ul>
          <br />
          <div class="btnthem">
          <asp:Button ID="addBtn" OnClick="addBtn_Click"   runat="server" Text="Thêm vào giỏ"  />
          </div>  
          </div>
       </div>
       <div class ="thongso">
           <table border="1" class="gridview">
               <tr>
                   <td>RAM</td>
                   
                   <td>Bộ nhớ</td>
                   <td>Màn hình</td>
                   <td>Dung lượng pin</td>
               </tr>
               <tr>
                   <td><b><asp:Label ID="sRam" runat="server"></asp:Label></b></td>
                   
                   <td><b><asp:Label ID="sBonho" runat="server"></asp:Label></b></td>
                   <td><b><asp:Label ID="sManhinh" runat="server"></asp:Label></b></td>
                   <td><b><asp:Label ID="sDungluong" runat="server"></asp:Label></b></td>
               </tr>
           </table>
       </div>
       <div class="ghichu">
           <asp:Label ID="sGhichu" runat="server"></asp:Label>
       </div>
       <asp:UpdatePanel ID="UpdatePanel2" style="background-color:white;" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
       <ContentTemplate>
           <div class="my_binhluan">
               <asp:TextBox id="txtbinhluan" CssClass="txtbinhluan" runat="server" TextMode="MultiLine"  Rows="3"></asp:TextBox>
               <asp:Button ID="btnbinhluan" CssClass="btnbinhluan" runat="server" Text="Nhập bình luận" OnClientClick="return Check();" OnClick="btnbinhluan_Click" />
           </div>
           <div class="list_binhluan">
               <h4>BÌNH LUẬN</h4>
                       <p id="comment_empty" runat="server">Hãy là người đầu tiên bình luận!!</p>
               <asp:Repeater ID="grvBinhluan" runat="server" OnItemDataBound="grvBinhluan_ItemDataBound" >  
                   <HeaderTemplate>  
                       
                   </HeaderTemplate>
                   <ItemTemplate>
                       <div class="binhluan">
                       <asp:Label ID="lblhoten" CSSclass="hoten" Text=' <%# Eval("sHovaten") %> ' runat="server" />
                       <asp:Label ID="lblthoigian" CSSclass="" Text=' <%# Eval("dNgaygio") %> ' runat="server" />
                       <br />
                       <asp:Label  ID="lblnoidung"  Cssclass="noidung" Text = ' <%# Eval("sNoidung") %> ' runat="server"></asp:Label>
                       
                       </div>
                   </ItemTemplate>
               </asp:Repeater>
           </div>
       </ContentTemplate>
           
       </asp:UpdatePanel>
       
    </form>
</asp:Content>
