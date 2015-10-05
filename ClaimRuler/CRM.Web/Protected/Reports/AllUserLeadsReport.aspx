<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true"
    CodeBehind="AllUserLeadsReport.aspx.cs" Inherits="CRM.Web.Protected.Reports.AllUserLeadsReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="mainbox">
        <h2>
            Lead Detail</h2>
        <div class="warrape">
            <div class="search_part" style="margin-bottom: 15px;">
                <fieldset style="width: 95%">
                    <legend style="font-weight: bold; color: Blue;">SEARCH</legend>
                    <table width="95%" border="0" cellspacing="0" cellpadding="00" class="new_user" align="center">
                        <tr>
                            <td align="right">
                                User Name&nbsp;
                            </td>
                            <td colspan="1">
                                <asp:TextBox ID="txtSearch" Width="85%" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td align="center">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="ClientPayments"
                                    CssClass="mysubmit" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150" align="right" valign="top" style="padding-top: 5px;">
                                Date From&nbsp;
                            </td>
                            <td width="188" align="left" valign="top">
                                <div style="width: 290px;">
                                    <asp:TextBox ID="txtDateFrom" onkeypress="javascript:return false;" Width="150px"
                                        runat="server" CssClass="myinput"></asp:TextBox>&nbsp;&nbsp;
                                    <button id="f_rangeEnd_trigger" tabindex="29" style="margin: 0; padding: 0; border: none;
                                        background: none; font-size: 1em;">
                                        <img src="../../Images/calendar.jpg" width="14" height="13" /></button>
                                    <script type="text/javascript">
                                        RANGE_CAL_2 = new Calendar({
                                            inputField: "<%=txtDateFrom.ClientID %>",
                                            dateFormat: "%d/%m/%Y",
                                            trigger: "f_rangeEnd_trigger",
                                            bottomBar: true,
                                            showTime: true,
                                            onSelect: function () {
                                                this.hide();
                                            }
                                        });
                                    </script>
                                </div>
                            </td>
                            <td width="150" align="right" valign="top" style="padding-top: 5px;">
                                Date To&nbsp;
                            </td>
                            <td width="10%">
                                <div style="width: 290px;">
                                    <asp:TextBox ID="txtDateTo" onkeypress="javascript:return false;" Width="150px" runat="server"
                                        CssClass="myinput"></asp:TextBox>&nbsp;&nbsp;
                                    <button id="f_rangeEnd_trigger1" tabindex="29" style="margin: 0; padding: 0; border: none;
                                        background: none; font-size: 1em;">
                                        <img src="../../Images/calendar.jpg" width="14" height="13" /></button>
                                    <script type="text/javascript">
                                        RANGE_CAL_2 = new Calendar({
                                            inputField: "<%=txtDateTo.ClientID %>",
                                            dateFormat: "%d/%m/%Y",
                                            trigger: "f_rangeEnd_trigger1",
                                            bottomBar: true,
                                            showTime: true,
                                            onSelect: function () {
                                                this.hide();
                                            }
                                        });
                                    </script>
                                </div>
                                &nbsp;
                            </td>
                            <td align="center">
                                <asp:Button ID="btnReset" Text="Reset" runat="server" CausesValidation="false" CssClass="mysubmit"
                                    Width="71px" OnClick="btnReset_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
    </div>
    <br />
    <div style="width: 945px; overflow: auto;">
        <fieldset><%-- "style="width: 925px; overflow: auto;--%>
            <legend style="font-weight: bold; color: Blue;">REPORT</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left">
                        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" Width="100%" AutoDataBind="true"
                            HasCrystalLogo="False" HasDrillUpButton="False" HasToggleGroupTreeButton="False"
                            HasZoomFactorList="False" SeparatePages="False" ToolPanelView="None" BestFitPage="False" />
                        <%-- --%>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hfFromDate" runat="server" Value="" />
    <asp:HiddenField ID="hfToDate" runat="server" Value="" />
    <asp:HiddenField ID="hfCriteria" runat="server" Value="" />
</asp:Content>
