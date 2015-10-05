<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllUserLeads.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucAllUserLeads" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebSchedule.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/Admin/ucFeeDesignation.ascx" TagPrefix="uc1" TagName="ucFeeDesignation" %>

<style type="text/css">
    .dialogClass{
         left: 401px !important;
    }
    .adjustDialogClass{
       left:486px !important;
    }
     .claimStatusdialogClass{
         left:290px !important;
    }
     .claimAdjustersListdialogClass{
          left:394px !important;
     }
</style>
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

<div class="paneContent">
    <div class="section-title">
        Claims
    </div>
    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="lbtrnSearchPanel" runat="server" CssClass="toolbar-item" OnClick="lbtrnSearchPanel_Click">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/toolbar_search.png)">Search</span>
                    </asp:LinkButton>

                </td>
                <td>
                    <asp:LinkButton ID="brnNewClient" runat="server" CssClass="toolbar-item" OnClick="brnNewClient_Click" Visible="<%# masterPage.hasAddPermission %>">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Claim</span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lbtnClientDelete" runat="server" CssClass="toolbar-item" OnClick="lbtnClientDelete_Click" Visible="<%# masterPage.hasDeletePermission %>" OnClientClick="javascript:return ConfirmDialog(this, 'Warning:  If you delete this claim, then it may not be recoverable.  Please do not delete a claim unless absolutely necessary.  Are you sure you want to delete this claim forever?');">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/delete_icon.png)">Delete Claim</span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div class="paneContentInner">
        <div style="text-align: center; padding-top: 5px;">
            <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
            <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
            <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
        </div>
        <table>
            <tr>
                <td class="top left">
                    <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
                        <div class="boxContainer" style="width: 200px">
                            <div class="section-title">Search Filters</div>
                            <div style="height: 700px; overflow-y: auto; overflow-x: hidden;">
                                <table style="width: 200px;" border="0" class="editForm no_min_width">
                                    <tr>
                                        <td style="text-align: center;">
                                            <div style="float: left;">
                                                <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" ImageUrl="~/Images/show-all.png" />
                                            </div>
                                            <div>
                                                <asp:LinkButton ID="lbtnClear" runat="server" OnClick="btnReset_Click" Text="Clear" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Insured Name:</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtClaimantName" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Claim Number</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtSearchClaimNumber" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Loss Date From</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebDatePicker ID="txtDateFrom" runat="server" CssClass="date_picker"
                                                StyleSetName="Default" Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Loss Date To</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebDatePicker ID="txtDateTo" runat="server" CssClass="date_picker"
                                                StyleSetName="Default" Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Carrier</td>
                                    </tr>
                                    <tr>
                                        <td class="no_min_width">
                                            <asp:DropDownList ID="ddlCarrier" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Policy Type</td>
                                    </tr>
                                    <tr>
                                        <td class="no_min_width">
                                            <asp:DropDownList ID="ddlPolicyType" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Policy Number</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtInsurancePolicyNumber" runat="server" Width="150px" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Mailing Address</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtClaimantAddress" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Claim Status</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Claim Sub-Status</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlSubStatus" runat="server" Width="150px" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Show Claims</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlShowType" runat="server" Width="100px">
                                                <asp:ListItem Text="All" Value="1" />
                                                <asp:ListItem Text="Open" Value="2" />
                                                <asp:ListItem Text="Closed" Value="3" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>User Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtUserName" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Contractor Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtSearchContractor" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Adjuster Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtSearchAdjuster" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Appraiser Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtSearchAppraiser" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Umpire Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtSearchUmpire" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Producer Name</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ig:WebTextEditor ID="txtSearchProducer" runat="server" />
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>

                    </asp:Panel>
                </td>
                <td class="top left" style="width: 100%">
                    <asp:Panel ID="pnlSearchResult" runat="server" Visible="false">

                        <div>
                            <div style="margin-bottom: 5px; display: none;">
                                <label>Active Count: &nbsp;&nbsp;</label><b><asp:Label ID="lblOpenLeadCount" runat="server"></asp:Label></b><br />
                                <label>Closed Count: &nbsp;</label><b><asp:Label ID="lblCloseLeadCount" runat="server"></asp:Label></b>
                            </div>
                            <div>
                                <asp:Panel ID="pnlUserLeads" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvUserLeads" runat="server"
                                                AllowSorting="true"
                                                AllowPaging="true"
                                                AlternatingRowStyle-BackColor="#e8f2ff"
                                                AutoGenerateColumns="False"
                                                CellPadding="2"
                                                CssClass="gridView csk-box"
                                                DataKeyNames="LeadId"
                                                EmptyDataText="No Record Found !!"
                                                OnPageIndexChanging="gvUserLeads_PageIndexChanging"
                                                OnRowCommand="gvUserLeads_RowCommand"
                                                OnRowDataBound="gvUserLeads_OnRowDataBound"
                                                OnSorting="gvUserLeads_Sorting"
                                                PageSize="20"
                                                RowStyle-HorizontalAlign="Center"
                                                PagerSettings-PageButtonCount="10"
                                                Width="100%">
                                                <PagerStyle CssClass="pager" Font-Bold="true" />
                                                <EmptyDataTemplate>
                                                    <a id="A1" runat="server" href="~/Protected/NewLead.aspx" target="_self" style="color: blue; text-decoration: underline;">New Claim</a>
                                                </EmptyDataTemplate>
                                                <Columns>

                                                    <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-Width="40px">
                                                        <HeaderTemplate>
                                                            <input id="chkAll" class="cbAll" onclick="javascript: SelectAllCheckboxes1(this);" runat="server" type="checkbox" />
                                                            All
                                                         
    
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectLeads" runat="server" Class="gvSelectLeadsClass" />
                                                            <asp:HiddenField ID="hdnSelectLeads" runat="server" Value='<%#Eval("LeadId") %>' />

                                                        </ItemTemplate>
                                                        <ItemStyle Width="125px" Wrap="False" VerticalAlign="Top" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="35px" HeaderText="Add Note">
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <div style="vertical-align: top;">
                                                                <asp:ImageButton ID="imgbtnNotes" runat="server"
                                                                    CommandName="AddNotes"
                                                                    CommandArgument='<%#Eval("ClaimId") %>'
                                                                    ToolTip="Add Notes"
                                                                    ImageAlign="Top" OnClick="imgbtnNotes_Click" 
                                                                    ImageUrl="~/Images/Notepad_icon.svg_.png"
                                                                      
                                                                    Visible='<%# masterPage.hasEditPermission %> '  Width="22px" CssClass="displayblock" />
                                                                <%-- <%# Eval("ClaimId")%>     --%>
                                                                <asp:HiddenField ID="hfClaimNotes" runat="server" Value='<%# Eval("ClaimId")%>' />
                                                                <asp:HiddenField ID="hfLeadId" runat="server" Value='<%# Eval("LeadId")%>' />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="35px" HeaderText="Add Expense">
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <div style="vertical-align: top;">
                                                                <asp:ImageButton ID="imgbtnExpense" runat="server"
                                                                    CommandName="AddExpense"
                                                                    CommandArgument='<%#Eval("ClaimId") %>'
                                                                    ToolTip="Add Expense"
                                                                    ImageAlign="Top"
                                                                    ImageUrl="~/Images/expensify-icon.gif"
                                                                    Visible='<%# masterPage.hasEditPermission %> '  Width="22px" CssClass="displayblock" />
                                                                <%-- <%# Eval("ClaimId")%>     --%>
                                                                <asp:HiddenField ID="hfClaimIdExpense" runat="server" Value='<%# Eval("ClaimId")%>' />
                                                                <asp:HiddenField ID="hfLeadIdExpense" runat="server" Value='<%# Eval("LeadId")%>' />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="35px" HeaderText="Add Invoice">
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <div style="vertical-align: top;">
                                                                <asp:ImageButton ID="imgbtnInvoice" runat="server"
                                                                    CommandName="AddInvoice"
                                                                    CommandArgument='<%#Eval("ClaimId") %>'
                                                                    ToolTip="Add Invoice"
                                                                    ImageAlign="Top"
                                                                    ImageUrl="~/Images/Add_Invoice.png"
                                                                    Visible='<%# masterPage.hasEditPermission %> ' OnClientClick="return HandleInvoiceClick(this);" Width="22px" CssClass="displayblock" />
                                                                <asp:HiddenField ID="hfClaimIdInvoice" runat="server" Value='<%# Eval("ClaimId")%>' />

                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="35px" HeaderText="Upload Docs">
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <div style="vertical-align: top;">
                                                                <asp:ImageButton ID="imgbtnUploadDocs" runat="server"
                                                                    CommandName="UploadDocs"
                                                                    CommandArgument='<%#Eval("ClaimId") %>'
                                                                    ToolTip="Upload Document"
                                                                    ImageAlign="Top"
                                                                    ImageUrl="~/Images/upload_docs.png"
                                                                    Visible='<%# masterPage.hasEditPermission %> ' OnClientClick="return showDocumentUploadDialog(this);" Width="22px" CssClass="displayblock" />
                                                                <asp:HiddenField ID="hfClaimIdUploadDocs" runat="server" Value='<%# Eval("ClaimId")%>' />

                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="35px" HeaderText="Add Photo">
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <div style="vertical-align: top;">
                                                                <asp:ImageButton ID="imgbtnAddPhotos" runat="server"
                                                                    CommandName="UploadDocs"
                                                                    CommandArgument='<%#Eval("ClaimId") %>'
                                                                    ToolTip="Add Photo"
                                                                    ImageAlign="Top"
                                                                    ImageUrl="~/Images/add_photo.png"
                                                                    Visible='<%# masterPage.hasEditPermission %> ' OnClientClick="return showAddPhotosDialog(this);" Width="22px" CssClass="displayblock" />
                                                                <asp:HiddenField ID="hfClaimIdAddPhotos" runat="server" Value='<%# Eval("ClaimId")%>' />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgInactivityFlag" runat="server" ImageUrl="~/Images/Flag-yellow.png"
                                                                Width="16px" Visible="false" />
                                                            <asp:HiddenField ID="hfClaimStatusCodes" runat="server" Value='<%# Eval("StatusCodes")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Insured Name" SortExpression="InsuredName"
                                                        ItemStyle-Wrap="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInsuredName" runat="server" Text ='<%# Eval("InsuredName")%>'></asp:Label>
                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Last Name" SortExpression="ClaimantLastName"
                                                        ItemStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <%# Eval("ClaimantLastName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="First Name" SortExpression="ClaimantFirstName">
                                                        <ItemTemplate>
                                                            <%# Eval("ClaimantFirstName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                 <%--   <asp:TemplateField HeaderText="Business Name" SortExpression="BusinessName">
                                                        <ItemTemplate>
                                                            <%# Eval("BusinessName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Policy Type" SortExpression="Coverage">
                                                        <ItemTemplate>
                                                            <%# Eval("Coverage")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     
                                                    <asp:TemplateField HeaderText="Adjuster Name" SortExpression="AdjusterName">
                                                        <ItemTemplate>
                                                            <%# Eval("AdjusterName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date Record Created" SortExpression="OriginalLeadDate"
                                                        ItemStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <%# Eval("OriginalLeadDate","{0:MM/dd/yy}") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Loss Date" SortExpression="LossDate" ItemStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <%# Eval("LossDate", "{0:MM/dd/yy h:mm tt}") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Insurer Claim #" SortExpression="InsurerClaimNumber"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnInsurerClaim" Text ='<%# Eval("InsurerClaimNumber") %>' runat="server" OnClick="lbtnClaim_Click2" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Adjuster File #" SortExpression="ClaimNumber"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnClaim" runat="server" OnClick="lbtnClaim_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Progress" SortExpression="ProgressDescription">
                                                        <ItemTemplate>
                                                            <%# Eval("ProgressDescription")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ToolTip="Change Status" ID="lbtnClaimStatus" runat="server" Text='<%# Eval("StatusName") %>' OnClientClick="return HandleStatusClick(this);" />
                                                            <asp:HiddenField ID="hfClaimIdClaimStatus" runat="server" Value='<%# Eval("ClaimId")%>' />
                                                            <asp:HiddenField ID="hfLeadIdClaimStatus" runat="server" Value='<%# Eval("LeadId")%>' />
                                                            <%-- <%# Eval("StatusName") %>    --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Status" SortExpression="SubStatusName">
                                                        <ItemTemplate>
                                                            <%# Eval("SubStatusName") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Client" SortExpression="InsuranceCompanyName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClient" runat="server" Text ='<%# Eval("InsuranceCompanyName")%>'></asp:Label>
                                                            <%--<%# Eval("InsuranceCompanyName") == null ? "" : Eval("InsuranceCompanyName")%>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Insurer/Branch" SortExpression="LocationName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBranch" runat="server" Text ='<%# Eval("LocationName")%>'></asp:Label>
                                                          <%--  <%# Eval("LocationName")%>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Loss City" SortExpression="CityName">
                                                        <ItemTemplate>
                                                            <%#Eval("CityName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Loss State" SortExpression="StateName">
                                                        <ItemTemplate>
                                                            <%#Eval("StateName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Loss Zip" SortExpression="Zip">
                                                        <ItemTemplate>
                                                            <%# Eval("Zip") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Lead Source" SortExpression="LeadSourceMaster.LeadSourceName">
                                                        <ItemTemplate>
                                                            <%#Eval("LeadSourceName") == null ? "" : Eval("LeadSourceName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type of Loss">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCauseOfLoss" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type Of Property" SortExpression="TypeOfProperty">
                                                        <ItemTemplate>
                                                            <%# Eval("TypeOfProperty") == null ? "" : Eval("TypeOfProperty")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contractor" SortExpression="ContractorName">
                                                        <ItemTemplate>
                                                            <%# Eval("ContractorName") == null ? "" : Eval("ContractorName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Appraiser" SortExpression="AppraiserName">
                                                        <ItemTemplate>
                                                            <%# Eval("AppraiserName") == null ? "" : Eval("AppraiserName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Umpire" SortExpression="UmpireName">
                                                        <ItemTemplate>
                                                            <%# Eval("UmpireName") == null ? "" : Eval("UmpireName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Primary Producer" SortExpression="ProducerName">
                                                        <ItemTemplate>
                                                            <%# Eval("ProducerName") == null ? "" : Eval("ProducerName")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Severity" SortExpression="Severity">
                                                        <ItemTemplate>
                                                            <%# Eval("Severity","{0:N0;; }") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Event Name" SortExpression="EventName">
                                                        <ItemTemplate>
                                                            <%# Eval("EventName") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Event Type" SortExpression="EventType">
                                                        <ItemTemplate>
                                                            <%# Eval("EventType") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Claim Workflow Type" SortExpression="ClaimWorkflowType">
                                                        <ItemTemplate>
                                                            <%# Eval("ClaimWorkflowType") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Last Activity Date" SortExpression="LastActivityDate"
                                                        ItemStyle-Wrap="false">
                                                        <ItemTemplate>
                                                            <%# Eval("LastActivityDate") == null ? "" : ((DateTime)Eval("LastActivityDate")).ToString("MM-dd-yy")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="User Name" SortExpression="UserName">
                                                        <ItemTemplate>
                                                            <%# Eval("UserName") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="35px">
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <div style="vertical-align: top;">
                                                                <asp:ImageButton ID="btnEdit" runat="server"
                                                                    CommandName="DoEdit"
                                                                    CommandArgument='<%#Eval("LeadId") %>'
                                                                    ToolTip="Edit"
                                                                    ImageAlign="Top"
                                                                    ImageUrl="~/Images/edit_icon.png"
                                                                    Visible='<%# masterPage.hasEditPermission %>' />
                                                                <asp:HyperLink ID="hypEditMenu" runat="server" ImageAlign="Middle" ImageUrl="~/Images/menu_dropdown.png"
                                                                    ToolTip="Show more edit options." />
                                                            </div>
                                                            <asp:CollapsiblePanelExtender ID="cpeEditMenu" runat="Server" TargetControlID="pnlEditMenu"
                                                                Collapsed="true" ExpandControlID="hypEditMenu" CollapseControlID="hypEditMenu"
                                                                AutoCollapse="False" ScrollContents="false" ExpandDirection="Vertical" />
                                                            <asp:Panel ID="pnlEditMenu" runat="server" Style="display: none;">
                                                                <div>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DoDelete"
                                                                        CommandArgument='<%#Eval("LeadId") %>'
                                                                        ToolTip="Delete" Text="Delete"
                                                                        OnClientClick="javascript:return ConfirmDialog(this, 'Warning:  If you delete this claim, then it may not be recoverable.  Please do not delete a claim unless absolutely necessary.  Are you sure you want to delete this claim forever?');"
                                                                        OnDataBinding="linkButton_OnDataBinding"
                                                                        Visible='<%# masterPage.hasDeletePermission %>' />
                                                                </div>
                                                                <div>
                                                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="DoView"
                                                                        CommandArgument='<%#Eval("LeadId") %>'
                                                                        ToolTip="View" Text="View"
                                                                        OnDataBinding="linkButton_OnDataBinding"
                                                                        Visible='<%# masterPage.hasViewPermission %>' />
                                                                </div>
                                                                <div>
                                                                    <asp:LinkButton ID="lnkCopy" runat="server" CommandName="DoCopy" CommandArgument='<%#Eval("LeadId") %>'
                                                                        ToolTip="Copy" Text="Copy"
                                                                        OnDataBinding="linkButton_OnDataBinding"
                                                                        Visible='<%# masterPage.hasEditPermission %>' />
                                                                </div>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Label ID="lblSearchResult" runat="server" Visible="false" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>




    </div>
