<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAdjuster.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucAdjuster" %>
<%@ Register Assembly="Infragistics45.Web.jQuery.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<div class="toolbar toolbar-body">
    <table>
        <tr>
            <td>
                <asp:LinkButton ID="btnReturnToList" runat="server" CssClass="toolbar-item" OnClick="btnReturnToList_Click" CausesValidation="false">
					<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">Adjuster List</span>
                </asp:LinkButton>
            </td>
            <td>
                <asp:LinkButton ID="btnCreateAccount" runat="server" CssClass="toolbar-item"
                    OnClick="btnCreateAccount_Click" ValidationGroup="Adjuster">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/people.png);">Create User Account</span>
                </asp:LinkButton>

            </td>

        </tr>
    </table>
</div>
<div class="paneContent">

    <div class="message_area">
        <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
        <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
        <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
    </div>
    <div class="paneContentInner">
        <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="nowrap">
            <tr>
                <td class="left top" style="width: 40%;">

                    <table style="width: 90%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                        <tr>
                            <td class="right">First Name</td>
                            <td class="redstar" style="width: 5px;">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtFirstName" MaxLength="100" runat="server" TabIndex="1"></ig:WebTextEditor>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtFirstName"
                                        ErrorMessage="Please enter first name." ValidationGroup="Adjuster" Display="Dynamic"
                                        CssClass="validation1" />
                                </div>
                            </td>
                            <td rowspan="4" class="top left">
                                <div class="boxContainer">
                                    <div class="paneContentInner">
                                        <a href="javascript:adjusterPhotoUploadDialog();">
                                            <asp:Image ID="adjusterPhoto" runat="server" Width="100px" Height="100px"
                                                ImageUrl="~/Images/user-thumbnail.png" ImageAlign="Middle" ToolTip="Change" />
                                        </a>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Last Name</td>
                            <td class="redstar" style="width: 5px;">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtLastName" MaxLength="100" runat="server" TabIndex="2"></ig:WebTextEditor>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtLastName"
                                        ErrorMessage="Please enter last name." ValidationGroup="Adjuster" Display="Dynamic"
                                        CssClass="validation1" />
                                </div>
                            </td>

                        </tr>

                         <tr>
                            <td class="right">
                                <label>
                                    Role
                                </label>
                            </td>
                            <td class="redstar">*
                            </td>
                            <td>

                                <asp:DropDownList ID="ddlRole" runat="server">
                                </asp:DropDownList>

                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="requiredFieldValidator6" ControlToValidate="ddlRole"
                                        ErrorMessage="Please select Role." ValidationGroup="register" Display="Dynamic"
                                        CssClass="validation1" InitialValue="0" />
                                </div>
                            </td>
                        </tr>
                         <tr>
                            <td class="right">View All Claims</td>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="cbxViewAllClaims" runat="server" />
                            </td>
                        </tr>

                        <tr>
                            <td class="right">Address</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtAddress" runat="server" MaxLength="100" TabIndex="3" />
                            </td>

                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtAddress2" runat="server" MaxLength="50" TabIndex="4" />
                            </td>

                        </tr>
                        <tr>
                            <td class="right">State</td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" TabIndex="5" />
                            </td>

                        </tr>
                        <tr>
                            <td class="right">City</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtCityName" runat="server" TabIndex="6" />
                            </td>

                        </tr>
                        <tr>
                            <td class="right">Zip Code
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtZip" runat="server" TabIndex="7" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Company Name</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtCompanyName" MaxLength="100" runat="server" Width="100px" TabIndex="8"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Mobile Phone</td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtPhoneNumber" MaxLength="20" runat="server" Width="100px" TabIndex="9"></ig:WebTextEditor>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPhone" ControlToValidate="txtPhoneNumber"
                                        ErrorMessage="Please enter phone number." ValidationGroup="Adjuster" Display="Dynamic"
                                        CssClass="validation1" SetFocusOnError="True" />
                                </div>
                            </td>
                        </tr>
                         <tr>
                            <td class="right">Company Phone</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtCompanyPhone" MaxLength="20" runat="server" Width="100px" TabIndex="9"></ig:WebTextEditor>
                               
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Fax Number</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtFaxNumber" MaxLength="20" runat="server" Width="100px" TabIndex="10"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Federal ID No.</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtFedID" MaxLength="20" runat="server" Width="100px" TabIndex="11"></ig:WebTextEditor>
                            </td>
                        </tr>

                        <tr>
                            <td class="right">Email</td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtEmail" MaxLength="100" runat="server" Width="300px" TabIndex="12"></ig:WebTextEditor>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="txtEmail"
                                        ErrorMessage="Please enter email address." ValidationGroup="Adjuster" Display="Dynamic"
                                        CssClass="validation1" SetFocusOnError="True" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Email not valid." ValidationGroup="Adjuster" Display="Dynamic"
                                        CssClass="validation1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Geographical Area of Service</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtServiceArea" MaxLength="100" runat="server" Width="300px" TabIndex="13"></ig:WebTextEditor>
                                <span>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtServiceArea" ControlToValidate="txtServiceArea"
                                        ErrorMessage="Please enter geographical area of service." ValidationGroup="Adjuster" Display="Dynamic"
                                        InitialValue="0" CssClass="validation1" SetFocusOnError="True" />
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Certifications</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtCertification" MaxLength="100" runat="server" Width="300px" TabIndex="14"></ig:WebTextEditor>
                                <span>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtCertification" ControlToValidate="txtCertification"
                                        ErrorMessage="Please enter certifications." ValidationGroup="Adjuster" Display="Dynamic"
                                        InitialValue="0" CssClass="validation1" SetFocusOnError="True" />
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Years of Experience</td>
                            <td class="redstar"></td>
                            <td class="no_min_width">
                                <ig:WebNumericEditor ID="txtYearExperience" MaxLength="5" runat="server" Width="50px" DataMode="Int"  HorizontalAlign="Right" TabIndex="15" CssClass="no_min_width"></ig:WebNumericEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Maximum Number of Claims</td>
                            <td class="redstar"></td>
                            <td class="no_min_width">
                                <ig:WebNumericEditor ID="txtMaxNumberClaims" runat="server" Width="50px" DataMode="Int" TabIndex="16" HorizontalAlign="Right" CssClass="no_min_width"></ig:WebNumericEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Supervisor</td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddlSupervisor" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Hourly Rate</td>
                            <td></td>
                            <td class="no_min_width">
                                <ig:WebNumericEditor ID="txtHourlyRate" runat="server" MinDecimalPlaces="2" DataMode="Decimal" Width="50px" TabIndex="17" HorizontalAlign="Right" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Commission Rate</td>
                            <td></td>
                            <td class="no_min_width">
                                <ig:WebPercentEditor ID="txtCommissionRate" runat="server" MinDecimalPlaces="2" DataMode="Decimal" Width="50px" HorizontalAlign="Right" TabIndex="18" CssClass="no_min_width"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">XactNet Address</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtXactNetAddress" MaxLength="50" runat="server" TabIndex="19"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right"></td>
                            <td></td>
                            <td>
                                <div>
                                    <asp:CheckBox ID="cbxNotification" runat="server" Text="Notify via email when assigned to a claim" TextAlign="Right" TabIndex="19" />
                                </div>
                                <div>
                                    <asp:CheckBox ID="cbxStatus" runat="server" Text="Is Active" TextAlign="Right" TabIndex="18" />
                                </div>
                                <div>
                                    <asp:CheckBox ID="cbxisW9" runat="server" Text="Is W-9" TextAlign="Right" TabIndex="19" />
                                </div>
                                <div>
                                    <asp:CheckBox ID="cbxUseDeploymentAddress" runat="server" Text="Use Deployment Address for Mapping" TextAlign="Right" TabIndex="20" />
                                </div>
                                <div>
                                    <asp:CheckBox ID="cbxNotifyUserUploadDocument" runat="server" Text="Notify via email when customer uploads documents" TextAlign="Right" TabIndex="20" />
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="3" class="center">
                                <asp:Button ID="btnSaveAdjuster" runat="server" Text="Save" CssClass="mysubmit" OnClick="btnSave_Click"
                                    ValidationGroup="Adjuster"
                                    CausesValidation="true" />
                            </td>
                        </tr>
                    </table>


                </td>
                <td class="left top">
                    <ajaxToolkit:TabContainer ID="tabContainer" runat="server" Width="100%" ActiveTabIndex="0">
                        <ajaxToolkit:TabPanel ID="tabPanelDeploymentAddress" runat="server">
                            <HeaderTemplate>Adjuster Deployment</HeaderTemplate>
                            <ContentTemplate>
                                <table style="width: 90%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                                    <tr>
                                        <td class="right top">Choose Deployment Address:</td>
                                         <td></td>
                                        <td>
                                            <asp:DropDownList ID="ddlDeploymentAddress" runat="server" TabIndex="23" />
                                        </td>
                                    </tr>
                                    <tr>
                                         <td class="right top"><b> Create a New Deployment Address</b>                             
                                        </td>
                                        <td></td>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right top">Deployment Address                             
                                        </td>
                                        <td></td>
                                        <td>
                                            <ig:WebTextEditor ID="txtDeployAddress" MaxLength="100" runat="server" TabIndex="21"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right top">Address2                             
                                        </td>
                                        <td></td>
                                        <td>
                                            <ig:WebTextEditor ID="txtDeployAddress2" MaxLength="100" runat="server" TabIndex="22"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">State</td>
                                        <td></td>
                                        <td>
                                            <asp:DropDownList ID="ddlDeployState" runat="server" TabIndex="23" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">City</td>
                                        <td></td>
                                        <td>
                                            <ig:WebTextEditor ID="txtDeployCity" runat="server" TabIndex="24"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Zip Code</td>
                                        <td></td>
                                        <td>
                                            <ig:WebTextEditor ID="txtDeployZipCode" runat="server" TabIndex="25"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <td colspan="3"><asp:CheckBox ID="chkSaveAddress" runat="server" Text="Save this Address for Future Use"/></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <div id="divEventAdd" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td colspan="3"><b>Event Deployment </b></td>
                                                </tr>
                                                <tr>
                                                   <td class="right">Event Name</td>
                                                   <td></td>
                                                    <td>
                                                        <ig:WebTextEditor ID="txtEventName" runat="server" TabIndex="101"></ig:WebTextEditor>
                                                         <div>
                                                      <asp:RequiredFieldValidator ID="reqEventName" runat="server" Display="Dynamic" ControlToValidate="txtEventName"
                                                        SetFocusOnError="true" CssClass="validation1" ErrorMessage="Please enter event name."
                                                        ValidationGroup="event" />
                                                     </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                   <td class="right">Event Type</td>
                                                   <td></td>
                                                    <td>
                                                        <ig:WebTextEditor ID="txtEventType" runat="server" TabIndex="102"></ig:WebTextEditor>
                                                         <div>
                                                      <asp:RequiredFieldValidator ID="reqEventType" runat="server" Display="Dynamic" ControlToValidate="txtEventType"
                                                        SetFocusOnError="true" CssClass="validation1" ErrorMessage="Please enter event type."
                                                        ValidationGroup="event" />
                                                     </div>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                   <td class="right">Deployment Date</td>
                                                   <td></td>
                                                    <td>
                                                       <ig:WebDatePicker ID="txtEventDeploymentDate" runat="server" CssClass="date_picker" TabIndex="103"></ig:WebDatePicker>
                                                        <div>
                                                      <asp:RequiredFieldValidator ID="reqEventDeploymentDate" runat="server" Display="Dynamic" ControlToValidate="txtEventDeploymentDate"
                                                        SetFocusOnError="true" CssClass="validation1" ErrorMessage="Please select deployment date."
                                                        ValidationGroup="event" />
                                                     </div>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                   <td class="right">Arrival Date</td>
                                                   <td></td>
                                                    <td>
                                                         <ig:WebDatePicker ID="txtEventArrivalDate" runat="server" CssClass="date_picker" TabIndex="104"></ig:WebDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                   <td class="right">Departure Date</td>
                                                   <td></td>
                                                    <td>
                                                         <ig:WebDatePicker ID="txtEventDepartureDate" runat="server" CssClass="date_picker" TabIndex="105"></ig:WebDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                   <td class="right">Date Returned</td>
                                                   <td></td>
                                                    <td>
                                                         <ig:WebDatePicker ID="txtEventDateReturned" runat="server" CssClass="date_picker" TabIndex="106"></ig:WebDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button id="btnEventSave" runat="server" Text="Save" OnClick="btnEventSave_Click" CssClass="mysubmit" ValidationGroup="event"/>
                                                        <asp:Button id="btnEventCancel" runat="server" Text="Cancel" OnClick="btnEventCancel_Click" CssClass="mysubmit"/>
                                                    </td>
                                                </tr>
                                            </table>
                                            </div>
                                        </td>
                                        </tr>
                                    <tr>
                                        <td colspan="3"><asp:LinkButton ID="lbtnEventSave" runat="server" Visible="false" Text="Add Deployment" OnClick="lbtnEventSave_Click"></asp:LinkButton> </td>
                                        </tr>
                                    <tr>
                                        <td colspan="3">
                                            <div style="width:100%;height:118px;overflow:auto">
                                             <asp:GridView ID="gvDeployEvents" CssClass="gridView" ShowFooter="false" Width="80%" runat="server" HorizontalAlign="Center"
                                    AutoGenerateColumns="False" CellPadding="2"
                                    AlternatingRowStyle-BackColor="#e8f2ff" HeaderStyle-Font-Size="11px"
                                    RowStyle-HorizontalAlign="Center">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false" HeaderText="Event Name">
                                            <ItemTemplate>
                                                  <%# Eval("EventName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Event Type">
                                            <ItemTemplate>
                                                <%# Eval("EventType") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                      
                                        <asp:TemplateField HeaderText="Deployment Date">
                                            <ItemTemplate>
                                                <%# Eval("DeploymentDate") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Arrival Date">
                                            <ItemTemplate>
                                                <%# Eval("ArrivalDate") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Departure Date">
                                            <ItemTemplate>
                                                <%# Eval("DepartureDate") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date Returned">
                                            <ItemTemplate>
                                                <%# Eval("DateReturned") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                       
                                     </Columns>
                                </asp:GridView>
                                                </div>
                                        </td>
                                    </tr>

                                </table>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tabPanelNotes" runat="server">
                            <HeaderTemplate>
                                Notes
                            </HeaderTemplate>
                            <ContentTemplate>
                                <div>
                                    <asp:LinkButton ID="blnkNewNote" runat="server" Text="New Note" OnClick="blnkNewNote_Click" Font-Size="11px" />
                                </div>
                                <asp:GridView ID="gvNotes" CssClass="gridView" ShowFooter="false" Width="100%" runat="server" HorizontalAlign="Center"
                                    OnRowCommand="gvNotes_RowCommand" AutoGenerateColumns="False" CellPadding="2"
                                    AlternatingRowStyle-BackColor="#e8f2ff" HeaderStyle-Font-Size="11px"
                                    RowStyle-HorizontalAlign="Center">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Width="45px" VerticalAlign="Top" Wrap="false" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditNote" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("NoteID") %>'
                                                    ToolTip="Edit" ImageUrl="~/Images/edit_icon.png" />
                                                &nbsp;
												<asp:ImageButton ID="btnDeleteNote" runat="server" CommandName="DoDelete"
                                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this note?');"
                                                    CommandArgument='<%#Eval("NoteID") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemStyle Width="120px" Font-Size="11px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <%# Eval("NoteDate") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="User">
                                            <ItemStyle Width="100px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <%# Eval("SecUser.UserName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Notes">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <%# Eval("Notes") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <asp:Panel ID="pnlNotes" runat="server" Visible="false">
                                    <div>
                                        Notes
                                    </div>
                                    <div>
                                        <ig:WebTextEditor ID="txtNote" runat="server" TextMode="MultiLine" MultiLine-Rows="10" Width="100%" TabIndex="30"></ig:WebTextEditor>
                                    </div>
                                    <div>
                                        <asp:Button ID="btnNoteSave" runat="server" Text="Save" OnClick="btnNoteSave_Click" CssClass="mysubmit" TabIndex="32" />
                                        &nbsp;
								        <asp:Button ID="btnNoteCancel" runat="server" Text="Cancel" OnClick="btnNoteCancel_Click" CssClass="mysubmit" TabIndex="33" />
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tabPanelstates" runat="server">
                            <HeaderTemplate>
                                State(s) of Service & Licensure per State
                            </HeaderTemplate>
                            <ContentTemplate>
                                <div style="float: left;">
                                    <asp:LinkButton ID="lbtnNewServiceStateLicense" runat="server" Text="New" OnClick="lbtnNewServiceStateLicense_Click" Font-Size="11px" />
                                </div>
                                <asp:GridView ID="gvServiceStateLicense" CssClass="gridView" ShowFooter="false" Width="100%" runat="server" HorizontalAlign="Center"
                                    OnRowCommand="gvServiceStateLicense_RowCommand" OnRowDataBound="gvServiceStateLicense_RowDataBound"
                                    AutoGenerateColumns="False" CellPadding="2"
                                    AlternatingRowStyle-BackColor="#e8f2ff" HeaderStyle-Font-Size="11px"
                                    RowStyle-HorizontalAlign="Center">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditLicense" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("ID") %>'
                                                    ToolTip="Edit" ImageUrl="~/Images/edit_icon.png" />
                                                &nbsp;
												<asp:ImageButton ID="btnDeleteLicense" runat="server" CommandName="DoDelete"
                                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this record?');"
                                                    CommandArgument='<%#Eval("ID") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Licensed State">
                                            <ItemStyle Width="120px" Font-Size="11px" />
                                            <ItemTemplate>
                                                <%# Eval("StateMaster.StateName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="License Number">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlnkLicense" runat="server" Text='<%# Eval("LicenseNumber") %>' Target="_blank" CssClass="link" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="License Type">
                                            <ItemTemplate>
                                                <%# Eval("LicenseType") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Appointment Type">
                                            <ItemTemplate>
                                                <%# Eval("AdjusterLicenseAppointmentType.LicenseAppointmentType") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Date">
                                            <ItemStyle Width="120px" Font-Size="11px" />
                                            <ItemTemplate>
                                                <%# Eval("LicenseEffectiveDate", "{0:MM/dd/yyyy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expiration Date">
                                            <ItemStyle Width="120px" Font-Size="11px" />
                                            <ItemTemplate>
                                                <%# Eval("LicenseExpirationDate", "{0:MM/dd/yyyy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <asp:Panel ID="pnlServiceStateLicense" runat="server" Visible="false">

                                    <table style="width: 90%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0" class="editForm">
                                        <tr>
                                            <td class="right">Service State</td>
                                            <td style="width: 5px;" class="redstar">*</td>
                                            <td>
                                                <asp:DropDownList ID="ddlStateLicense" runat="server" TabIndex="41"></asp:DropDownList>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvLicenseState" runat="server" Display="Dynamic" ControlToValidate="ddlStateLicense"
                                                        SetFocusOnError="true" CssClass="validation1" ErrorMessage="Please select state."
                                                        ValidationGroup="license" InitialValue="0" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="right">License Number</td>
                                            <td class="redstar">*</td>
                                            <td>
                                                <ig:WebTextEditor ID="txtLicenseNumber" runat="server" TabIndex="42"></ig:WebTextEditor>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="tfvLicenseNumber" runat="server" Display="Dynamic" ControlToValidate="txtLicenseNumber"
                                                        SetFocusOnError="true" CssClass="validation1" ErrorMessage="Please enter license number."
                                                        ValidationGroup="license" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="right">Effective Date</td>
                                            <td></td>
                                            <td>
                                                <ig:WebDatePicker ID="txtEffectiveDate" runat="server" CssClass="date_picker" TabIndex="43"></ig:WebDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="right">Expiration Date</td>
                                            <td></td>
                                            <td>
                                                <ig:WebDatePicker ID="txtExpirationDate" runat="server" CssClass="date_picker" TabIndex="44"></ig:WebDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="right">Appointment Type</td>
                                            <td></td>
                                            <td>
                                                <asp:DropDownList ID="ddlAppointmentType" runat="server" TabIndex="45"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" class="center">
                                                <asp:Button ID="btnServiceStateLicenseSave" runat="server" Text="Save" TabIndex="46"
                                                    OnClick="btnServiceStateLicenseSave_Click" CssClass="mysubmit" ValidationGroup="license" CausesValidation="true" />
                                                &nbsp;
												<asp:Button ID="btnServiceStateLicenseCancel" runat="server" Text="Close" TabIndex="47"
                                                    OnClick="btnServiceStateLicenseCancel_Click" CssClass="mysubmit" CausesValidation="false" />
                                                &nbsp;
                                                <asp:Button ID="btnUploadLicense" runat="server" Visible="false" Text="Upload License" CssClass="mysubmit" TabIndex="48"
                                                    OnClientClick="javascript:return showUploadLicense();" />
                                                <asp:Button ID="btnRefreshLicense" runat="server" OnClick="btnRefreshLicense_Click" Style="display: none;" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <!-- keep this outside panle for upload control to work-->
                                <div id="div_licenseUpload" class="boxContainer" style="display: none;" title="Upload Copy of License">
                                    <span class="redstar">PDF files allowed only.</span>
                                    <div id="webUpload_license">
                                    </div>
                                </div>

                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tabPanelClaimHandle" runat="server">
                            <HeaderTemplate>
                                Types of Claims Handled
                            </HeaderTemplate>
                            <ContentTemplate>
                                <div class="editForm">
                                    <span class="label">Type of Claim Handled:</span> &nbsp;<asp:DropDownList ID="ddlPolicyType" runat="server" OnSelectedIndexChanged="ddlPolicyType_SelectedIndexChanged" AutoPostBack="true" />
                                </div>
                                <br />
                                <asp:GridView ID="gvTypeClaimHandled" CssClass="gridView" ShowFooter="false" Width="80%" runat="server" HorizontalAlign="Center"
                                    OnRowCommand="gvTypeClaimHandled_RowCommand" AutoGenerateColumns="False" CellPadding="2"
                                    AlternatingRowStyle-BackColor="#e8f2ff" HeaderStyle-Font-Size="11px"
                                    RowStyle-HorizontalAlign="Center">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnDeleteTypeClaimHandled" runat="server" CommandName="DoDelete"
                                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this record?');"
                                                    CommandArgument='<%#Eval("ID") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type of Claim">
                                            <ItemTemplate>
                                                <%# Eval("LeadPolicyType.Description") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
                        <ajaxToolkit:TabPanel ID="tabPanelReferences" runat="server">
                            <HeaderTemplate>
                                References
                            </HeaderTemplate>
                            <ContentTemplate>
                                <div>
                                    <asp:LinkButton ID="lbtnNewReference" runat="server" Text="New Reference" OnClick="lbtnNewReference_Click" Font-Size="11px" />
                                </div>
                                <asp:GridView ID="gvReferences" CssClass="gridView" ShowFooter="false" Width="100%" runat="server" HorizontalAlign="Center"
                                    OnRowCommand="gvReferences_RowCommand" AutoGenerateColumns="False" CellPadding="2"
                                    AlternatingRowStyle-BackColor="#e8f2ff" HeaderStyle-Font-Size="11px"
                                    RowStyle-HorizontalAlign="Center">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditLicense" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("ID") %>'
                                                    ToolTip="Edit" ImageUrl="~/Images/edit_icon.png" />
                                                &nbsp;
												<asp:ImageButton ID="btnDeleteLicense" runat="server" CommandName="DoDelete"
                                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this reference?');"
                                                    CommandArgument='<%#Eval("ID") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reference Name">
                                            <ItemTemplate>
                                                <%# Eval("RereferenceName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <%# Eval("CompanyName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Position">
                                            <ItemTemplate>
                                                <%# Eval("Position") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Phone">
                                            <ItemTemplate>
                                                <%# Eval("Phone") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email">
                                            <ItemTemplate>
                                                <%# Eval("Email") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <asp:Panel ID="pnlReference" runat="server" Visible="false">
                                    <table style="width: 90%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                                        <tr>
                                            <td class="right">Reference Name</td>
                                            <td>
                                                <ig:WebTextEditor ID="txtRefenreceName" runat="server" TabIndex="51" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="right">Company Name</td>
                                            <td>
                                                <ig:WebTextEditor ID="txtReferenceCompanyName" runat="server" TabIndex="52" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="right">Position</td>
                                            <td>
                                                <ig:WebTextEditor ID="txtReferencePosition" runat="server" TabIndex="53" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="right">Phone</td>
                                            <td>
                                                <ig:WebTextEditor ID="txtRefenrecePhone" runat="server" TabIndex="54" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="right">Email</td>
                                            <td>
                                                <ig:WebTextEditor ID="txtRefenreceEmail" runat="server" TabIndex="55" />
                                                <asp:RegularExpressionValidator ID="revtxtRefenreceEmail" runat="server" ControlToValidate="txtRefenreceEmail"
                                                    ErrorMessage="Email not valid!" ValidationGroup="Reference" Display="Dynamic"
                                                    CssClass="validation1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    SetFocusOnError="True"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="center">
                                                <asp:Button ID="btnReferenceSave" runat="server" OnClick="btnReferenceSave_Click" TabIndex="56"
                                                    Text="Save" CssClass="mysubmit" ValidationGroup="Reference" />
                                                &nbsp;
                                                <asp:Button ID="btnReferenceCancel" runat="server" OnClick="btnReferenceCancel_Click" TabIndex="57"
                                                    Text="Cancel" CssClass="mysubmit" CausesValidation="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>

                        <ajaxToolkit:TabPanel ID="tabPanelAdjusterSettingsAndPayroll" runat="server">
                            <HeaderTemplate>Adjuster Settings & Payroll</HeaderTemplate>
                            <ContentTemplate>
                                <table style="width: 90%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                                  
                                    <tr>
                                        <td class="right top">
                                            Adjuster Branch 
                                        </td>
                                         <td></td>
                                            
                                        <td>
                                             <ig:WebTextEditor ID="txtAdjusterBranch" MaxLength="100" runat="server" TabIndex="87"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right top">
                                          Branch Code
                                        </td>
                                         <td></td>
                                        <td>
                                             <ig:WebTextEditor ID="txtBranchCode" MaxLength="100" runat="server" TabIndex="87"></ig:WebTextEditor>
                                         </td>
                                    </tr>

                                      <tr>
                                        <td class="right top">Adjuster Rating                             
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:DropDownList ID="ddlAdjusterRating" runat="server" TabIndex="80" >
                                                <asp:ListItem Text="---Select---" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Excellent" Value="Excellent"></asp:ListItem>
                                                <asp:ListItem Text="Very Good" Value="Very Good"></asp:ListItem>
                                                <asp:ListItem Text="Good" Value="Good"></asp:ListItem>
                                                <asp:ListItem Text="Needs Improvement" Value="Needs Improvement"></asp:ListItem>
                                                <asp:ListItem Text="Do Not Use" Value="Do Not Use"></asp:ListItem>
                                                <asp:ListItem Text="Deceased" Value="Deceased"></asp:ListItem>
                                                <asp:ListItem Text="New " Value="New "></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Adjuster QA Score</td>
                                        <td></td>
                                        <td>
                                             <ig:WebNumericEditor ID="txtAdjusterQaScore" MaxLength="9" runat="server"  DataMode="Int"  HorizontalAlign="Right" TabIndex="81" CssClass="no_min_width"></ig:WebNumericEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Adjuster Designation</td>
                                        <td></td>
                                        <td>
                                             <asp:DropDownList ID="ddlAdjusterDesignation" runat="server" TabIndex="82" >
                                                 <asp:ListItem Text="---Select---" Value="0"></asp:ListItem>
                                                 <asp:ListItem Text="General Adjuster" Value="General Adjuster"></asp:ListItem>
                                                <asp:ListItem Text="Flood Adjuster" Value="Flood Adjuster"></asp:ListItem>
                                                <asp:ListItem Text="CAT Adjuster" Value="CAT Adjuster"></asp:ListItem>
                                             </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right"># of Years Adjusting Experience</td>
                                        <td></td>
                                        <td>
                                              <ig:WebNumericEditor ID="txtAdjusterExperience" MaxLength="4" runat="server"  DataMode="Int"  HorizontalAlign="Right" TabIndex="83" CssClass="no_min_width"></ig:WebNumericEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Maximum # of Claims</td>
                                        <td></td>
                                        <td>
                                            
                                             <ig:WebNumericEditor ID="txtAdjusterMaximumClaims" MaxLength="9" runat="server"  DataMode="Int"  HorizontalAlign="Right" TabIndex="84" CssClass="no_min_width"></ig:WebNumericEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Maximum Reserves</td>
                                        <td></td>
                                        <td>
                                             <ig:WebCurrencyEditor ID="txtAdjusterMaximumReserve" MaxLength="9" runat="server" MaxDecimalPlaces="2" DataMode="Decimal" HorizontalAlign="Right" TabIndex="85" CssClass="no_min_width"></ig:WebCurrencyEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">National Producer #</td>
                                        <td></td>
                                        <td>
                                            <ig:WebNumericEditor ID="txtAdjusterNationalProducer" MaxLength="9" runat="server"  DataMode="Int"  HorizontalAlign="Right" TabIndex="86" CssClass="no_min_width"></ig:WebNumericEditor>
                                     
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Geographical Area of Service</td>
                                        <td></td>
                                        <td>
                                             <ig:WebTextEditor ID="txtAdjusterGeoAreaOfService" MaxLength="100" runat="server" TabIndex="87"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Adjuster Is Active</td>
                                        <td></td>
                                        <td>
                                            <asp:CheckBox ID="chkAdjusterIsActive" runat="server" TabIndex="88"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Supervisor</td>
                                        <td></td>
                                        <td>
                                              <asp:CheckBox ID="chkAdjusterSupervisor" runat="server" TabIndex="89"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right"><b>Payroll Settings</b></td>
                                        <td></td>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Employee Type</td>
                                        <td></td>
                                        <td>
                                            <ig:WebTextEditor ID="txtAdjusterEmployeeType" MaxLength="100" runat="server" TabIndex="90"></ig:WebTextEditor>
                                          
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Default Adjuster Hourly Rate</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtAdjusterHourlyRate" MaxLength="9" runat="server" MaxDecimalPlaces="2" DataMode="Decimal" HorizontalAlign="Right" TabIndex="91" CssClass="no_min_width"></ig:WebCurrencyEditor>
                                     
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Default Adjuster Commission Rate</td>
                                        <td></td>
                                        <td>
                                            <ig:WebPercentEditor ID="txtAdjusterComissionRate" MaxLength="9" runat="server" MaxDecimalPlaces="2" DataMode="Decimal" HorizontalAlign="Right" TabIndex="92" CssClass="no_min_width"></ig:WebPercentEditor>
                                     
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Independent Contractor Agreement on File</td>
                                        <td></td>
                                        <td>
                                            <asp:CheckBox ID="chkAdjusterContractorAgreementOnFile" runat="server" TabIndex="93"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Last Year 1099 Agreement on File</td>
                                        <td></td>
                                        <td>
                                             <ig:WebNumericEditor ID="txtAdjusterLastYearAgreementoOnFile" MaxLength="4" runat="server"  DataMode="Int"  HorizontalAlign="Right" TabIndex="94" CssClass="no_min_width"></ig:WebNumericEditor>
                                     
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Resume on File</td>
                                        <td></td>
                                        <td>
                                            <asp:CheckBox ID="chkResumeOnFile" runat="server" TabIndex="95"/>
                                        </td>
                                    </tr>
                                  <tr>
                                      <td></td>
                                      <td></td>
                                            <td ><%--class="center"--%>
                                                <asp:Button ID="btnSavePayrollSetting" runat="server" OnClick="btnSavePayrollSetting_Click" TabIndex="56"
                                                    Text="Save" CssClass="mysubmit"/>
                                                
                                            </td>
                                        </tr>

                                    </table>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                         <%--  <asp:DropDownList ID="ddlAdjusterEmployeeType" runat="server" TabIndex="90" ></asp:DropDownList> --%>


                    </ajaxToolkit:TabContainer>

                </td>
            </tr>
        </table>

    </div>
