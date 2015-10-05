<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="SubLimitOfLiability.aspx.cs" Inherits="CRM.Web.Protected.Admin.SubLimitOfLiability" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

    <div class="paneContent">
        <div class="page-title">
            Sub-Limits of Liability
        </div>
        <div class="paneContentInner">
            <div class="message_area">
                <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
            </div>
            <div style="text-align: center; margin-bottom: 10px; margin-top: 10px;">
                Sub-Limit of Liability
			    <asp:TextBox ID="txtSublimit" MaxLength="100" runat="server" Width="350px"></asp:TextBox>
                &nbsp;
                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click"
                    ValidationGroup="SubLimit" />
                <div>
                    <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtSublimit"
                        ErrorMessage="Please enter sub-limit description." ValidationGroup="SubLimit" Display="Dynamic"
                        CssClass="validation1" />
                </div>
            </div>
            <asp:GridView ID="gvSublimit" CssClass="gridView" OnRowCommand="gv_RowCommand" Width="80%" HorizontalAlign="Center"
                runat="server" AutoGenerateColumns="False" CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff"
                AllowPaging="true" PageSize="18" OnPageIndexChanging="gv_PageIndexChanging"
                PagerSettings-PageButtonCount="5" PagerStyle-Font-Bold="true"
                PagerStyle-Font-Size="9pt">
                <PagerStyle CssClass="pager" />
                <RowStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    No sub-limit of liability available.
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex+1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sub-Limit">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgEdit" 
                                CommandName="DoEdit" 
                                CommandArgument='<%#Eval("SublimitLiabilityID") %>'
                                ToolTip="Edit" 
                                ImageUrl="~/Images/edit_icon.png"
                                Visible='<%# Convert.ToBoolean(Master.hasEditPermission) %>' />
                            &nbsp;
						<asp:ImageButton runat="server" ID="imgDelete" 
                            title="Are you sure you want to delete this record?"
                            CommandName="DoDelete" 
                            CommandArgument='<%#Eval("SublimitLiabilityID") %>' 
                            ToolTip="Delete"
                            ImageUrl="~/Images/delete_icon.png"
                            Visible='<%# Convert.ToBoolean(Master.hasDeletePermission) %>'
                            OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this sub-limit?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
    <asp:HiddenField ID="hfStatus" runat="server" Value="0" />
    <asp:HiddenField ID="hdId" runat="server" Value="0" />
</asp:Content>
