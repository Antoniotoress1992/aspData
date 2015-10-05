<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true" CodeBehind="ClaimExport.aspx.cs" Inherits="CRM.Web.Protected.ClaimExport" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    Claim Export
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">
    <td>
        <asp:LinkButton ID="btnExport" runat="server" CssClass="toolbar-item" OnClick="btnExport_Click">
					<span class="toolbar-img-and-text" style="background-image: url(../images/export.png)">Print & Email Claim Report</span>
        </asp:LinkButton>       
    </td>
    <td>
         <asp:LinkButton ID="btnPrintOnly" runat="server" CssClass="toolbar-item" OnClick="btnPrintOnly_Click">
					<span class="toolbar-img-and-text" style="background-image: url(../images/export.png)">Print Claim Report</span>
        </asp:LinkButton>
    </td>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div class="message_area">
                <asp:Label ID="lblMessage" runat="server" />
            </div>

            <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px;" border="0" class="editForm">

                <tr>
                    <td class="right top" style="width: 15%;">
                        <div>Recipients</div>
                        <div>
                            <a class="link" href="javascript:clearEmailTextbox();">Clear</a>                            
                        </div>
                    </td>
                    <td class="redstar top"></td>
                    <td>
                        <asp:TextBox ID="txtEmailTo" runat="server" Width="90%" TextMode="MultiLine" Rows="2" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <div id="divContactList" style="display: block; width: 90%;">
                            <div class="boxContainer">
                                <div class="section-title">Select Recipients</div>
                                <ig:WebDataGrid ID="contractGrid" runat="server" CssClass="gridView smallheader" DataSourceID="edsContacts"
                                    AutoGenerateColumns="false" EnableViewState="true" EnableDataViewState="true" Height="200px"
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
                      
                        <div>
                            <asp:RequiredFieldValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailTo"
                                Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please select email recipients."
                                CssClass="validation1" ValidationGroup="email" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="redstar"></td>
                    <td>
                        <div style="display: block; width: 90%;">
                            <div class="boxContainer">
                                <div class="section-title">
                                    Select Export Options
                                </div>
                                <table style="border-collapse: separate; border-spacing: 7px; padding: 2px;" border="0">
                                    <tr>
                                        <td class="left">
                                            <asp:CheckBox ID="cbxClaimLog" runat="server" Text=" Claim Log" />
                                        </td>
                                        <td>
                                           <asp:Label ID="lblTextMessage" runat="server" ForeColor="Red" Text="“Please only check this box if sending report to legal.”"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="left" colspan="2">
                                            <asp:CheckBox ID="cbxPhotos" runat="server" Text=" Photos" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="left" colspan="2">
                                            <asp:CheckBox ID="cbxDocuments" runat="server" Text=" Documents" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="left" colspan="2">
                                            <asp:CheckBox ID="cbxDemographics" runat="server" Text=" Demographics" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="left" colspan="2">
                                            <asp:CheckBox ID="cbxCoverage" runat="server" Text=" Coverage" />
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>
                    </td>
                </tr>

            </table>


        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function clearEmailTextbox() {
            $("#<%= txtEmailTo.ClientID%>").val('');
            return false;
        }
        function contractGrid_rowsSelected(sender, args) {


            var selectedEmails = $('#<%= txtEmailTo.ClientID %>').val();

            var selectedRows = args.getSelectedRows();

            var email = args.getSelectedRows().getItem(0).get_cell(3).get_text();

            selectedEmails += email + ";";

            $('#<%= txtEmailTo.ClientID %>').val(selectedEmails);
        }

        function sendEmail() {
            <%-- $("#<%= btnSend_hidden.ClientID %>").click(); --%>
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



    </script>
   
</asp:Content>
