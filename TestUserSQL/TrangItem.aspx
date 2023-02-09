<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TrangItem.aspx.cs" Inherits="TestUserSQL.TrangItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <script type="text/javascript">
        // Get the button:
        var mybutton;
        window.onload = function () { mybutton = document.getElementById('myBtn') };
        // When the user scrolls down 20px from the top of the document, show the button
        window.onscroll = function () { scrollFunction() };

        function scrollFunction() {
            if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
                mybutton.style.display = "block";
            } else {
                mybutton.style.display = "none";
            }
        }

        // When the user clicks on the button, scroll to the top of the document
        function topFunction() {
            document.body.scrollTop = 0; // For Safari
            document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
        }
    </script>
    <link href="css/ItemDisplayCSS.css" rel="stylesheet" type="text/css" />
    <style>
        #form1 {
            min-height:450px;
        }
      /*  .liitem ul {
	        position: absolute;
  	        background-color: #f9f9f9;
  	        text-align:right;
  	        z-index: 1;
  	        visibility: hidden;
  	        padding-inline-start: 0px;
            border: 1px solid;
            
	    }
	    nav ul:hover li ul li  {
	        visibility: visible;
    	    display:inline-block;
            
	    }
        nav ul:hover li ul li a {
	        text-decoration:none;
            
	    }
	    .liitem {
	        display:block;
	    }
	    .ul_first{
	        padding-inline-start: 0px;
		    display: inline-block;
            min-width: 150px;
        }
        .dropdownlink {
            text-decoration:none;
        } */
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
        
     <div id="banner" style="width:100%; background-color:white; text-align:center; ">
         <a href="ChiTietSanPham.aspx?id=20"><img src="/images/800-20016-800x200.gif" width="95%;" style="margin-top:-5px; " /></a>
     </div>
    <div style="display:none;">
        <p>Giao diện, hồ sơ khảo sát, nghiệp vụ bán hàng 2 chiều</p>
    </div>
     <div id="divtimkiem" style="text-align:right; margin-bottom:10px; ">
         <asp:TextBox CssClass="txtTimKiem" ID="txtTimKiem" runat="server"  ></asp:TextBox>
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
              <div style="background:linear-gradient(#d70018 0%,#ff8a97); margin-bottom:20px; border-radius: 0.5rem!important;">
              <div class ="danhmuc_title">
                <p>DANH MỤC SẢN PHẨM</p>
              </div>
              <div style="text-align: right;padding-right: 10px;">
                <asp:DropDownList ID="ddlHang" style="margin-bottom:-10px;" 
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
              
                <asp:Repeater ID="grvData" runat="server" OnItemDataBound="grvData_ItemDataBound" >                   
                        <HeaderTemplate>                                             
                         </HeaderTemplate>                  
                        <ItemTemplate >                     
                            <div class ="item" >
                                
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
                                <asp:Button style="display:none;" CommandName=' <%# Eval("iGiaban")%>'  CommandArgument= '<%# Eval("PK_iMasanpham") %> '  runat="server" Text="Thêm vào giỏ" OnClick="Unnamed_Click" />
                            </div>                                                           
                        </ItemTemplate>                  
                </asp:Repeater>
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
                <div style="background:linear-gradient(90deg,#ffecd2 0%,#fcb69f 100%); margin-bottom:20px; border-radius: 0.5rem!important;">
                    <div class ="danhmuc_title">
                        <p style="color:black;">BÌNH LUẬN NHIỀU</p>
                    </div>
                
                <asp:Repeater ID="grvData_Binhluan" runat="server" OnItemDataBound="grvData_Binhluan_ItemDataBound">
                    <HeaderTemplate>                                             
                         </HeaderTemplate>    
                    <ItemTemplate>
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
                 <div>
        
                    <div id="paging_2">
                    <asp:Repeater ID="repeater_binhluannhieu" runat="server" 
                     OnItemCommand="repeater_binhluannhieu_ItemCommand"  >
                        <ItemTemplate >
                            <asp:LinkButton ID="btnPage" CssClass="numb"       
                        CommandName="Page" CommandArgument="<%# Container.DataItem %>"
                        runat="server" ForeColor="Black" Font-Bold="True" Text="<%# Container.DataItem %>"> 
                            </asp:LinkButton>
                        </ItemTemplate>             
                                           
                    </asp:Repeater>
                    </div>
    </div>  
            </div>
            
            </ContentTemplate>
        </asp:UpdatePanel>
         
     </div>
    
         
         <button onclick="topFunction()" class="imgButton"  id="myBtn" title="Go to top"></button>
      
    </form>
</asp:Content>