</div>
<asp:HiddenField ID="hfClaimStatusCodes" runat="server" />
<asp:HiddenField ID="hfInactivityPeriod" runat="server" />
<asp:HiddenField ID="hfSystemClosedStatuses" runat="server" />
<asp:HiddenField ID="hf_taskDate" runat="server" />




<div id="div_ClaimServiceAdjustersList" style="display: none; width: 90%;" title="Add Note">
    <div class="boxContainer">
        <div class="section-title">
            Service Detail
        </div>
        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">
            <tr>
                <td colspan="3" class="center">
                    <asp:Label ID="Label1" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="right">Service Type</td>
                <td class="redstar"></td>
                <td>
                    <asp:DropDownList ID="ddlInvoiceServiceType" runat="server" onblur="return BlankControl2();" />
                    <div>
                        <asp:Label ID="lblInvoiceServiceType" runat="server" Text="Please select service type." Font-Size="Small" ForeColor="Red"></asp:Label>
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlInvoiceServiceType" InitialValue="0"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select service type."
                            ValidationGroup="service" />--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Adjuster
                </td>
                <td class="redstar"></td>
                <td class="nowrap">
                    <ig:WebTextEditor ID="txtServiceAdjuster" runat="server" Enabled="false" Width="250px" onblur="return BlankControl();" />
                    <a href="javascript:findAdjusterForServiceDialog();">
                        <asp:Image ID="imgAdjusterFind" runat="server" ImageUrl="~/Images/searchbg.png" ImageAlign="Top" />
                    </a>
                    <div>
                        <asp:Label ID="lblServiceAdjuster" runat="server" Text="Please select adjuster." Font-Size="Small" ForeColor="Red"></asp:Label>
                       <%-- <asp:RequiredFieldValidator ID="rfvAdjuster" runat="server" ControlToValidate="txtServiceAdjuster"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select adjuster."
                            ValidationGroup="service" />--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Service Date
                </td>
                <td class="redstar"></td>
                <td>

                    <ig:WebDatePicker ID="txtServiceDate" runat="server" CssClass="date_picker" Width="150px" onblur="return BlankControl();" DisplayModeFormat="MM/dd/yyyy h:mm tt" EditModeFormat="MM/dd/yyyy h:mm tt">
                        <Buttons>
                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                        </Buttons>
                    </ig:WebDatePicker>
                    <div>
                        <asp:Label ID="lblServiceDate" runat="server" Text="Please enter expense date." Font-Size="Small" ForeColor="Red"></asp:Label>
                        <%-- <asp:RequiredFieldValidator ID="tfvServiceDate" runat="server" ControlToValidate="txtServiceDate" ValidationGroup="service"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter expense date." CssClass="validation1" />--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Description</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtServiceDescription" MaxLength="500" onblur="return BlankControl();" TextMode="MultiLine" MultiLine-Rows="3" Width="100%"></ig:WebTextEditor>
                    <div>
                        <asp:Label ID="lblServiceDescription" runat="server" Text="Please enter description." Font-Size="Small" ForeColor="Red"></asp:Label>
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtServiceDescription" ValidationGroup="service"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter description." CssClass="validation1" />--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Quantity</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtServiceQty" runat="server" MinDecimalPlaces="2" onblur="return BlankControl();" Width="80px"></ig:WebNumericEditor>
                    <div>
                        <asp:Label ID="lblServiceQty" runat="server" Text="Please enter quantity." Font-Size="Small" ForeColor="Red"></asp:Label>
                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtServiceQty" ValidationGroup="service"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter quantity." CssClass="validation1" InitialValue="0" />--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">E-mail To</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor ID="txtEmailTo" runat="server" Width="99%" TabIndex="60" disabled="false" />
                    <%-- onblur="return CheckEmailTo()"--%>
                    <div>
                        <asp:Label ID="lblEmailTo" runat="server" Text="Please select any recipients" Font-Size="Small" ForeColor="Red"></asp:Label>

                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Select Recipients</td>
                <td class="redstar top"></td>
                <td>
                    <div style="width: 552px; height: 250px; overflow-y: auto; border: 1px solid;">
                        <asp:GridView Width="100%" ID="gvSelectRecipients" CssClass="gridView csk-box" runat="server" AutoGenerateColumns="False" CellPadding="2">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelectRecipients" runat="server" Class="gvSelectRecipientsClass checkbox-align-center" />
                                        <asp:HiddenField ID="hdnSelectRecipients" runat="server" Value='<%#Eval("ContactID") %>' />
                                        <asp:HiddenField ID="hdn2SelectRecipients" runat="server" Value='<%#Eval("IdOf") %>' />
                                        <asp:HiddenField ID="hdnEmailSelectRecipients" runat="server" Value='<%#Eval("Email") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="125px" Wrap="False" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="First Name">
                                    <ItemTemplate>
                                        <%#Eval("FirstName") %>
                                        <%-- <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("SecUser.UserName") %>' />--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Name">
                                    <ItemTemplate>
                                        <%#Eval("LastName") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <%#Eval("CompanyName") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <%#Eval("Email") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="Contact Type">
                    <ItemTemplate>
                        <%#Eval("ContactType") %>                       
                    </ItemTemplate>
                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>

                    </div>
                </td>
            </tr>

            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnSaveClaimService" runat="server" Text="Save" CssClass="mysubmit" OnClientClick="return SaveNotes();" CausesValidation="true" ValidationGroup="service" />
                    &nbsp;
                    <%--<asp:Button ID="btnCancelClaimService" runat="server" Text="Cancel" CssClass="mysubmit"   CausesValidation="false" />--%>
                </td>
            </tr>
        </table>


    </div>
</div>


<%--<div id="exposeMask" style="position: absolute; width:85%;height:100%;
                 display: none; opacity: 0.5; z-index: 10; background-color: rgb(226, 222, 203);">
             
            </div>--%>

<div id="div_AdjustersList" style="display: none; width: 90%;" title="Select Adjuster">
    <div class="boxContainer">
        <div class="section-title">
            Adjusters
        </div>
        <ig:WebDataGrid ID="adjusterGrid" runat="server" CssClass="gridView smallheader" DataSourceID="edsAdjusters"
            AutoGenerateColumns="false" Height="300px"
            Width="100%">
            <Columns>
                <ig:BoundDataField DataFieldName="AdjusterId" Key="AdjusterId" Header-Text="ID" Width="50px" />
                <ig:BoundDataField DataFieldName="AdjusterName" Key="AdjusterName" Header-Text="Adjuster Name" />
                <ig:BoundDataField DataFieldName="email" Key="email" Header-Text="E-mail Address" />
            </Columns>
            <Behaviors>

                <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                    ThresholdFactor="0.5" Enabled="true" />

                <ig:Selection RowSelectType="Single" Enabled="True" CellClickAction="Row">
                    <SelectionClientEvents RowSelectionChanged="claimServiceAdjusterGrid_rowsSelected" />
                </ig:Selection>
            </Behaviors>
        </ig:WebDataGrid>
    </div>
</div>

<div>
    <asp:Button ID="hidden" runat="server" style = "display:none" />
     <asp:Panel ID="pnlService" runat="server" CssClass="HellowWorldPopup"  align="center" style = "text-decoration-color:black; display:none">
    Claim not assigned to this lead.<br /><br /><br />
    <asp:Button ID="Button1" runat="server" Width="80px" Text="OK" />
</asp:Panel>
 <asp:ModalPopupExtender ID="popUpService" PopupControlID="pnlService" 
     BackgroundCssClass="ModalPopupBG" CancelControlID="Button1" TargetControlID="hidden" DropShadow="true" runat="server"></asp:ModalPopupExtender>

</div>

<div id="div_NoClaim" style="display: none; width: 90%;" title="No Claim">
    <div class="boxContainer">
        Claim not assigned to this lead.
        
        
    </div>
</div>

<div id="div_NoteSave" style="display: none; width: 90%;" title="Add Note">
    <div class="boxContainer">
        Note saved successfully.
        
        
    </div>
</div>

<div id="div_NoteNotSave" style="display: none; width: 90%;" title="Add Note">
    <div class="boxContainer">
        Note not saved successfully.
        
        
    </div>
</div>

<div id="divSelectRecords" style="display: none; width: 90%;" title="Select Claim">
    <div class="boxContainer">
        Please select Record(s).
    </div>
</div>


<asp:EntityDataSource ID="edsAdjusters" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
    EnableFlattening="False" EntitySetName="AdjusterMaster"
    Where="it.ClientId = @ClientID && it.Status = true"
    OrderBy="it.AdjusterName Asc">
    <WhereParameters>
        <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="hf_serviceAdjusterID" runat="server" Value="0" />
<asp:HiddenField ID="hdnClaimIDNew" runat="server" Value="0" />
<asp:HiddenField ID="hdnLeadIDNew" runat="server" Value="0" />

<%--Status change div--%>



