<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimService.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimService" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<style>
    .ModalPopupBG
{
    background-color: black;
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
        padding-top: 10px;
        padding-left: 10px;
        width: 400px;
        height: 140px;
}
</style>


<asp:Button ID="hidden" runat="server" style = "display:none" />
<asp:Panel ID="Panel1" runat="server" CssClass="HellowWorldPopup" align="center" style = "display:none">
    There are no services or expenses set up for this client! <br />Please contact your portal administrator.<br /><br />
    <asp:Button ID="btnClose" runat="server" Width="80px" OnClick="btnClose_Click" Text="Ok" />
</asp:Panel>
 <ajaxToolkit:ModalPopupExtender ID="popup" PopupControlID="Panel1" TargetControlID="hidden"
     BackgroundCssClass="ModalPopupBG" DropShadow="true" runat="server"></ajaxToolkit:ModalPopupExtender>

<asp:Panel ID="pnlEditService" runat="server" Visible="false" DefaultButton="btnSaveClaimService">

    <div class="boxContainer">
        <div class="section-title">
            Service Detail
        </div>
       

        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">
            <tr>
                <td colspan="3" class="center">
                    <asp:Label ID="lblMessage" runat="server" />
                </td>
            </tr>
             <tr>
                <td class="right">Adjuster
                </td>
                <td class="redstar">*</td>
                <td class="nowrap">
                    <ig:WebTextEditor ID="txtServiceAdjuster" runat="server" Enabled="false" Width="250px" />
                    <a href="javascript:findAdjusterForServiceDialog();">
                        <asp:Image ID="imgAdjusterFind" runat="server" ImageUrl="~/Images/searchbg.png" ImageAlign="Top" />
                    </a>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvAdjuster" runat="server" ControlToValidate="txtServiceAdjuster"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select adjuster."
                            ValidationGroup="service" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Service</td>
                <td class="redstar">*</td>
                <td>
                    <asp:DropDownList ID="ddlInvoiceServiceType" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlInvoiceServiceType" InitialValue="0"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select service type."
                            ValidationGroup="service" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Activity</td>
                <td class="redstar">*</td>
                <td>
                    <asp:DropDownList ID="ddlActivity" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlActivity" InitialValue="0"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select an activity."
                            ValidationGroup="service" />
                    </div>
                </td>
            </tr>
             <tr>
                <td class="right">Time Quantity</td>
                <td class="redstar">*</td>
                <td>
                    <ig:WebNumericEditor ID="txtServiceQty" runat="server" MinDecimalPlaces="2" Width="80px"></ig:WebNumericEditor>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtServiceQty" ValidationGroup="service"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter quantity." CssClass="validation1" InitialValue="0" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Activity Date</td>
                <td class="redstar">*</td>
                <td>
                    <ig:WebDatePicker ID="txtServiceDate" runat="server"   CssClass="date_picker" Width="150px"  DisplayModeFormat="MM/dd/yyyy h:mm tt" EditModeFormat="MM/dd/yyyy h:mm tt">
                        <Buttons>
                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                        </Buttons>
                    </ig:WebDatePicker>
                    <div>
                        <asp:RequiredFieldValidator ID="tfvServiceDate" runat="server" ControlToValidate="txtServiceDate" ValidationGroup="service"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter expense date." CssClass="validation1" />
                    </div>
                </td>
            </tr>
             <tr>
                <td class="right">Start Date</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebDatePicker ID="txtStartDate" runat="server"   CssClass="date_picker" Width="150px"  DisplayModeFormat="MM/dd/yyyy h:mm tt" EditModeFormat="MM/dd/yyyy h:mm tt">
                        <Buttons>
                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                        </Buttons>
                    </ig:WebDatePicker>
                   
                </td>
            </tr>
             <tr>
                <td class="right">End Date</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebDatePicker ID="txtEndDate" runat="server"   CssClass="date_picker" Width="150px"  DisplayModeFormat="MM/dd/yyyy h:mm tt" EditModeFormat="MM/dd/yyyy h:mm tt">
                        <Buttons>
                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                        </Buttons>
                    </ig:WebDatePicker>
                    
                </td>
            </tr>
             <tr>
                <td class="right">Billable to Client</td>
                <td class="redstar"></td>
                <td>
                    <asp:CheckBox ID="cbIsBillable" Checked="true" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="right top">Description<br /><asp:Label runat="server" Font-Italic="true" ForeColor="Red" Font-Size="Smaller" Text="(Will display on invoice)"></asp:Label></td>
                <td style="font-style:italic" class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtServiceDescription" MaxLength="500" TextMode="MultiLine" MultiLine-Rows="3" Width="100%"></ig:WebTextEditor>
                    <div>
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtServiceDescription" ValidationGroup="service"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter description." CssClass="validation1" />--%>
                    </div>
                </td>
            </tr>
            
             <tr>
                <td class="right top">Internal Comments</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtMyComments" MaxLength="500" TextMode="MultiLine" MultiLine-Rows="3" Width="100%"></ig:WebTextEditor>
                    <div>
                    </div>
                </td>
            </tr>
             <tr>
                <td class="right top">Email Adjuster</td>
                <td class="redstar top"></td>
                <td>
                    <asp:CheckBox ID="cbEmailAdjuster" runat="server" />
                    <div>
                    </div>
                </td>
            </tr>
             <tr>
                <td class="right top">Email Client Contact</td>
                <td class="redstar top"></td>
                <td>
                    <asp:CheckBox ID="cbEmailClient" runat="server" />
                    <div>
                    </div>
                </td>
            </tr>
             <tr>
                <td class="right top">Email to:</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" TextMode="Email" ID="txtEmailTo" MaxLength="500"  MultiLine-Rows="3" Width="100%"></ig:WebTextEditor>
                    
                    <div>
                    </div>
                </td>
            </tr>
           
            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnSaveClaimService" runat="server" Text="Save" CssClass="mysubmit" OnClick="btnSaveClaimService_Click" CausesValidation="true" ValidationGroup="service" />
                    &nbsp;
                    <asp:Button ID="btnCancelClaimService" runat="server" Text="Cancel" CssClass="mysubmit" OnClick="btnCancelClaimService_Click" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>

