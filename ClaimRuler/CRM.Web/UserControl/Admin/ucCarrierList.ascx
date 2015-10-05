<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCarrierList.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucCarrierList" %>
<div class="toolbar toolbar-body">
    <table>
        <tr>
            <td>
                <asp:LinkButton ID="btnNew" runat="server" Text="" CssClass="toolbar-item" OnClick="btnNew_Click"  Visible='<%# Convert.ToBoolean(masterPage.hasAddPermission) %>'>
						<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Client</span>
                </asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
<asp:GridView ID="gvCarriers" CssClass="gridView"
    AllowPaging="true"
    AllowSorting="true"
    AutoGenerateColumns="False"
    AlternatingRowStyle-BackColor="#e8f2ff"
    CellPadding="2"
    HorizontalAlign="Center"
    OnPageIndexChanging="gvCarriers_PageIndexChanging"
    OnSorting="gvCarriers_Sorting"
    OnRowCommand="gvCarriers_RowCommand"
    PageSize="20"
    Width="80%" runat="server"
    PagerSettings-PageButtonCount="10">
    <PagerStyle CssClass="pager" />
    <RowStyle HorizontalAlign="Center" />
    <HeaderStyle HorizontalAlign="Center" />
    <EmptyDataTemplate>
        No carriers available.
    </EmptyDataTemplate>
    <Columns>
        <asp:TemplateField HeaderText="Client" SortExpression="CarrierName">
            <ItemTemplate>
                <%#Eval("CarrierName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Street Address">
            <ItemTemplate>
                <div>
                    <%#Eval("AddressLine1")%>&nbsp;<%#Eval("AddressLine2")%>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="State" SortExpression="StateName">
            <ItemTemplate>
                <div>
                    <%#Eval("StateName")%>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="City" SortExpression="CityName">
            <ItemTemplate>
                <div>
                    <%#Eval("CityName")%>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Postal Code">
            <ItemTemplate>
                <div>
                    <%#Eval("ZipCode")%>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
            <ItemStyle HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:ImageButton ID="btnEdit" runat="server"
                    CommandName="DoEdit"
                    CommandArgument='<%#Eval("CarrierID") %>'
                    ToolTip="Edit"
                    ImageUrl="~/Images/edit.png"
                    Visible='<%# masterPage.hasEditPermission %>' />
                &nbsp;
				<asp:ImageButton ID="btnCopy" runat="server"
                    CommandName="DoCopy"
                    CommandArgument='<%#Eval("CarrierID") %>'
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to clone this insurance company?');"
                    ToolTip="Copy Carrier"
                    ImageUrl="~/Images/copy.png"
                    Visible='<%# masterPage.hasEditPermission %>' />
                &nbsp;
				<asp:ImageButton ID="btnDelete" runat="server"
                    CommandName="DoDelete"
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this insurance company?');"
                    CommandArgument='<%#Eval("CarrierID") %>'
                    ToolTip="Delete Carrier"
                    ImageUrl="~/Images/delete_icon.png"
                    Visible='<%# masterPage.hasDeletePermission %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
