<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeadImportEmail.aspx.cs"
    MasterPageFile="~/Protected/ClaimRulerClaim.master" Inherits="CRM.Web.Protected.LeadImportEmail" %>

<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>


<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    Import Email
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">
    <td>
        <a class="toolbar-item" href="javascript:importEmail();">
            <span class="toolbar-img-and-text" style="background-image: url(../images/add.png)">Import</span>
        </a>
    </td>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnImportEmail_hidden" runat="server" Style="display: none;" OnClick="btnImportEmail_Click" />
            <div class="message_area">
                <asp:Label ID="lblMessage" runat="server" />
            </div>
            <table style="width: 100%;">        
                <tr>
                    <td class="top left" style="width: 250px;">
                        <div class="boxContainer">
                            <div class="section-title">Import Instructions</div>

                            <div class="paneContentInner instructions">
                                <p>
                                    Please follow the instructions below when importing emails.
                                </p>
                                <div class="paneContentInner">
                                    <ul>
                                        <li>1. Create a folder named "export" (use lower case) in your email account.</li>
                                        <li>2. Move into the "export" folder the emails you wish to import.</li>
                                        <li>3. Mark all emails inside "export" as unread.</li>
                                        <li>4. Select checkbox for each email to be imported.</li>
                                    </ul>
                                </div>
                            </div>

                        </div>
                    </td>
                    <td class="top left">
                        <div class="boxContainer">
                            <div class="section-title">
                                <asp:Label ID="lblEmail" runat="server" />
                            </div>
                            <asp:GridView ID="gvMails" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="gridView" HorizontalAlign="Center"
                                CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff"
                                PageSize="10" RowStyle-HorizontalAlign="Center" PagerSettings-PageButtonCount="5"
                                PagerStyle-Font-Bold="true" PagerStyle-Font-Size="9pt">
                                <PagerStyle CssClass="pager" />
                                <RowStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                <EmptyDataTemplate>
                                    No emails available.
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbxImport" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="ReceivedDate" runat="server" Text='<%# Eval("ReceivedDate", "{0:MM/dd/yyyy HH:mm:ss tt}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Author">
                                        <ItemTemplate>
                                            <div>
                                                <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval("From") %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Title/Summary" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <div>
                                                <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject") %>' />
                                                &nbsp;&nbsp;
                                                <a href="javascript:showEmailDetails('<%# Container.DataItemIndex + 1 %>');">
                                                    <span id="txt_moreless_<%# Container.DataItemIndex + 1 %>">More...</span>
                                                </a>
                                            </div>
                                            <div id="div_comment_<%# Container.DataItemIndex + 1 %>" style="display: none;">
                                                <ighedit:WebHtmlEditor ID="txtContents" runat="server" Text='<%# Server.HtmlDecode(Eval("TextBody").ToString()) %>' ReadOnly="true" Width="99%" TabStripDisplay="false">
                                                </ighedit:WebHtmlEditor>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function importEmail() {
            $("#<%= btnImportEmail_hidden.ClientID %>").click();
        }

        function showEmailDetails(id) {
            var divControl = '#div_comment_' + id;
            var txt_moreless = "#txt_moreless_" + id;

            var css = $(divControl).css('display');

            if (css == 'none') {
                $(divControl).show();
                $(txt_moreless).text("Less...");
            } else {
                $(divControl).hide();
                $(txt_moreless).text("More...");
            }
        }
    </script>
</asp:Content>
