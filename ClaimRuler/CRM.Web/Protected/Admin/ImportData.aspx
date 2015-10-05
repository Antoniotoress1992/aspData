<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="ImportData.aspx.cs" Inherits="CRM.Web.Protected.Admin.ImportData" %>

<%@ Register Assembly="Infragistics45.Web.jQuery.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Data Import
        </div>

        <div class="paneContentInner">
        </div>
        <div class="message_area">
            <asp:Label ID="lblMessage" runat="server" />
        </div>
        <asp:Wizard ID="wizardImport" runat="server" ActiveStepIndex="0" CssClass="boxContainer" Width="50%"
            DisplaySideBar="false" Style="margin: auto;" OnNextButtonClick="Wizard1_NextButtonClick" OnFinishButtonClick="wizardImport_FinishButtonClick">
            <NavigationButtonStyle CssClass="mysubmit" />
            <WizardSteps>
                <asp:WizardStep ID="WizardStep1" runat="server" Title="Select File to Import">

                    <div class="section-title">
                        Select Type of Data to Import
                    </div>
                    <div class="paneContentInner">
                        <p class="center"><span class="red">Billing Alert</span></p>
                        <p>WARNING:  You are about to import claims into Claim Ruler software and your per file transaction fee will apply. You will be billed your contract rate per claim file for each file imported on your monthly Claim Ruler software billing. </p>
                        <p>If you are a new customer and you need to import historical claims, then please contact your account executive to arrange a one-time import.</p>
                        <p>If you do not wish to continue with billing for additional file imports, then please press Cancel.  Please press Continue if you wish to incur additional billing per file imported.</p>
                        <p class="center">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="mysubmit" PostBackUrl="~/Protected/Dashboard.aspx" />
                        </p>
                    </div>
                    <div class="paneContentInner" style="margin-top: 50px;">


                        <table style="width: 100%; border-collapse: separate; border-spacing: 0px; padding: 0px; margin:auto;" border="0">
                            <tr>
                                <td class="right" style="width: 30%;">Data to Import</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlImportDataType" runat="server">
                                        <asp:ListItem Text="--- Select ---" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Claim Data" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="ddlImportDataType" SetFocusOnError="True" Display="Dynamic"
                                            CssClass="validation1" ErrorMessage="Please select data of data to import." />
                                    </div>
                                </td>
                            </tr>

                        </table>

                    </div>
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStep2" runat="server" StepType="Auto">
                    <div class="section-title">
                        Select .CSV File 
                    </div>
                    <div class="paneContentInner" style="margin-top: 50px;">
                        <table style="width: 100%; border-collapse: separate; border-spacing: 0px; padding: 0px;" border="0">

                            <tr>
                                <td></td>
                                <td>
                                    <asp:FileUpload ID="fileUpload" runat="server" Width="80%" CssClass="boxContainer" />
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="fileUpload" SetFocusOnError="True" Display="Dynamic"
                                            CssClass="validation1" ErrorMessage="Please select file to import." />
                                    </div>

                                </td>
                            </tr>
                        </table>

                    </div>
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStep3" runat="server" StepType="Auto">
                    <div class="section-title">
                        Map Import Fields
                    </div>
                    <div class="paneContentInner" style="margin-top: 5px;">

                        <div style="height: 500px; overflow: auto;">
                            <asp:GridView ID="gvClaimFieldMap" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="gridView"
                                CellPadding="4"
                                HorizontalAlign="Center"
                                OnRowDataBound="gvClaimFieldMap_RowDataBound"
                                Width="100%">
                                <RowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Your Field Names">
                                        <ItemTemplate>
                                            <asp:Label ID="lblYourFieldName" runat="server" Text='<%# Eval("UserFieldName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Claim Ruler Field Names">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlClaimRulerFields" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>


                        </div>
                    </div>
                </asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>

    </div>

</asp:Content>