<div id="div_ClaimStatusReview" style="display: none; width: 90%;" title="Claim Status/Review">
    <div class="boxContainer">
        <div class="section-title">
            Claim Status/Review
        </div>
        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">
            <tr>
                <td colspan="3" class="center">
                    <asp:Label ID="lblMsgSaveReview" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="right">Update Status To</td>
                <td class="redstar">*</td>
                <td>
                    <asp:DropDownList ID="ddlClaimStatusReview" runat="server" Width="258px" TabIndex="53" />
                    <div>
                        <asp:Label ID="lblClaimStatusReview" runat="server" Text="Please select service type." Font-Size="Small" ForeColor="Red"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlClaimStatusReview" InitialValue="0"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select service type."
                            ValidationGroup="service" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Insurer Claim ID #</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtInsurerClaimId" Width="250px" onblur="return BlankControl2()" TabIndex="54" />
                    <div>
                        <asp:Label ID="lblInsurerClaimId" runat="server" Text="Please enter insurer claim #" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>

                </td>
            </tr>
            <tr>
                <td class="right top">Insured Name</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtInsurerName" TabIndex="55" Width="250px" onblur="return BlankControl2()" />
                    <div>
                        <asp:Label ID="lblInsurerName" runat="server" Text="Please enter insurer name." Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Carrier</td>
                <td class="redstar top">*</td>
                <td>
                    <%--<ig:WebTextEditor runat="server" ID="txtCarrier" TabIndex="1" Width="250px" />--%>
                    <asp:DropDownList ID="ddlClaimCarrier" runat="server" Width="258px" TabIndex="56" />
                    <div>
                        <asp:Label ID="lblClaimCarrier" runat="server" Text="Please select carrier." Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Adjuster
                </td>
                <td class="redstar top">*</td>
                <td class="nowrap">
                    <ig:WebTextEditor ID="txtClaimAdjuster" runat="server" Enabled="false" Width="250px" onblur="return BlankControl2()" TabIndex="57" />
                    <a href="javascript:findAdjusterForClaimDialog();">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/searchbg.png" ImageAlign="Top" />
                    </a>
                    <div>
                        <asp:Label ID="lblClaimAdjuster" runat="server" Text="Please select adjuster." Font-Size="Small" ForeColor="Red"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtClaimAdjuster"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select adjuster."
                            ValidationGroup="service" />
                    </div>
                </td>
            </tr>

            <tr>
                <td class="right top">Adjuster Company Name</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtAdjusterComapnyName" TabIndex="58" Width="250px" onblur="return BlankControl2()" ReadOnly="true" />
                    <div>
                        <%--<asp:Label ID="lblAdjusterComapnyName" runat="server" Text="Please enter company name." Font-Size="Small" ForeColor="Red"></asp:Label>--%>
                    </div>
                </td>
            </tr>
            <%--<tr>
                <td class="right top">Updated By</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtUpdatedby" TabIndex="1"  Width="250px"/>
                  
                </td>
            </tr>--%>
            <tr>
                <td class="right top">Comment/Note</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtCommentNote" MaxLength="500" TabIndex="59" TextMode="MultiLine" MultiLine-Rows="3" Width="100%" onblur="return BlankControl2()"></ig:WebTextEditor>
                    <div>

                        <asp:Label ID="lblCommentNote" runat="server" Text="Please enter comment." Font-Size="Small" ForeColor="Red"></asp:Label>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCommentNote" ValidationGroup="service"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter description." CssClass="validation1" />
                    </div>
                </td>
            </tr>

            <tr>
                <td class="right top">E-mail To</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor ID="txtEmailToStatus" runat="server" Width="100%" TabIndex="60" disabled="true" />
                    <%-- onblur="return CheckEmailTo()"--%>
                    <div>
                       <%-- <asp:Label ID="lblEmailToStatus" runat="server" Text="Please select any recipient." Font-Size="Small" ForeColor="Red"></asp:Label>--%>

                    </div>
                </td>
            </tr>

            <tr>
                <td class="right top">Select Recipients</td>
                <td class="redstar top"></td>
                <td>
                    <div style="width: 552px; height: 250px; overflow-y: auto; border: 1px solid;">
                        <asp:GridView Width="100%" ID="gvSelectRecipientsStatus" CssClass="gridView csk-box" runat="server" AutoGenerateColumns="False" CellPadding="2">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelectRecipientsStatus" runat="server" Class="gvSelectRecipientsClass checkbox-align-center" onclick="return HandleCheckBox(this);" />
                                        <asp:HiddenField ID="hdnSelectRecipientsStatus" runat="server" Value='<%#Eval("ContactID") %>' />
                                        <asp:HiddenField ID="hdn2SelectRecipientsStatus" runat="server" Value='<%#Eval("IdOf") %>' />
                                        <asp:HiddenField ID="hdnEmailSelectRecipientsStatus" runat="server" Value='<%#Eval("Email") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="125px" Wrap="False" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="First Name">
                                    <ItemTemplate>
                                        <%#Eval("FirstName") %>
                                        <%-- <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("SecUser.UserName") %>' />--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Name">
                                    <ItemTemplate>
                                        <%#Eval("LastName") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <%#Eval("CompanyName") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <%#Eval("Email") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="Contact Type">
                    <ItemTemplate>
                        <%#Eval("ContactType") %>                       
                    </ItemTemplate>
                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>

                    </div>
                </td>
            </tr>

            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnSaveClaimNote" runat="server" Text="Save" CssClass="mysubmit" OnClientClick="return SaveClaimStatus();" />
                    &nbsp;
                    <%--<asp:Button ID="btnCancelClaimService" runat="server" Text="Cancel" CssClass="mysubmit"   CausesValidation="false" />--%>
                </td>
            </tr>
        </table>


    </div>
</div>

<div id="div_ClaimAdjustersList" style="display: none; width: 90%;" title="Select Adjuster">
    <div class="boxContainer">
        <div class="section-title">
            Adjusters
        </div>
        <ig:WebDataGrid ID="claimAdjusterGrid" runat="server" CssClass="gridView smallheader" DataSourceID="EntityDataSource1"
            AutoGenerateColumns="false" Height="300px"
            Width="100%">
            <Columns>
                <ig:BoundDataField DataFieldName="AdjusterId" Key="AdjusterId" Header-Text="ID" Width="50px" />
                <ig:BoundDataField DataFieldName="AdjusterName" Key="AdjusterName" Header-Text="Adjuster Name" />
                <ig:BoundDataField DataFieldName="email" Key="email" Header-Text="E-mail Address" />
            </Columns>
            <Behaviors>

                <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                    ThresholdFactor="0.5" Enabled="true" />

                <ig:Selection RowSelectType="Single" Enabled="True" CellClickAction="Row">
                    <SelectionClientEvents RowSelectionChanged="claimAdjusterGrid_rowsSelected" />
                </ig:Selection>
            </Behaviors>
        </ig:WebDataGrid>
    </div>
</div>

<div id="div_StatusSave" style="display: none; width: 90%;" title="Status Change">
    <div class="boxContainer">
        Status changed successfully.
        
        
    </div>
</div>

<div id="div_StatusNotSave" style="display: none; width: 90%;" title="Status Change">
    <div class="boxContainer">
        Status not changed successfully.
        
        
    </div>
</div>

<asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
    EnableFlattening="False" EntitySetName="AdjusterMaster"
    Where="it.ClientId = @ClientID && it.Status = true"
    OrderBy="it.AdjusterName Asc">
    <WhereParameters>
        <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>

<asp:HiddenField ID="hf_ClaimAdjusterID" runat="server" Value="0" />
<asp:HiddenField ID="hf_ClaimIdForStatus" runat="server" Value="0" />
<asp:HiddenField ID="hdnEmailToList" runat="server" Value="0" />




<%--Status change div--%>



<%--Expense change div--%>



<div id="div_ExpenseReview" style="display: none; width: 90%;" title="Add Expense">
    <div class="boxContainer">
        <div class="section-title">
            Add Claim Expense
        </div>
        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">
            <tr>
                <td colspan="3" class="center">
                    <asp:Label ID="lblMsgSaveExpense" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="right">Expense Type</td>
                <td class="redstar">*</td>
                <td>
                    <asp:DropDownList ID="ddlExpenseType" runat="server" Width="258px" TabIndex="101" />
                    <div>
                        <asp:Label ID="lblExpenseType" runat="server" Text="Please select expense type." Font-Size="Small" ForeColor="Red"></asp:Label>

                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Insurer Claim ID #</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtExpenseInsurerClaimId" Width="250px" onblur="return BlankControlExpense()" TabIndex="102" />
                    <div>
                        <asp:Label ID="lblExpenseInsurerClaimId" runat="server" Text="Please enter insurer claim #" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>

                </td>
            </tr>
            <tr>
                <td class="right top">Insured Name</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtExpenseInsurerName" TabIndex="103" Width="250px" onblur="return BlankControlExpense()" />
                    <div>
                        <asp:Label ID="lblExpenseInsurerName" runat="server" Text="Please enter insurer name." Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Carrier</td>
                <td class="redstar top">*</td>
                <td>
                    <%--<ig:WebTextEditor runat="server" ID="txtCarrier" TabIndex="1" Width="250px" />--%>
                    <asp:DropDownList ID="ddlExpenseClaimCarrier" runat="server" Width="258px" TabIndex="104" />
                    <div>
                        <asp:Label ID="lblExpenseClaimCarrier" runat="server" Text="Please select carrier." Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Adjuster
                </td>
                <td class="redstar top">*</td>
                <td class="nowrap">
                    <ig:WebTextEditor ID="txtExpenseClaimAdjuster" runat="server" Enabled="false" Width="250px" onblur="return BlankControlExpense()" TabIndex="105" />
                    <a href="javascript:findExpenseAdjusterForClaimDialog();">
                        <asp:Image ID="ImageExpense" runat="server" ImageUrl="~/Images/searchbg.png" ImageAlign="Top" />
                    </a>
                    <div>
                        <asp:Label ID="lblExpenseClaimAdjuster" runat="server" Text="Please select adjuster." Font-Size="Small" ForeColor="Red"></asp:Label>

                    </div>
                </td>
            </tr>

            <tr>
                <td class="right top">Adjuster Company Name</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtExpenseAdjusterComapnyName" TabIndex="106" Width="250px" onblur="return BlankControlExpense()" ReadOnly="true" />
                    <div>
                        <asp:Label ID="lblExpenseAdjusterComapnyName" runat="server" Text="Please enter company name." Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Expense Date</td>
                <td class="redstar top">*</td>
                <td>
                    <div class="getdate">
                        <ig:WebDatePicker ID="txtExpenseDate" runat="server" CssClass="date_picker" onblur="return BlankControlExpense()">
                            <Buttons>
                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                            </Buttons>
                        </ig:WebDatePicker>
                    </div>
                    <div>
                        <asp:Label ID="lblExpenseDate" runat="server" Text="Please enter expense date." Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Description</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtExpenseCommentNote" MaxLength="500" TabIndex="107" TextMode="MultiLine" MultiLine-Rows="3" Width="100%" onblur="return BlankControlExpense()"></ig:WebTextEditor>
                    <div>

                        <asp:Label ID="lblExpenseCommentNote" runat="server" Text="Please enter description." Font-Size="Small" ForeColor="Red"></asp:Label>


                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Reimburse</td>
                <td class="redstar top"></td>
                <td>
                    <asp:CheckBox runat="server" ID="chkExpenseReimburse" TabIndex="108" ReadOnly="true" />

                </td>
            </tr>
            <tr>
                <td class="right top">Quantity</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebNumericEditor ID="txtExpenseQty" runat="server" MinDecimalPlaces="2" Width="80px" TabIndex="109">
                    </ig:WebNumericEditor>

                </td>
            </tr>
            <tr>
                <td class="right top"></td>
                <td class="redstar top"></td>
                <td>OR                   
                </td>
            </tr>
            <tr>
                <td class="right top">Amount</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebNumericEditor ID="txtExpenseAmount" runat="server" MinDecimalPlaces="2" Width="80px" TabIndex="110">
                    </ig:WebNumericEditor>

                </td>
            </tr>

            <tr>
                <td class="right top">E-mail To</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor ID="txtEmailToExpense" runat="server" Width="100%" TabIndex="60" disabled="true" />
                    <%-- onblur="return CheckEmailTo()"--%>
                    <div>
                        <asp:Label ID="lblEmailToExpense" runat="server" Text="Please select any recipient." Font-Size="Small" ForeColor="Red"></asp:Label>

                    </div>
                </td>
            </tr>

            <tr>
                <td class="right top">Select Recipients</td>
                <td class="redstar top"></td>
                <td>
                    <div style="width: 552px; height: 250px; overflow-y: auto; border: 1px solid;">
                        <asp:GridView Width="100%" ID="gvSelectRecipientsExpense" CssClass="gridView csk-box" runat="server" AutoGenerateColumns="False" CellPadding="2">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelectRecipientsExpense" runat="server" Class="gvSelectRecipientsClass checkbox-align-center" onclick="return HandleCheckBoxExpense(this);" />
                                        <asp:HiddenField ID="hdnSelectRecipientsExpense" runat="server" Value='<%#Eval("ContactID") %>' />
                                        <asp:HiddenField ID="hdn2SelectRecipientsExpense" runat="server" Value='<%#Eval("IdOf") %>' />
                                        <asp:HiddenField ID="hdnEmailSelectRecipientsExpense" runat="server" Value='<%#Eval("Email") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="125px" Wrap="False" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="First Name">
                                    <ItemTemplate>
                                        <%#Eval("FirstName") %>
                                        <%-- <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("SecUser.UserName") %>' />--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Name">
                                    <ItemTemplate>
                                        <%#Eval("LastName") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <%#Eval("CompanyName") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <%#Eval("Email") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="Contact Type">
                    <ItemTemplate>
                        <%#Eval("ContactType") %>                       
                    </ItemTemplate>
                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>

                    </div>
                </td>
            </tr>

            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnSaveClaimExpense" runat="server" Text="Save" CssClass="mysubmit" OnClientClick="return SaveClaimExpense();" />
                    &nbsp;
                    <%--<asp:Button ID="btnCancelClaimService" runat="server" Text="Cancel" CssClass="mysubmit"   CausesValidation="false" />--%>
                </td>
            </tr>
        </table>


    </div>
</div>

<div id="div_ClaimAdjustersListExpense" style="display: none; width: 90%;" title="Select Adjuster">
    <div class="boxContainer">
        <div class="section-title">
            Adjusters
        </div>
        <ig:WebDataGrid ID="claimAdjusterGridExpense" runat="server" CssClass="gridView smallheader" DataSourceID="EntityDataSourceExpense"
            AutoGenerateColumns="false" Height="300px"
            Width="100%">
            <Columns>
                <ig:BoundDataField DataFieldName="AdjusterId" Key="AdjusterId" Header-Text="ID" Width="50px" />
                <ig:BoundDataField DataFieldName="AdjusterName" Key="AdjusterName" Header-Text="Adjuster Name" />
                <ig:BoundDataField DataFieldName="email" Key="email" Header-Text="E-mail Address" />
            </Columns>
            <Behaviors>

                <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                    ThresholdFactor="0.5" Enabled="true" />

                <ig:Selection RowSelectType="Single" Enabled="True" CellClickAction="Row">
                    <SelectionClientEvents RowSelectionChanged="claimAdjusterGrid_rowsSelectedExpense" />
                </ig:Selection>
            </Behaviors>
        </ig:WebDataGrid>
    </div>
</div>

<div id="div_ExpenseSave" style="display: none; width: 90%;" title="Add Expense">
    <div class="boxContainer">
        Expense saved successfully.
        
        
    </div>
</div>

<div id="div_ExpenseNotSave" style="display: none; width: 90%;" title="Add Expense">
    <div class="boxContainer">
        Expense not saved successfully.
        
        
    </div>
</div>

<asp:EntityDataSource ID="EntityDataSourceExpense" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
    EnableFlattening="False" EntitySetName="AdjusterMaster"
    Where="it.ClientId = @ClientID && it.Status = true"
    OrderBy="it.AdjusterName Asc">
    <WhereParameters>
        <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>

<asp:HiddenField ID="hf_ClaimAdjusterIDExpense" runat="server" Value="0" />
<asp:HiddenField ID="hf_ClaimIdForExpense" runat="server" Value="0" />
<asp:HiddenField ID="hdnEmailToListExpense" runat="server" Value="0" />




