<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Protected/ClaimRuler.Master" CodeBehind="BatchAssignment.aspx.cs" Inherits="CRM.Web.Protected.Admin.BatchAssignment" %>


<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Src="~/UserControl/Admin/ucPolicyType.ascx" TagName="ucPolicyType"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

    <div class="paneContent">
        <div class="page-title">
            Batch Assignment 
        </div>
        <asp:Panel ID="pnlToolbar" runat="server" CssClass="toolbar toolbar-body" Visible="false">
            <table>
                <tr>
                    <td>
                        <a class="toolbar-item" onclick="ShowAdjusters()">
                            <span class="toolbar-img-and-text" style="background-image: url(../../images/adjuster.png)">Show Adjusters</span>
                        </a>
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <div class="paneContentInner">
            <ig:WebSplitter ID="WebSplitter1" runat="server" Width="100%" Height="650px">
                <Panes>
                    <ig:SplitterPane runat="server" Size="200px" CollapsedDirection="PreviousPane">
                        <Template>

                            <div class="section-title">
                                Filters
                            </div>
                            <table style="border-collapse: separate; border-spacing: 3px; padding: 2px; width: 100%;" border="0">
                                <tr>
                                    <td>
                                        <div style="float: left;">
                                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" ImageUrl="~/Images/show-all.png" />
                                        </div>
                                        <div style="float: right;">
                                            <asp:LinkButton ID="lbtnClear" runat="server" OnClick="lbtnClear_Click" Text="Clear" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>

                                    <td>
                                        <div>Claim Number</div>
                                        <ig:WebTextEditor ID="txtClaimNumber" runat="server" Width="150px" TabIndex="1"></ig:WebTextEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>Carrier Name</div>
                                        <ig:WebTextEditor ID="txtCarrierName" runat="server" Width="150px" TabIndex="2"></ig:WebTextEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>Create Date From</div>
                                        <ig:WebDatePicker ID="createDateFrom" runat="server" Width="150px" TabIndex="3"
                                            Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif">
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            Create Date To
                                        </div>
                                        <ig:WebDatePicker ID="createDateTo" runat="server" Width="150px" TabIndex="4"
                                            Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif">
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>Loss Date From</div>
                                        <div>
                                            <ig:WebDatePicker ID="lossDateFrom" runat="server" Width="150px" TabIndex="5"
                                                Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif">
                                            </ig:WebDatePicker>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>Loss Date To</div>
                                        <div>
                                            <ig:WebDatePicker ID="lossDateTo" runat="server" Width="150px" TabIndex="6"
                                                Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif">
                                            </ig:WebDatePicker>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>Loss State</div>
                                        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" Width="150px" TabIndex="7"
                                            OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>Loss City</div>
                                        <asp:DropDownList ID="ddlCity" runat="server" Width="150px" TabIndex="8"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>Loss Zip Code</div>
                                        <div>
                                            <ig:WebTextEditor ID="txtZipCode" runat="server" Width="150px" TabIndex="9"></ig:WebTextEditor>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>Coverage Type</div>
                                        <uc1:ucPolicyType ID="ucPolicyType1" runat="server" requiredValidation="false" />
                                    </td>
                                </tr>


                            </table>

                        </Template>
                    </ig:SplitterPane>
                    <ig:SplitterPane runat="server">
                        <Template>
                            <asp:Panel ID="pnlResult" runat="server" Visible="false">
                                <div id="div_mapPlaceHolder"></div>
                                <div id="myMap" style="position: relative; width: 700px; height: 450px; margin: 5px;" class="boxContainer left" title="Double click for full screen">
                                </div>
                                <div style="text-align: center; margin-bottom: 5px; margin-top: 5px;">
                                    <asp:UpdatePanel ID="updatePanel_grid" runat="server">
                                        <ContentTemplate>
                                            Assign selected files to adjuster:&nbsp;<span class="redstar">*</span>&nbsp;<asp:DropDownList ID="ddlAdjuster" runat="server"></asp:DropDownList>&nbsp;
											<asp:Button ID="btnAssign" runat="server" Text="Assign" CssClass="mysubmit" OnClick="btnAssign_Click" ValidationGroup="assign" />
                                            <asp:RequiredFieldValidator ID="rfvAssign" runat="server" ControlToValidate="ddlAdjuster" CssClass="validation1"
                                                InitialValue="0" SetFocusOnError="true" ErrorMessage="" Display="Dynamic" ValidationGroup="assign" />
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td class="left top">
                                                        <asp:GridView ID="gvAdjusters" runat="server" AutoGenerateColumns="False" Width="250px"
                                                            CssClass="gridView" HorizontalAlign="Center" AlternatingRowStyle-BackColor="#e8f2ff" CellPadding="2">
                                                            <Columns>
                                                                <asp:BoundField DataField="AdjusterName" HeaderText="Adjuster Name" />
                                                                <asp:BoundField DataField="MumberOfClaims" HeaderText="# Claims" ItemStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                    <td class="left top">
                                                        <asp:GridView ID="gvSearchResult" CssClass="gridView" DataKeyNames="ClaimID" HorizontalAlign="Center"
                                                            Width="100%" runat="server" AutoGenerateColumns="False" CellPadding="2" AlternatingRowStyle-BackColor="#e8f2ff"
                                                            AllowSorting="true" OnSorting="gvSearchResult_Sorting" OnRowDataBound="gvSearchResult_RowDataBound">
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="cbxAll" runat="server" AutoPostBack="true" OnCheckedChanged="cbxAll_CheckedChanged" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbxSelected" runat="server" />
                                                                        <asp:HiddenField ID="hf_lossLocation" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Claim Number" SortExpression="ClaimNumber">
                                                                    <ItemTemplate>
                                                                        <%#Eval("ClaimNumber") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Adjuster">
                                                                    <ItemTemplate>
                                                                        <%#Eval("AdjusterName") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Carrier" SortExpression="InsuranceCompanyName">
                                                                    <ItemTemplate>
                                                                        <%#Eval("InsuranceCompanyName") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Type of Loss" SortExpression="TypeOfLoss">
                                                                    <ItemTemplate>
                                                                        <%#Eval("TypeOfLoss") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Insured" SortExpression="InsuredName">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="hlnkLead" Text='<%# Eval("InsuredName") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Loss Address">
                                                                    <ItemTemplate>
                                                                        <div>
                                                                            <%#Eval("LossAddress") %>
                                                                        </div>
                                                                        <div>
                                                                            <%#Eval("CityName") %>,&nbsp;<%#Eval("StateName") %>&nbsp;<%#Eval("Zip") %>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Loss Date" SortExpression="LossDate">
                                                                    <ItemTemplate>
                                                                        <%#Eval("LossDate", "{0:MM/dd/yyyy}") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Loss City" SortExpression="CityName">
                                                                    <ItemTemplate>
                                                                        <%#Eval("CityName") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Loss State" SortExpression="StateName">
                                                                    <ItemTemplate>
                                                                        <%#Eval("StateName") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Loss Zip" SortExpression="Zip">
                                                                    <ItemTemplate>
                                                                        <%#Eval("Zip") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>

                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </asp:Panel>
                        </Template>
                    </ig:SplitterPane>
                </Panes>
            </ig:WebSplitter>




        </div>
    </div>
    <asp:HiddenField ID="hf_adjusters" runat="server" />

    <script type="text/javascript" src="https://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0&s=1"></script>
    <script type="text/javascript" src="../../js/batchAssingment.js"></script>
    <script type="text/javascript">


        $(document).ready(function () {
            //  LoadMap();
            //    $("#myMap").resizable();
        });

        $(document).keyup(function (e) {

            if (e.keyCode == 27 && isFullsize) {
                // esc key
                resetMapSize();
            }
        });

    </script>
</asp:Content>
