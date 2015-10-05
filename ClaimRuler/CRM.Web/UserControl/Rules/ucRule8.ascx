﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRule8.ascx.cs" Inherits="CRM.Web.UserControl.Rules.ucRule8" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnSave">

    <div class="boxContainer">
        <div class="section-title">
            <asp:Label ID="lblTitle" runat="server" />
        </div>
        <div class="paneContentInner">
            <div class="message_area">
                <asp:Label ID="lblMessage" runat="server" />
            </div>
            <table style="width: 100%" class="editForm no_min_width">
                <tr>
                    <td class="left">Flag and Review for Expense &nbsp;<asp:DropDownList ID="ddlExpenseType" runat="server" />

                        &nbsp;in on the claim/file for Carrier &nbsp;
                        <asp:DropDownList ID="ddlCarrier" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <ig:WebTextEditor ID="txtDescription" runat="server" TextMode="MultiLine" Width="100%" MultiLine-Rows="3" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="cbxActive" runat="server" Text="Active" TextAlign="Right" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:CheckBox ID="cbxEmailAdjuster" runat="server" Text="E-mail Adjuster when alert occurs" TextAlign="Right" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:CheckBox ID="cbxEmailSupervisor" runat="server" Text="E-mail Supervisor when alert occurs" TextAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="ddlExpenseType"
                            ErrorMessage="Please select expense." ValidationGroup="rule" Display="Dynamic" InitialValue="0"
                            CssClass="validation1" />
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlCarrier"
                            ErrorMessage="Please select carrier." ValidationGroup="rule" Display="Dynamic"
                            CssClass="validation1" InitialValue="0" />
                    </td>
                </tr>
                <tr>
                    <td class="center paneContentInner">
                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="rule" CausesValidation="true" OnClick="btnSave_Click" CssClass="mysubmit" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" ValidationGroup="rule" CausesValidation="false" OnClick="btnCancel_Click" CssClass="mysubmit" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Panel>