<%--Expense change div--%>

<%--invoice add div--%>

<div id="div_InvoiceGenerate" style="display: none; width: 90%;" title="Add Auto Invoice">
    <div class="boxContainer">
        <div class="section-title">
            Add Auto Invoice
        </div>
        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">
            <tr>
                <td class="right">Carrier Invoice Profile</td>
                <td class="redstar"></td>
                <td>
                    <asp:DropDownList ID="ddlCarrierInvoiceProfile" runat="server">
                        <%-- <asp:ListItem Value="0" Text="---Select---"></asp:ListItem>--%>
                    </asp:DropDownList>
                    <div>
                        <asp:Label ID="lblCarrierInvoiceProfile" runat="server" Text="Please select carrier invoice profile." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>

                    </div>
                </td>
            </tr>

            <tr>
                <td class="right">Fee Invoice Designation</td>
                <td class="redstar"></td>
                <td>
                    <uc1:ucFeeDesignation ID="ddlPropertyFeeInvoiceDesignation" runat="server" />
                    <div>
                        <asp:Label ID="lblPropertyFeeInvoiceDesignation" runat="server" Text="Please select carrier invoice profile." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>

                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Gross Claim Payable</td>
                <td></td>
                <td>
                    <ig:WebCurrencyEditor ID="txtGrossLossPayable" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="38">
                        <ClientEvents ValueChanged="calculateNetClaimPayable" />
                    </ig:WebCurrencyEditor>

                </td>
            </tr>
            <tr>
                <td class="right">Depreciation</td>
                <td></td>
                <td>
                    <ig:WebCurrencyEditor ID="txtDepreciation" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="39">
                        <ClientEvents ValueChanged="calculateNetClaimPayable" />
                    </ig:WebCurrencyEditor>

                </td>
            </tr>
            <tr>
                <td class="right">Deductible</td>
                <td></td>
                <td>
                    <ig:WebCurrencyEditor ID="txtPolicyDeductible" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="40">
                        <ClientEvents ValueChanged="calculateNetClaimPayable" />
                    </ig:WebCurrencyEditor>

                </td>
            </tr>
            <tr>
                <td class="right">Net Claim Payable</td>
                <td></td>
                <td>
                    <ig:WebCurrencyEditor ID="txtNetClaimPayable" runat="server" MinDecimalPlaces="2" DataMode="Decimal" Enabled="False" TabIndex="41" />

                </td>
            </tr>
            <tr>
                <td class="right"><%--Auto-Invoice Claim--%></td>
                <td></td>
                <td>
                    <asp:CheckBox ID="cbxInvoiceReady" runat="server" TabIndex="42" Visible="False" />
                </td>
            </tr>
            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnAutoInvoiceGenerate" runat="server" Text="Save" CssClass="mysubmit" OnClientClick="return AutoInvoiceGenerate();" />
                    &nbsp;
                    <%--<asp:Button ID="btnCancelClaimService" runat="server" Text="Cancel" CssClass="mysubmit"   CausesValidation="false" />--%>
                </td>
            </tr>

        </table>


    </div>
</div>

<div id="div_InvoiceSave" style="display: none; width: 90%;" title="Auto Invoice">
    <div class="boxContainer">
        <asp:Label ID="lblInvoiceSave" runat="server"></asp:Label>



    </div>
</div>

<div id="div_InvoiceNotSave" style="display: none; width: 90%;" title="Auto Invoice">
    <div class="boxContainer">
        Auto invoice not generated successfully. 
    </div>
</div>
<asp:HiddenField ID="hf_ClaimIdForInvoice" runat="server" Value="0" />

<%--invoice add div--%>

<%--upload docs  div--%>
<div id="documentUpload" style="display: none;" title="Document Upload">
    <table style="width: 100%; border-collapse: separate; border-spacing: 2px; padding: 2px; text-align: left;" border="0" class="editForm">
        <tr>
            <td class="right">Category</td>
            <td class="redstar"></td>
            <td>
                <asp:DropDownList ID="ddlDocumentCategory" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 10%;" class="top">Description</td>
            <td class="redstar top"></td>
            <td>
                <textarea id="txtDocumentDescription" maxlength="500" rows="5" style="width: 100%;"></textarea>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div id="webUpload">
                </div>
            </td>
        </tr>
    </table>
</div>
<asp:HiddenField ID="hf_claimIDUploaddocument" runat="server" Value="0" />


<div id="documentUploadSuccess" style="display: none; width: 90%;" title="Document Upload">
    <div class="boxContainer">
        Document uploaded successfully. 
    </div>
</div>
<%--upload docs  div--%>


<%--upload docs  div--%>
<div id="divAddPhoto" style="display: none;" title="Add Photo">
    <table style="width: 100%; border-collapse: separate; border-spacing: 2px; padding: 2px; text-align: left;" border="0" class="editForm">
        <tr>
            <td>
                <div id="webuUpload2">
                </div>
                <div id="result">
                </div>
            </td>
        </tr>
    </table>
</div>
<asp:HiddenField ID="hf_claimIDAddPhoto" runat="server" Value="0" />


<div id="divAddPhotoSuccess" style="display: none; width: 90%;" title="Document Upload">
    <div class="boxContainer">
        Document uploaded successfully. 
    </div>
</div>

<div id="div_NoClaimPhotoUpload" style="display: none; width: 90%;" title="No Claim">
    <div class="boxContainer">
        A claim number is required to upload photo.     
        
    </div>
</div>


<%--upload docs  div--%>



<script type="text/javascript">

    var listEmail = [];

    $(document).ready(function () {

        //$('#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_gvSelectRecipients').find('input[type=checkbox]:checked').each(function () {
        $('.gvSelectRecipientsClass.checkbox-align-center').find('input[type=checkbox]').click(function () {
            var id = jQuery(this).attr('id');
            var hdnfield = id.replace("chk", "hdnEmail");
            var emailto = $("#<%= txtEmailTo.ClientID %>").val();
            if (emailto == "") {
                $("#<%= lblEmailTo.ClientID %>").show();
            }
            else {
                $("#<%= lblEmailTo.ClientID %>").hide();
            }

            if (this.checked) {
                var result = validateEmail($("#" + hdnfield).val());
                if (result) {
                    if (!IsExists(listEmail, $("#" + hdnfield).val())) {
                        listEmail.push($("#" + hdnfield).val());
                    }
                    PopulateEmailTo(listEmail);
                } else {
                    alert('You can not select this recipient as no email available for this recipient');
                    this.checked = false;
                }
            }
            else {
                listEmail = RemoveElementFromArray(listEmail, $("#" + hdnfield).val());
                PopulateEmailTo(listEmail);
            }

        });
    });

    function PopulateEmailTo(listArray) {
        var stringEmailTo = ''
        for (counter = 0; counter < listArray.length; counter++) {
            if (counter != 0) {
                stringEmailTo += ";";
            }
            stringEmailTo += listArray[counter];
        }
        $("#<%= txtEmailTo.ClientID %>").val(stringEmailTo);

        var emailto = $("#<%= txtEmailTo.ClientID %>").val();
        if (emailto == "") {
            $("#<%= lblEmailTo.ClientID %>").show();
        }
        else {
            $("#<%= lblEmailTo.ClientID %>").hide();
        }
    }

    function IsExists(array, element) {
        for (var counter = 0; counter < array.length; counter++) {
            if (array[counter] == element) {
                return true;
            }
        }
        return false;
    }

    function RemoveElementFromArray(array, element) {
        var listArray = [];
        if (IsExists(array, element)) {
            for (var counter = 0; counter < array.length; counter++) {
                if (array[counter] != element) {
                    listArray.push(array[counter]);
                }
            }
        }
        return listArray;
    }




    function SelectAllCheckboxes1(chk) {

        $('#<%=gvUserLeads.ClientID%>').find("input:checkbox").each(function () {
            if (this != chk) {
                this.checked = chk.checked;
            }
        });
    }

    $(".gvSelectLeadsClass").change(function () {
        var all = $('.gvSelectLeadsClass');
        if (all.length === all.find(':checked').length) {
            $(".cbAll").attr("checked", true);
        } else {
            $(".cbAll").attr("checked", false);
        }
    });


    function checkSelected(btn) {

        var flg;

        var gridView = document.getElementById("<%=gvUserLeads.ClientID %>");
        var checkBoxes = gridView.getElementsByTagName("input");
        for (var i = 0; i < checkBoxes.length; i++) {
            if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                flg = true;
                return;
            }
        }
        flg = false;

        if (!flg) {



            $("#divSelectRecords").dialog({
                modal: false,
                width: 300,
                close: function (e, ui) {
                    $(this).dialog('destroy');
                },
                buttons:
                   {
                       //'Done': function () {
                       //    $(this).dialog('close');
                       //}
                   }
            });
            //alert("Please select Record(s).");


            return false
        }

        return true;
    }


    //String.Format = function () {
    //	var s = arguments[0];
    //	for (var i = 0; i < arguments.length - 1; i++) {
    //		var reg = new RegExp("\\{" + i + "\\}", "gm");
    //		s = s.replace(reg, arguments[i + 1]);
    //	}
    //	return s;
    //}

    //var dialogConfirmed = false;
    //function ConfirmDialog(obj, title, dialogText) {
    //	if (!dialogConfirmed) {
    //		$('body').append(String.Format("<div id='dialog' title='{0}'><p>{1}</p></div>",
    //                title, dialogText));

    //		$('#dialog').dialog
    //            ({
    //            	height: 150,
    //            	modal: true,
    //            	resizable: false,
    //            	draggable: true,
    //            	close: function (event, ui) { $('body').find('#dialog').remove(); },
    //            	buttons:
    //                {
    //                	'Yes': function () {
    //                		$(this).dialog('close');
    //                		dialogConfirmed = true;
    //                		if (obj) obj.click();
    //                	},
    //                	'No': function () {
    //                		$(this).dialog('close');
    //                	}
    //                }
    //            });
    //	}

    //	return dialogConfirmed;
    //}
    function showSearch() {
        var css = $('#search_div').css('display');

        if (css == 'none') {
            $('#search_div').show();
            $("#search_button").attr('value', 'Hide Search');
        } else {
            $('#search_div').hide();
            $("#search_button").attr('value', 'Search');
        }
    }


    function showNotesPanel(img)
    {
        var id = img.id;
        var id2 = img.id;
        var hdnfield = id.replace("imgbtnNotes", "hfClaimNotes");

        var leadidhdn = id2.replace("imgbtnNotes", "hfLeadId");
        var claimId = $("#" + hdnfield).val();
        var leadId = $("#" + leadidhdn).val();

        if (claimId > 0) {
            //alert(claimId);
            $("#<%= hdnClaimIDNew.ClientID %>").val(claimId);
            $("#<%= hdnLeadIDNew.ClientID %>").val(leadId);

            //$(".displayblock").prop("disabled", true);



            // check tutorial mode and step 6
            var stepNumber = readCookie("currentStep");

            if (stepNumber == 6)
            {

                $.powerTip.hide();
                $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_gvUserLeads tr:contains('10001')").find($("input[title='Add Notes']"))
                    .data('powertipjq', $('<div style="height:auto;width:214px;white-space: normal;"><b>Add Notes and Log Time with 1-Click</b><br/>' +
                            '<p>Easy-to-Add Notes or Send Notes to adjusters' +
                            'or any other stakeholders.  You may also Log Service Time' +
                            'here and get it all done quickly in a single leap and bound!' +
                            'Any time here is auto-written into the Claim\'s Time & Expense</p> <hr/>' +
                            '<p class="step">Step 6 of 12 <input type="button" id="step6" class="floatRgt" value="Next >>" /></p></div>'));

                $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_gvUserLeads tr:contains('10001')").find($("input[title='Add Notes']")).powerTip({
                    placement: 'w',
                    smartPlacement: false,
                    manual: true,

                });

                //$.powerTip.hide();
                $.powerTip.show($("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_gvUserLeads tr:contains('10001')").find($("input[title='Add Notes']")));
            }
            else
            {

                //                $.powerTip.hide();
            }


            // show dialog
            $("#div_ClaimServiceAdjustersList").dialog({
                modal: false,
                width: 700,
                dialogClass: 'dialogClass',
                close: function (e, ui) {
                    $(this).dialog('destroy');
                    $(".displayblock").prop("disabled", false);

                },
            });

            //blank control
            $("#<%= txtServiceQty.ClientID %>").val("");
            $("#<%= txtServiceDescription.ClientID %>").val("");
            $("#<%= txtServiceAdjuster.ClientID %>").val("");
            $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_txtServiceDate .igte_Office2007BlueInner').children().val("");
            $("#<%= ddlInvoiceServiceType.ClientID %>").prop('selectedIndex', 0);

            $("#<%= lblInvoiceServiceType.ClientID %>").hide();
            $("#<%= lblServiceAdjuster.ClientID %>").hide();
            $("#<%= lblServiceDate.ClientID %>").hide();
            $("#<%= lblServiceQty.ClientID %>").hide();
            $("#<%= lblServiceDescription.ClientID %>").hide();
            $("#<%= lblEmailTo.ClientID %>").hide();



        }
        else
        {

            // check tutorial mode and step 6
            var stepNumber = readCookie("currentStep");

            if (stepNumber == 6)
            {
                $("#stepAddNote").show();
            }
            else
            {
                $("#stepAddNote").hide();
            }

            // show dialog
            $(".displayblock").prop("disabled", true);
            $("#div_NoClaim").dialog({
                modal: false,
                width: 300,
                close: function (e, ui)
                {
                    $(this).dialog('destroy');
                    $(".displayblock").prop("disabled", false);
                },
                buttons:
                   {
                       //'Done': function () {
                       //    $(this).dialog('close');
                       //}
                   }
            });
        }
        return false;
    }



    function findAdjusterForServiceDialog() {
        var serviceAdjuster = $("#<%= txtServiceAdjuster.ClientID %>").val();

        // show dialog
        $("#div_AdjustersList").dialog({
            modal: false,
            width: 600,
            dialogClass: 'adjustDialogClass',
            close: function (e, ui) {
                $(this).dialog('destroy');
                if (serviceAdjuster != "") {
                    $("#<%= lblServiceAdjuster.ClientID %>").hide();
                }
            },
            buttons:
               {
                   'Done': function () {
                       $(this).dialog('close');
                       if (serviceAdjuster != "") {
                           $("#<%= lblServiceAdjuster.ClientID %>").hide();
                       }
                   }
               }
        });
       }

       function claimServiceAdjusterGrid_rowsSelected(sender, args) {
           var selectedRows = args.getSelectedRows();

           var adjusterID = args.getSelectedRows().getItem(0).get_cell(0).get_text();
           var adjusterName = args.getSelectedRows().getItem(0).get_cell(1).get_text();

           $("#<%= hf_serviceAdjusterID.ClientID %>").val(adjusterID);
           $find("<%= txtServiceAdjuster.ClientID %>").set_value(adjusterName);
       }

       function SaveNotes() {
           var claimID = $("#<%= hdnClaimIDNew.ClientID %>").val();
           var leadID = $("#<%= hdnLeadIDNew.ClientID %>").val();
           var serviceQty = $("#<%= txtServiceQty.ClientID %>").val();
           var serviceDate = $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_txtServiceDate .igte_Office2007BlueInner').children().val();

           var descp = $("#<%= txtServiceDescription.ClientID %>").val();

           //var skillsSelect = document.getElementById("ddlInvoiceServiceType");
           //var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
           $("#<%= lblServiceQty.ClientID %>").hide();
           $("#<%= lblServiceDescription.ClientID %>").hide();
           $("#<%= lblInvoiceServiceType.ClientID %>").hide();
           $("#<%= lblServiceAdjuster.ClientID %>").hide();
           $("#<%= lblServiceDate.ClientID %>").hide();

           $("#<%= lblEmailTo.ClientID %>").hide();
           var invoiceServiceType = $("#<%= ddlInvoiceServiceType.ClientID %> :selected").text();
           var invoiceServiceTypeId = $("#<%= ddlInvoiceServiceType.ClientID %>").val();
           var serviceAdjuster = $("#<%= txtServiceAdjuster.ClientID %>").val();
           var emailTo = $("#<%= txtEmailTo.ClientID %>").val();
           var serviceAdjustId = $("#<%= hf_serviceAdjusterID.ClientID %>").val();

           if (invoiceServiceTypeId == 0) {

               $("#<%= lblInvoiceServiceType.ClientID %>").show();
                $("<%=popUpService%>").show();
            }
           if (serviceQty == "") {
               $("#<%= lblServiceQty.ClientID %>").show();
           }

           if (descp == "") {
               $("#<%= lblServiceDescription.ClientID %>").show();
           }
          

        if (serviceAdjuster == "") {
            $("#<%= lblServiceAdjuster.ClientID %>").show();
        }

        if (serviceDate == "") {
            $("#<%= lblServiceDate.ClientID %>").show();
        }

        if (emailTo == "") {
            $("#<%= lblEmailTo.ClientID %>").show();
        }

        if (serviceQty != "" && descp != "" && invoiceServiceType != "" && serviceAdjuster != "" && serviceDate != "" && emailTo != "") {

            var myParams = "{ 'claimID':'" + claimID + "', 'serviceQty':'" + serviceQty + "','serviceDate':'" + serviceDate + "', 'descp':'" + descp + "','invoiceServiceType':'" + invoiceServiceType + "', 'invoiceServiceTypeId':'" + invoiceServiceTypeId + "','serviceAdjuster':'" + serviceAdjuster + "', 'serviceAdjustId':'" + serviceAdjustId + "','leadID':'" + leadID + "','emailTo':'" + emailTo + "'}";

            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/SaveNotes") %>',
                success: function (data) {

                    $("#<%= txtServiceQty.ClientID %>").val("");
                    $("#<%= txtServiceDescription.ClientID %>").val("");
                    $("#<%= txtServiceAdjuster.ClientID %>").val("");
                    $('.igte_Office2007BlueInner').children().val("");
                    $("#<%= ddlInvoiceServiceType.ClientID %>").prop('selectedIndex', 0);

                    //$("#div_ClaimServiceAdjustersList").dialog('destroy');
                    $("#div_ClaimServiceAdjustersList").dialog('close');
                    $("#div_NoteSave").dialog({
                        modal: false,
                        width: 300,
                        close: function (e, ui) {
                            $(this).dialog('destroy');
                            try {
                                $("#div_ClaimServiceAdjustersList").dialog('destroy');
                            } catch (ex) { }
                            try {
                                $("#div_AdjustersList").dialog('destroy');
                            } catch (ex) { }
                            window.location.href = window.location.href + '&search=all';
                        },
                        buttons:
                           {
                               //'Done': function () {
                               //    $(this).dialog('close');
                               //}
                           }
                    });



                },
                error: function () {
                    $("#div_NoteNotSave").dialog({
                        modal: false,
                        width: 300,
                        close: function (e, ui) {
                            $(this).dialog('destroy');

                        },
                        buttons:
                           {
                               //'Done': function () {
                               //    $(this).dialog('close');
                               //}
                           }
                    });
                }
            });

        }
        return false;
    }

    function BlankControl() {
        var serviceQty = $("#<%= txtServiceQty.ClientID %>").val();
        var serviceDate = $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_txtServiceDate .igte_Office2007BlueInner').children().val();
        var descp = $("#<%= txtServiceDescription.ClientID %>").val();

    var serviceAdjuster = $("#<%= txtServiceAdjuster.ClientID %>").val();
        var emailTo = $("#<%= txtEmailTo.ClientID %>").val();


        if (serviceQty != "") {
            $("#<%= lblServiceQty.ClientID %>").hide();
    }

    if (descp != "") {
        $("#<%= lblServiceDescription.ClientID %>").hide();
    }


    if (serviceAdjuster != "") {
        $("#<%= lblServiceAdjuster.ClientID %>").hide();
    }

    if (serviceDate != "") {
        $("#<%= lblServiceDate.ClientID %>").hide();
        }
        if (emailTo != "") {
            $("#<%= lblEmailTo.ClientID %>").hide();
        }
        return false;
    }



    $("#<%= ddlInvoiceServiceType.ClientID %>").change(function () {
        var invoiceServiceTypeId = $("#<%= ddlInvoiceServiceType.ClientID %>").val();

        if (invoiceServiceTypeId != 0) {

            $("#<%= lblInvoiceServiceType.ClientID %>").hide();
        }
    });




