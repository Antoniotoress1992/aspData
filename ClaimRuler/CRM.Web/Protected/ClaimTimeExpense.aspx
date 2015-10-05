<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true" CodeBehind="ClaimTimeExpense.aspx.cs" Inherits="CRM.Web.Protected.ClaimTimeExpense" %>

<%@ Register Src="~/UserControl/ucClaimExpense.ascx" TagName="ucClaimExpenses" TagPrefix="uc10" %>
<%@ Register Src="~/UserControl/ucClaimService.ascx" TagName="ucClaimServices" TagPrefix="uc11" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    Time and Expense
    
    <asp:Button ID="hidden" runat="server" style = "display:none" />
<asp:Panel ID="Panel2" runat="server" CssClass="HellowWorldPopup"  align="center" style = "text-decoration-color:black; display:none">
    There are no services or expenses set up for this client. <br />Please contact your portal administrator.<br /><br /><br />
    <asp:Button ID="btnClose" runat="server" Width="80px" OnClick="btnClose_Click" Text="OK" />
</asp:Panel>
 <ajaxToolkit:ModalPopupExtender ID="popup" PopupControlID="Panel2" 
     BackgroundCssClass="ModalPopupBG" TargetControlID="hidden" DropShadow="true" runat="server"></ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlService" runat="server" CssClass="HellowWorldPopup"  align="center" style = "text-decoration-color:black; display:none">
    There are no services set up for this client. <br />Please contact your portal administrator.<br /><br /><br />
    <asp:Button ID="Button1" runat="server" Width="80px" OnClick="btnClose_Click" Text="OK" />
</asp:Panel>
 <ajaxToolkit:ModalPopupExtender ID="popUpService" PopupControlID="pnlService" 
     BackgroundCssClass="ModalPopupBG" CancelControlID="Button1" TargetControlID="hidden" DropShadow="true" runat="server"></ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlExpense" runat="server" CssClass="HellowWorldPopup"  align="center" style = "text-decoration-color:black; display:none">
    There are no expenses set up for this client. <br />Please contact your portal administrator.<br /><br /><br />
    <asp:Button ID="Button2" runat="server" Width="80px" OnClick="btnClose_Click" Text="OK" />
</asp:Panel>
 <ajaxToolkit:ModalPopupExtender ID="popUpExpense" PopupControlID="pnlExpense" 
     BackgroundCssClass="ModalPopupBG" CancelControlID="Button2" TargetControlID="hidden" DropShadow="true" runat="server"></ajaxToolkit:ModalPopupExtender>
    
    <style>
    .ModalPopupBG
    {
    background-color:black;
    filter: alpha(opacity=50);
    opacity: 0.7;
    }

        .HellowWorldPopup 
        {
            /*min-width:200px;
    min-height:150px;
    background:white;*/
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            color:darkslategrey;
            
            padding-top: 10px;
            padding-left: 10px;
            width: 400px;
            height: 140px;
        }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">
    <td>
        <asp:LinkButton ID="btnNewService" runat="server" CssClass="toolbar-item" OnClick="btnNewService_Click">
					<span class="toolbar-img-and-text" style="background-image: url(../images/money.png)">New Service</span>
        </asp:LinkButton>
    </td>
    <td>
        <asp:LinkButton ID="lbntnNewExpense" runat="server" CssClass="toolbar-item" OnClick="lbntnNewExpense_Click">
					<span class="toolbar-img-and-text" style="background-image: url(../images/expense.png)">New Expense</span>
        </asp:LinkButton>
    </td>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td class="top left" style="width: 50%;">

                        <div class="section-title">Claim Services</div>
                        <uc11:ucClaimServices ID="claimServices" runat="server" />

                    </td>
                    <td class="top left">

                        <div class="section-title">Claim Expenses</div>
                        <uc10:ucClaimExpenses ID="claimExpenses" runat="server" />

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:EntityDataSource ID="edsAdjusters" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
        EnableFlattening="False" EntitySetName="AdjusterMaster"
        Where="it.ClientId = @ClientID && it.Status = true"
        OrderBy="it.AdjusterName Asc">
        <WhereParameters>
            <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
    

    <script type="text/javascript">
        // handles claim services events
        function processInvoiceServiceType(ddl, txtDescID, txtQtyID) {
            var id = ddl.options[ddl.selectedIndex].value;
            $.ajax({
                url: "ClaimTimeExpense.aspx/getServiceTypeDetail",  // page method  
                data: JSON.stringify({ id: id }), // parameter map  
                type: "POST", // data has to be POSTed                
                contentType: "application/json",
                timeout: 10000,
                dataType: "json",
                success: function (service) {
                    serviceType = service.d;
                    if ($.trim(serviceType) != '') {
                        var valuePair = serviceType.split('|');

                        $find(txtDescID).set_value(valuePair[1]);
                        $find(txtQtyID).set_value(valuePair[0]);
                    }
                },
                error: function (status, xhr) {
                    debugger;
                    alert("An error occurred");
                }
            });

        }
    </script>
</asp:Content>
