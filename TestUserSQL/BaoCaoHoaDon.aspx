<%@ Page Title="" Language="C#"  MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BaoCaoHoaDon.aspx.cs" Inherits="TestUserSQL.WebForm10" %>
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        #form1 {
            min-height:450px;
        }
        @media only screen and (max-width: 768px) {
            img {
                height:auto !important;
                width:100% !important;
                max-width:100% !important;
            }
            .ap_css_lbl {
                display:inherit;
            }
        }
    </style>
    <script type="text/javascript">
        function checkthang() {
            var thang = document.getElementById('ContentPlaceHolder1_txtThang').value;
            if (isNaN(thang.trim()) || thang.trim() <= 0 || thang.trim() > 12) {
                alert("Tháng phải là số nguyên từ 1 đến 12");
                return false;
            }
            return true;
        }
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=400,width=800');
            printWindow.document.write('<html><head><title></title>');
            printWindow.document.write('<style>body { text-align: center; } .paneltbl{margin-left: auto;margin-right: auto; }</style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
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
            <div style="display:none;">
                Xử lý nghiệp vụ khi hủy đơn đối với đơn hàng đã xác nhận
                Báo cáo theo bán chạy,....
                
                Bên phía admin không dc cập nhật thông tin(Nghiệp vụ chỉnh sửa đơn hàng bên admin)
                Giỏ hàng thêm số lượng
            </div>
        <div><asp:Button ID="quaylaibtn" runat="server" Text="Quay về trang quản trị" OnClick="quaylaibtn_Click" /></div>
            <br />
            <div style="text-align:center;">
            <table  class="input_table" style="margin-left:auto;margin-right:auto; margin-bottom:10px;" >
                <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlChedoxem"   AutoPostBack="true" OnSelectedIndexChanged="ddlChedoxem_SelectedIndexChanged" runat="server">
                            <asp:ListItem Value="0" Text="Danh sách hóa đơn"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Bán chạy"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Hàng tồn"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td>
                    <asp:Label ID="lblngay" runat="server"   CssClass="ap_css_lbl">Ngày lập(từ - đến)</asp:Label>
                    <asp:TextBox   Type="Date" ID="txtTuNgay" runat="server" ></asp:TextBox>
                    <span>-</span>
                    <asp:TextBox   Type="Date" ID="txtDenNgay" runat="server" ></asp:TextBox>
                    <asp:Button ID="btnXemBaoCao" OnClick="btnXemBaoCao_Click" runat="server" Text="Xem báo cáo" />
                 <asp:Button ID="btnExcel" OnClick="btnExcel_Click" runat="server" Text="Xuất excel" />
                 <!--   <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick = "return PrintPanel();" /> --> 
                </td>
                
                
                </tr>
                <tr>
                    <td><asp:DropDownList ID="ddlThang"   runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlNam"   runat="server">
                        </asp:DropDownList>
                    <asp:Button ID="btnBaocao" runat="server" Text="Xem biểu đồ"  OnClick="btnBaocao_Click" />
                    <asp:Button ID="btnAnBaocao" style="display:none;" runat="server" Text="Ẩn biểu đồ" OnClick="btnAnBaocao_Click" />
                    </td>
                </tr>
                
            </table>
            </div>
            <asp:Panel id="pnlContents" style="display:none; "  runat = "server">
                    <span style="font-size: 10pt; font-weight:bold; font-family: Arial">
                         Cửa hàng điện thoại Hải Nam
                    </span>
                    <br />
                    <span>
                        Hóa đơn bán hàng
                    </span>
                    <br />
                    <table class="paneltbl" >
                        <tr>
                            <td>STT</td>
                            <td>Tên sản phẩm</td>
                            <td>SL</td>
                            <td>Đơn giá</td>
                            <td>Thành tiền</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>SamSung s9 Plus</td>
                            <td>1</td>
                            <td>9.000.000</td>
                            <td>9.000.000</td>
                        </tr>
                    </table>
            </asp:Panel>
            <div style="overflow-x:auto;">
            <div style="text-align:right;">
            <asp:Label ID="lbltongtien" runat="server"></asp:Label>
            </div>
            <asp:gridview  runat="server"  ID="grvHoaDon" DataKeyNames="PK_iMahoadon" AutoGenerateColumns="false"  CssClass="gridview"
                      AllowPaging="true" PageSize="8" AllowSorting="true" OnSorting="grvHoaDon_Sorting" OnPageIndexChanging="grvHoaDon_PageIndexChanging"
                      ShowHeaderWhenEmpty="true"  OnRowDataBound="grvHoaDon_RowDataBound" OnSelectedIndexChanged="grvHoaDon_SelectedIndexChanged">
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Mã hóa đơn" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >
                            <ItemTemplate>
                                <%#Eval("PK_iMahoadon") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên khách hàng" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <%#Eval("tenkhachhang") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nhân viên xác nhận" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="12%" >
                            <ItemTemplate>
                                <%#Eval("tennhanvien") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SĐT" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" >
                            <ItemTemplate>
                                <%#Eval("sSDT") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >
                            <ItemTemplate>
                                <%#Eval("sDiachi") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày lập" ItemStyle-HorizontalAlign="Center" SortExpression="dNgayLap" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%# Eval("dNgayLap", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phụ phí" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" >
                            <ItemTemplate>
                                <%# Convert.ToInt32(Eval("iPhuphi")) > 10 ? String.Format("{0:0,0}", Eval("iPhuphi")).Replace(',', '.') : Convert.ToInt32(Eval("iPhuphi")).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.')%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ghi chú" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                  <%#Eval("sGhichu") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                        <asp:TemplateField HeaderText="Tổng tiền" SortExpression="iTongtien" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="7%" >
                            <ItemTemplate>
                                  <asp:label ID="lbltien" runat="server" Text=' <%#Eval("iTongtien") %> '></asp:label>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass = "gridPager" />
                </asp:gridview>
            </div>
            <div style="overflow-x:auto;">
            <asp:gridview  runat="server"  ID="grvBanChay" DataKeyNames="PK_iMasanpham" AutoGenerateColumns="false"  CssClass="gridview"
                      AllowPaging="true" PageSize="8" AllowSorting="true" OnSorting="grvBanChay_Sorting" OnPageIndexChanging="grvBanChay_PageIndexChanging"
                      ShowHeaderWhenEmpty="true"  OnRowDataBound="grvBanChay_RowDataBound" OnSelectedIndexChanged="grvBanChay_SelectedIndexChanged">
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Mã sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <%#Eval("PK_iMasanpham") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >
                            <ItemTemplate>
                                <%#Eval("sTensanpham") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Số lượng bán" ItemStyle-HorizontalAlign="Center" SortExpression="soluong" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <%#Eval("soluong") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Tổng tiền" ItemStyle-HorizontalAlign="Center" SortExpression="tongtien" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%# Convert.ToInt64(Eval("tongtien")) > 10 ? String.Format("{0:0,0}", Eval("tongtien")).Replace(',', '.') : Convert.ToInt64(Eval("tongtien")).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.')%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass = "gridPager" />
                </asp:gridview>
            </div>
            <div style="overflow-x:auto;">
            <asp:gridview  runat="server"  ID="grvHangTon" DataKeyNames="PK_iMasanpham" AutoGenerateColumns="false"  CssClass="gridview"
                      AllowPaging="true" PageSize="8" AllowSorting="true" OnSorting="grvHangTon_Sorting" OnPageIndexChanging="grvHangTon_PageIndexChanging"
                      ShowHeaderWhenEmpty="true"  OnRowDataBound="grvHangTon_RowDataBound" >
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Mã sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <%#Eval("PK_iMasanpham") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên sản phẩm" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" >
                            <ItemTemplate>
                                <%#Eval("sTensanpham") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Số lượng tồn" ItemStyle-HorizontalAlign="Center" SortExpression="iSoluong" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <%#Eval("iSoluong") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Giá bán" ItemStyle-HorizontalAlign="Center" SortExpression="iGiaban" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%# Convert.ToInt32(Eval("iGiaban")) > 10 ? String.Format("{0:0,0}", Eval("iGiaban")).Replace(',', '.') : Convert.ToInt32(Eval("iGiaban")).ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(',', '.')%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass = "gridPager" />
                </asp:gridview>
            </div>
            <div style="text-align:center;">
            <asp:Chart   ID="Chart1"  style="max-width:50%; " runat="server">
                <Titles>
                    <asp:Title Font="Times New Roman, 12pt, style=Bold, Italic" Name="Title1" >
                    </asp:Title>
                </Titles>
                <series>
                    
                    <asp:Series  ChartType="Column" Name="Series1"   IsValueShownAsLabel="true"  XValueType="String" XValueMember="2" YValueType="Int64" YValueMembers="3">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea  Name="ChartArea1" >
                        <AxisY Title="Doanh thu " ></AxisY>
                        <AxisX Title="Ngày"  Minimum="0" IntervalAutoMode="VariableCount"  Interval="200" IntervalType="Number"></AxisX>
                    </asp:ChartArea>
                </chartareas>
            </asp:Chart>
            </div>
        </ContentTemplate>
            <Triggers>
    <asp:PostBackTrigger ControlID="btnExcel" />
            </Triggers>
        </asp:UpdatePanel>
        
    </form>
</asp:Content>
