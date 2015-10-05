<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucServiceList.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucServiceList" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>



<div class="paneContentInner">
    <div class="message_area">
        <asp:Label ID="lblMessage" runat="server" />
    </div>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false" DefaultButton="btnSave">

        <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm no_min_width">
            <tr>
                <td class="right">Service Description
                </td>
                <td style="width: 5px;" class="redstar">*</td>
                <td>
                    <ig:WebTextEditor ID="txtServiceDescription" MaxLength="100" runat="server" Width="300px" />
                    <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtServiceDescription"
                        ErrorMessage="Please enter service description" ValidationGroup="service" Display="Dynamic"
                        CssClass="validation1" />
                </td>
            </tr>
            <tr>
                <td class="right middle">
                    <br />
                    Service Rate
                </td>
                <td class="redstar"></td>
                <td>
                    <table>
                        <tr>
                            <td class="center">Amount ($)</td>
                            <td class="redstar"></td>
                            <td class="center">Percent (%)</td>
                        </tr>
                        <tr>
                            <td class="left">
                                <ig:WebNumericEditor ID="txtRate" runat="server" Width="80px" DataMode="Decimal" MinDecimalPlaces="2"></ig:WebNumericEditor>
                            </td>
                            <td class="center">OR</td>
                            <td class="left">
                                <ig:WebNumericEditor ID="txtPercentage" runat="server" Width="80px" DataMode="Decimal" MinDecimalPlaces="2"></ig:WebNumericEditor>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td class="right">Minimum Fee</td>
                <td></td>
                <td>
                    <ig:WebNumericEditor ID="txtMinimumFee" runat="server" Width="80px" DataMode="Decimal" MinDecimalPlaces="2"></ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td class="right">Default Quantity</td>
                <td></td>
                <td>
                    <ig:WebNumericEditor ID="txtDefaultQty" runat="server" Width="80px" DataMode="Decimal" MinDecimalPlaces="2"></ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td class="right">Is Taxable?</td>
                <td></td>
                <td>
                    <asp:CheckBox ID="cbxIsTaxable" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="right">Earning Code</td>
                <td></td>
                <td>
                    <ig:WebTextEditor ID="txtEarningCode" runat="server" MaxLength="20" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: center;">
                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click" CausesValidation="true"
                        ValidationGroup="service" />
                    &nbsp;
								<asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
                                    CssClass="mysubmit" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>

    </asp:Panel>
    <asp:Panel ID="pnlGrid" runat="server">
        <asp:LinkButton ID="btnNew" runat="server" CssClass="link" Text="New Service" OnClick="btnNew_Click" />
        <asp:GridView ID="gvServices" Width="100%" runat="server"
            AllowSorting="true"
            AutoGenerateColumns="False"
            AlternatingRowStyle-BackColor="#e8f2ff"
            CellPadding="4"
            CssClass="gridView"
            OnRowCommand="gvServices_RowCommand"
            OnRowDataBound="gvServices_RowDataBound"
            OnSorting="gv_onSorting"
            HorizontalAlign="Center"
            RowStyle-HorizontalAlign="Center"
            PagerSettings-PageButtonCount="5">
            <PagerStyle CssClass="pager" />
            <RowStyle HorizontalAlign="Center" />
            <EmptyDataTemplate>
                No services available.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
                    ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server"
                            CommandName="DoEdit"
                            CommandArgument='<%#Eval("ServiceTypeID") %>'
                            ToolTip="Edit"
                            ImageUrl="~/Images/edit.png"
                            Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>'></asp:ImageButton>
                        &nbsp;
							<asp:ImageButton ID="btnDelete" runat="server"
                                CommandName="DoDelete"
                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this service?');"
                                CommandArgument='<%#Eval("ServiceTypeID") %>'
                                ToolTip="Delete"
                                ImageUrl="~/Images/delete_icon.png"
                                Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>'></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Service Description"
                    SortExpression="ServiceDescription" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("ServiceDescription") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Earning Code"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("EarningCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle Width="100px" />
                    <ItemTemplate>
                        <%# Eval("ServiceRate", "${0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Percentage" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle Width="100px" />
                    <ItemTemplate>
                        <%# Eval("ServicePercentage", "{0:N2} %")%>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </asp:Panel>
</div>

<asp:HiddenField ID="hfId" runat="server" Value="0" />
