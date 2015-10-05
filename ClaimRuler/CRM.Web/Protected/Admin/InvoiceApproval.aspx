<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="InvoiceApproval.aspx.cs" Inherits="CRM.Web.Protected.Admin.InvoiceApproval" %>

<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Invoice Approval
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>

                <div class="paneContentInner">
                    <div class="message_area">
                        <asp:Label ID="lblMessage" runat="server" />
                    </div>
                    <asp:GridView ID="gvInvoiceQ" runat="server"
                        AllowSorting="true"
                        AlternatingRowStyle-BackColor="#e8f2ff"
                        AutoGenerateColumns="False"                         
                        CellPadding="4"
                        CssClass="gridView"
                        DataKeyNames="InvoiceID,InvoiceNumber"
                        HorizontalAlign="Center"
                        EmptyDataText="No inboices available."
                        OnRowCommand="gvInvoiceQ_RowCommand"
                        OnRowDataBound="gvInvoiceQ_RowDataBound"
                        OnSorting="gvInvoiceQ_Sorting"
                        RowStyle-HorizontalAlign="Center"
                        Width="100%">
                        <EmptyDataTemplate>
                            No invoices available in queue.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div class="float_right">
                                        <a id="action_batch" class="link" onclick="showMenu_batch(event);">Action</a>
                                    </div>
                                    <div class="float_left">
                                        <asp:CheckBox ID="cbxSelectedAll" runat="server" OnCheckedChanged="cbxSelectedAll_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                </HeaderTemplate>
                                <ItemStyle Width="55px" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <div class="float_left">
                                        <asp:CheckBox ID="cbxSelected" runat="server" />
                                    </div>
                                    <div class="float_right top">
                                        <asp:HyperLink ID="hlnkAction" runat="server" Text="Action" CssClass="link" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" ItemStyle-Width="110px" DataFormatString="{0:MM/dd/yyyy}" SortExpression="InvoiceDate" />
                            <asp:BoundField DataField="DueDate" HeaderText="Due Date" ItemStyle-Width="110px" DataFormatString="{0:MM/dd/yyyy}" SortExpression="DueDate"/>
                            <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice Number" ItemStyle-Width="100px" SortExpression="InvoiceNumber"/>
                             <asp:TemplateField HeaderText="Insurer Claim ID #" ItemStyle-HorizontalAlign="Center" SortExpression="InsurerClaimNumber"
                                    >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnClaim" runat="server" Text=' <%# Eval("InsurerClaimNumber")%>' OnClick="lbtnClaim_Click" ValidationGroup=' <%# Eval("AdjusterClaimNumber")%>' />
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Type" ItemStyle-HorizontalAlign="Center" SortExpression="InvoiceTypes"
                                    >
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceType" runat="server"    /> 
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:BoundField DataField="PolicyNumber" HeaderText="Policy Number" ItemStyle-Width="150px" SortExpression="PolicyNumber"/>
                            <asp:BoundField DataField="BillToName" HeaderText="Bill-To" SortExpression="BillToName"/>
                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:n2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" SortExpression="TotalAmount" />
                        </Columns>
                    </asp:GridView>

                </div>

                <ig:WebDataMenu runat="server" ID="ContextMenu" IsContextMenu="true"
                    BorderStyle="Solid"
                    BorderWidth="1"
                    BorderColor="#CCCCCC">
                    <ClientEvents ItemClick="MenuItem_Click" />
                    <Items>
                        <ig:DataMenuItem Text="Edit" Key="edit" ImageUrl="../../Images/edit.png"  />
                        <ig:DataMenuItem Text="View" Key="view" ImageUrl="../../Images/view_icon.png" />
                        <ig:DataMenuItem Text="Approve" Key="approve" ImageUrl="../../Images/thumb_up.png" />
                        <ig:DataMenuItem Text="E-mail" Key="email" ImageUrl="../../Images/email_send.png" />
                        <ig:DataMenuItem Text="Reject" Key="delete" ImageUrl="../../Images/delete_icon.png"/>
                    </Items>
                </ig:WebDataMenu>
                <ig:WebDataMenu runat="server" ID="ContextMenu_Batch"
                    IsContextMenu="true"
                    BorderStyle="Solid"
                    BorderWidth="1"
                    BorderColor="#CCCCCC">
                    <ClientEvents ItemClick="ContextMenu_Batch_ItemClick" />
                    <Items>
                        <ig:DataMenuItem Text="Approve" Key="approve" ImageUrl="../../Images/thumb_up.png" />
                        <ig:DataMenuItem Text="E-mail" Key="email" ImageUrl="../../Images/email_send.png" />
                        <ig:DataMenuItem Text="Reject" Key="delete" ImageUrl="../../Images/delete_icon.png" />
                    </Items>
                </ig:WebDataMenu>
                <asp:Button ID="btnRefreshgvInvoiceQ" runat="server" OnClick="btnRefreshgvInvoiceQ_Click" Style="display: none" />
                <asp:Button ID="btnEmailInvoice" runat="server" OnClick="btnEmailInvoice_Click" Style="display: none;" />
                <asp:Button ID="btnBatchEmailInvoice" runat="server" OnClick="btnBatchEmailInvoice_Click" Style="display: none;" />
                <asp:Button ID="btnBatchApproveInvoice" runat="server" OnClick="btnBatchApproveInvoice_Click" Style="display: none;" />
                <asp:Button ID="btnBatchDeleteInvoice" runat="server" OnClick="btnBatchDeleteInvoice_Click" Style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <div id="email_invoice" title="Email Invoices" style="display: none;">
        <div class="boxContainer">
            <div class="section-title">Recipients</div>
            <div class="paneContentInner">
                <ig:WebTextEditor ID="txtEmailTo" runat="server" Width="100%" TextMode="MultiLine" MultiLine-Rows="2" MultiLine-Wrap="true" MultiLine-Overflow="Visible" />
            </div>

        </div>
        <div class="paneContentInner">
            <ig:WebDataGrid ID="contractGrid" runat="server" CssClass="gridView smallheader" DataSourceID="edsContacts"
                AutoGenerateColumns="false" Height="200px"
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
                    <ig:Sorting Enabled="true" SortingMode="Single"/>
                </Behaviors>
            </ig:WebDataGrid>
        </div>
        <div class="paneContentInner">
            <ig:WebTextEditor ID="txtEmailText" runat="server" Width="100%" TextMode="MultiLine" MultiLine-Rows="8" MultiLine-Wrap="true" MultiLine-Overflow="Visible" />
        </div>
    </div>
    <asp:EntityDataSource ID="edsContacts" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
        EnableFlattening="False" EntitySetName="vw_Contact"
        Where="it.ClientId = @ClientID"
        OrderBy="it.ContactName Asc">
        <WhereParameters>
            <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:HiddenField ID="hf_encryptedInvoiceID" runat="server" Value="0" />
    
    <script type="text/javascript">

        function ContextMenu_Batch_ItemClick(menu, eventArgs) {
            switch (eventArgs.getItem().get_key()) {
                case "approve":
                    $("#<%= btnBatchApproveInvoice.ClientID %>").click();
                    break;
                case "delete":
                    $("#<%= btnBatchDeleteInvoice.ClientID %>").click();
                    break;
                case "email":
                    emailInvoice_batch();
                    break;

                default:
                    break;
            }
        }
        function emailInvoice_batch() {

            // handles email invoice functions
            $("#email_invoice").dialog({
                modal: false,
                width: 800,
                close: function (e, ui) {
                    $(this).dialog('destroy');
                },
                buttons:
                {
                    'Send': function () {
                        if (validateEmail()) {
                            $(this).dialog('close');
                            $("#<%= btnBatchEmailInvoice.ClientID %>").click();
                        }
                    },
                    'Cancel': function () {
                        $(this).dialog('close');
                    }
                }
            });

            return false;
        }
    </script>
    <script type="text/javascript">
        function emailInvoice() {

            // handles email invoice functions
            $("#email_invoice").dialog({
                modal: false,
                width: 800,
                close: function (e, ui) {
                    $(this).dialog('destroy');
                },
                buttons:
                {
                    'Send': function () {
                        if (validateEmail()) {
                            $(this).dialog('close');
                            $("#<%= btnEmailInvoice.ClientID %>").click();
                        }
                    },
                    'Cancel': function () {
                        $(this).dialog('close');
                    }
                }
            });

            return false;
        }

        function contractGrid_rowsSelected(sender, args) {
            var selectedEmails = $find('<%= txtEmailTo.ClientID %>').get_value();

            var selectedRows = args.getSelectedRows();
            var email = args.getSelectedRows().getItem(0).get_cell(3).get_text();
            selectedEmails += email + ";";

            $find('<%= txtEmailTo.ClientID %>').set_value(selectedEmails);
        }

        function validateEmail() {
            var email = $find("<%= txtEmailTo.ClientID %>").get_value();
            //$.find("<%=txtEmailTo.ClientID%>").stylattr('class', '');
            if ($.trim(email) == "") {
                $find("<%=txtEmailTo.ClientID%>").focus();
                return false;
            }

            return true;
        }
    </script>
    <script type="text/javascript">
        function viewInvoice(invoiceID) {
            var url = '../../Content/PrintInvoice.aspx?q=' + invoiceID.toString();
            PopupCenter(url, 'Invoice', 800, 800);
        }
        function editInvoice(invoiceID) {
            var url = '../InvoiceEditPopUp.aspx?q=' + invoiceID.toString();
            PopupCenter(url, 'Invoice', 1000, 600);
        }
    </script>
    <script type="text/javascript">
        var encryptedInvoiceID = '';

        function showMenu(oEvent) {
            var menu = $find("<%= this.ContextMenu.ClientID %>");

            menu.showAt(null, null, oEvent);
        }
        function showMenu(oEvent, encryptedInvoiceString) {
            var menu = $find("<%= this.ContextMenu.ClientID %>");

            encryptedInvoiceID = encryptedInvoiceString;

            $("#<%= this.hf_encryptedInvoiceID.ClientID %>").val(encryptedInvoiceID);

            menu.showAt(null, null, oEvent);
            return oEvent.preventDefault();
        }
        function MenuItem_Click(menu, eventArgs) {
            switch (eventArgs.getItem().get_key()) {
                case "approve":
                    PageMethods.approveInvoice(encryptedInvoiceID, OnSuccess, OnFailure_Delete);
                    break;
                case "edit":
                    editInvoice(encryptedInvoiceID);
                    break;
                case "email":
                    emailInvoice();
                    break;

                case "view":
                    viewInvoice(encryptedInvoiceID);
                    break;
                case "delete":
                    if (confirm("Are you sure you want to reject this invoice?"))
                        PageMethods.deleteInvoice(encryptedInvoiceID, OnSuccess, OnFailure_Delete);
                    break;
                default:
                    break;
            }
        }
        function OnFailure_Delete() {
            showAlert("Unable to delete invoice.");
        }
        function OnSuccess() {
            $("#<%= btnRefreshgvInvoiceQ.ClientID %>").click();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
        });

        function showMenu_batch(oEvent) {
            var menu = $find("<%= this.ContextMenu_Batch.ClientID %>");

            menu.showAt(null, null, oEvent);
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            // check for step10 
            var step10 = readCookie("currentStep");
            if (step10 == 10) {
                $.powerTip.hide();
                $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_gvInvoiceQ td:first-child').css("border", "solid 2px yellow");
                // step 10

                $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_gvInvoiceQ_ctl02_hlnkAction').data('powertipjq', $([
                '<div style="height:431px;width:209px !important;white-space: normal;"><p><b>Let\'s Get This Invoice Out the Door…</b></p>',
                '<p>You may Approve, Reject, E-mail, or Edit any or ALL invoices',
                'from here to into the claim file, ledger, and Quickbooks',
                '(TIP: This feature may be turned off if you don\'t want it)',
                'After approved, an invoice saves to the claim docs and logs it. </p><hr/>',
                '<p class="step">Step 10 of 12 <input type="button" style="margin-left: 14px; !important;float:right"  id="step10" value="Next >>" /></p></div>'
                ].join('\n')));

                $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_gvInvoiceQ_ctl02_hlnkAction').powerTip({
                    placement: 'w',
                    smartPlacement: false,
                    manual: true
                });

                $.powerTip.show($('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_gvInvoiceQ_ctl02_hlnkAction'));

                $("#powerTip").css({ "right": "", "top": "0px", "margin-left": "30px" }); //1095px 11px

                // step 11
                $(document).on("click", "#step10", function () {


                    $.powerTip.hide();

                    // create cookie for step 11 and redirect to page
                    createCookie("currentStep", 11);

                    // navigate to All claim page and 
                    window.location = $("a:contains('All Claims'):first").attr("href");



                    //          $("a:contains('All Claims'):first").data('powertipjq', $([
                    //'<p><b>Drill-Down Directly into a Claim With 1-Click</b> </p>',
                    //'<p>Need more information?  Need to review or fine tune a claim?</p>',
                    //'<p>Use the friendly Claim # blue links to go right into a claim.</p>',
                    //'<p>From here you can navigate to the Policy or Policyholder</p>',
                    //'<p>screens for the claim if needed, print reports, view logs + MORE!</p><hr/>',
                    //'<p class="step">Step 11 of 12 <button style="margin-left: 116px;" id="step11" >Next >> </button></p>' //
                    //          ].join('\n')));

                    //          $("a:contains('All Claims'):first").powerTip({
                    //              placement: 'e',
                    //              smartPlacement: true,
                    //              manual: true
                    //          });

                    //          // remove animate effect from business rule
                    //          $("a:contains('Claim/Invoice Approval Queue')").parent().css({ "background-color": "white" });
                    //          textObject.flash($("a:contains('All Claims'):first").parent(), "colour", 300);


                    //          $.powerTip.show($("a:contains('All Claims'):first"));
                });




            }


        });


    </script>
</asp:Content>
