<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="DanhSachMuaHang.aspx.cs" Inherits="TestUserSQL.WebForm9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <link rel="stylesheet" href="css/tablecss.css" />
    <style>
        #form1 {
            min-height:450px;
        }
        .ItemIMG {
            width:100px;
            min-height:100px;
            max-height:150px;
        }
        .ddl_chitiet {
            height:auto;
        }
    </style>
    <script type="text/javascript">
        function CheckCoBan() {
            var sdt = document.getElementById('ContentPlaceHolder1_txtSDT').value;
            if (sdt.trim() == "")
                return false;
            var DiaChi = document.getElementById('ContentPlaceHolder1_txtDiaChi').value;
            if (DiaChi.trim() == "")
                return false;
            
            var conf = confirm("Bạn có muốn thay đổi thông tin đơn hàng này?");
            if (!conf)
                return false;
            return true;
        }
        function Check() {
            var sanpham = document.getElementById('ContentPlaceHolder1_ddlSanpham').value;
            if (sanpham == "0") {
                alert("Chưa chọn sản phẩm");
                return false;
            }
            var soluong = document.getElementById('ContentPlaceHolder1_txtSoluong').value;
            var dongia = document.getElementById('ContentPlaceHolder1_txtDongia').value;
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
            var search = document.getElementById('ContentPlaceHolder1_txtTimkiem').value;
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
            <div class="timkiem">
            <asp:TextBox ID="txtTimkiem" runat="server" placeholder="Nhập mã hóa đơn" ></asp:TextBox>
            <asp:Button ID="btnTimkiem" runat="server" OnClientClick="return timkiemcheck();" OnClick="btnTimkiem_Click" Text="Tìm kiếm" />
            <asp:Button ID="btnRefesh" runat="server"  OnClick="btnRefesh_Click" Text="Làm mới" />
            <asp:Label runat="server" Text="Từ ngày"></asp:Label><asp:TextBox ID="txtTuNgay" runat="server" Type="Date" AutoPostBack="true" OnTextChanged="txtTuNgay_TextChanged" />
            <asp:Label runat="server" Text="Đến ngày"></asp:Label><asp:TextBox Type="Date" ID="txtDenNgay" runat="server" AutoPostBack="true" OnTextChanged="txtDenNgay_TextChanged" ></asp:TextBox>
            <asp:Label ID="Label1" runat="server" Text="Chế độ xem"></asp:Label>
                        <asp:DropDownList ID="ddlChedoxem" AutoPostBack="true" OnSelectedIndexChanged="ddlChedoxem_SelectedIndexChanged" runat="server">
                            <asp:ListItem Value="0" Text="Tất cả"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Chờ xác nhận"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Đang giao hàng"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Đơn hàng hủy"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Đã thanh toán"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Đơn hàng trả"></asp:ListItem>
                        </asp:DropDownList>
            </div>
            <div class="thongbao" style="background-color:white;">
                <span style="color:red;padding-bottom:8px;"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
            </div>
            <div style="overflow-x:auto;">
            <asp:gridview  style="width:100%;"  CssClass="gridview" runat="server" ID="grvHoaDon" DataKeyNames="PK_iMahoadon" AutoGenerateColumns="false" AllowPaging="true" PageSize="6"
                     OnPageIndexChanging="grvHoaDon_PageIndexChanging" ShowHeaderWhenEmpty="true"  OnRowDataBound="grvHoaDon_RowDataBound" OnSelectedIndexChanged="grvHoaDon_SelectedIndexChanged"
                      AllowSorting="true" OnSorting="grvHoaDon_Sorting">
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Mã hóa đơn" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" >
                            <ItemTemplate>
                                <%#Eval("PK_iMahoadon") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="SĐT" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <%#Eval("sSDT") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="22%" >
                            <ItemTemplate>
                                <%#Eval("sDiachi") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày lập"  SortExpression="dNgayLap" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%#Eval("dNgayLap") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phụ phí" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >
                            <ItemTemplate>
                                 <%#Eval("iPhuphi") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ghi chú" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" >
                            <ItemTemplate>
                                  <%#Eval("sGhichu") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trạng thái" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="11%"  >
                            <ItemTemplate>
                                  <asp:label ID="lblTrangthai"  runat="server" Text='<%#Eval("iTrangthai") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tổng tiền" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"  >
                            <ItemTemplate>
                                  <asp:label ID="lblTongtien"  runat="server" Text='<%#Eval("iTongtien") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chi tiết" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="3%" >
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbtnChiTiet"   CommandArgument='<%# Eval("PK_iMahoadon") %>' runat="server" ToolTip="Chi tiết" OnClick="lkbtnChiTiet_Click"  >
                                            <img src="/images/detail.png" border=0 />
                                        </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                    </Columns>
                    
                </asp:gridview>
                </div>
                <table runat="server" id="tblCoBan" class="input_table">
                    
                    <tr>
                        <td>SĐT</td>
                        <td><asp:TextBox ID="txtSDT" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Địa chỉ</td>
                        <td><asp:TextBox ID="txtDiaChi" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Người nhận</td>
                        <td><asp:TextBox ID="txtNguoiNhan" runat="server"  ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Phụ phí</td>
                        <td><asp:TextBox ID="txtPhuPhi" runat="server" Enabled="false"></asp:TextBox></td>
                    </tr>
                    
                    <tr>
                        <td>Ghi chú</td>
                        <td><asp:TextBox ID="txtGhiChu" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:label ID="lblLydo" runat="server" Text="Lý do hủy"></asp:label></td>
                        <td><asp:TextBox ID="txtLydoHuy" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center">
                            <asp:Button ID="btnTraHang" runat="server" Text="Yêu cầu trả hàng" OnClick="btnTraHang_Click" OnClientClick="return confirm('Xác nhận gửi yêu cầu?');" />
                            <asp:Button ID="btnLuuCoBan" runat="server" Text="Lưu" OnClick="btnLuuCoBan_Click" OnClientClick="return CheckCoBan();" />
                            <asp:Button ID="btnThayDoiKhiGiaoHang" runat="server" Text="Yêu cầu thay đổi" OnClick="btnThayDoiKhiGiaoHang_Click" OnClientClick="return confirm('Bạn có muốn thay đổi đơn hàng này?');" />
                            <asp:Button ID="btnHuyYeuCauThayDoi" runat="server" Text="Hủy yêu cầu" OnClick="btnHuyYeuCauThayDoi_Click" />
                            <asp:Button ID="btnHuy" runat="server" Text="Hủy" OnClick="btnHuy_Click" OnClientClick="return confirm('Bạn có muốn hủy đơn hàng này?');" />
                            <asp:Button ID="btnHuyKhiGiaoHang" runat="server" Text="Yêu cầu hủy" OnClick="btnHuyKhiGiaoHang_Click" OnClientClick="return confirm('Bạn có muốn hủy đơn hàng này?');" />
                            <asp:Button ID="thanhtoanBtn" runat="server" Text="Thanh toán" OnClick="thanhtoanBtn_Click"/>
                        </td>
                    </tr>
                </table>
                <div style="overflow-x:auto;">
                <table runat="server" class="input_grid_table"  id="tblChiTiet" style="width:100%; text-align:center; ">
                    <tr>
                        <td class="classtd" style="width:30%; text-align:left; vertical-align:top;">
                            
                            <table class="input_table">
                                
                                <tr>
                                    <td>Tên sản phẩm</td>
                                    <td><asp:DropDownList runat="server" ID="ddlSanpham" AutoPostBack="true" OnSelectedIndexChanged="ddlSanpham_SelectedIndexChanged" >
                                    </asp:DropDownList> </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:DropDownList runat="server" CssClass="ddl_chitiet" ID="ddlBoNho" AutoPostBack="true" OnSelectedIndexChanged="ddlBoNho_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:DropDownList runat="server" CssClass="ddl_chitiet" ID="ddlMausac" AutoPostBack="true" OnSelectedIndexChanged="ddlMausac_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
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
                                    <td colspan="2" style="text-align:center;">
                                        <asp:Button ID="addBtn" runat="server" OnClientClick="return Check();" OnClick="addBtn_Click" Text="Thêm sản phẩm" />
                                        <asp:Button ID="saveBtn" runat="server" OnClientClick="return Check();" OnClick="saveBtn_Click" Text="Lưu" />
                                        <asp:Button ID="delBtn" runat="server" OnClick="delBtn_Click" Text="Xóa sản phẩm" OnClientClick="return confirm('Bạn có thật sự muốn xóa dòng đơn hàng này?');" />
                                        
                                    </td>
                                </tr>
                            </table>
                            
                        </td>
                        <td class="classtd" style="width:70%; vertical-align:top;">
                            <div style="overflow-x:auto;">
                            <asp:GridView ID="grvChiTiet"  CssClass="gridview" runat="server" DataKeyNames="PK_iCT_HoaDonID"  AutoGenerateColumns="false"
                                OnRowDataBound="grvChiTiet_RowDataBound" OnSelectedIndexChanged="grvChiTiet_SelectedIndexChanged">
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
                            <asp:TemplateField HeaderStyle-Width="0%" >
                                <ItemTemplate>
                                    <asp:label style="display:none;" id="idsp_saveCheck" Text=' <%#Eval("FK_iMasanpham") %> '  runat="server"></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="0%" >
                                <ItemTemplate>
                                    <asp:label style="display:none;" id="bonho_saveCheck" Text=' <%#Eval("sBonho") %> '  runat="server"></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                            </Columns>
                            </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
