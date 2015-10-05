<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimList.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucClaimList" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:GridView ID="gvClaims" runat="server"
    AllowSorting="false"
    AutoGenerateColumns="False"
    Width="100%"
    CellPadding="4"
    DataKeyNames="ClaimID"
    CssClass="gridView"
    HorizontalAlign="Center"
    OnRowCommand="gvClaims_RowCommand">
    <RowStyle HorizontalAlign="Center" />
    <Columns>
        <asp:TemplateField ItemStyle-Width="20px" ItemStyle-Wrap="false">
            <ItemTemplate>
                <asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%# string.Format("{0}|{1}", Eval("ClaimID"), Eval("PolicyID")) %>'
                    ToolTip="Edit" ImageUrl="~/Images/edit_icon.png" />
                &nbsp;
                <asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this claim?');"
                    CommandArgument='<%#Eval("ClaimID") %>'
                    ToolTip="Delete claim" ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="AdjusterClaimNumber" HeaderText="Adjuster File #" />
        <asp:BoundField DataField="LossDate" HeaderText="Date of Loss" DataFormatString="{0:MM/dd/yyyy}" />
        <asp:BoundField DataField="DateOpenedReported" HeaderText="Date Open/Reported" DataFormatString="{0:MM/dd/yyyy}" />
        <asp:BoundField DataField="DateClosed" HeaderText="Date Closed" DataFormatString="{0:MM/dd/yyyy}" />
        <asp:BoundField DataField="EventType" HeaderText="Event Type" />
        <asp:BoundField DataField="EventName" HeaderText="Event Name" />
        <asp:BoundField DataField="SeverityNumber" HeaderText="Severity" />
        <asp:BoundField DataField="CycleTime" HeaderText="Cycle Time" DataFormatString="{0:###}" />
        <asp:BoundField DataField="ReopenCycleTime" HeaderText="Reopen Cycle Time" DataFormatString="{0:###}" />
        <asp:TemplateField HeaderText="Adjuster">
            <ItemTemplate>
                <%# Eval("AdjusterMaster.adjusterName")  %>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>   
</asp:GridView>