</div>

<div id="div_adjusterPhotoDialog" class="boxContainer" title="Upload Adjuster Photo" style="display: none;">
    <div id="webUpload_adjusterPhoto">
    </div>
</div>
<asp:Button ID="btnRefreshAdjusterPhoto" runat="server" OnClick="btnRefreshAdjusterPhoto_Click" Style="display: none;" />
<asp:HiddenField ID="hdId" runat="server" Value="0" />
<asp:HiddenField ID="hfStateID" runat="server" Value="0" />

<script type="text/javascript">
    // adjuster license
    function showUploadLicense() {
        // build upload control for license
        $("#webUpload_license").igUpload({
            mode: 'single',
            progressUrl: "IGUploadStatusHandler.ashx",
            onError: function (e, args) {
                showAlert(e);
            },
            fileUploading: function (e, args) {
            },
            fileUploaded: function (e, args) {
                // alert(args.fileID + " " + args.filePath);               
                var adjusterID = parseInt($("#<%= hdId.ClientID %>").val());
                var stateID = parseInt($("#<%= hfStateID.ClientID %>").val());

                // save license pdf
                PageMethods.saveLicenseFile(adjusterID, stateID, args.filePath);

                // close upload dialog
                $("#div_licenseUpload").dialog('close');

                $("#<%= btnRefreshLicense.ClientID%>").click();
            }
        });

        $("#div_licenseUpload").dialog({
            modal: true,
            width: 400,
            buttons:
			{
			    'Close': function () {
			        $(this).dialog('close');
			    }
			}
        });
        return false;
    }
</script>
<script type="text/javascript">
    // adjuster photo
    // adjuster license
    function adjusterPhotoUploadDialog() {
        // build upload control for license
        $("#webUpload_adjusterPhoto").igUpload({
            mode: 'single',
            autostartupload: true,
            progressUrl: "http://appv3.claimruler.com/IGUploadStatusHandler.ashx",
            onError: function (e, args) {
                showAlert(args.errorMessage);
            },
            fileUploading: function (e, args) {
            },
            fileUploaded: function (e, args) {
                // alert(args.fileID + " " + args.filePath);               
                var adjusterID = parseInt($("#<%= hdId.ClientID %>").val());

                // save license pdf
                PageMethods.saveAdjusterPhoto(adjusterID, args.filePath);

                // close upload dialog
                $("#div_adjusterPhotoDialog").dialog('close');

                // refresh photo
                $("#<%= btnRefreshAdjusterPhoto.ClientID %>").click();
            }
        });


        $("#div_adjusterPhotoDialog").dialog({
            modal: true,
            width: 600,
            close: function () {
                $(this).dialog('destroy');
            },
            buttons:
            {
                'Close': function () {
                    $(this).dialog('close');
                }
            }
        });
        return false;
    }

</script>