</script>

<script>

    //save claim status
    function SaveClaimStatus() {
        var idOf = [];
        var recipientId = [];
        var recipientEmail = [];
        $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_gvSelectRecipientsStatus').find('input[type=checkbox]:checked').each(function () {
            var id = jQuery(this).attr('id');
            var hdnfield = id.replace("chk", "hdn");
            recipientId.push($("#" + hdnfield).val());

            var hdnfield2 = id.replace("chk", "hdn2");
            idOf.push($("#" + hdnfield2).val());
        })

       // $("# lblEmailToStatus.ClientID %>").hide();

        var claimStatus = $("#<%= ddlClaimStatusReview.ClientID %>").val();
        var claimStatusName = $("#<%= ddlClaimStatusReview.ClientID %> :selected").text();
        var insurerClaimId = $("#<%= txtInsurerClaimId.ClientID %>").val();

        var insurerName = $("#<%= txtInsurerName.ClientID %>").val();
        var claimAdjuster = $("#<%= txtClaimAdjuster.ClientID %>").val();
        var claimAdjusterId = $("#<%= hf_ClaimAdjusterID.ClientID %>").val();
        var adjusterComapnyName = $("#<%= txtAdjusterComapnyName.ClientID %>").val();
        var updatedby = "";
        var commentNote = $("#<%= txtCommentNote.ClientID %>").val();
        var emailTo = $("#<%= txtEmailToStatus.ClientID %>").val();
        emailTo = '';

        var carrier = $("#<%= ddlClaimCarrier.ClientID %> :selected").text();
        var carrierID = $("#<%= ddlClaimCarrier.ClientID %>").val();
        var claimID = $("#<%= hf_ClaimIdForStatus.ClientID %>").val();


        if (claimStatus == 0) {
            $("#<%= lblClaimStatusReview.ClientID %>").show();
        }
        if (insurerClaimId == "") {
            $("#<%= lblInsurerClaimId.ClientID %>").show();
        }
        if (insurerName == "") {
            $("#<%= lblInsurerName.ClientID %>").show();
        }
        if (claimAdjuster == "") {
            $("#<%= lblClaimAdjuster.ClientID %>").show();
         }
         //if (adjusterComapnyName == "") {
            // $("#lblAdjusterComapnyName.ClientID %>").show();
       // }
        if (commentNote == "") {
            $("#<%= lblCommentNote.ClientID %>").show();
        }
        if (carrierID == 0) {
            $("#<%= lblClaimCarrier.ClientID %>").show();
        }
       // if (recipientId.length == 0) {
            //$("#lblEmailToStatus.ClientID %>").show();
       // }


        emailTo = "";
        if (claimStatus != 0 && insurerClaimId != "" && insurerName != "" && claimAdjusterId != 0 && commentNote != "" && carrierID != 0 && claimID != 0) // && emailcheck==true    --remove by client && adjusterComapnyName != ""   ---&& recipientId.length > 0
        {

            var myParams = "{ 'claimStatus':'" + claimStatus + "', 'insurerClaimId':'" + insurerClaimId + "','insurerName':'" + insurerName + "','claimAdjusterId':'" + claimAdjusterId + "', 'adjusterComapnyName':'" + adjusterComapnyName + "','updatedby':'" + updatedby + "', 'commentNote':'" + commentNote + "','emailTo':'" + emailTo + "','carrierID':'" + carrierID + "','claimID':'" + claimID + "','recipientId':'" + recipientId + "','claimAdjuster':'" + claimAdjuster + "','claimStatusName':'" + claimStatusName + "','carrier':'" + carrier + "','idOf':'" + idOf + "'}";


            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/SaveClaimStatus") %>',
                success: function (data) {

                    $("#div_ClaimStatusReview").dialog('close');

                    //control blank
                    $("#<%= txtEmailToStatus.ClientID %>").val('');

                    $("#<%= txtCommentNote.ClientID %>").val('');

                    $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_gvSelectRecipientsStatus').find('input[type=checkbox]:checked').each(function () {
                        var id = jQuery(this).attr('id');
                        $("#" + id).prop('checked', false);
                    })

                    $("#div_StatusSave").dialog({
                        modal: false,
                        width: 300,
                        close: function (e, ui) {
                            $(this).dialog('destroy');



                            window.location.href = window.location.href + '&search=all';
                        },
                        buttons:
                                   {
                                       //'Done': function () {
                                       //    $(this).dialog('close');
                                       //}
                                   }
                    });



                },
                error: function () {

                    $("#div_StatusNotSave").dialog({
                        modal: false,
                        width: 300,
                        close: function (e, ui) {
                            $(this).dialog('destroy');

                        },
                        buttons:
                                   {
                                       //'Done': function () {
                                       //    $(this).dialog('close');
                                       //}
                                   }
                    });

                }
            });

        }

        return false;
    }


    function BlankControl2() {



        var insurerClaimId = $("#<%= txtInsurerClaimId.ClientID %>").val();

        var insurerName = $("#<%= txtInsurerName.ClientID %>").val();




        var commentNote = $("#<%= txtCommentNote.ClientID %>").val();


        if (insurerClaimId != "") {
            $("#<%= lblInsurerClaimId.ClientID %>").hide();
        }
        if (insurerName != "") {
            $("#<%= lblInsurerName.ClientID %>").hide();
        }


        if (commentNote != "") {
            $("#<%= lblCommentNote.ClientID %>").hide();
        }

        return false;
    }


    $("#<%= ddlClaimStatusReview.ClientID %>").change(function () {
        var claimStatus = $("#<%= ddlClaimStatusReview.ClientID %>").val();

        if (claimStatus != 0) {

            $("#<%= lblClaimStatusReview.ClientID %>").hide();
        }
    });

    $("#<%= ddlClaimCarrier.ClientID %>").change(function () {
        var claimCarrier = $("#<%= ddlClaimCarrier.ClientID %>").val();

        if (claimCarrier != 0) {

            $("#<%= lblClaimCarrier.ClientID %>").hide();
        }
    });




    //open popup for status change
    function HandleStatusClick(elem) {

        $("#<%= lblClaimStatusReview.ClientID %>").hide();
        $("#<%= lblInsurerClaimId.ClientID %>").hide();
        $("#<%= lblInsurerName.ClientID %>").hide();
        $("#<%= lblClaimAdjuster.ClientID %>").hide();
     //   $("# lblAdjusterComapnyName.ClientID %>").hide();
        $("#<%= lblCommentNote.ClientID %>").hide();
        $("#<%= lblClaimCarrier.ClientID %>").hide();
       // $("# lblEmailToStatus.ClientID %>").hide();


        var id = jQuery(elem).attr('id');
        var hdnfieldclaimid = id.replace("lbtn", "hfClaimId");
        var claimID = $("#" + hdnfieldclaimid).val()
        $("#<%= hf_ClaimIdForStatus.ClientID %>").val(claimID);
    var myParams = "{ 'claimID':'" + claimID + "'}";


    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: myParams,
        url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/GetStatusData") %>',
            success: function (data) {


                $("#<%= ddlClaimStatusReview.ClientID %>").val(data.d[0]);
                $("#<%= txtInsurerClaimId.ClientID %>").val(data.d[1]);
                $("#<%= txtInsurerName.ClientID %>").val(data.d[2]);
                $("#<%= ddlClaimCarrier.ClientID %>").val(data.d[3]);
                $("#<%= txtClaimAdjuster.ClientID %>").val(data.d[4]);
                $("#<%= hf_ClaimAdjusterID.ClientID %>").val(data.d[5]);

            },
            error: function () {

            }
        });


        // read current step
        var currentStep = readCookie("currentStep");

        if (currentStep == 7) {

            $.powerTip.hide();
            $(rowWithClaimNumber).find($("a[title='Change Status']")).data('powertipjq', $([
       '<div style="height:auto;width:180px;white-space: normal;"><p><b>Change Claim Status On-The-Fly</b> </p>',
       '<p>How would you like to change the claim status from right here?',
       'WAIT…  You can!  Click on any claim status and it pops up the',
       'Claim Status Change window, and lets you do it in 1 click!',
       '3-for-1, Updates Claim Log, XactAnalysis, AND sends emails!</p><hr/>',
       '<p>Step 7 of 12 <input type="button" style="margin-left: -2px; !important; float:right;"  id="step7" value="Next >>" /></p></div>'
            ].join('\n')));
            $(rowWithClaimNumber).find($("a[title='Change Status']")).powerTip({
                placement: 'e',
                smartPlacement: false,
                manual: true
            });

            $.powerTip.show($(rowWithClaimNumber).find($("a[title='Change Status']")));


        } else {

        }


        $("#div_ClaimStatusReview").dialog({
            modal: false,
            width: 761,
            dialogClass: 'claimStatusdialogClass',
            close: function (e, ui) {
                $(this).dialog('destroy');

            },
            //buttons:
            //   {
            //       'Done': function () {                       
            //           $(this).dialog('close');
            //       }
            //   }
        });




        return false;
    }





    //select recipient in status

    var listEmail2 = [];

    function HandleCheckBox(elem) {
        var id = jQuery(elem).attr('id');
        var hdnfield = id.replace("chk", "hdnEmail");
        if (elem.checked) {
            var result = validateEmail($("#" + hdnfield).val());
            if (result) {
                if (!IsExistsStatus(listEmail2, $("#" + hdnfield).val())) {
                    listEmail2.push($("#" + hdnfield).val());
                }
                PopulateEmailToStatus(listEmail2);
            } else {
                alert('You can not select this recipient as no email available for this recipient');
                elem.checked = false;
            }
        }
        else {
            listEmail2 = RemoveElementFromArrayStatus(listEmail2, $("#" + hdnfield).val());
            PopulateEmailToStatus(listEmail2);
        }

        var emailto = $("#<%= txtEmailToStatus.ClientID %>").val();
        //if (emailto == "") {
          //  $("#lblEmailToStatus.ClientID %>").show();
       // }
       // else {
        //    $("# lblEmailToStatus.ClientID %>").hide();
       // }

    }

    function PopulateEmailToStatus(listArray) {
        var stringEmailTo = ''
        for (counter = 0; counter < listArray.length; counter++) {
            if (counter != 0) {
                stringEmailTo += ";";
            }
            stringEmailTo += listArray[counter];
        }
        $("#<%= txtEmailToStatus.ClientID %>").val(stringEmailTo);

        var emailto = $("#<%= txtEmailToStatus.ClientID %>").val(stringEmailTo);
       // if (emailto == "") {
          //  $("# lblEmailToStatus.ClientID %>").show();
       // }
       // else {
         //   $("# lblEmailToStatus.ClientID %>").hide();
       // }
    }

    function IsExistsStatus(array, element) {
        for (var counter = 0; counter < array.length; counter++) {
            if (array[counter] == element) {
                return true;
            }
        }
        return false;
    }

    function RemoveElementFromArrayStatus(array, element) {
        var listArray = [];
        if (IsExistsStatus(array, element)) {
            for (var counter = 0; counter < array.length; counter++) {
                if (array[counter] != element) {
                    listArray.push(array[counter]);
                }
            }
        }
        return listArray;
    }



    function validateEmail(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }




    //for adjuster in status

    function findAdjusterForClaimDialog() {

        // show dialog
        $("#div_ClaimAdjustersList").dialog({
            modal: false,
            width: 600,
            dialogClass: "claimAdjustersListdialogClass",
            close: function (e, ui) {
                $(this).dialog('destroy');
            },
            buttons:
               {
                   'Done': function () {
                       $(this).dialog('close');
                   }
               }
        });
    }
    function claimAdjusterGrid_rowsSelected(sender, args) {
        var selectedRows = args.getSelectedRows();

        var adjusterID = args.getSelectedRows().getItem(0).get_cell(0).get_text();
        var adjusterName = args.getSelectedRows().getItem(0).get_cell(1).get_text();

        $("#<%= hf_ClaimAdjusterID.ClientID %>").val(adjusterID);
        $find("<%= txtClaimAdjuster.ClientID %>").set_value(adjusterName);

    }

