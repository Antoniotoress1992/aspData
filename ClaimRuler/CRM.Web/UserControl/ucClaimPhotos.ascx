<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimPhotos.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimPhotos" %>
<div class="message_area">
    <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
</div>

<asp:Button ID="btnGridBind" OnClick="btnGridBind_Click" runat="server" Style="display: none" />

<asp:DataList ID="dtlist" runat="server"
    class="layout"
    CellPadding="15"
    DataKeyField="ClaimImageID"
    OnItemCommand="dtlist_ItemCommand"
    RepeatColumns="5"
    OnItemDataBound="dtlist_ItemDataBound">
    <ItemTemplate>
        <table style="width: 100%; padding: 3px;">
            <tr>
                <td style="width: 33%; text-align: left;">
                    <asp:CheckBox ID="cbxPrint" runat="server" Checked='<%# Eval("IsPrint")%>' ToolTip="Include in report/export" />
                </td>
                <td style="width: 33%; text-align: center">
                    <asp:ImageButton ID="btnRotate" Width="15" runat="server"
                        CommandArgument='<%#Eval("ClaimImageID") %>'
                        ImageUrl="~/Images/rotate.PNG"
                        CommandName="DoRotate"
                        ToolTip="Rotate"
                        />
                </td>
                <td class="right">
                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Remove" CommandName="DoDelete"
                        ImageUrl="~/Images/delete_icon.png"                       
                        CommandArgument='<%#Eval("ClaimImageID") %>'
                        OnClientClick="javascript:return ConfirmDialog(this, 'Do you want to delete this photo?');" />
                </td>
            </tr>
        </table>
        <div style="width: 100%; float: left; vertical-align: top;">
            <div style="width: 160px;">
                <a href="#" onclick="claimPhotoDescription(<%#Eval("ClaimImageID") %>)">
                    <div class="divcontainer" style="width: 160px; vertical-align: top">
                        <asp:Image ID="Image1" Height="150px" Width="150px" runat="server" CssClass="uimage" />
                    </div>
                    <div>
                        <%# Eval("Location") %>
                    </div>
                    <div>
                        <%# Eval("Description") %>
                    </div>
                </a>
            </div>
            <div style="color: #555; font-weight: bold; font-family: Arial; font-size: 10px; width: 150px; height: 25px; float: left;">
            </div>
        </div>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
</asp:DataList>
<table id="t1" runat="server" align="center">
    <tr>
        <td>
            <asp:ImageButton ID="btnfirst" runat="server" Font-Bold="true" ToolTip="First" ImageUrl="~/Images/nav-left-x2.gif"
                Height="15px" Width="20px" OnClick="btnfirst_Click" />
        </td>
        <td>
            <asp:ImageButton ID="btnprevious" runat="server" ToolTip="Previous" ImageUrl="~/Images/nav-left.gif"
                Font-Bold="true" Height="15px" Width="20px" OnClick="btnprevious_Click" />
        </td>
        <td>
            <asp:Label ID="lblPageCount" runat="server"></asp:Label>
        </td>
        <td>
            <asp:ImageButton ID="btnnext" ImageUrl="~/Images/nav-right.gif" ToolTip="Next" runat="server"
                Font-Bold="true" Height="15px" Width="20px" OnClick="btnnext_Click" />
        </td>
        <td>
            <asp:ImageButton ID="btnlast" ImageUrl="~/Images/nav-right-x2.gif" ToolTip="Last"
                runat="server" Font-Bold="true" Height="15px" Width="20px" OnClick="btnlast_Click" />
        </td>
    </tr>
</table>
<script type="text/javascript">

    function RebindImages() {
        // post page
        $("#<%=btnGridBind.ClientID %>").click();
    }

</script>
