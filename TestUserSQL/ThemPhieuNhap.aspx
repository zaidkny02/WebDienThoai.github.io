<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ThemPhieuNhap.aspx.cs" Inherits="TestUserSQL.WebForm7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        #form1 {
            min-height:450px;
        }
    </style>
    <script type="text/javascript">
        function Check() {
            var nhacungcap = document.getElementById('ContentPlaceHolder1_ddlNhaCungCap').value;
            if (nhacungcap == "0") {
                alert('Chưa chọn nhà cung cấp');
                return false;
            }
            var tennguoigiao = document.getElementById('ContentPlaceHolder1_txtTennguoigiao').value;
            if (tennguoigiao.trim() == "") {
                alert('Thiếu người giao hàng');
                return false;
            }
            return true;
        }
        function timkiemcheck() {
            var search = document.getElementById('ContentPlaceHolder1_txtTimKiem').value;
            if (search.trim() == "")
                return false;
            return true;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1"
             runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
        <div><asp:Button ID="quaylaibtn" runat="server" Text="Quay về trang quản trị" OnClick="quaylaibtn_Click" /></div>
        <br />
            <div>
            <asp:TextBox  ID="txtTimKiem" runat="server" placeholder="Nhập tên nhà cung cấp"  ></asp:TextBox>
            <asp:Button ID="btnTimKiem" runat="server" Text="Tìm kiếm" OnClientClick="return timkiemcheck();"  OnClick="btnTimKiem_Click" />
            <asp:Button ID="btnrefesh" runat="server" Text="Tạo lại dữ liệu" OnClick="btnrefesh_Click" />
        </div>
        <table style="width:100%" class="input_grid_table">
        <tr>
            <td style="width:25%;  vertical-align:top;" class="classtd" >
                <table cellpadding="0" cellspacing="1" style="width:100%" class="input_table">
                    <tr>
                        <td colspan="2" style="padding-bottom: 8px;">
                           <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Label ID="lblTennhacungcap" runat="server" Text="Label">Tên nhà cung cấp</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlNhaCungCap" >
                                
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTennguoigiao" runat="server" Text="Label">Người giao hàng</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox  runat="server" ID="txtTennguoigiao" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNgaylap" runat="server" Text="Label">Ngày lập</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox   Type="Date" ID="txtNgaylap" runat="server" Enabled="false" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblGhichu" runat="server" Text="Label">Ghi chú</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox  runat="server" ID="txtGhichu" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Trạng thái</td>
                        <td >
                            <asp:TextBox  runat="server" ID="txtTrangThai" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" ID="addBtn" Text="Thêm mới" OnClick="addBtn_Click" OnClientClick="return Check();"/>
                            <asp:Button runat="server" ID="saveBtn" Text="Lưu" OnClick="saveBtn_Click" OnClientClick="return Check();"/>
                            <asp:Button runat="server" ID="delBtn" Text="Xóa" OnClick="delBtn_Click" OnClientClick="return confirm('Bạn có thật sự muốn xóa phiếu nhập này?');"/>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width:75% ; vertical-align:top; text-align:center; " class="classtd">
                <div style="overflow-x:auto;">
                <asp:gridview  CssClass="gridview" runat="server" ID="grvPhieunhap" DataKeyNames="PK_iMaphieunhap" AutoGenerateColumns="false"
                      OnRowDataBound="grvPhieunhap_RowDataBound" OnSelectedIndexChanged="grvPhieunhap_SelectedIndexChanged"
                     ShowHeaderWhenEmpty="true"  >
                    
                    <Columns>
                        <asp:TemplateField HeaderText="Tên nhà cung cấp"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" >
                            <ItemTemplate>
                                <%#Eval("sTenNCC") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên người giao"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%#Eval("sTennguoigiao") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày lập"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <%#Eval("dNgaylap","{0:dd/MM/yyyy hh:mm}") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ghi chú"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="17%" >
                            <ItemTemplate>
                                <%#Eval("sGhichu") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Người lập"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" >
                            <ItemTemplate>
                                <asp:label id="lblnguoilap" runat="server" Text=' <%#Eval("sHovaten") %> '></asp:label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trạng thái" ItemStyle-Font-Size="80%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                            <ItemTemplate>
                                <asp:label id="trangthai" runat="server" Text=' <%#Eval("iTrangthai") %> '></asp:label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chi tiết" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="3%" >
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbtnChiTiet"   CommandArgument='<%# Eval("PK_iMaphieunhap") %>' runat="server" ToolTip="Chi tiết" OnClick="lkbtnChiTiet_Click"  >
                                            <img src="/images/detail.png" border=0 />
                                        </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate>Không tìm thấy dữ liệu</EmptyDataTemplate>
                </asp:gridview>
                </div>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    <div id="paging" style="float:right; width:100%;">
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
    </form>
</asp:Content>