</script>

<script>
    /// function for save expense
    function SaveClaimExpense() {
        var idOf = [];
        var recipientId = [];
        var recipientEmail = [];
        var reimbrance;
        $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_gvSelectRecipientsExpense').find('input[type=checkbox]:checked').each(function () {
            var id = jQuery(this).attr('id');
            var hdnfield = id.replace("chk", "hdn");
            recipientId.push($("#" + hdnfield).val());

            var hdnfield2 = id.replace("chk", "hdn2");
            idOf.push($("#" + hdnfield2).val());
        })

        $("#<%= lblEmailToExpense.ClientID %>").hide();

        var expenseType = $("#<%= ddlExpenseType.ClientID %>").val();
        var expenseTypeName = $("#<%= ddlExpenseType.ClientID %> :selected").text();
        var expenseQty = $("#<%= txtExpenseQty.ClientID %>").val();
        var expenseAmount = $("#<%= txtExpenseAmount.ClientID %>").val();

        var expenseDate = $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_txtExpenseDate .igte_Office2007BlueInner').children().val();

        var insurerClaimId = $("#<%= txtExpenseInsurerClaimId.ClientID %>").val();

        var insurerName = $("#<%= txtExpenseInsurerName.ClientID %>").val();
        var claimAdjuster = $("#<%= txtExpenseClaimAdjuster.ClientID %>").val();
        var claimAdjusterId = $("#<%= hf_ClaimAdjusterIDExpense.ClientID %>").val();
        var adjusterComapnyName = $("#<%= txtExpenseAdjusterComapnyName.ClientID %>").val();
        var updatedby = "";
        var commentNote = $("#<%= txtExpenseCommentNote.ClientID %>").val();
        var emailTo = $("#<%= txtEmailToExpense.ClientID %>").val();
        emailTo = '';

        var carrier = $("#<%= ddlExpenseClaimCarrier.ClientID %> :selected").text();
        var carrierID = $("#<%= ddlExpenseClaimCarrier.ClientID %>").val();
        var claimID = $("#<%= hf_ClaimIdForExpense.ClientID %>").val();

        if ($("#<%= chkExpenseReimburse.ClientID %>").is(':checked'))
            reimbrance = 1;  // checked
        else
            reimbrance = 0;



        if (expenseDate == "") {
            $("#<%= lblExpenseDate.ClientID %>").show();
        }


        if (expenseType == 0) {
            $("#<%= lblExpenseType.ClientID %>").show();
        }
        if (insurerClaimId == "") {
            $("#<%= lblExpenseInsurerClaimId.ClientID %>").show();
        }
        if (insurerName == "") {
            $("#<%= lblExpenseInsurerName.ClientID %>").show();
        }
        if (claimAdjuster == "") {
            $("#<%= lblClaimAdjuster.ClientID %>").show();
        }
        if (adjusterComapnyName == "") {
            $("#<%= lblExpenseAdjusterComapnyName.ClientID %>").show();
         }
         if (commentNote == "") {
             $("#<%= lblExpenseCommentNote.ClientID %>").show();
        }
        if (carrierID == 0) {
            $("#<%= lblExpenseClaimCarrier.ClientID %>").show();
        }
        if (recipientId.length == 0) {
            $("#<%= lblEmailToExpense.ClientID %>").show();
        }


        EmailTo = "";
        if (expenseType != 0 && insurerClaimId != "" && insurerName != "" && claimAdjusterId != 0 && adjusterComapnyName != "" && commentNote != "" && carrierID != 0 && claimID != 0 && recipientId.length > 0 && expenseDate != "") // && emailcheck==true
        {

            var myParams = "{ 'expenseType':'" + expenseType + "', 'insurerClaimId':'" + insurerClaimId + "','insurerName':'" + insurerName + "','claimAdjusterId':'" + claimAdjusterId + "', 'adjusterComapnyName':'" + adjusterComapnyName + "','updatedby':'" + updatedby + "', 'commentNote':'" + commentNote + "','emailTo':'" + emailTo + "','carrierID':'" + carrierID + "','claimID':'" + claimID + "','recipientId':'" + recipientId + "','claimAdjuster':'" + claimAdjuster + "','expenseTypeName':'" + expenseTypeName + "','carrier':'" + carrier + "','idOf':'" + idOf + "','expenseQty':'" + expenseQty + "','expenseAmount':'" + expenseAmount + "','expenseDate':'" + expenseDate + "','reimbrance':'" + reimbrance + "'}";


            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/SaveClaimExpense") %>',
                success: function (data) {

                    $("#div_ExpenseReview").dialog('close');

                    $("#div_ExpenseSave").dialog({
                        modal: false,
                        width: 300,
                        close: function (e, ui) {
                            $(this).dialog('destroy');

                            window.location.href = window.location.href + '&search=all';
                        },
                        buttons:
                                   {
                                       //'Done': function () {
                                       //    $(this).dialog('close');
                                       //}
                                   }
                    });



                },
                error: function () {

                    $("#div_ExpenseNotSave").dialog({
                        modal: false,
                        width: 300,
                        close: function (e, ui) {
                            $(this).dialog('destroy');

                        },
                        buttons:
                                   {
                                       //'Done': function () {
                                       //    $(this).dialog('close');
                                       //}
                                   }
                    });

                }
            });

        }

        return false;
    }



    //open popup for expense change
    function HandleExpenseClick(elem) {
        $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_gvSelectRecipientsExpense').find('input[type=checkbox]:checked').each(function () {
            var id = jQuery(this).attr('id');
            $("#" + id).prop('checked', false);
        })

        $("#<%= txtExpenseAmount.ClientID %>").val('');
        $("#<%= txtExpenseQty.ClientID %>").val('');
        $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_txtExpenseDate .igte_Office2007BlueInner').children().val('');
        $("#<%= ddlExpenseType.ClientID %>").prop('selectedIndex', 0);
        $("#<%= txtEmailToExpense.ClientID %>").val('');
        $("#<%= txtExpenseCommentNote.ClientID %>").val('');
        $("#<%= chkExpenseReimburse.ClientID %>").prop('checked', false);;

        $("#<%= lblExpenseType.ClientID %>").hide();
        $("#<%= lblExpenseInsurerClaimId.ClientID %>").hide();
        $("#<%= lblExpenseInsurerName.ClientID %>").hide();
        $("#<%= lblExpenseClaimAdjuster.ClientID %>").hide();
        $("#<%= lblExpenseAdjusterComapnyName.ClientID %>").hide();
        $("#<%= lblExpenseCommentNote.ClientID %>").hide();
        $("#<%= lblExpenseClaimCarrier.ClientID %>").hide();
        $("#<%= lblEmailToExpense.ClientID %>").hide();
        $("#<%= lblExpenseDate.ClientID %>").hide();
        $("#<%= lblExpenseType.ClientID %>").hide();


        var id = jQuery(elem).attr('id');
        var hdnfieldclaimid = id.replace("imgbtn", "hfClaimId");
        var claimID = $("#" + hdnfieldclaimid).val()
        $("#<%= hf_ClaimIdForExpense.ClientID %>").val(claimID);

        var myParams = "{ 'claimID':'" + claimID + "'}";
        if (claimID > 0) {

            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/GetStatusData") %>',
                success: function (data) {



                    $("#<%= txtExpenseInsurerClaimId.ClientID %>").val(data.d[1]);
                    $("#<%= txtExpenseInsurerName.ClientID %>").val(data.d[2]);
                    $("#<%= ddlExpenseClaimCarrier.ClientID %>").val(data.d[3]);
                    $("#<%= txtExpenseClaimAdjuster.ClientID %>").val(data.d[4]);
                    $("#<%= hf_ClaimAdjusterIDExpense.ClientID %>").val(data.d[5]);

                },
                error: function () {

                }
            });

            // change step 8 pop up postition
            var currentStep = readCookie("currentStep");


            if (currentStep == 8) {
                $.powerTip.hide();
                $(rowWithClaimNumber).find($("input[title='Add Expense']")).data('powertipjq', $([
       '<div style="height:auto;width:214px;white-space: normal;"><p><b>1-Click Expense Entry.  Is that Possible?</b></p>',
       '<p>Let\'s turn it up a notch!  How about we make it easy to',
       'also Add Expenses to a claim from right here?  Done.',
       'Add expenses and then create custom invoices from here!',
       'This automatically updates the claim log and writes to the claim.</p><hr/>',
       '<p class="step">Step 8 of 12 <input type="button" class="floatRgt" id="step8" value="Next >>" /></p></div>'
                ].join('\n')));

                $(rowWithClaimNumber).find($("input[title='Add Expense']")).powerTip({
                    placement: 'w',
                    smartPlacement: false,
                    manual: true
                });

                $.powerTip.show($(rowWithClaimNumber).find($("input[title='Add Expense']")));
            } else {
                //  $.powerTip.hide();
            }



            $("#div_ExpenseReview").dialog({
                modal: false,
                width: 761,
                dialogClass: 'dialogClass',
                close: function (e, ui) {
                    $(this).dialog('destroy');

                },
                //buttons:
                //   {
                //       'Done': function () {                       
                //           $(this).dialog('close');
                //       }
                //   }
            });

        }
        else {
            // show dialog
            $(".displayblock").prop("disabled", true);
            $("#div_NoClaim").dialog({
                modal: false,
                width: 300,
                close: function (e, ui) {
                    $(this).dialog('destroy');
                    $(".displayblock").prop("disabled", false);
                },
                buttons:
                   {
                       //'Done': function () {
                       //    $(this).dialog('close');
                       //}
                   }
            });
        }


        return false;
    }




    //for adjuster in status

    function findExpenseAdjusterForClaimDialog() {

        // show dialog
        $("#div_ClaimAdjustersListExpense").dialog({
            modal: false,
            width: 600,
            dialogClass: 'adjustDialogClass',
            close: function (e, ui) {
                $(this).dialog('destroy');
            },
            buttons:
               {
                   'Done': function () {
                       $(this).dialog('close');
                   }
               }
        });
    }
    function claimAdjusterGrid_rowsSelectedExpense(sender, args) {
        var selectedRows = args.getSelectedRows();

        var adjusterID = args.getSelectedRows().getItem(0).get_cell(0).get_text();
        var adjusterName = args.getSelectedRows().getItem(0).get_cell(1).get_text();

        $("#<%= hf_ClaimAdjusterIDExpense.ClientID %>").val(adjusterID);
    $find("<%= txtExpenseClaimAdjuster.ClientID %>").set_value(adjusterName);

}


