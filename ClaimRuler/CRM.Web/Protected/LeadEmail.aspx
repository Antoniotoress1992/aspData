<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true"
    CodeBehind="LeadEmail.aspx.cs" Inherits="CRM.Web.Protected.LeadEmail" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    Email Policyholder
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">
    <td>
        <a class="toolbar-item" href="javascript:sendEmail();">
            <span class="toolbar-img-and-text" style="background-image: url(../images/mail_send.png)">Send</span>
        </a>
    </td>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnSend_hidden" runat="server" Style="display: none;" ValidationGroup="email" CausesValidation="true" OnClick="btnSend_Click" />
            <div class="paneContentInner">
                <div style="text-align: center; margin-top: 10px; margin-bottom: 10px;">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
                <table style="width: 95%; border-collapse: separate; border-spacing: 2px; padding: 3px;" border="0" class="editForm">
                    <tr>
                        <td style="width: 10%; text-align: right; vertical-align: top;">
                            <igtxt:WebImageButton ID="WebImageButton1" runat="server" Text="Click Here to Add Recipients" AutoSubmit="false" CausesValidation="false">
                                <Appearance ContentShift="None">
                                    <Image Url="~/Images/people_many.png"></Image>
                                </Appearance>
                                <ClientSideEvents Click="showContacts();" />
                            </igtxt:WebImageButton>
                        </td>
                        <td>

                            <asp:TextBox ID="txtEmailTo" runat="server" Width="90%" TextMode="MultiLine" Rows="2" />
                            <div id="divEmailList">
                            </div>
                            <div id="divContactList" style="display: none; width: 90%;">
                                <div class="boxContainer">
                                    <div class="section-title">
                                         <img runat="server" id="img_expand" src="~/images/close.gif" alt="recipients" title="Hide/Show"
                                            style="border-width: 0px; vertical-align: bottom;" onclick="javascript:showHideDiv('divContactList');" /> &nbsp;Recipients
                                    </div>                                  
                                    <ig:WebDataGrid ID="contractGrid" runat="server" CssClass="gridView smallheader" DataSourceID="edsContacts"
                                        AutoGenerateColumns="false" EnableViewState="true" EnableDataViewState="true" Height="300px"
                                        Width="100%">
                                        <Columns>
                                            <ig:BoundDataField DataFieldName="FirstName" Key="FirstName" Header-Text="First Name" />
                                            <ig:BoundDataField DataFieldName="LastName" Key="LastName" Header-Text="Last Name" />
                                            <ig:BoundDataField DataFieldName="CompanyName" Key="CompanyName" Header-Text="Company Name" />
                                            <ig:BoundDataField DataFieldName="Email" Key="Email" Header-Text="Email" />
                                            <ig:BoundDataField DataFieldName="ContactType" Key="ContactType" Header-Text="Contact Type" />
                                        </Columns>
                                        <Behaviors>

                                            <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                                                ThresholdFactor="0.5" Enabled="true" />

                                            <ig:Selection RowSelectType="Multiple" Enabled="True" CellClickAction="Row">
                                                <SelectionClientEvents RowSelectionChanged="contractGrid_rowsSelected" />
                                            </ig:Selection>

                                        </Behaviors>
                                    </ig:WebDataGrid>
                                </div>
                                <asp:EntityDataSource ID="edsContacts" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
                                    EnableFlattening="False" EntitySetName="vw_Contact"
                                    Where="it.ClientId = @ClientID"
                                    OrderBy="it.FirstName, it.LastName Asc">
                                    <WhereParameters>
                                        <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
                                    </WhereParameters>
                                </asp:EntityDataSource>
                            </div>
                            <ajaxToolkit:AutoCompleteExtender ID="txtAutoCompleteforCompany_AutoCompleteExtender"
                                MinimumPrefixLength="3" TargetControlID="txtEmailTo" CompletionSetCount="10"
                                CompletionInterval="100" ServiceMethod="getContactEmail" CompletionListElementID="divEmailList"
                                DelimiterCharacters=";," runat="server">
                            </ajaxToolkit:AutoCompleteExtender>
                            <div>
                                <asp:RequiredFieldValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailTo"
                                    Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please select email recipients."
                                    CssClass="validation1" ValidationGroup="email" />
                            </div>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td style="text-align: right; vertical-align: top;">From:
                        </td>
                        <td>
                            <ig:WebTextEditor ID="txtEmailFrom" runat="server" Width="89%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align: top;">CC:
                        </td>
                        <td>
                            <ig:WebTextEditor ID="txtEmailCC" runat="server" Width="90%" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align: top;">Subject:
                        </td>
                        <td>
                            <ig:WebTextEditor ID="txtEmailSubject" runat="server" Width="90%" TextMode="MultiLine"
                                Rows="2" />
                            <div>
                                <asp:RequiredFieldValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmailSubject"
                                    Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please enter email subject."
                                    CssClass="validation1" ValidationGroup="email" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align: top;"></td>
                        <td style="vertical-align: top;">
                            <div id="divAttachments" style="display: none; width: 90%;">
                                <div class="caption">
                                    <div style="float: left;">
                                        Available Documents
                                    </div>

                                    <div style="float: right; padding-right: 5px; vertical-align: middle;">
                                        <img runat="server" id="img1_attachments" src="~/images/close.gif" alt="attachments" title="Hide/Show"
                                            style="border-width: 0px; vertical-align: bottom;" onclick="javascript:showHideDiv('divAttachments');" />
                                    </div>
                                </div>

                                <asp:ListBox ID="lbxDocuments" runat="server" SelectionMode="Multiple" Width="100%" Height="100px"
                                    DataTextField="DocumentName" DataValueField="DocumentPath"></asp:ListBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; vertical-align: top;">
                            <igtxt:WebImageButton ID="wbAttachments" runat="server" Text=" Docs " AutoSubmit="false" CausesValidation="false">
                                <Appearance ContentShift="None">
                                    <Image Url="~/Images/attachment.png"></Image>
                                </Appearance>
                                <ClientSideEvents Click="showHideDiv('divAttachments');" />
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <ighedit:WebHtmlEditor ID="txtEmailText" runat="server" Width="90%"></ighedit:WebHtmlEditor>

                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <div style="margin-top: 10px;">
                            </div>
                        </td>
                    </tr>


                    <tr>
                        <td style="text-align: right; vertical-align: top;">Signature
                        </td>
                        <td>
                            <ighedit:WebHtmlEditor ID="txtSignature" runat="server" Width="90%" Height="50px">
                            </ighedit:WebHtmlEditor>

                        </td>
                    </tr>
                </table>
            </div>
            <script type="text/javascript">
                function contractGrid_rowsSelected(sender, args) {


                    var selectedEmails = $('#<%= txtEmailTo.ClientID %>').val();

                    var selectedRows = args.getSelectedRows();
                    //for (var i = 0; i < args.getSelectedRows().length; i++) {
                    var email = args.getSelectedRows().getItem(0).get_cell(3).get_text();
                    //alert(email);
                    selectedEmails += email + ";";
                    //}
                    $('#<%= txtEmailTo.ClientID %>').val(selectedEmails);

                    //var billingName = args.getSelectedRows().getItem(0).get_cell(1).get_text();
                    // set row value in ddl
                    //dd.set_currentValue(billingName, true);

                    // get mailing address to show on UI
                    //var mailingAddress = args.getSelectedRows().getItem(0).get_cell(2).get_text();
                    //var cityname = args.getSelectedRows().getItem(0).get_cell(3).get_text();
                    //var statename = args.getSelectedRows().getItem(0).get_cell(4).get_text();
                    //var zipcode = args.getSelectedRows().getItem(0).get_cell(5).get_text();

                  <%--  //var email = sender.get_currentValue();
                    var dropDown = $find('<%=ddlContacts.ClientID %>');
                    var selectedList = dropDown.get_selectedItems();

                    for (var i = 0; i < selectedList.length; i++) {
                        var email = selectedList[i].get_value();
                        if (email != '')
                            selectedEmails += email + ";";
                    }--%>


                }

                function sendEmail() {
                    $("#<%= btnSend_hidden.ClientID %>").click();
                }

                function validateForm() {
                    var emailTo = $("#<%= txtEmailTo.ClientID%>").val();

                    if ($.trim(emailTo.replace(";", "")) == "") {
                        alert('Email address is required.');
                        return false;
                    }

                    return true;
                }

                function showContacts() {
                    $("#divContactList").show();
                }

                <%-- $(document).ready(function () {
                    var selections;

                    $("#<%= lbxContacts.ClientID %>").click(function () {
                        selections = "";
                        $("#<%= lbxContacts.ClientID%> option:selected").each(function () {
                            selections += $(this).val() + ";";
                        });


                        $("#<%= txtEmailTo.ClientID %>").val(selections);
                    });
                });--%>

            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
