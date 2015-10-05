<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCarrierEdit.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucCarrierEdit" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControl/Admin/ucCarrierContact.ascx" TagName="ucCarrierContact" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Admin/ucInvoiceProfileFeeSchedule.ascx" TagName="ucInvoiceProfileFeeSchedule" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/Admin/ucInvoiceProfileFeeProvision.ascx" TagName="ucInvoiceProfileFeeProvision" TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/Admin/ucCarrierInvoicProfileType.ascx" TagName="ucCarrierInvoicProfileType" TagPrefix="uc6" %>
<%@ Register Src="~/UserControl/Admin/ucInvoiceProfileFeeItemized.ascx" TagName="ucInvoiceProfileFeeItemized" TagPrefix="uc7" %>
<%@ Register Src="~/UserControl/Admin/ucCarrierComment.ascx" TagName="ucCarrierComment" TagPrefix="uc8" %>
<%@ Register Src="~/UserControl/Admin/ucCarrierDocument.ascx" TagName="ucCarrierDocument" TagPrefix="uc9" %>
<%@ Register Src="~/UserControl/Admin/ucCarrierTask.ascx" TagName="ucCarrierTask" TagPrefix="uc10" %>
<div class="toolbar toolbar-body">
    <table>
        <tr>
            <td>
                <asp:LinkButton ID="btnBack" runat="server" CssClass="toolbar-item" PostBackUrl="~/Protected/Admin/CarrierList.aspx">
							<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">Client List</span>
                </asp:LinkButton>
            </td>
            <td>
                <asp:LinkButton ID="btnSave" runat="server" CssClass="toolbar-item" OnClick="btnSave_Click" ValidationGroup="carrier" CausesValidation="true">
							<span class="toolbar-img-and-text" style="background-image: url(../../images/toolbar_save.png)">Save</span>
                </asp:LinkButton>
            </td>
        </tr>
    </table>
</div>

<div class="message_area">
    <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
    <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
    <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
</div>