//select recipient in status

var listEmailExpense = [];

function HandleCheckBoxExpense(elem) {
    var id = jQuery(elem).attr('id');
    var hdnfield = id.replace("chk", "hdnEmail");
    if (elem.checked) {
        var result = validateEmailExpense($("#" + hdnfield).val());
        if (result) {
            if (!IsExistsExpense(listEmailExpense, $("#" + hdnfield).val())) {
                listEmailExpense.push($("#" + hdnfield).val());
            }
            PopulateEmailToExpense(listEmailExpense);
        } else {
            alert('You can not select this recipient as no email available for this recipient');
            elem.checked = false;
        }
    }
    else {
        listEmailExpense = RemoveElementFromArrayExpense(listEmailExpense, $("#" + hdnfield).val());
        PopulateEmailToExpense(listEmailExpense);
    }

    var emailto = $("#<%= txtEmailToExpense.ClientID %>").val();
    if (emailto == "") {
        $("#<%= lblEmailToExpense.ClientID %>").show();
        }
        else {
            $("#<%= lblEmailToExpense.ClientID %>").hide();
        }

    }

    function PopulateEmailToExpense(listArray) {
        var stringEmailTo = ''
        for (counter = 0; counter < listArray.length; counter++) {
            if (counter != 0) {
                stringEmailTo += ";";
            }
            stringEmailTo += listArray[counter];
        }
        $("#<%= txtEmailToExpense.ClientID %>").val(stringEmailTo);

        var emailto = $("#<%= txtEmailToExpense.ClientID %>").val(stringEmailTo);
        if (emailto == "") {
            $("#<%= lblEmailToExpense.ClientID %>").show();
        }
        else {
            $("#<%= lblEmailToExpense.ClientID %>").hide();
        }
    }

    function IsExistsExpense(array, element) {
        for (var counter = 0; counter < array.length; counter++) {
            if (array[counter] == element) {
                return true;
            }
        }
        return false;
    }

    function RemoveElementFromArrayExpense(array, element) {
        var listArray = [];
        if (IsExistsExpense(array, element)) {
            for (var counter = 0; counter < array.length; counter++) {
                if (array[counter] != element) {
                    listArray.push(array[counter]);
                }
            }
        }
        return listArray;
    }



    function validateEmailExpense(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }


    //check validation and blank the control

    function BlankControlExpense() {



        var InsurerClaimId = $("#<%= txtExpenseInsurerClaimId.ClientID %>").val();

        var InsurerName = $("#<%= txtExpenseInsurerName.ClientID %>").val();




        var CommentNote = $("#<%= txtExpenseCommentNote.ClientID %>").val();

        var ExpenseDate = $('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_txtExpenseDate .igte_Office2007BlueInner').children().val();


        if (InsurerClaimId != "") {
            $("#<%= lblExpenseInsurerClaimId.ClientID %>").hide();
        }
        if (InsurerName != "") {
            $("#<%= lblExpenseInsurerName.ClientID %>").hide();
        }


        if (CommentNote != "") {
            $("#<%= lblExpenseCommentNote.ClientID %>").hide();
        }

        if (ExpenseDate != "") {
            $("#<%= lblExpenseDate.ClientID %>").hide();
         }

         return false;
     }


     $("#<%= ddlExpenseType.ClientID %>").change(function () {
        var expenseType = $("#<%= ddlExpenseType.ClientID %>").val();

        if (expenseType != 0) {

            $("#<%= lblExpenseType.ClientID %>").hide();
        }
    });

    $("#<%= ddlExpenseClaimCarrier.ClientID %>").change(function () {
        var claimCarrier = $("#<%= ddlExpenseClaimCarrier.ClientID %>").val();

        if (claimCarrier != 0) {

            $("#<%= lblExpenseClaimCarrier.ClientID %>").hide();
        }
    });


</script>
<script>
    function HandleInvoiceClick(elem) {

        var id = jQuery(elem).attr('id');
        var hdnfieldclaimid = id.replace("imgbtn", "hfClaimId");
        var claimID = $("#" + hdnfieldclaimid).val()
        $("#<%= hf_ClaimIdForInvoice.ClientID %>").val(claimID);
        $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_ddlCarrierInvoiceProfile > option").remove();

        $('#<%=ddlCarrierInvoiceProfile.ClientID%>').append($("<option></option>").val("0").html("---Select---"));
        if (claimID > 0) {
            var myParams = "{ 'claimID':'" + claimID + "'}";
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/GetCarrierInvoiceProfile") %>',
                success: function (data) {

                    var jsdata = JSON.parse(data.d);
                    $.each(jsdata, function (key, value) {

                        $('#<%=ddlCarrierInvoiceProfile.ClientID%>').append($("<option></option>").val(value.ID).html(value.Value1));

                    });
                },
                error: function () {
                    alert('error');
                }
            });



                $.ajax({
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: 'POST',
                    data: myParams,
                    url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/GetCarrierInvoiceProfileData") %>',
                success: function (data) {
                    $("#<%= txtGrossLossPayable.ClientID %>").val(data.d[0]);
                    $("#<%= txtDepreciation.ClientID %>").val(data.d[1]);
                    $("#<%= txtPolicyDeductible.ClientID %>").val(data.d[2]);
                    $("#<%= txtNetClaimPayable.ClientID %>").val(data.d[3]);
                    $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_ddlPropertyFeeInvoiceDesignation_ddlFeeDesignation").val(data.d[4]);
                    $("#<%= ddlCarrierInvoiceProfile.ClientID %>").val(data.d[5]);
                },
                error: function () {

                }
            });


            // move pop up position step9

            var step9 = readCookie("currentStep");
            if (step9 == 9) {

                $.powerTip.hide();


                $(rowWithClaimNumber).find($("input[title='Add Invoice']")).data('powertipjq', $([
    '<div style="height:auto;width:214px;white-space: normal;"><p><b>Now Presenting... 1-Click Invoicing!!!</b> </p>',
    '<p>Now that you have expenses and time  in your claim, why not',
    'take it to the next level and create an invoice from here?',
    'Is this really possible?  YES IT IS!',
    'This automatically saves an invoice to the Invoice Review Queue </p><hr/>',
    '<p class="step">Step 9 of 12 <input type="button" class="floatRgt" id="step9" value="Next >>" /></p></div>'
                ].join('\n')));

                $(rowWithClaimNumber).find($("input[title='Add Invoice']")).powerTip({
                    placement: 'w',
                    smartPlacement: false,
                    manual: true
                });

                $.powerTip.show($(rowWithClaimNumber).find($("input[title='Add Invoice']")));
            } else {

            }


        $("#div_InvoiceGenerate").dialog({
            modal: false,
            width: 761,
            dialogClass: 'dialogClass',
            close: function (e, ui) {
                $(this).dialog('destroy');

            },
            //buttons:
            //   {
            //       'Done': function () {                       
            //           $(this).dialog('close');
            //       }
            //   }
        });
    }
    else {
            // show dialog
        $(".displayblock").prop("disabled", true);
        $("#div_NoClaim").dialog({
            modal: false,
            width: 300,
            close: function (e, ui) {
                $(this).dialog('destroy');
                $(".displayblock").prop("disabled", false);
            },
            buttons:
               {
                   //'Done': function () {
                   //    $(this).dialog('close');
                   //}
               }
        });
    }


    return false;

}

function AutoInvoiceGenerate() {
    var claimID = $("#<%= hf_ClaimIdForInvoice.ClientID %>").val();

    var grossLossPayable = $("#<%= txtGrossLossPayable.ClientID %> ").val();
    var depreciation = $("#<%= txtDepreciation.ClientID %>").val();
    var policyDeductible = $("#<%= txtPolicyDeductible.ClientID %>").val();
    var netClaimPayable = $("#<%= txtNetClaimPayable.ClientID %>").val();

    var feeDesignation = $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_ddlPropertyFeeInvoiceDesignation_ddlFeeDesignation").val();
    var carrierInvoiceProfile = $("#<%= ddlCarrierInvoiceProfile.ClientID %>").val();

    var myParams = "{ 'claimID':'" + claimID + "','carrierInvoiceProfile':'" + carrierInvoiceProfile + "','feeDesignation':'" + feeDesignation + "', 'grossLossPayable':'" + grossLossPayable + "','depreciation':'" + depreciation + "', 'policyDeductible':'" + policyDeductible + "','netClaimPayable':'" + netClaimPayable + "'}";

    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: 'POST',
        data: myParams,
        url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/AutoInvoiceGenerate") %>',
            success: function (data) {

                $("#div_InvoiceGenerate").dialog('close');
                $("#<%= lblInvoiceSave.ClientID %>").text(data.d);
                $("#div_InvoiceSave").dialog({
                    modal: false,
                    width: 300,
                    close: function (e, ui) {
                        $(this).dialog('destroy');
                        window.location.href = window.location.href + '&search=all';
                    },
                    buttons:
                               {
                               }
                });
            },
            error: function () {
                $("#div_InvoiceNotSave").dialog({
                    modal: false,
                    width: 300,
                    close: function (e, ui) {
                        $(this).dialog('destroy');

                    },
                    buttons:
                                   {
                                       //'Done': function () {
                                       //    $(this).dialog('close');
                                       //}
                                   }
                });
            }
        });
        return false;
    }


    function calculateNetClaimPayable(sender, e) {
        var txtNetClaimPayable = 0;

        var txtGrossLossPayable = $find("<%= txtGrossLossPayable.ClientID %>").get_value();
        var txtDepreciation = $find("<%= txtDepreciation.ClientID %>").get_value();
        var txtPolicyDeductible = $find("<%= txtPolicyDeductible.ClientID %>").get_value();

        if (txtGrossLossPayable > 0) {
            var netClaimPayable = txtGrossLossPayable - txtDepreciation - txtPolicyDeductible;

            $find("<%= txtNetClaimPayable.ClientID %>").set_value(netClaimPayable);
        }
        else {

            $find("<%= txtNetClaimPayable.ClientID %>").set_value(0);
            $find("<%= txtDepreciation.ClientID %>").set_value(0);
            $find("<%= txtPolicyDeductible.ClientID %>").set_value(0);
        }

    }

</script>


