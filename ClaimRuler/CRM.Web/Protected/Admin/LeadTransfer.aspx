<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Protected/ClaimRuler.Master" CodeBehind="LeadTransfer.aspx.cs" Inherits="CRM.Web.Protected.Admin.LeadTransfer" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Lead/Claim Reassignment
        </div>
        <div class="paneContentInner">
            <asp:UpdatePanel ID="updatePanel" runat="server">
                <ContentTemplate>
                    <div class="message_area">
                        <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                        <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                        <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
                    </div>
                    <asp:Panel ID="pnlSearch" runat="server" Height="100px">
                        <div>
                            Enter insured name to search
                        </div>
                        <div class="search_box" style="margin-top: 10px; margin-bottom: 1px; margin-right: 3px;">
                            <asp:TextBox ID="txtSeach" runat="server" Width="400px" Height="18px" /><asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/Images/search_button_32x32.png"
                                ImageAlign="Top" Width="32px"
                                OnClick="btnSearch_Click" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                        <div style="margin-bottom: 15px; font-size: smaller;">
                            <ul>
                                <li>1. Select user name to be assigned.</li>
                                <li>2. Check box for each lead/claim to be transferred.</li>
                                <li>3. Click "Transfer" button to start transfer.</li>
                            </ul>
                        </div>
                        <div style="margin-left: 5px; margin-bottom: 5px;">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 30%;">Assign To &nbsp;<asp:DropDownList ID="ddlUsers" runat="server" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlUsers"
                                            CssClass="validation1" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please select a user!" InitialValue="0"
                                            ValidationGroup="transferClaim" />
                                    </td>
                                    <td class="left">
                                        <asp:Button ID="btnSave" runat="server" Text="Transfer" class="mysubmit" OnClick="btnSave_click" ValidationGroup="transferClaim" />
                                        &nbsp;
					         <asp:Button ID="btnAnother" runat="server" Text="Search Again" class="mysubmit" OnClick="btnAnotheSearch_click" />
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <asp:GridView ID="gvUserLeads" DataKeyNames="LeadId" ShowFooter="true" Width="99%" HorizontalAlign="Center"
                            runat="server" CssClass="gridView" AutoGenerateColumns="False" CellPadding="4"
                            AlternatingRowStyle-BackColor="#e8f2ff" AllowPaging="false"
                            EmptyDataText="No information available." AllowSorting="true" OnSorting="gvUserLeads_Sorting">
                            <RowStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Wrap="false">

                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbxLead" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Assigned User Name" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("UserName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Name" ItemStyle-Wrap="false" SortExpression="ClaimantLastName">
                                    <ItemTemplate>
                                        <%# Eval("ClaimantLastName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="First Name" SortExpression="ClaimantFirstName">
                                    <ItemTemplate>
                                        <%# Eval("ClaimantFirstName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date Record Created" ItemStyle-Wrap="false" SortExpression="OriginalLeadDate">
                                    <ItemTemplate>
                                        <%# Eval("OriginalLeadDate") == null ? "" : Convert.ToDateTime(Eval("OriginalLeadDate")).ToString("MM-dd-yy") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contract Date" ItemStyle-Wrap="false" SortExpression="ClaimantFirstName">
                                    <ItemTemplate>
                                        <%# Eval("ContractDate") == null ? "" : Convert.ToDateTime(Eval("ContractDate")).ToString("MM-dd-yy")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
