<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="XacNhanDonHang.aspx.cs"
     Inherits="TestUserSQL.WebForm2" EnableEventValidation="false"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        @media only screen and (max-width: 768px) {
            .txtTimkiem {
                width:50% !important;
            }
        }
    </style>
    <link rel="stylesheet" href="css/tablecss.css" />
    <script type="text/javascript">
        const popupCenter = ({url, title, w, h}) => {
            // Fixes dual-screen position                             Most browsers      Firefox
            const dualScreenLeft = window.screenLeft !==  undefined ? window.screenLeft : window.screenX;
            const dualScreenTop = window.screenTop !==  undefined   ? window.screenTop  : window.screenY;

            const width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            const height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

            const systemZoom = width / window.screen.availWidth;
            const left = (width - w) / 2 / systemZoom + dualScreenLeft
            const top = (height - h) / 2 / systemZoom + dualScreenTop
            const newWindow = window.open(url, title, 
              `
      scrollbars=yes,
      width=${w / systemZoom}, 
      height=${h / systemZoom}, 
      top=${top}, 
      left=${left}
      `
            )

            if (window.focus) newWindow.focus();
        }

        function createpopup(mahoadon){
            popupCenter({url: '/ChiTietDonHang.aspx?id=' + mahoadon, title: 'Chi tiết đơn hàng', w: 800, h: 400});  
        }
        function timkiemcheck() {
            var search = document.getElementById('ContentPlaceHolder1_txtTimkiem').value;
            if (search.trim() == "")
                return false;
            return true;

        }
        function Check() {
            var sdt = document.getElementById('ContentPlaceHolder1_txtSDT').value;
            if (sdt.trim() == "")
                return false;
            var DiaChi = document.getElementById('ContentPlaceHolder1_txtDiaChi').value;
            if (DiaChi.trim() == "")
                return false;
            var PhuPhi = document.getElementById('ContentPlaceHolder1_txtPhuPhi').value;
            if (isNaN(PhuPhi))
                return false;
            if ( PhuPhi < 0)
                return false;
            var conf = confirm("Bạn có muốn thay đổi thông tin đơn hàng này?");
            if (!conf)
                return false;
            return true;
        }
        function xacnhan(trangthai) {
            
            if(trangthai == "-1")
            {
                var conf = confirm("Bạn có muốn khôi phục đơn hàng này?");
            }
            else
            {
                
                var conf = confirm("Bạn có muốn thay đổi trạng thái đơn hàng này?");
            }
            if (!conf)
                return false;
            return true;
        }
        
        function PrintPanel() {
            console.log("1");
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=400,width=800');
            printWindow.document.write('<html><head><title>HOA DON BAN HANG</title>');
            printWindow.document.write('<style>body { text-align: center; }  table, tr,th, td{ border:none !important; } .paneltbl{margin-left: auto;margin-right: auto; }</style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
           
        }
    </script>
    <style>
        #form1 {
            min-height:450px;
        }
    </style>
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
                <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                <table style="width:100%;" class="input_grid_table">
            <tr>
               <td style="width:40%; " class="classtd">
                <table class="input_table">
                    <tr>
                        <td>SĐT</td>
                        <td><asp:TextBox ID="txtSDT" Enabled="false" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Địa chỉ</td>
                        <td><asp:TextBox ID="txtDiaChi" Enabled="false" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Người nhận</td>
                        <td><asp:TextBox ID="txtNguoiNhan" Enabled="false" runat="server"  ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Phụ phí</td>
                        <td><asp:TextBox ID="txtPhuPhi" runat="server" ></asp:TextBox></td>
                    </tr>
                    
                    <tr>    
                        <td>Ghi chú</td>
                        <td><asp:TextBox ID="txtGhiChu" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center">
                            <asp:Button ID="saveBtn" runat="server" Text="Lưu" OnClick="saveBtn_Click" OnClientClick="return Check();" /> 
                            <asp:Button ID="TuChoiHuy" runat="server" Text="Từ chối yêu cầu" OnClick="TuChoiHuy_Click" OnClientClick="return confirm('Bạn có muốn từ chối yêu cầu?');" />
                            <asp:Button ID="delBtn" runat="server" Text="Hủy" OnClick="delBtn_Click" OnClientClick="return confirm('Bạn có muốn hủy đơn hàng này?');" />
                            <asp:Button ID="btnPrint" runat="server" Text="In hóa đơn" OnClick="btnPrint_Click" />  
                        </td>
                    </tr>

                </table>
               </td>
               <td style="width:60%;  vertical-align:bottom;  " class="classtd" >
                   <asp:Label ID="lblTongTien" runat="server" Style="float:right; display:none;"></asp:Label>
                   <asp:GridView style="display:none;" ID="grvChiTiet" runat="server"  CssClass="gridview" DataKeyNames="PK_iCT_HoaDonID" AutoGenerateColumns="false"
                       OnRowDataBound="grvChiTiet_RowDataBound">
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
                       </Columns>
                   </asp:GridView>
                    <div class="timkiem" style="text-align:right;  ">
                        <asp:Label runat="server" Text="Chế độ xem"></asp:Label>
                        <asp:DropDownList ID="ddlChedoxem" AutoPostBack="true" OnSelectedIndexChanged="ddlChedoxem_SelectedIndexChanged" runat="server">
                            <asp:ListItem Value="0" Text="Chờ xác nhận"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Đang giao hàng"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Đã bị hủy"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Yêu cầu hủy"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Yêu cầu thay đổi"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Tất cả"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <asp:TextBox CssClass="txtTimkiem" ID="txtTimkiem" runat="server" placeholder="Nhập mã hóa đơn" style="width:20%;"></asp:TextBox>
                        <asp:Button ID="btnTimkiem" runat="server" OnClientClick="return timkiemcheck();" OnClick="btnTimkiem_Click" Text="Tìm kiếm" />
                        <asp:Button ID="btnRefesh" runat="server"  OnClick="btnRefesh_Click" Text="Làm mới" />
                        <br />
                        <p><asp:Label runat="server" Text="Từ ngày"></asp:Label><asp:TextBox ID="txtTuNgay" runat="server" Type="Date" AutoPostBack="true" OnTextChanged="txtTuNgay_TextChanged" /></p>
                        <p><asp:Label runat="server" Text="Đến ngày"></asp:Label><asp:TextBox Type="Date" ID="txtDenNgay" runat="server" AutoPostBack="true" OnTextChanged="txtDenNgay_TextChanged" ></asp:TextBox></p>
                    </div>
               </td>
            </tr>
        </table>
                <div style="overflow-x:auto;">
                <asp:gridview runat="server" ID="grvHoaDon"   CssClass="gridview" DataKeyNames="PK_iMahoadon" AutoGenerateColumns="false" PageSize="5" OnPageIndexChanging="grvHoaDon_PageIndexChanging"
                      ShowHeaderWhenEmpty="true" AllowPaging="true"  OnRowDataBound="grvHoaDon_RowDataBound" OnSelectedIndexChanged="grvHoaDon_SelectedIndexChanged"
                      AllowSorting="true" OnSorting="grvHoaDon_Sorting" >
                    
                    <Columns>
                       
                        <asp:TemplateField HeaderText="Mã hóa đơn" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >
                            <ItemTemplate>
                                <asp:label id="lblhd_mahoadon" runat="server" Text=' <%#Eval("PK_iMahoadon") %> '></asp:label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên người dùng" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%#Eval("sHovaten") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SĐT" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <%#Eval("sSDT") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30%" >
                            <ItemTemplate>
                                <%#Eval("sDiachi") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày lập" SortExpression="dNgayLap" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%#Eval("dNgayLap") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tổng tiền" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >
                            <ItemTemplate>
                                <%# Convert.ToInt64(Eval("iTongtien")) > 10 ? String.Format("{0:0,0}", Eval("iTongtien")).Replace(',', '.') : Convert.ToInt64(Eval("iTongtien")).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.')%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ghi chú" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="9%" >
                            <ItemTemplate>
                                  <%#Eval("sGhichu") %>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Chi tiết" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="3%" >
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbtnChiTiet"   CommandArgument='<%# Eval("PK_iMahoadon") %>' runat="server" ToolTip="Chi tiết" OnClientClick='<%# "createpopup(" + Eval("PK_iMahoadon") + ")"%>'  >
                                            <img src="/images/detail.png" border=0 />
                                        </asp:LinkButton>
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trạng thái" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"  >
                            <ItemTemplate>
                                  <asp:Button ID="xacnhanbtn"   runat="server" Text='<%#Eval("iTrangthai") %>' OnClientClick='<%# "return xacnhan(" + Eval("iTrangthai") + ")"%>' OnClick="xacnhanbtn_Click" CommandArgument='<%#Eval("PK_iMahoadon") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    
                </asp:gridview>
                </div>
        
        <div>
            
        </div>
        <asp:Panel id="pnlContents" style="display:none"   runat = "server">
                    <span  style="font-size: 10pt; font-weight:bold; font-family: Arial; ">
                         CỬA HÀNG ĐIỆN THOẠI HẢI NAM
                    </span>
                    <br /  >
                    <span>
                        ĐC: Xóm 3c - Khánh Nhạc - Yên Khánh - Ninh Bình
                    </span>
                    <br />
                    <span>
                        SĐT: 0879455495
                    </span>
                    <br />
                     <span>----------------</span>
                    <br />
                    
                    <span>
                        HÓA ĐƠN BÁN HÀNG
                    </span>
                    <br />
                    <asp:Label id="lblngayin" runat="server"></asp:Label>
                    <br / >
                    <asp:Label id="lbltennhanvien" runat="server"></asp:Label>
                    <br />
                    <div class="paneltbl" >
                    
                        <asp:GridView ID="grvHoaDon_In" runat="server" DataKeyNames="PK_iCT_HoaDonID" AutoGenerateColumns="false"
                        OnRowDataBound="grvHoaDon_In_RowDataBound">
                       <Columns>
                           <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Tên sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%" >
                                <ItemTemplate>
                                    <%#Eval("sTensanpham") %>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Số lượng" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:label id="lblSoluong" runat="server" Text=' <%#Eval("iSoluong") %> '></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Đơn giá" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:label id="lbltDonGia" runat="server" Text=' <%#Eval("iDonGia") %> '></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Thành tiền" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:label id="lblThanhtien" runat="server"></asp:label>
                                </ItemTemplate>
                           </asp:TemplateField>
                       </Columns>
                   </asp:GridView>
                    </div>
                    <br />
                    <div style="text-align:right;">
                        <asp:Label ID="lblphuphi" runat="server"></asp:Label>
                    </div>
                    <br />
                    <div style="text-align:right;">
                        <asp:Label ID="lbltongtienin" runat="server"></asp:Label>
                    </div>
                   <br />
                    <span>----------------</span>
                    <br />
                    <span>Xin cảm ơn, hẹn gặp lại quý khách!</span>
                   </asp:Panel>
            
        </ContentTemplate>
       </asp:UpdatePanel>
    </form>
</asp:Content>
