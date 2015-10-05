<%@ Page Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"  CodeBehind="ClaimListing.aspx.cs" Inherits="CRM.Web.Protected.Reports.Claim.claimListing" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div style ="text-align:center; margin-top:20px;">
        <table class="editForm no_min_width" style ="margin:0 auto;">
             <tr>
                                    <td>From Date</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebDatePicker ID="txtFromDate" runat="server" CssClass="date_picker" DisplayModeFormat="MM/dd/yyyy" EditModeFormat="MM/dd/yyyy">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
              </tr>
              <tr>
                                    <td>To Date</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebDatePicker ID="txtToDate" runat="server" CssClass="date_picker" DisplayModeFormat="MM/dd/yyyy" EditModeFormat="MM/dd/yyyy">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
             </tr>   
           </table>                
           <div style ="display:inline-block;height:100px;margin-top:20px;">
               <p style ="float:left;">Client Carrier</p>
               <div style ="float:left;">
                   <asp:ListBox ID="ddlCarrier" runat="server" SelectionMode="Multiple"  >
		           </asp:ListBox>
               </div>
            
           </div>
           <br />
           <div style ="display:inline-block;height:100px;">
               <p style ="float:left;">Insurer/Branch:</p>
               <div style ="float:left;">
                    <asp:ListBox ID="gvLocation" runat="server" SelectionMode="Multiple"  >
		                                   
	                </asp:ListBox>
               </div>
               
           </div>                       
     </div>   
                           
    <div style="text-align:center" >
              <asp:Button ID="btnGenerateReport" runat="server" OnClick="btnGenerateReport_Click"  CssClass="mysubmit" Text="Generate Report" />
                                
    </div>
                            


    <div style="margin: auto;">
             <rsweb:ReportViewer ID="reportViewer" runat="server" Height="100%" Width="100%" ShowToolBar="true"
                                    Font-Names="Tahoma"
                                    ProcessingMode="Local" ShowExportControls="true" ShowFindControls="true"
                                    ShowPageNavigationControls="true" ShowPrintButton="true" ShowReportBody="true"
                                    ShowZoomControl="true" SizeToReportContent="true" KeepSessionAlive="true" Visible="true">
               </rsweb:ReportViewer>
    </div>
    
</asp:Content>
