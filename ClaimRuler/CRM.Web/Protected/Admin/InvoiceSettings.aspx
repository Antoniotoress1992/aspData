<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceSettings.aspx.cs"
    MasterPageFile="~/Protected/ClaimRuler.Master" Inherits="CRM.Web.Protected.Admin.InvoiceSettings" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Invoice Settings 
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <div class="toolbar toolbar-body">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnSave" runat="server" CssClass="toolbar-item" OnClick="btnSave_Click">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/toolbar_save.png)">Save</span>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="message_area">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
                <div class="paneContentInner">

                    <table style="width: 100%;" class="editForm no_min_width">
                        <tr>
                            <td class="top left" style="width: 50%">
                                <div runat="server" class="boxContainer">
                                    <div class="section-title">
                                        Invoice Settings
                                    </div>
                                    <div class="boxContainerInner">
                                        <table style="width: 100%;" class="editForm">
                                            <tr>
                                                <td class="right">Automatic Invoice Method
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAutomaticInvoiceMethod" runat="server">
                                                        <asp:ListItem Text="--- Select ---" Value="0" />
                                                        <asp:ListItem Text="Carrier Fee Schedule Invoicing (IA)" Value="1" />
                                                        <asp:ListItem Text="Loss Percentage Fee Invoicing (PA)" Value="2" />
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="right">Global Loss Percentage Fee (%)
                                                </td>
                                                <td>
                                                    <ig:WebPercentEditor ID="txtLossPercentageFee" runat="server" MinValue="0" MinDecimalPlaces="2" Width="100px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="right">Global Invoice Payment Terms: Net</td>
                                                <td>
                                                    <ig:WebNumericEditor ID="txtInvoicePaymentTerms" runat="server" Width="100px"></ig:WebNumericEditor>
                                                    &nbsp;Days
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>                            
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
