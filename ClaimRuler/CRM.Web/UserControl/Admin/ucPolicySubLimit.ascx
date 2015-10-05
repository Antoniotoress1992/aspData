<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPolicySubLimit.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucPolicySubLimit" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>



<asp:GridView Width="100%" ID="gvLimits"
    CssClass="gridView no_min_width"
    runat="server"
    AutoGenerateColumns="False"
    CellPadding="2"
    DataKeyNames="PolicySublimitID, LimitType"
    HorizontalAlign="Center"
    OnRowCommand="gvLimits_RowCommand"
    >
    <RowStyle HorizontalAlign="Center" />
    <Columns>
        <asp:TemplateField>
            <ItemStyle Width="20px" />
            <ItemTemplate>
                <asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this limit?');"
                    CommandArgument='<%# string.Format("{0},{1}", Eval("PolicySublimitID"),Eval("PolicyID")) %>' 
                    ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" 
                     Visible='<%# !Eval("PolicySublimitID").Equals(0) %>'/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Coverage">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>.&nbsp;
                <ig:WebTextEditor ID="txtCoverage" runat="server" Width="90%" Value='<%# Bind("SublimitDescription") %>'>
                </ig:WebTextEditor>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Sub-Limit ($ or % (Ex: .10))">
            <ItemStyle Width="20%" />
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtSublimit" runat="server" Width="100px" Value='<%# Bind("Sublimit") %>' MinDecimalPlaces="2">
                </ig:WebNumericEditor>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
