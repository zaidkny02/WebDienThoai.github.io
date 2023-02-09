<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ThemMoiSP.aspx.cs" Inherits="TestUserSQL.WebForm11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <style>
        img {
            max-height:500px; width:100%;
        }
        @media only screen and (max-width: 768px) {
            img {
                height:80%;
                width:auto;
                
            }
            table {
                margin-left:auto;
                margin-right:auto;
            }
                tr:first-child {
                    text-align:center;
                }
        }
    </style>
    <script type="text/javascript">
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
        function Check() {
            var tensanpham = document.getElementById('ContentPlaceHolder1_txtTenSP').value;
            var giaban = document.getElementById('ContentPlaceHolder1_txtGiaban').value;
            var soluong = document.getElementById('ContentPlaceHolder1_txtSoluong').value;
            var url = document.getElementById('ContentPlaceHolder1_fileImport');
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
            if (url.files.length == 0) {
                alert("Không được để trống hình ảnh");
                return false;
            }
            return true;
        }
    </script>
    <style>
        #form1 {
            min-height:450px;
        }
    </style>
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
            <table style="width:100%;" class="input_grid_table">
                <tr>

                    <td style="width:40%; vertical-align:top;" class="classtd">
                        <asp:Image ID="Image1" runat="server"   ImageUrl="#" />
                    </td>
                    <td style="width:30%; vertical-align:top;" class="classtd">
                        <span style="color:red"><asp:Literal  runat="server" ID="lblThongBaoLoi"></asp:Literal></span>
                        <table class="input_table">
                            <tr>
                                <td>Hình ảnh</td>
                                <td><asp:FileUpload runat="server" ID="fileImport" onchange="ShowPreview(this)" Width="100%" /></td>
                            </tr>
                            <tr>
                                <td>Tên sản phẩm</td>
                                <td><asp:TextBox ID="txtTenSP" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Thương hiệu</td>
                                <td><asp:DropDownList ID="ddlThuongHieu" runat="server">
                                </asp:DropDownList></td>
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
                                <asp:TextBox runat="server" ID="txtSoluong" TextMode="Number"  min="1"  step="1"  ></asp:TextBox>
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
                                <td colspan="2" style="text-align:center;">
                                    <asp:Button ID="btnThem" runat="server" OnClick="btnThem_Click" Text="Thêm mới" OnClientClick="return Check();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width:30%; vertical-align:top;" class="classtd">
                        <table class="input_table">
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
                                    <td>Ghi chú thông số</td>
                                    <td>
                                        <asp:TextBox ID="txtGhichu" runat="server"></asp:TextBox>
                                    </td>
                             </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnThem" />
        </Triggers>
        </asp:UpdatePanel>
    </form>
</asp:Content>
