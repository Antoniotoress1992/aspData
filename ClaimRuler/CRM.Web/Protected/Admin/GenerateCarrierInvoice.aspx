<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="GenerateCarrierInvoice.aspx.cs" Inherits="CRM.Web.Protected.Admin.GenerateCarrierInvoice" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Src="~/UserControl/Admin/ucCarrierList.ascx" TagName="ucCarrierList" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Generate Invoice
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <ig:WebSplitter ID="WebSplitter1" runat="server" Width="100%" Height="650px">
                    <Panes>
                        <ig:SplitterPane runat="server" Size="13%" CollapsedDirection="PreviousPane">
                            <Template>
                                <div class="section-title">
                                    Filters
                                </div>
                                <table style="border-collapse: separate; border-spacing: 3px; padding: 2px; width: 100%;" border="0">
                                    <tr>
                                        <td>
                                            <div style="float: left;">
                                            </div>
                                            <div style="float: right;">
                                                <asp:LinkButton ID="lbtnClear" runat="server" Text="Clear" OnClick="lbtnClear_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Invoice Mode
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlInvoiceMode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceMode_SelectedIndexChanged">
                                                <asp:ListItem Text="--- Select ---" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Automatic" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Manual" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </Template>
                        </ig:SplitterPane>
                        <ig:SplitterPane runat="server">
                            <Template>
                                <asp:Panel ID="pnlToolbar" runat="server" Visible="false">
                                    <div class="toolbar toolbar-body">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btnGenerate" runat="server" CssClass="toolbar-item" OnClick="btnGenerate_Click">
									                    <span class="toolbar-img-and-text" style="background-image: url(../../images/invoice.png)">Generate</span>
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnRefesh" runat="server" CssClass="toolbar-item" OnClick="btnRefresh_Click">
									                    <span class="toolbar-img-and-text" style="background-image: url(../../images/rerfesh.png)">Refresh</span>
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                                <div class="paneContentInner">
                                    <div class="message_area">
                                        <asp:Label ID="lblMessage" runat="server" />
                                    </div>
                                    <asp:Panel ID="pnlManualInvoice" runat="server" Visible="false">
                                        <table style="border-collapse: separate; border-spacing: 3px; padding: 2px; width: 100%;" border="0" class="editForm">
                                            <tr>
                                                <td class="left" style="width: 10%;">Select Carrier</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCarrier" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCarrier_SelectedIndexChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="left">Invoice Profile</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlInvoiceProfile" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceProfile_SelectedIndexChanged" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <div class="section-title">Claim File Ready for Invoice</div>
                                                    <asp:GridView ID="gvCarrierPolicy" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="gridView"
                                                        DataKeyNames="ID, LeadId, PolicyType"
                                                        HorizontalAlign="Left" CellPadding="2" OnRowDataBound="gvCarrierPolicy_RowDataBound">
                                                        <RowStyle HorizontalAlign="Center" />
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cbxSelect" runat="server" Checked="true" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Policy Type">
                                                                <ItemTemplate>
                                                                    <%#Eval("LeadPolicyType.Description") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Policyholder">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPolicyHolder" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Net Claim Payable">
                                                                <ItemTemplate>
                                                                    <%# Eval("NetClaimPayable", "{0:N2}") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceAmount" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Loss Date">
                                                                <ItemTemplate>
                                                                    <%# Eval("LossDate", "{0:MM/dd/yyyy}") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Claim Number">
                                                                <ItemTemplate>
                                                                    <%#Eval("ClaimNumber") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Policy Number">
                                                                <ItemTemplate>
                                                                    <%#Eval("PolicyNumber") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Policy Period">
                                                                <ItemTemplate>
                                                                    <%#Eval("PolicyPeriod") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                            </Template>
                        </ig:SplitterPane>
                    </Panes>
                </ig:WebSplitter>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
