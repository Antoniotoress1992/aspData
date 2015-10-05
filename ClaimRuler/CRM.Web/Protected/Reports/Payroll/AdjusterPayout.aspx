<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Protected/ClaimRuler.Master" CodeBehind="AdjusterPayout.aspx.cs" Inherits="CRM.Web.Protected.Reports.Payroll.AdjusterPayout" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolderMiddArea">
    <div class="page-title">
        Adjuster Payroll 
    </div>
    <asp:UpdatePanel runat="server" ID="udpPayroll">
        <ContentTemplate>
            <div class="paneContentInner">
                <ajaxToolkit:TabContainer ID="tabContainerReport" runat="server" Width="100%" ActiveTabIndex="0">
                    <ajaxToolkit:TabPanel ID="tabPanelParameters" runat="server">
                        <HeaderTemplate>
                            Report Parameters						            
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table class="editForm no_min_width">
                                <%--<tr>
                                    <td>Branch</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlBranch" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                            <asp:ListItem Value="" Selected="True">**All Branches**</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>Adjuster</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlAdjusters" Width="150px">
                                            <asp:ListItem Value="" Selected="True">--- Select ---</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
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

                                <tr>

                                     <td><label for="cbxIncludeReimbursable">  Include Reimbursable Expenses In Payroll Report</label></td>
                                    <td class="redstar"></td>
                                    <td>
                                         <asp:CheckBox ID="cbxIncludeReimbursable" ClientIDMode="Static" runat="server" /> 
                                    </td>
                                    
                                </tr>

                               <tr>
                                   <td>Choose Your Report	</td> 
                                   <td class="redstar"></td>
                                    <td>
                                         <asp:RadioButton ID="rbtAllBilledServicesAndExpenses" Text=" Payroll for All Billed Services and Expenses" Checked="true" GroupName="PayrollMethod" runat="server" />   
                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtAllBilledAndNonBilled" Text=" Payroll for All Billed and Non-Billed Services and Expenses" GroupName="PayrollMethod" runat="server" />     
                                    </td>
                               </tr> 
                               <%-- <tr>
                                    <td>Run Payroll for All Billed Services and Expenses</td>
                                    <td class="redstar"></td>
                                    <td>
                                    <asp:RadioButton ID="rbtAllBilledServicesAndExpenses" GroupName="PayrollMethod" runat="server" />      
                                    </td>
                                </tr>

                                <tr>
                                    <td>Run Payroll for All Billed and Non-Billed Services and Expenses</td>
                                    <td class="redstar"></td>
                                    <td>
                                    <asp:RadioButton ID="rbtAllBilledAndNonBilled" GroupName="PayrollMethod" runat="server" />      
                                    </td>
                                </tr>--%>

                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <input type="button" value="Choice for Report Type" onclick="return openDialogReportType()" class="mysubmit" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabPanelReport" runat="server" Visible="false">
                        <HeaderTemplate>
                            Payroll Report						            
                        </HeaderTemplate>
                        <ContentTemplate>
                            <rsweb:ReportViewer ID="reportViewer" runat="server" Height="100%" Width="100%" ShowToolBar="true"
                                Font-Names="Tahoma"
                                ProcessingMode="Local" ShowExportControls="true" ShowFindControls="true" ShowParameterPrompts="true"
                                ShowPageNavigationControls="true" ShowPrintButton="true" ShowReportBody="true"
                                ShowZoomControl="true"  SizeToReportContent="true" KeepSessionAlive="true" Visible="true">
                            </rsweb:ReportViewer>
                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="reportType" style="display:none" title="Report Type">
         <input type="radio" name="reportType" runat="server" id="rbtSummaryType" />   <label for="<%=rbtSummaryType.ClientID %>">Adjuster Payroll Report (Summary, No Breakdown of Expenses)</label><br />

         <input type="radio" name="reportType" runat="server" id="rbtDetailedType" />   <label for="<%=rbtDetailedType.ClientID %>">Adjuster Payroll Report (Detailed, With Breakdown of Expenses)</label>
        <div id="errorMessage" style="color:red">


        </div>
        <br />
  
  <div style="text-align:right" >
    <asp:Button ID="btnGenerateReport" runat="server" OnClick="btnGenerateReport_Click" OnClientClick="return validateRadio()" CssClass="mysubmit" Text="Generate Report" />
        <input type="button" id="btnClose" value="Close" onclick="closeReportTypeDialog()" class="mysubmit"/>
      </div>
    </div>

    <script type="text/javascript">

        var statusHider = function () {
            setInterval(function () {
                var rptViewer = $find("<%= reportViewer.ClientID %>");
                if (rptViewer != null) {
                    if (!rptViewer.get_isLoading()) {
                        $("a:contains('Word')").removeAttr("onclick");
                        $("a:contains('Excel')").removeAttr("onclick");

                        // hide table row in report 


                        //clearInterval(statusHider);
                    }
                }
            }, 200);
        }
       
        statusHider();

        $(document).ready(function () {          

           // $("#<%=//ddlBranch.ClientID%> option:first").text('-- All Branch --');
            $("#<%=ddlAdjusters.ClientID%> option:first").text('-- All Adjuster --');

            //$("a:contains('Word')") .parent().hide();
            //$("a:contains('Excel')").parent().hide();
            //$("a:contains('Word')").removeAttr("onclick");
            //$("a:contains('Excel')").removeAttr("onclick");
            //alert($("a:contains('Excel')").attr("onclick"));

        });


        function closeReportTypeDialog() {

            $("#reportType").dialog('close');

        }

        function validateRadio() {



           <%--  // validate drop down of branch and adjuster
            if ($("#<%=ddlBranch.ClientID%>").val() == "0") {

                alert("Please select branch.");
                $("#reportType").dialog('close');
                return false;
            }
            // validate drop down of branch and adjuster
            if ($("#<%=ddlAdjusters.ClientID%>").val() == "0") {

                alert("Please select Adjuster.");
                $("#reportType").dialog('close');
                return false;
            }  --%>


            if ($("#reportType input[type='radio']:checked").length == 0) {

                $("#errorMessage").html("Please select at least one Report Type");


                return false;
            } else {

                $("#reportType").dialog('close');

                return true;
            }
        }

        function openDialogReportType() {

            $("#reportType").dialog({
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                },
                modal: false,
                width: 'auto',
                close: function () {
                    $("#errorMessage").html('');
                    $(this).dialog('destroy');
                }
            }
            );

            return true;
        }


    </script>
</asp:Content>
