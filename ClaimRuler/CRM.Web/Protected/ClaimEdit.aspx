lo<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true" CodeBehind="ClaimEdit.aspx.cs" Inherits="CRM.Web.Protected.ClaimEdit" %>

<%@ Register Src="~/UserControl/ucClaimEdit.ascx" TagName="ucClaimEdit" TagPrefix="uc1" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    <div style="float: left;">Claim Details</div>
    <div style="float: right; padding-right: 5px; vertical-align: middle;">
        <asp:HyperLink ID="hlnkScreenFields" runat="server" NavigateUrl="~/Protected/Admin/ScreenFields.aspx?id=1" Visible="false">
              <img id="form_fields" src="../images/gear.png" height="16" style="border-width: 0px; vertical-align: top;" />
        </asp:HyperLink>
    </div>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">

    <td>
        <asp:LinkButton ID="lbtnSave" runat="server" CssClass="toolbar-item" OnClientClick="javascript:return saveClaimForm();">
				<span class="toolbar-img-and-text" style="background-image: url(../images/toolbar_save.png)">Save</span>
        </asp:LinkButton>
    </td>
    <td>
        <asp:LinkButton ID="lbtnTimeExpense" runat="server" CssClass="toolbar-item" PostBackUrl="~/protected/ClaimTimeExpense.aspx">
				<span class="toolbar-img-and-text" style="background-image: url(../images/money.png)">Time & Expense</span>
        </asp:LinkButton>
    </td>
    <td>
        <asp:LinkButton ID="btnEmail" runat="server" CssClass="toolbar-item" PostBackUrl="~/Protected/LeadEmail.aspx">
				<span class="toolbar-img-and-text" style="background-image: url(../images/email_edit.png)">Email</span>
        </asp:LinkButton>
    </td>
    <td>
        <asp:LinkButton ID="btnPhoto" runat="server" CssClass="toolbar-item" PostBackUrl="~/protected/LeadsImagesUpload.aspx">
				<span class="toolbar-img-and-text" style="background-image: url(../images/photo_add.gif)">Photos</span>
        </asp:LinkButton>
    </td>
    <td>
        <asp:LinkButton ID="btnExport" runat="server" CssClass="toolbar-item" PostBackUrl="~/protected/ClaimExport.aspx">
				<span class="toolbar-img-and-text" style="background-image: url(../images/export.png)">Print Claim Report</span>
        </asp:LinkButton>
    </td>
    <td>
        <asp:LinkButton ID="btnAutoInvoice" runat="server" CssClass="toolbar-item" OnClientClick="javascript:return generateInvoice();">
				<span class="toolbar-img-and-text" style="background-image: url(../images/invoice.png)">Auto-Invoice</span>
        </asp:LinkButton>
    </td>
    <td> 
         <asp:LinkButton ID="btnNewTask" runat="server" PostBackUrl="~/Protected/LeadSchedule.aspx" CssClass="toolbar-item" >
				<span class="toolbar-img-and-text">New Task</span>
        </asp:LinkButton>
       
    </td>
</asp:Content>




<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnSave_hidden" runat="server" OnClick="btnSave_Click" style="display: none;" CausesValidation="true" />
            <asp:Button ID="btnGenerateInvoice_hidden" runat="server" OnClick="btnGenerateInvoice_hidden_Click" style="display: none;" CausesValidation="true" />

            <div class="paneContentInner">
                <uc1:ucClaimEdit ID="claimEdit" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
    
    <script type="text/javascript">
        function generateInvoice() {
            $("#<%= btnGenerateInvoice_hidden.ClientID %>").click();
             return false;
        }

        function saveClaimForm() {
            $("#<%= btnSave_hidden.ClientID %>").click();
            return false;
        }

        $(document).ready(function () {
            //$(".helptip img[title]").tooltip();

            calculatePropertyLimitsTotal(null, null);
            calculateCasualtyLimitsTotal(null, null);




            // added Getting Started to top choice
            var firstLi = $("ul").find("li:contains('Getting Started')");

            $("ul").find("li:contains('Getting Started')").remove();

            $(firstLi).prependTo("ul:first");

            var cookiesMode = readCookie("tutorialMode");

            if (cookiesMode == "true") {

                //var mode = cookies[2].split('=');

                //// check if mode is turn on then show tutorial
                //if (mode[1] == 'true') {

                // step 12
                $("span:contains('Print Claim Report')").data('powertipjq', $([
      '<p><b>Let\'s Get Paid Already and Print That Report</b> </p>',
      '<p>We are on the 1-Yard Line.  Let\'s Bring it Home!</p>',
      '<p>Select the files to include in your report by checking the boxes.</p>',
      '<p>(TIP: Do not select the Claim Log unless this is going to Legal)</p>',
      '<p>We hope you enjoy Claim Ruler as much as we do!  THANKS!!!</p><hr/>',
      '<p class="step">Step 12 of 12  <button style="margin-left: 116px;" id="step12">Done</button> </p>'
                ].join('\n')));

                $("span:contains('Print Claim Report')").powerTip({
                    placement: 'e',
                    smartPlacement: true,
                    manual: true
                });

                $.powerTip.hide();
                $.powerTip.show($("span:contains('Print Claim Report')"));

                $(document).on("click", "#step12", function () {
                    eraseCookie("startTutorialMode");
                    document.cookie = "tutorialMode=;path=/;expires=Thu, 01 Jan 1970 00:00:00 GMT";
                    $.powerTip.hide();
                });
                // }
            }

        });

    </script>

</asp:Content>
