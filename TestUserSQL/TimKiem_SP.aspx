<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TimKiem_SP.aspx.cs" Inherits="TestUserSQL.WebForm4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <link href="css/ItemDisplayCSS.css" rel="stylesheet" type="text/css" />
    <style>
        #form1 {
            min-height:450px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 runat="server" id="h3"></h3>
    <form id="form1" runat="server">
 <!--   <nav style=" text-align:right; width:100%;" >
            
			<ul class= "ul_first" style="text-align:right; ">
			    <asp:HyperLink CssClass="dropdownlink"  ID="hyperlinktaikhoan" runat="server" ></asp:HyperLink>
			    <li class = "liitem">
				    <ul runat="server"  id="taikhoanul">
					
				    </ul>
			    </li>
			</ul>
	</nav> -->
     <div style="text-align:right; margin-bottom:10px; display:none;">
         <asp:TextBox CssClass="txtTimKiem"  ID="txtTimKiem" runat="server"  ></asp:TextBox>
         <asp:Button CssClass="btnTimKiem" ID="btnTimKiem" runat="server" Text="Tìm kiếm" OnClick="btnTimKiem_Click" />
     </div>
     <div>
         <asp:ScriptManager ID="ScriptManager1"
             runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="snackbar">Thêm hàng vào giỏ thành công</div>
              
              <div class ="lstitem">
              <div>
              <div class ="danhmuc_title">
                <p style="color:black;">DANH MỤC SẢN PHẨM</p>
              </div>
              <div style="text-align: right;padding-right: 10px;">
                  <asp:DropDownList ID="ddlHang"  style="margin-bottom:-10px;"
                    runat="server"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlHang_SelectedIndexChanged">

                </asp:DropDownList>
                <asp:DropDownList ID="DropDownList1" style="margin-bottom:-10px;"
                    runat="server"
                    AutoPostBack="true"
                    OnSelectedIndexChanged ="DropDownList1_SelectedIndexChanged">
                    <asp:ListItem Value="0" Text="Sắp xếp"></asp:ListItem>
                    <asp:ListItem Value="ASC" Text="Tăng dần"></asp:ListItem>
                    <asp:ListItem Value="DESC" Text="Giảm dần"></asp:ListItem>
                    <asp:ListItem Value="SELL_DESC" Text="Bán chạy"></asp:ListItem>
                 </asp:DropDownList>
              </div> 
              </div>   
                <asp:Repeater ID="grvData" runat="server" OnItemDataBound="grvData_ItemDataBound" >                   
                        <HeaderTemplate>                                             
                         </HeaderTemplate>                  
                        <ItemTemplate >                     
                            <div class ="item">
                                
                                <a href='<%# "ChiTietSanPham.aspx?id=" + Eval("PK_iMasanpham") %>'><asp:Image ID="Image1" CSSclass="itemIMG" runat="server" ImageUrl='<%# Eval("sNguonhinhanh") %>' /></a>
                                <br />
                                <a href='<%# "ChiTietSanPham.aspx?id=" + Eval("PK_iMasanpham") %>' style="text-decoration:none; color:black;"><asp:Label ID="Label1" CSSclass="itemName" Text=' <%# Eval("sTensanpham") %> ' runat="server" /></a>   
                                <br />
                                <asp:Label ID="Label2"  Cssclass="itemPrice" Text = ' <%# Eval("iGiaban") + " VND"%> ' runat="server"></asp:Label>
                                <asp:Label ID="giamgia" CssClass="itemReduce"  runat="server" Text=' <%# Eval("iTilekhuyenmai") %> '></asp:Label>
                                <br />
                                <asp:Label ID="oldprice" CssClass="old" Text="" runat="server"></asp:Label>
                                
                               
                                <br />
                                <asp:HyperLink ID="HyperLink1" Cssclass="itemDetail" style="display:none;"  NavigateUrl='<%# "ChiTietSanPham.aspx?id=" + Eval("PK_iMasanpham") %>' Text="Chi tiết" runat="server" />
                                <asp:Button ID="Button1" style="display:none;" CommandName=' <%# Eval("iGiaban")%>'  CommandArgument= '<%# Eval("PK_iMasanpham") %> '  runat="server" Text="Thêm vào giỏ" OnClick="Unnamed_Click" />
                            </div>                                                             
                        </ItemTemplate>                  
                </asp:Repeater>    
            </div>
            
            </ContentTemplate>
        </asp:UpdatePanel>
         
     </div>
    <div>
        
        <div id="paging">
                    <asp:Repeater ID="repeaterPaging" runat="server" 
                     onitemcommand="repeaterPaging_ItemCommand"  >
                        <ItemTemplate >
                            <asp:LinkButton ID="btnPage" CssClass="numb"       
                        CommandName="Page" CommandArgument="<%# Container.DataItem %>"
                        runat="server" ForeColor="Black" Font-Bold="True" Text="<%# Container.DataItem %>"> 
                            </asp:LinkButton>
                        </ItemTemplate>             
                                           
                    </asp:Repeater>
            </div>
    </div>
    </form>
</asp:Content>