<asp:GridView ID="gvClaimService" Width="100%" ShowHeaderWhenEmpty="true" CssClass="gridView"
    AutoGenerateColumns="False"
    CellPadding="2"
    runat="server"
    DataKeyNames="ClaimServiceID, ServiceTypeID"
    HorizontalAlign="left"
    OnRowCommand="gvClaimService_RowCommand"
    OnRowDataBound="gvClaimService_RowDataBound"
    AlternatingRowStyle-BackColor="#e8f2ff">
    <RowStyle HorizontalAlign="Center" />
    <HeaderStyle HorizontalAlign="Center" />
    <EmptyDataTemplate>
        No services have been defined.
    </EmptyDataTemplate>
    <Columns>
        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
            ItemStyle-Wrap="false">
            <ItemTemplate>
                <asp:ImageButton ID="btnEdit" runat="server"
                    CommandName="DoEdit"
                    CommandArgument='<%#Eval("ClaimServiceID") %>'
                    ToolTip="Edit"
                     Visible="true"
                    ImageUrl="~/Images/edit.png"></asp:ImageButton>
                &nbsp;
				<asp:ImageButton ID="btnDelete" runat="server"
                    CommandName="DoDelete"
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this service?');"
                    CommandArgument='<%#Eval("ClaimServiceID") %>'
                    ToolTip="Delete" 
                     Visible="true"
                    ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date">
            <ItemStyle Width="80px" />
            <ItemTemplate>
                <%# Eval("ServiceDate", "{0:MM/dd/yyyy}")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Service">
            <ItemTemplate>
                <%# Eval("InvoiceServiceType.ServiceDescription") %>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="Activity">
            <ItemTemplate>
                <%# Eval("Activity") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <%# Eval("ServiceDescription") %>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Comments">
            <ItemTemplate>
                <%# Eval("InternalComments") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Adjuster">
            <ItemTemplate>
                <%# Eval("AdjusterMaster") == null ? "" : Eval("AdjusterMaster.adjusterName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Quantity">
            <ItemStyle Width="80px" />
            <ItemTemplate>
                <%# Eval("ServiceQty", "{0:N2}") %>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Is Billable">
            <ItemStyle Width="80px" />
            <ItemTemplate>
                <%# (Boolean.Parse(Eval("IsBillable").ToString()) ? "Yes" : "No") %>
                <asp:HiddenField ID="hfBilled" runat="server" Value='<%# Eval("Billed") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>


<div id="div_ClaimServiceAdjustersList" style="display: none; width: 90%;" title="Select Adjuster">
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
<asp:HiddenField ID="hf_serviceAdjusterID" runat="server" Value="0" />

  

<script type="text/javascript">
    // handles events for claim services
    function findAdjusterForServiceDialog() {

        // show dialog
        $("#div_ClaimServiceAdjustersList").dialog({
            modal: false,
            width: 600,
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
    function claimServiceAdjusterGrid_rowsSelected(sender, args) {
        var selectedRows = args.getSelectedRows();

        var adjusterID = args.getSelectedRows().getItem(0).get_cell(0).get_text();
        var adjusterName = args.getSelectedRows().getItem(0).get_cell(1).get_text();

        $("#<%= hf_serviceAdjusterID.ClientID %>").val(adjusterID);
        $find("<%= txtServiceAdjuster.ClientID %>").set_value(adjusterName);
    }
    </script>
   