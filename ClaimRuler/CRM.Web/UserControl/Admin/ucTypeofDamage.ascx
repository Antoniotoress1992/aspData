<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTypeofDamage.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucTypeofDamage" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div class="message_area">
    <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
    <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
    <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
</div>

<table style="border-collapse: separate; border-spacing: 3px; padding: 2px; width: 60%;" border="0">
    <tr>
        <td class="right">Type of Damage
        </td>
        <td>
            <ig:WebTextEditor ID="txtTypeofDamage" MaxLength="100" runat="server" Width="400px" />
            <div>
                <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtTypeofDamage"
                    ErrorMessage="Please Enter Type of Damage" ValidationGroup="TypeofDamage" Display="Dynamic"
                    CssClass="validation1" />
            </div>
        </td>
    </tr>
    <tr>
        <td class="right">Sort</td>
        <td>
            <ig:WebTextEditor ID="txtSort" runat="server" MaxLength="2" TextMode="Number" />
        </td>
    </tr>
    <tr>
        <td></td>
        <td class="left">
            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click" CausesValidation="true"
                ValidationGroup="TypeofDamage" />
            &nbsp;
            <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
                CssClass="mysubmit" OnClick="btnCancel_Click" />
        </td>
    </tr>
</table>

<div class="vendor_list" id="dvData" runat="server">
    <asp:ListView runat="server" ID="lvData" ItemPlaceholderID="itemPlaceHolder1" OnItemCommand="lvData_ItemCommand"
        DataKeyNames="TypeofDamageId">
        <LayoutTemplate>
            <table style="border-collapse: separate; border-spacing: 3px; padding: 2px; width: 80%; text-align: center;" border="0">
                <tr style="background: #c9dffb">
                    <td style='width: 5%;'>
                        <strong>S.No.</strong>
                    </td>
                    <td style='width: 70%;'>
                        <strong>Type of Damage</strong>
                    </td>
                    <td style='width: 10%;'>
                        <strong>Sort</strong>
                    </td>
                    <td style='width: 5%;'><b>Hide</b></td>
                    <td style="width: 5%;">&nbsp;
                    </td>

                </tr>
                <tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr style='color: black; background: #FFFFFF'>
                <td class="center">
                    <%# Container.DataItemIndex+1%>
                </td>
                <td>
                    <asp:Label ID="lblTypeofDamage" runat="server" Text='<%#Eval("TypeofDamage")%>'></asp:Label>&nbsp;&nbsp;
                </td>
                <td class="center">
                    <asp:Label ID="lblSort" runat="server" Text='<%#Eval("Sort")%>'></asp:Label>&nbsp;&nbsp;
                </td>
                <td>
                    <asp:CheckBox ID="cbxHide" runat="server" OnCheckedChanged="cbxHidden_CheckedChanged" AutoPostBack="true"
                        Checked='<%# Eval("IsHidden") == null ? false : Eval("IsHidden") %>' />

                </td>
                <td class="center">
                    <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("TypeofDamageId") %>'
                        ToolTip="Edit"
                        ImageUrl="~/Images/edit_icon.png"
                        Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>' />
                    &nbsp;                        
                    <asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
                        CommandName="DoDelete" CommandArgument='<%#Eval("TypeofDamageId") %>'
                        ToolTip="Delete"
                        ImageUrl="~/Images/delete_icon.png"
                        Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>'
                        OnClientClick="javascript:return ConfirmDialog(this,  'Do you want to delete this record ?');"
                       />
                </td>

            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style='color: black; background: #e8f2ff'>
                <td class="center">
                    <%# Container.DataItemIndex+1%>
                </td>
                <td style="">
                    <asp:Label ID="lblTypeofDamage" runat="server" Text='<%#Eval("TypeofDamage")%>'></asp:Label>&nbsp;&nbsp;
                </td>
                <td class="center">
                    <asp:Label ID="lblSort" runat="server" Text='<%#Eval("Sort")%>'></asp:Label>&nbsp;&nbsp;
                </td>
                <td>
                    <asp:CheckBox ID="cbxHide" runat="server" OnCheckedChanged="cbxHidden_CheckedChanged" AutoPostBack="true"
                        Checked='<%# Eval("IsHidden") == null ? false : Eval("IsHidden") %>' />

                </td>
                <td class="center">
                    <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("TypeofDamageId") %>'
                        ToolTip="Edit" ImageUrl="~/Images/edit_icon.png"
                        Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>' />
                    &nbsp;
                    <asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
                        CommandName="DoDelete" CommandArgument='<%#Eval("TypeofDamageId") %>' ToolTip="Delete"
                        ImageUrl="~/Images/delete_icon.png"
                        Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>'
                        OnClientClick="javascript:return ConfirmDialog(this, 'Confirmation', 'Do you want to delete this record ?');" />
                </td>

            </tr>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>
            <tr>
                <td colspan="5">
                    <div style="padding-top: 10px; padding-bottom: 10px;">
                        <asp:Label ID="lblRecordNotFound" runat="server" CssClass="info" Text="Records Not Found !!!" />
                    </div>
                </td>
            </tr>
        </EmptyDataTemplate>
    </asp:ListView>

</div>
<div class="paneContentInner">
    <div class="pager">
        <asp:DataPager ID="PagerRow" Visible="true" PageSize="25" runat="server" PagedControlID="lvData"
            OnPreRender="lvData_PreRender">
            <Fields>
                <asp:NumericPagerField ButtonCount="5" NextPageText=">>" PreviousPageText="<<" CurrentPageLabelCssClass="PagerCurrent"
                    NextPreviousButtonCssClass="PagerNormal" NumericButtonCssClass="PagerNormal" />
            </Fields>
        </asp:DataPager>

    </div>
</div>
<asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
<asp:HiddenField ID="hfStatus" runat="server" Value="0" />
<asp:HiddenField ID="hdId" runat="server" Value="0" />
