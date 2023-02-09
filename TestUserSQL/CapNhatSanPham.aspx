<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CapNhatSanPham.aspx.cs" Inherits="TestUserSQL.WebForm6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"  >
    <title></title>
    <style>
        #form1 {
            min-height:500px;
        }
        .itemIMG {
            height:150px;
            max-width:150px;
        }
        @media only screen and (max-width: 768px) {
            .itemIMG {
                width:100%;
                height:auto;
            }
            .gridview tr td:nth-child(2) {
                overflow: hidden;
                text-overflow: ellipsis;
                white-space: nowrap;
                max-width: 100px;
            }
        }
        
    </style>
    <script type="text/javascript">
        function check() {
            var tensanpham = document.getElementById('ContentPlaceHolder1_txtTensanpham').value;
            var giaban = document.getElementById('ContentPlaceHolder1_txtGiaban').value;
            var soluong = document.getElementById('ContentPlaceHolder1_txtSoluong').value;
            if (tensanpham.trim() == "") {
                alert("Tên sản phẩm không được trống");
                return false;
            }
            if (isNaN(giaban.trim()) || giaban.trim() <= 0) {
                alert("Giá bán phải là số nguyên lớn hơn 0");
                return false;
            }
            if (isNaN(soluong.trim()) || soluong.trim() <= 0) {
                alert("Số lượng phải là số nguyên lớn hơn 0");
                return false;
            }
            return true;
        }
        function themchitietcheck() {
            var bonho = document.getElementById('ContentPlaceHolder1_txtBonho').value;
            var mausac = document.getElementById('ContentPlaceHolder1_txtMausac').value;
            if (bonho.trim() == "") {
                alert("Không được để trống thông số bộ nhớ");
                return false;
            }
            if (mausac.trim() == "") {
                alert("Không được để trống thông số màu sắc");
                return false;
            }
            return true;
        }
        function chitietcheck() {
            var ram = document.getElementById('ContentPlaceHolder1_txtRAM').value;
            var bonho = document.getElementById('ContentPlaceHolder1_txtBonho').value;
            var manhinh = document.getElementById('ContentPlaceHolder1_txtManhinh').value;
            var dungluongpin = document.getElementById('ContentPlaceHolder1_txtDungluongpin').value;
            var mausac = document.getElementById('ContentPlaceHolder1_txtMausac').value;
            if (ram.trim() == "") {
                alert("Không được để trống thông số RAM");
                return false;
            }
            if (bonho.trim() == "") {
                alert("Không được để trống thông số bộ nhớ");
                return false;
            }
            if (manhinh.trim() == "") {
                alert("Không được để trống thông số màn hình");
                return false;
            }
            if (dungluongpin.trim() == "") {
                alert("Không được để trống thông số dung lượng pin");
                return false;
            }
            if (mausac.trim() == "") {
                alert("Không được để trống thông số màu sắc");
                return false;
            }
            return true;
        }
        function timkiemcheck() {
            var search = document.getElementById('ContentPlaceHolder1_txtTimkiem').value;
            if (search.trim() == "")
                return false;
            return true;

        }
        function ShowPreview(input) {
            if (input.files && input.files[0]) {
             //   var image = document.getElementyId('ContentPlaceHolder1_Image1');
                //   img.src = URL.createObjectURL(event.target.files[0]);
              //  console.log('a');
                var ImageDir = new FileReader();
                ImageDir.onload = function (e) {
                    //Jquery
                  //  $('#ContentPlaceHolder1_Image1').attr('src', e.target.result);
                    var image = document.getElementById('ContentPlaceHolder1_Image1');
                    image.src = e.target.result;
                }
                ImageDir.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager ID="ScriptManager1"
             runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
        <div><asp:Button ID="quaylaibtn" runat="server" Text="Quay về trang quản trị" OnClick="quaylaibtn_Click" /></div>
        <br />
            <div class="timkiem">
            <asp:TextBox ID="txtTimkiem" runat="server" placeholder="Nhập tên sản phẩm" ></asp:TextBox>
            <asp:Button ID="btnTimkiem" runat="server" OnClientClick="return timkiemcheck();" OnClick="btnTimkiem_Click" Text="Tìm kiếm" />
            <asp:Button ID="btnRefesh" runat="server"  OnClick="btnRefesh_Click" Text="Làm mới" />
        </div>
        <table style="width:100%" class="input_grid_table">
            <tr>
                <td style="width:60%; vertical-align:top" class="classtd">
                    <div style="overflow-x:auto;">
                    <asp:gridview runat="server" ID="grvSanPham"  CssClass="gridview"  DataKeyNames="PK_iMasanpham" AutoGenerateColumns="false"
                      OnRowDataBound="grvSanPham_RowDataBound" OnSelectedIndexChanged="grvSanPham_SelectedIndexChanged" PageSize="3"
                         ShowHeaderWhenEmpty="true" AllowPaging="true" AllowSorting="true" OnSorting="grvSanPham_Sorting" OnPageIndexChanging="grvSanPham_PageIndexChanging" >
                    
                        <Columns>
                            <asp:TemplateField HeaderText="Tên sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >
                                <ItemTemplate>
                                    <%#Eval("sTensanpham") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mô tả" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%" >
                                <ItemTemplate>
                                    <%#Eval("sMota") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Giá bán"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                                <ItemTemplate>
                                    <%# Convert.ToInt32(Eval("iGiaban")) > 10 ? String.Format("{0:0,0}", Eval("iGiaban")).Replace(',', '.') : Convert.ToInt32(Eval("iGiaban")).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.')%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Số lượng" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                                <ItemTemplate>
                                    <%#Eval("iSoluong") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Trạng thái" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                                <ItemTemplate>
                                    <asp:label id="trangthai" runat="server" Text=' <%#Eval("iTrangthai") %> '></asp:label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Chi tiết" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbtnChiTiet" CommandName="<%# Container.DataItemIndex %>"  CommandArgument='<%# Eval("PK_iMasanpham") %>' runat="server" ToolTip="Chi tiết" OnClick="lkbtnChiTiet_Click"  >
                                            <img src="/images/detail.png" border=0 />
                                        </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>Không tìm thấy dữ liệu</EmptyDataTemplate>
                    </asp:gridview>
                    </div>
                    <div id="paging" style="float:left; width:100%; display:none;">
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
                </td>
                <td style="width:40%; vertical-align:top" class="classtd">
                        <table cellpadding="0" cellspacing="1" style="width:100%; background-color:white; border:1px solid;" class="input_table">
                        <tr>
                        <td colspan="2">
                            <table id="binhthuong" runat="server">
                            <tr>
                            <td colspan="2" style="padding-bottom: 8px;">
                                <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTensanpham" runat="server" Text="Label">Tên sản phẩm</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox  runat="server" ID="txtTensanpham" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblGiaban" runat="server" Text="Label">Giá bán</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtGiaban"  ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSoluong" runat="server" Text="Label">Số lượng</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSoluong"  TextMode="Number"  min="1"  step="1" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMota" runat="server" Text="Label">Mô tả</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMota" TextMode="MultiLine" Rows="3" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTrangthai" runat="server" Text="Label">Trạng Thái</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTrangThai">
                                    <asp:ListItem Value="1" Text="Còn hàng"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Hết hàng"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:center;"><asp:Button runat="server" ID="saveBtn" Text="Lưu" OnClick="saveBtn_Click" 
                             OnClientClick="return check();"/>
                                <asp:Button runat="server" ID="delBtn" Text="Xóa" OnClientClick="return confirm('Bạn có muốn xóa sản phẩm này?!');" OnClick="delBtn_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:center;">------------------------------</td>
                        </tr>
                        </table>
                        </td>
                        </tr>
                            <%--Phần chi tiết--%>
                                <tr>
                                <td colspan="2">
                                <div id="chitiet" runat="server">
                                <table class="input_grid_table" >
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" CSSclass="itemIMG" ImageUrl='<%# Eval("sNguonhinhanh") %>' />
                                    </td>
                                    <td>
                                        <asp:FileUpload runat="server" ID="fileImport" onchange="ShowPreview(this)" Width="100%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CSSclass="itemName" Text="Thương hiệu" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlThuongHieu" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>RAM</td>
                                    <td>
                                        <asp:TextBox ID="txtRAM" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Bộ nhớ</td>
                                    <td>
                                        <asp:TextBox ID="txtBonho" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Màn hình</td>
                                    <td>
                                        <asp:TextBox ID="txtManhinh" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Dung lượng pin</td>
                                    <td>
                                        <asp:TextBox ID="txtDungluongpin" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Màu sắc</td>
                                    <td>
                                        <asp:TextBox ID="txtMausac" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Ghi chú</td>
                                    <td>
                                        <asp:TextBox ID="txtGhichu" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align:center;">
                                        <asp:Button ID="btnThemChiTiet" runat="server"  OnClick="btnThemChiTiet_Click" OnClientClick="return themchitietcheck();" Text="Thêm" />
                                        <asp:Button ID="btnLuuChiTiet" runat="server"  OnClick="btnLuuChiTiet_Click" OnClientClick="return chitietcheck();" Text="Lưu thông số" />
                                    </td>
                                </tr>
                                </table>
                                </div>
                                </td>
                                </tr>
                            </table>
                            
                    
                </td>
            </tr>
        </table>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnLuuChiTiet" />
        <asp:PostBackTrigger ControlID="btnThemChiTiet" />
        </Triggers>
        </asp:UpdatePanel>
            
        
        
        
        
        
        
    
    </form>
    

</asp:Content>
