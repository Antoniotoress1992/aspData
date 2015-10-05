<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLeadSource.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucLeadSource" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div class="page-title">
    Lead Sources 
</div>
<div class="toolbar toolbar-body">
    <table>
        <tr>            
            <td>
                <asp:LinkButton ID="btnSend1" runat="server" Text="Return to Claim" CssClass="toolbar-item" OnClick="btnReturnToClaim_Click">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">Return to Claim</span>
                </asp:LinkButton>
            </td>
            <td></td>
        </tr>
    </table>
</div>
<div class="paneContent">

    <div class="message_area">
        <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
        <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
        <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
    </div>
    <div style="text-align: center; margin-top: 10px; margin-bottom: 10px;">
        Lead Source &nbsp;				
		<ig:WebTextEditor ID="txtLeadSource" MaxLength="100" runat="server" Width="300px"/>

        &nbsp;<asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click"
            ValidationGroup="leadSource" />
        &nbsp;<asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
            CssClass="mysubmit" OnClick="btnCancel_Click" />
        <div>
            <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtLeadSource"
                ErrorMessage="Please Enter Lead Source" ValidationGroup="leadSource" Display="Dynamic"
                CssClass="validation1" />
        </div>
    </div>
    <asp:GridView ID="gvSources" CssClass="gridView" Width="50%" runat="server" HorizontalAlign="Center"
        OnRowCommand="gv_RowCommand" AutoGenerateColumns="False" CellPadding="4"
        AlternatingRowStyle-BackColor="#e8f2ff" PageSize="20"
        PagerSettings-PageButtonCount="10"
        OnSorting="gv_onSorting" AllowSorting="true">
        <PagerStyle CssClass="pager" />
        <RowStyle HorizontalAlign="Center" />
        <EmptyDataRowStyle BorderStyle="None" />
        <EmptyDataTemplate>
            No producers available.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="Source Name" SortExpression="LeadSourceName"
                ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%# Eval("LeadSourceName") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle Width="100px" />
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" 
                        CommandArgument='<%#Eval("LeadSourceId") %>'
                        ToolTip="Edit" 
                        ImageUrl="~/Images/edit_icon.png"
                        Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>' 
                        />
						&nbsp;
						<asp:ImageButton runat="server" ID="imgDelete" 
                            title="Are you sure you want to delete this record?"
                            CommandName="DoDelete" 
                            CommandArgument='<%#Eval("LeadSourceId") %>' ToolTip="Delete"                                        
                            ImageUrl="~/Images/delete_icon.png"
                            Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>' 
                            OnClientClick="javascript:return ConfirmDialog(this, 'Confirmation', 'Do you want to delete this source ?');" />

                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>




</div>

<asp:HiddenField ID="hdId" runat="server" Value="0" />