<script type="text/javascript">

    // added for tutorial

    $(document).ready(function () {




        // added Getting Started to top choice
        var firstLi = $("ul").find("li:contains('Getting Started')");

        $("ul").find("li:contains('Getting Started')").remove();

        $(firstLi).prependTo("ul:first");

        var cookiesMode = readCookie("tutorialMode");

        if (cookiesMode == "true") {

            //var mode = cookies[2].split('=');


            //// check if mode is turn on then show tutorial
            //if (mode[1] == 'true') {

            // get table row which has claim number 10001
            rowWithClaimNumber = $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_gvUserLeads tr:contains('10001')");



            // step 6
            //        $("input[title='Add Notes']").data('powertipjq', $([
            //'<p><b>Add Notes and Log Time with 1-Click</b></p>',
            //'<p>Easy-to-Add Notes or Send Notes to adjusters</p>',
            //'<p>or any other stakeholders.  You may also Log Service Time</p>',
            //'<p>here and get it all done quickly in a single leap and bound!</p>',
            //'<p>Any time here is auto-written into the Claim\'s Time & Expense</p><hr/>',
            //'<p class="step">Step 6 of 12 <button style="margin-left: 116px;" id="step6" >Next >> </button></p>'
            //        ].join('\n')));

            //        $("input[title='Add Notes']").powerTip({
            //            placement: 'e',
            //            smartPlacement: true,
            //            manual: true
            //        });

            $(rowWithClaimNumber).find($("input[title='Add Notes']")).data('powertipjq', $([
'<p><b>Add Notes and Log Time with 1-Click</b></p>',
'<p>Easy-to-Add Notes or Send Notes to adjusters</p>',
'<p>or any other stakeholders.  You may also Log Service Time</p>',
'<p>here and get it all done quickly in a single leap and bound!</p>',
'<p>Any time here is auto-written into the Claim\'s Time & Expense</p><hr/>',
'<p class="step">Step 6 of 12 <button style="margin-left: 116px;" id="step6" >Next >> </button></p>'
            ].join('\n')));

            $(rowWithClaimNumber).find($("input[title='Add Notes']")).powerTip({
                placement: 'e',
                smartPlacement: true,
                manual: true
            });


            $.powerTip.hide();



            //  textObject.flash($(rowWithClaimNumber).find($("input[title='Add Notes']")).parent().parent(), "colour", 300);
            var currentStep11 = readCookie("currentStep");
            if (currentStep11 == 11) {

               // $("a:contains('All Claims'):first").attr("href", $($(rowWithClaimNumber).find("a:contains('13-1025')")).attr("href"));

                $.powerTip.hide();

                // step for claim number click
                $(rowWithClaimNumber).find("a:contains('10001')").data('powertipjq', $([
                     '<p><b>Drill-Down Directly into a Claim With 1-Click</b> </p>',
                   '<p>Need more information?  Need to review or fine tune a claim?</p>',
                   '<p>Use the friendly Claim # blue links to go right into a claim.</p>',
                   '<p>From here you can navigate to the Policy or Policyholder</p>',
                   '<p>screens for the claim if needed, print reports, view logs + MORE!</p><hr/>',
                   '<p class="step">Step 11 of 12 <button style="margin-left: 116px;" id="step11" >Next >> </button></p>' //
                ].join('\n')));

                // tutorial code ends here

                $(rowWithClaimNumber).find("a:contains('10001')").powerTip({
                    placement: 'e',
                    smartPlacement: true,
                    manual: true
                });


              
              //  $("#claimStep").show();

             
                //      $.powerTip.hide();
                //   textObject.flash($("a:contains('All Claims'):first").parent(), "colour", 300);

                //           $("a:contains('All Claims'):first").data('powertipjq', $([
                //'<p><b>Drill-Down Directly into a Claim With 1-Click</b> </p>',
                //'<p>Need more information?  Need to review or fine tune a claim?</p>',
                //'<p>Use the friendly Claim # blue links to go right into a claim.</p>',
                //'<p>From here you can navigate to the Policy or Policyholder</p>',
                //'<p>screens for the claim if needed, print reports, view logs + MORE!</p><hr/>',
                //'<p class="step">Step 11 of 12 <button style="margin-left: 116px;" id="step11" >Next >> </button></p>' //
                //           ].join('\n')));

                //           $("a:contains('All Claims'):first").powerTip({
                //               placement: 'e',
                //               smartPlacement: true,
                //               manual: true                   
                //           });

                //           // remove animate effect from business rule
                //           // $("a:contains('Claim/Invoice Approval Queue')").parent().css({ "background-color": "white" });

                //           $.powerTip.show($("a:contains('All Claims'):first"));

            } else {

                if (currentStep11 == 6) {
                    $.powerTip.show($(rowWithClaimNumber).find($("input[title='Add Notes']")));
                }
            }




            $(document).on("click", "#step6", function () {

                //$(rowWithClaimNumber).find($("input[title='Add Notes']")).parent().parent().css("background-color", "white");
                //textObject.flash($(rowWithClaimNumber).find($("a[title='Change Status']")).parent().parent(), "colour", 300);
                //create cookie
                createCookie("currentStep", 7);

                $.powerTip.hide();
                $.powerTip.show($(rowWithClaimNumber).find($("a[title='Change Status']")));
            });


            // just to show step 7 of 12 in correct place 
            $(rowWithClaimNumber).find($("a[title='Change Status']")).append('<a id="claimStatusChangeAnchor">&nbsp;</a>');

            // step 7

            // $(rowWithClaimNumber).find($("a[title='Change Status']")).data('powertipjq', $([
            $(rowWithClaimNumber).find($("a[title='Change Status']")).data('powertipjq', $([
         '<p><b>Change Claim Status On-The-Fly</b> </p>',
         '<p>How would you like to change the claim status from right here?</p>',
         '<p>WAIT…  You can!  Click on any claim status and it pops up the</p>',
         '<p>Claim Status Change window, and lets you do it in 1 click!</p>',
         '<p>3-for-1, Updates Claim Log, XactAnalysis, AND sends emails!</p><hr/>',
         '<p class="step">Step 7 of 12 <button style="margin-left: 116px;"  id="step7" >Next >> </button></p>'
            ].join('\n')));

            $(rowWithClaimNumber).find($("a[title='Change Status']")).powerTip({
                placement: 'e',
                smartPlacement: true,
                manual: true
            });


            $(document).on("click", "#step7", function () {

                //create cookie
                createCookie("currentStep", 8);
                $.powerTip.hide();
                //$(rowWithClaimNumber).find($("a[title='Change Status']")).parent().parent().css("background-color", "white");
                //textObject.flash($(rowWithClaimNumber).find($("input[title='Add Expense']")).parent().parent(), "colour", 300);
                $.powerTip.show($(rowWithClaimNumber).find($("input[title='Add Expense']")));
            });


            // step 8

            $(rowWithClaimNumber).find($("input[title='Add Expense']")).data('powertipjq', $([
          '<p><b>1-Click Expense Entry.  Is that Possible?</b></p>',
          '<p>Let\'s turn it up a notch!  How about we make it easy to</p>',
          '<p>also Add Expenses to a claim from right here?  Done.</p>',
          '<p>Add expenses and then create custom invoices from here!</p>',
          '<p>This automatically updates the claim log and writes to the claim.</p><hr/>',
          '<p class="step">Step 8 of 12<button style="margin-left: 116px;"  id="step8" >Next >> </button></p>'
            ].join('\n')));

            $(rowWithClaimNumber).find($("input[title='Add Expense']")).powerTip({
                placement: 'e',
                smartPlacement: true,
                manual: true
            });

            $(document).on("click", "#step8", function () {
                //create cookie
                createCookie("currentStep", 9);
                $.powerTip.hide();
                $.powerTip.show($(rowWithClaimNumber).find($("input[title='Add Invoice']")));
            });

            // step 9

            $(rowWithClaimNumber).find($("input[title='Add Invoice']")).data('powertipjq', $([
          '<p><b>Now Presenting... 1-Click Invoicing!!!</b> </p>',
          '<p>Now that you have expenses and time  in your claim, why not</p>',
          '<p>take it to the next level and create an invoice from here?</p>',
          '<p>Is this really possible?  YES IT IS!</p>',
          '<p>This automatically saves an invoice to the Invoice Review Queue</p><hr/>',
          '<p class="step">Step 9 of 12<button style="margin-left: 116px;"  id="step9" >Next >> </button></p>'
            ].join('\n')));

            $(rowWithClaimNumber).find($("input[title='Add Invoice']")).powerTip({
                placement: 'e',
                smartPlacement: true,
                manual: true
            });



            $(document).on("click", "#step9", function () {


                //create cookie
                createCookie("currentStep", 10);
                //$find("navBar").getExplorerBarItems().getItem(1).set_expanded(false);
                //$find("navBar").getExplorerBarItems().getItem(1).toggle();//.set_expanded(true);
                //$find("navBar").getExplorerBarItems().getItem(1).set_selected(false);//.set_expanded(true);
                $find("ctl00_WebSplitter1_tmpl0_navBar").getExplorerBarItems().getItem(2).set_selected(true);//.set_expanded(true);
                $find("ctl00_WebSplitter1_tmpl0_navBar").getExplorerBarItems().getItem(2).toggle();//.set_expanded(true);
                //$find("navBar").selectGroup(2, true, true);

                textObject.flash($("a:contains('Claim/Invoice Approval Queue')").parent(), "colour", 300);

                $.powerTip.hide();
                $.powerTip.show($("a:contains('Claim/Invoice Approval Queue')"));
            });

            // step 10

            $("a:contains('Claim/Invoice Approval Queue')").data('powertipjq', $([
            '<p><b>Let\'s Get This Invoice Out the Door…</b></p>',
            '<p>You may Approve, Reject, E-mail, or Edit any or ALL invoices</p>',
            '<p>from here to into the claim file, ledger, and Quickbooks</p>',
            '<p>(TIP: This feature may be turned off if you don\'t want it)</p>',
            '<p>After approved, an invoice saves to the claim docs and logs it.</p><hr/>',
            '<p class="step">Step 10 of 12 <button style="margin-left: 116px;" id="step10Of12" >Next >> </button></p>' //
            ].join('\n')));

            $("a:contains('Claim/Invoice Approval Queue')").powerTip({
                placement: 'e',
                smartPlacement: true,
                manual: true
            });


            // step 11
            $(document).on("click", "#step10Of12", function () {

                window.location = $("a:contains('Claim/Invoice Approval Queue')").attr("href");

            });


            // step 11
            $(document).on("click", "#step10", function () {


                $find("ctl00_WebSplitter1_tmpl0_navBar").getExplorerBarItems().getItem(1).set_selected(true);//.set_expanded(true);
                $find("ctl00_WebSplitter1_tmpl0_navBar").getExplorerBarItems().getItem(2).toggle();//.set_expanded(true);
                //$find("navBar").selectGroup(2, true, true);

                $("a:contains('All Claims')").data('powertipjq', $([
      '<p><b>Drill-Down Directly into a Claim With 1-Click</b> </p>',
      '<p>Need more information?  Need to review or fine tune a claim?</p>',
      '<p>Use the friendly Claim # blue links to go right into a claim.</p>',
      '<p>From here you can navigate to the Policy or Policyholder</p>',
      '<p>screens for the claim if needed, print reports, view logs + MORE!</p><hr/>',
      '<p class="step">Step 11 of 12 <button style="margin-left: 116px;" id="step11" >Next >> </button></p>' //
                ].join('\n')));

                $("a:contains('All Claims')").powerTip({
                    placement: 'e',
                    smartPlacement: true,
                    manual: true
                });

                // remove animate effect from business rule
                $("a:contains('Claim/Invoice Approval Queue')").parent().css({ "background-color": "white" });
                textObject.flash($("a:contains('All Claims')").parent(), "colour", 300);

                $.powerTip.hide();
                $.powerTip.show($("a:contains('All Claims')"));
            });



            $(document).on("click", "#step11", function () {

                createCookie("currentStep", 12);
                // $("#claimStep").hide();
                // remove animate effect from business rule
               // $("a:contains('All Claims')").parent().css({ "background-color": "white" });

               // textObject.flash($(rowWithClaimNumber).find("a:contains('13-1025')").parent(), "colour", 300);

                $.powerTip.hide();
                window.location = $(rowWithClaimNumber).find("a:contains('10001')").attr("href");

                // $.powerTip.show($("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucAllUserLeads1_gvUserLeads_ctl02_lbtnClaim"));
               // $.powerTip.show($(rowWithClaimNumber).find("a:contains('13-1025')"));

            });


            // 

          //  $("a:contains('All Claims'):first").data('powertipjq', $([
               //'<p><b>Drill-Down Directly into a Claim With 1-Click</b> </p>',
               //'<p>Need more information?  Need to review or fine tune a claim?</p>',
               //'<p>Use the friendly Claim # blue links to go right into a claim.</p>',
               //'<p>From here you can navigate to the Policy or Policyholder</p>',
               //'<p>screens for the claim if needed, print reports, view logs + MORE!</p><hr/>',
               //'<p class="step">Step 11 of 12 <button style="margin-left: 116px;" id="step11" >Next >> </button></p>' //
               //           ].join('\n')));

               //           $("a:contains('All Claims'):first").powerTip({
               //               placement: 'e',
               //               smartPlacement: true,
               //               manual: true                   
               //           });

            //

            // step for claim number click
            $(rowWithClaimNumber).find("a:contains('10001')").data('powertipjq', $([
                 '<p><b>Drill-Down Directly into a Claim With 1-Click</b> </p>',
               '<p>Need more information?  Need to review or fine tune a claim?</p>',
               '<p>Use the friendly Claim # blue links to go right into a claim.</p>',
               '<p>From here you can navigate to the Policy or Policyholder</p>',
               '<p>screens for the claim if needed, print reports, view logs + MORE!</p><hr/>',
               '<p class="step">Step 11 of 12 <button style="margin-left: 116px;" id="step11" >Next >> </button></p>' //
                          ].join('\n')));

            // tutorial code ends here

            $(rowWithClaimNumber).find("a:contains('10001')").powerTip({
                placement: 'e',
                smartPlacement: true,
                manual: true
            });
            //}
        }



    }); // ready close



</script>


<script>
    // handle document upload
    function showDocumentUploadDialog(elem) {
        var id = jQuery(elem).attr('id');
        var hdnfieldclaimid = id.replace("imgbtn", "hfClaimId");
        var claimID = $("#" + hdnfieldclaimid).val()
        $("#<%= hf_claimIDUploaddocument.ClientID %>").val(claimID);
        if (claimID > 0) {
            // clear fields
            $("#txtDocumentDescription").val('');

            // build upload control
            $("#webUpload").igUpload({
                mode: 'single',
                //progressUrl: "http://appv3.claimruler.com/IGUploadStatusHandler.ashx",
                //progressUrl: "/samplesbrowser/IGUploadStatusHandler.ashx",
                progressUrl: "http://localhost:6375/IGUploadStatusHandler.ashx",
                onError: function (e, args) {
                    showAlert(e);
                },
                fileUploading: function (e, args) {
                    if (!validateDocumentUploadFields()) {
                        $("#txtDocumentDescription").focus();
                        return false;   // cancel upload
                    }
                },
                fileUploaded: function (e, args) {
                    // alert(args.fileID + " " + args.filePath);     
                    // find hf_claimID in ucClaimDocuments.ascx
                    var claimID = parseInt($("[id$='hf_claimIDUploaddocument']").val());

                    var documentDescription = $("#txtDocumentDescription").val();

                    var categoryID = $("[id$='ddlDocumentCategory']").val();
                    var documentCategoryID = parseInt(categoryID);

                    var m = PageMethods.saveFile(claimID, args.filePath, documentDescription, documentCategoryID);
                    $("#<%= ddlDocumentCategory.ClientID %>").prop('selectedIndex', 0);
                    $("#txtDocumentDescription").val('');

                    $("#documentUpload").dialog('close');
                    $("#documentUploadSuccess").dialog({
                        modal: false,
                        width: 600,
                        close: function (e, ui) {
                            $(this).dialog('destroy');
                            window.location.href = window.location.href + '&search=all';
                        },
                        buttons:
                        {
                            //'Close': function () {
                            //    $(this).dialog('close');
                            //}
                        }
                    });

                    // $("[id$='btnHiddenRefreshDocument']").click();
                }
            });

            // show upload dialog
            $("#documentUpload").dialog({
                modal: false,
                width: 600,
                close: function (e, ui) {
                    $(this).dialog('destroy');
                    window.location.href = window.location.href + '&search=all';
                },
                buttons:
                {
                    'Close': function () {
                        $(this).dialog('close');
                        window.location.href = window.location.href + '&search=all';
                    }
                }
            });


        }
        else {
            // show dialog
            $(".displayblock").prop("disabled", true);
            $("#div_NoClaim").dialog({
                modal: false,
                width: 300,
                close: function (e, ui) {
                    $(this).dialog('destroy');
                    $(".displayblock").prop("disabled", false);
                },
                buttons:
                   {
                       //'Done': function () {
                       //    $(this).dialog('close');
                       //}
                   }
            });
        }

        return false;

    }


    function validateDocumentUploadFields() {
        var isValid = true;
        var documentCategoryID = $("#ddlDocumentCategory").val();

        if (parseInt(documentCategoryID) < 1)
            isValid = false;

        return isValid;
    }
</script>


<script>
    function showAddPhotosDialog(elem) {
        var id = jQuery(elem).attr('id');
        var hdnfieldclaimid = id.replace("imgbtn", "hfClaimId");
        var claimID = $("#" + hdnfieldclaimid).val();
        if (claimID > 0) {
            var myParams = "{'claimID':'" + claimID + "'}";


            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/CreateClaimIdSession") %>',
                success: function (data) {

                },
                error: function () {

                }
            });


            window.open('<%= ResolveUrl("~/protected/LeadsImagesUploadPopUp.aspx") %>', "", "status=yes,toolbar=no,menubar=no,location=no,scrollbars=1, width = 700, height = 600");

            //window.open("", "", "width=200, height=100");

        }
        else {
            $("#div_NoClaimPhotoUpload").dialog({
                modal: false,
                width: 300,
                close: function (e, ui) {
                    $(this).dialog('destroy');                    
                },
                buttons:
                   {
                       //'Done': function () {
                       //    $(this).dialog('close');
                       //}
                   }
            });
        }
        return false;
    }
</script>