<div class="paneContentInner">
    <table style="width: 100%;">
        <tr>
            <td class="left top">
                <div class="boxContainer" style="height: 310px">
                    <div class="section-title">
                        <%--Insurer/Carrier--%>
                        Client
                    </div>
                    <table style="width: 100%; border-collapse: separate; border-spacing: 2px; padding: 2px; text-align: left;" border="0" class="editForm">
                        <tr>
                            <td class="right">Client Name</td>
                            <td class="redstar" style="width: 5px;">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtName" MaxLength="100" runat="server" Width="250px" />
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtName"
                                        ErrorMessage="Please enter name." ValidationGroup="carrier" Display="Dynamic" SetFocusOnError="true"
                                        CssClass="validation1" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Address</td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtAddress" runat="server" MaxLength="100" Width="250px" />
                                <div>
                                    <asp:RequiredFieldValidator ID="tfvtxtAddress" runat="server" CssClass="validation1"
                                        ControlToValidate="txtAddress" Display="Dynamic" ErrorMessage="Please enter address."
                                        ValidationGroup="carrier" SetFocusOnError="True" />
                                </div>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtAddress2" runat="server" MaxLength="50" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Country</td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">State</td>
                            <td class="redstar">*</td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="true" />
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvstate" ControlToValidate="ddlState"
                                        ErrorMessage="Please select state." ValidationGroup="carrier" Display="Dynamic"
                                        CssClass="validation1" SetFocusOnError="True" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">City</td>
                            <td class="redstar">*</td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" />
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvcity" ControlToValidate="ddlCity" InitialValue="0"
                                        ErrorMessage="Please select city." ValidationGroup="carrier" Display="Dynamic"
                                        CssClass="validation1" SetFocusOnError="True" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Postal Code
                            </td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtZipCode" runat="server" MaxLength="20" />
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvzipcode" ControlToValidate="txtZipCode"
                                        ErrorMessage="Please enter zip code." ValidationGroup="carrier" Display="Dynamic"
                                        CssClass="validation1" SetFocusOnError="True" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="width: 70%;" class="left top">
                <ajaxToolkit:TabContainer ID="tabContainer" runat="server" Width="100%" ActiveTabIndex="0" Visible="false">
                    <ajaxToolkit:TabPanel ID="tabPanel1" runat="server">
                        <HeaderTemplate>
                            Invoice Settings                        
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div style="margin-bottom: 5px;">
                                <asp:LinkButton CssClass="link" ID="blnkNewInvoiceProfile" runat="server" Text="New Invoice Profile" OnClick="blnkNewInvoiceProfile_Click" />

                            </div>

                            <asp:Panel ID="pnlInvoiceProfile" runat="server" Visible="False">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="top left" style="width: 40%;">

                                            <table class="editForm nowrap no_min_width" style="width: 100%;">
                                                <tr>
                                                    <td class="right">Program Name</td>
                                                    <td class="redstar" style="width: 5px;">*</td>
                                                    <td>
                                                        <ig:WebTextEditor ID="txtProgramName" runat="server" MaxLength="100" Width="250px" />

                                                        <asp:RequiredFieldValidator ID="rfvPRogramName" runat="server" ControlToValidate="txtProgramName"
                                                            SetFocusOnError="True" ValidationGroup="Profile" ErrorMessage="Please enter program name."
                                                            Display="Dynamic" CssClass="validation1" />

                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                                    <td class="right top">Coverage Area</td>
                                                    <td></td>
                                                    <td>
                                                        <div id="divdamage" style="height: 100px; width: 250px; overflow: auto;" class="checkboxlist_container">
                                                            <asp:CheckBoxList ID="cbxCoverageArea" runat="server" Width="230px" />

                                                        </div>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td class="right">Effective Date</td>
                                                    <td class="redstar" style="width: 5px;">*</td>
                                                    <td>
                                                        <ig:WebDatePicker ID="effetiveDate" runat="server" CssClass="date_picker" />
                                                        <asp:RequiredFieldValidator ID="rfEffDate" runat="server" ControlToValidate="effetiveDate"
                                                            SetFocusOnError="True" ValidationGroup="Profile" ErrorMessage="Please enter effective date."
                                                            Display="Dynamic" CssClass="validation1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="right">Expiration Date</td>
                                                    <td class="redstar" style="width: 5px;">*</td>
                                                    <td>
                                                        <ig:WebDatePicker ID="expirationDate" runat="server" CssClass="date_picker" />
                                                        <asp:RequiredFieldValidator ID="rfExpDate" runat="server" ControlToValidate="expirationDate"
                                                            SetFocusOnError="True" ValidationGroup="Profile" ErrorMessage="Please enter expiration date."
                                                            Display="Dynamic" CssClass="validation1" />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="right">Profile Type</td>
                                                    <td class="redstar">*</td>
                                                    <td>
                                                        <uc6:ucCarrierInvoicProfileType runat="server" ID="ucProfileType" />

                                                        <div>
                                                            <asp:RequiredFieldValidator ID="rfvInvoiceProfile" runat="server" ControlToValidate="ucProfileType$ddlInvoicProfileType"
                                                                Display="Dynamic" ErrorMessage="Please select invoice profile." SetFocusOnError="True" ValidationGroup="Profile"
                                                                CssClass="validation1" InitialValue="0"></asp:RequiredFieldValidator>

                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="right">Invoice Type</td>
                                                    <td class="redstar">*</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlInvoiceType" runat="server" />

                                                        <div>
                                                            <asp:RequiredFieldValidator ID="rfvInvoiceTYpe" runat="server" ControlToValidate="ddlInvoiceType"
                                                                Display="Dynamic" ErrorMessage="Please select invoice type." SetFocusOnError="True" ValidationGroup="Profile"
                                                                CssClass="validation1" InitialValue="0"></asp:RequiredFieldValidator>

                                                        </div>
                                                    </td>
                                                </tr>
                                              
                                                <tr>
                                                    <td class="right">Firm Discount Percentage</td>
                                                    <td></td>
                                                    <td>
                                                        <ig:WebPercentEditor ID="txtFirmDiscountPercentage" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="right">Client Contact</td>
                                                    <td class="redstar" style="width: 5px;">*</td>
                                                    <td>
                                                        <ig:WebTextEditor ID="txtAccoutingContact" runat="server" MaxLength="100" Width="250px" />
                                                         <asp:RequiredFieldValidator ID="rfAccountContact" runat="server" ControlToValidate="txtAccoutingContact"
                                                            SetFocusOnError="True" ValidationGroup="Profile" ErrorMessage="Please enter account contact."
                                                            Display="Dynamic" CssClass="validation1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="right">Client Contact Email</td>
                                                    <td class="redstar" style="width: 5px;">*</td>
                                                    <td>
                                                        <ig:WebTextEditor ID="txtAccoutingContactEmail" runat="server" MaxLength="100" Width="250px" />
                                                         <asp:RequiredFieldValidator ID="rfAccountEmail" runat="server" ControlToValidate="txtAccoutingContactEmail"
                                                            SetFocusOnError="True" ValidationGroup="Profile" ErrorMessage="Please enter account contact email."
                                                            Display="Dynamic" CssClass="validation1" />
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                                    <td class="right">Adjuster Payroll % for Flat/CAT</td>
                                                    <td ></td>
                                                    <td>
                                                        <ig:WebPercentEditor ID="txtFlatCatPercent" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" MaxLength="100" Width="80px" />
                                                        
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td class="right"> Adjuster Flat Fee for Flat/CAT</td>
                                                    <td></td>
                                                    <td>
                                                        <ig:WebNumericEditor ID="txtFlatCatFee" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" MaxLength="100" Width="80px" />
                                                        
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td colspan="3"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" class="center">

                                                        <asp:Button ID="btnProfileSave" runat="server" Text="Save"
                                                            OnClick="btnProfileSave_Click" ValidationGroup="Profile" CssClass="mysubmit" />

                                                        &nbsp;
														<asp:Button ID="btnProfileCancel" runat="server" Text="Cancel"
                                                            OnClick="btnProfileCancel_Click" CausesValidation="False" CssClass="mysubmit" />

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="top left">
                                            <ajaxToolkit:TabContainer ID="tabContainerFee" runat="server" Width="100%" ActiveTabIndex="0">
                                                <ajaxToolkit:TabPanel ID="tabPanelFeeSchedule" runat="server">
                                                    <HeaderTemplate>
                                                        Full Cost of Repair
                                                    
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <uc4:ucInvoiceProfileFeeSchedule ID="ucFeeSchedule" runat="server" />


                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <%--  <ajaxToolkit:TabPanel ID="tabPanelProvision" runat="server">
                                                    <HeaderTemplate>
                                                        Pricing Provision
                                                    
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <uc5:ucInvoiceProfileFeeProvision ID="ucFeeProvision" runat="server" />

                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>--%>
                                                <ajaxToolkit:TabPanel ID="tabPanelItemized" runat="server">
                                                    <HeaderTemplate>
                                                        Time and Expense Billing
                                                    
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <uc7:ucInvoiceProfileFeeItemized ID="ucFeeItemized" runat="server" />

                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                            </ajaxToolkit:TabContainer>

                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:GridView ID="gvInvoiceProfile" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="gridView"
                                HorizontalAlign="Center" CellPadding="2" OnRowCommand="gvInvoiceProfile_RowCommand" OnRowDataBound="gvInvoiceProfile_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Program Name">
                                        <ItemTemplate>
                                            <%#Eval("ProfileName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Profile Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProfileTypeDescription" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Type">
                                        <ItemTemplate>
                                            <%#Eval("CarrierInvoiceType.InvoiceType") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective Date">
                                        <ItemTemplate>
                                            <%# Eval("EffiectiveDate", "{0:MM/dd/yyyy}") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expiration Date">
                                        <ItemTemplate>
                                            <%# Eval("ExpirationDate", "{0:MM/dd/yyyy}") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("CarrierInvoiceProfileID") %>' CommandName="DoEdit" ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                                            &#160;
                                            <asp:ImageButton ID="btnCopy" runat="server"
                                                CommandName="DoCopy"
                                                CommandArgument='<%#Eval("CarrierInvoiceProfileID") %>'
                                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to clone this profile?');"
                                                ToolTip="Copy"
                                                ImageUrl="~/Images/copy.png"
                                                />
                                            &nbsp;
											<asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%#Eval("CarrierInvoiceProfileID") %>' CommandName="DoDelete" ImageUrl="~/Images/delete_icon.png" OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this profile?');" ToolTip="Delete" />


                                        </ItemTemplate>

                                        <ItemStyle HorizontalAlign="Center" Width="45px" Wrap="False"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>

                                <FooterStyle HorizontalAlign="Center" />

                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>


                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabPanelLocation" runat="server">
                        <HeaderTemplate>
                            Insurers/Branches
                        
                        </HeaderTemplate>

                        <ContentTemplate>
                            <div style="margin-bottom: 5px;">
                                <asp:LinkButton CssClass="link" ID="lbtnNewLocation" runat="server" Text="New Branch" OnClick="lbtnNewLocation_Click" />
                            </div>
                            <asp:GridView ID="gvLocation" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="gridView"
                                HorizontalAlign="Center" CellPadding="2" OnRowCommand="gvLocation_RowCommand" OnRowDataBound="gvLocation_RowDataBound">
                                <RowStyle HorizontalAlign="Center" />
                                <FooterStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Branch Name">
                                        <ItemTemplate>
                                            <%#Eval("LocationName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <ItemTemplate>
                                            <%#Eval("DepartmentName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Street Address">
                                        <ItemTemplate>
                                            <div>
                                                <%#Eval("AddressLine1") %> &nbsp;<%#Eval("AddressLine2") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Country">
                                        <ItemTemplate>
                                            <%#Eval("CountryMaster.CountryName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="State">
                                        <ItemTemplate>
                                            <%#Eval("StateMaster.StateName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="City">
                                        <ItemTemplate>
                                            <%#Eval("CityMaster.CityName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Postal Code">
                                        <ItemTemplate>
                                            <%#Eval("ZipCode") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
                                        ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("CarrierLocationID") %>'
                                                ToolTip="Edit" ImageUrl="~/Images/edit.png" />
                                            &nbsp;
											<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
                                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this adjuster?');"
                                                CommandArgument='<%#Eval("CarrierLocationID") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Panel ID="pnlLocation" runat="server" Visible="false">
                                <table style="width: 100%; border-collapse: separate; border-spacing: 2px; padding: 2px; text-align: left;" border="0" class="editForm">
                                    <tr>
                                        <td class="right" style="width: 15%;">Location Name</td>
                                        <td class="redstar">*</td>
                                        <td>
                                            <ig:WebTextEditor ID="txtLocationName" runat="server"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Department</td>
                                        <td></td>
                                        <td>
                                            <ig:WebTextEditor ID="txtLocationDepartment" runat="server"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Street Address</td>
                                        <td></td>
                                        <td>
                                            <ig:WebTextEditor ID="txtLocationAddressLine1" runat="server"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right"></td>
                                        <td></td>
                                        <td>
                                            <ig:WebTextEditor ID="txtLocationAddressLine2" runat="server"></ig:WebTextEditor>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Country</td>
                                        <td></td>
                                        <td>
                                            <asp:DropDownList ID="ddlLocationCountry" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">State</td>
                                        <td></td>
                                        <td>
                                            <asp:DropDownList ID="ddlLocationState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocationState_SelectedIndexChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">City Name</td>
                                        <td></td>
                                        <td>
                                            <asp:DropDownList ID="ddlLocationCity" runat="server" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="right">Postal Code</td>
                                        <td></td>
                                        <td>
                                            <ig:WebTextEditor ID="txtLocationZipCode" runat="server"></ig:WebTextEditor>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:Button ID="btnLocationSave" runat="server" Text="Save"
                                                OnClick="btnLocationSave_Click" CausesValidation="true" ValidationGroup="Location" CssClass="mysubmit" />
                                            &nbsp;
											<asp:Button ID="btnLocationCancel" runat="server" Text="Cancel"
                                                OnClick="btnLocationCancel_Click" CausesValidation="false" CssClass="mysubmit" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabPanelContacts" runat="server">
                        <HeaderTemplate>
                            Contacts
                        
                        </HeaderTemplate>

                        <ContentTemplate>
                            <uc3:ucCarrierContact ID="carrierContacts" runat="server" />

                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabPanelComment" runat="server">
                        <HeaderTemplate>
                            Comments
                        
                        </HeaderTemplate>

                        <ContentTemplate>
                            <uc8:ucCarrierComment ID="carrierComments" runat="server" />

                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabPanelDocuments" runat="server">
                        <HeaderTemplate>
                            Documents
                        
                        </HeaderTemplate>

                        <ContentTemplate>
                            <uc9:ucCarrierDocument ID="carrierDocuments" runat="server" />

                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabPanelTasks" runat="server">
                        <HeaderTemplate>
                            Tasks
                        
                        </HeaderTemplate>

                        <ContentTemplate>

                             <ig:WebDatePicker ID="myyyyyytxtLossDate" style="display:none"   runat="server" TabIndex="18" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>

                            <uc10:ucCarrierTask ID="carrierTasks" runat="server" />

                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabPanelDocumentPackageSettings" runat="server">
                        <HeaderTemplate>
                            Document Package Settings
                        
                        </HeaderTemplate>

                        <ContentTemplate>
                            <div>
                                Requires configured per Integration / Data Exchange requirements at Carrier.
                            </div>

                        </ContentTemplate>

                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
            </td>
        </tr>
    </table>
</div>

