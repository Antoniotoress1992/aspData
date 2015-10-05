<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadImages.ascx.cs" Inherits="CRM.Web.UserControl.Admin.UploadImages" %>

<%@ Register Assembly="Infragistics45.Web.jQuery.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>


<style type="text/css">
    .uimage {
        vertical-align: top;
    }

    table.layout {
        /*height:100%;
      width: 100%;
      border-collapse:collapse;*/
    }

        table.layout td {
            /*padding:3px;
	    border:solid 1px #d4d4d4;
	    border-collapse:collapse;*/
            vertical-align: top;
        }


    .ModalPopupBG {
        background-color: #666699;
        filter: alpha(opacity=50);
        opacity: 0.7;
    }

    .myinput1 {
        background-color: #ffffff;
        border: 2px solid #184687;
        -moz-border-radius: 2px;
        -webkit-border-radius: 2px;
        border-radius: 2px;
        color: #000000;
        border-color: #184687;
        width: 300px;
        padding: 5px;
        outline: none;
    }

    .HellowWorldPopup {
        min-width: 800px;
        min-height: 500px;
        background: white;
        border: 3px solid #184687;
    }

    .enablebutton {
        color: #000000;
        font-family: "Trebuchet MS", arial, verdana, sans-serif;
        font-size: 13px;
        border: 3px solid #CCC;
        background-color: #CCC;
        ;
    }

    .disabledbutton {
        color: #FFFFFF;
        font-family: arial, verdana, sans-serif;
        font-size: 12px;
        border: 1px solid #f0eed9;
        ;
        background-color: #cccccc;
    }
</style>


<div class="message_area">
    <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
    <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
    <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
</div>




    <asp:UpdatePanel runat="server" ID="updatePanel2">
        <ContentTemplate>
            <div style="height: 500px; overflow: auto">
                <asp:DataList ID="dtlist" runat="server" OnItemCommand="dtlist_ItemCommand" DataKeyField="LeadImageId"
                    RepeatColumns="5" CellPadding="15" OnItemDataBound="dtlist_ItemDataBound" class="layout">
                    <ItemTemplate>
                        <table style="width: 100%; padding: 3px;">
                            <tr>
                                <td style="width: 33%; text-align: left;">
                                    <asp:CheckBox ID="cbxPrint" runat="server" Checked='<%# Eval("isPrint")%>' ToolTip="Include in report/export" />
                                </td>
                                <td style="width: 33%; text-align: center">
                                    <asp:ImageButton ID="btnRotate" Width="15" runat="server" Visible='<%# (Session["View"] != null && Convert.ToString(Session["View"]).Length > 0 && Convert.ToInt32(Session["View"]) == 1) ? false : true %>'
                                        CommandArgument='<%#Eval("LeadImageId") %>' ImageUrl="~/Images/rotate.PNG" CommandName="DoRotate"
                                        ToolTip="Rotate" />
                                </td>
                                <td class="right">
                                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Remove" CommandName="DoDelete"
                                        ImageUrl="~/Images/delete_icon.png" Visible='<%# (Session["View"] != null && Convert.ToString(Session["View"]).Length > 0 && Convert.ToInt32(Session["View"]) == 1) ? false : true %>'
                                        CommandArgument='<%#Eval("LeadImageId") %>' OnClientClick="javascript:return ConfirmDialog(this, 'Confirmation', 'Do you want to delete this image ?');" />
                                </td>
                            </tr>
                        </table>
                        <div style="width: 100%; float: left; vertical-align: top;">
                            <div style="width: 160px;">
                                <a href="#" onclick="leadPhotoDescription(<%#Eval("LeadImageId") %>);">
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
                        <br />
                        <asp:HiddenField ID="hfLeadId" runat="server" Value='<%#Eval("LeadId") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                </asp:DataList>
                <table id="t1" runat="server" align="center">
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnfirst" runat="server" Font-Bold="true" ToolTip="First" ImageUrl="~/Images/nav-left-x2.gif"
                                Height="15px" Width="20px" OnClick="btnfirst_Click" />
                        </td>
                        <%--ImageUrl="~/Images/nav-left-x2.gif"--%>
                        <td>
                            <asp:ImageButton ID="btnprevious" runat="server" ToolTip="Previous" ImageUrl="~/Images/nav-left.gif"
                                Font-Bold="true" Height="15px" Width="20px" OnClick="btnprevious_Click" />
                        </td>
                        <td>
                            <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                        </td>
                        <%--ImageUrl="~/Images/nav-left.gif"--%>
                        <td>
                            <asp:ImageButton ID="btnnext" ImageUrl="~/Images/nav-right.gif" ToolTip="Next" runat="server"
                                Font-Bold="true" Height="15px" Width="20px" OnClick="btnnext_Click" />
                        </td>
                        <%--ImageUrl="~/Images/nav-right.gif"--%>
                        <td>
                            <asp:ImageButton ID="btnlast" ImageUrl="~/Images/nav-right-x2.gif" ToolTip="Last"
                                runat="server" Font-Bold="true" Height="15px" Width="20px" OnClick="btnlast_Click" />
                        </td>
                        <%--ImageUrl="~/Images/nav-right-x2.gif"--%>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnfirst" />
            <asp:AsyncPostBackTrigger ControlID="btnprevious" />
            <asp:AsyncPostBackTrigger ControlID="btnnext" />
            <asp:AsyncPostBackTrigger ControlID="btnlast" />
            <asp:PostBackTrigger ControlID="dtlist" />
        </Triggers>
    </asp:UpdatePanel>


