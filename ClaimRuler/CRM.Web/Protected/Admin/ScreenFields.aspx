<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="ScreenFields.aspx.cs" Inherits="CRM.Web.Protected.Admin.ScreenFields" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div class="paneContent">
                <div class="page-title">
                    Screen Fields Template
                </div>
                <div class="toolbar toolbar-body">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnSaveScreenFields" runat="server" CssClass="toolbar-item" OnClick="btnSaveScreenFields_Click">
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

                    <div class="center" style="margin: auto">
                        <asp:Panel ID="pnlSelectScreen" runat="server">
                            <div style="margin: auto; width: 100%; margin-bottom: 20px;">
                                Screen Name &nbsp;<asp:DropDownList ID="ddlScreens" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlScreens_SelectedIndexChanged" />
                            </div>
                        </asp:Panel>
                        <asp:GridView ID="gvFields" runat="server"
                            AutoGenerateColumns="false"
                            CellPadding="3"
                            CssClass="gridView"
                            HorizontalAlign="Center"
                            ShowFooter="false"
                            Width="50%">
                            <Columns>
                                <asp:TemplateField HeaderText="Show">
                                    <ItemStyle Width="30px" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbxSelected" runat="server" Checked='<%# Eval("IsVisible") %>' />
                                        <asp:HiddenField ID="hf_templateID" runat="server" Value='<%# Eval("TemplateID") %>' />
                                        <asp:HiddenField ID="hf_formID" runat="server" Value='<%# Eval("FormID") %>' />
                                        <asp:HiddenField ID="hf_fieldID" runat="server" Value='<%# Eval("FieldID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="FieldPrompt" HeaderText="Field Name" />
                            </Columns>
                        </asp:GridView>
                    </div>


                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
