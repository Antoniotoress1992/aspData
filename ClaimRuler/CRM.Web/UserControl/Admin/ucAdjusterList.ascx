<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAdjusterList.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucAdjusterList" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div class="toolbar toolbar-body">
    <table>
        <tr>
            <td>
                <asp:LinkButton ID="btnReturnToClaim" runat="server" CssClass="toolbar-item" OnClick="btnReturnToClaim_Click">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">Return to Claim</span>
                </asp:LinkButton>
            </td>
            <td>
                <asp:LinkButton ID="btnSend1" runat="server" CssClass="toolbar-item" OnClick="btnNew_Click" Visible='<%# Convert.ToBoolean(masterPage.hasAddPermission) %>'>
							<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Adjuster</span>
                </asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
<table style="border-collapse: separate; border-spacing: 3px; padding: 2px; width: 100%;" border="0">
    <tr>
        <td class="left top" style="width: 200px; border: 1px solid #DDDDE4;">
            <div class="section-title">
                Filters
            </div>
            <table style="border-collapse: separate; border-spacing: 3px; padding: 2px; width: 100%;" border="0" class="editForm">
                <tr>
                    <td>
                        <div style="float: left;">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" ImageUrl="~/Images/searchbg.png" />
                        </div>
                        <div style="float: right;">
                            <asp:LinkButton ID="lbtnClear" runat="server" OnClick="lbtnClear_Click" Text="Clear" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>Adjuster Name</div>
                        <ig:WebTextEditor ID="txtName" runat="server"></ig:WebTextEditor>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>Company Name</div>
                        <ig:WebTextEditor ID="txtCompanyName" runat="server"></ig:WebTextEditor>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>Street Address</div>
                        <ig:WebTextEditor ID="txtStreetAddress" runat="server"></ig:WebTextEditor>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>State</div>
                        <asp:DropDownList ID="ddlState" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>City</div>
                        <ig:WebTextEditor ID="txtCity" runat="server"></ig:WebTextEditor>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>Zip</div>
                        <ig:WebTextEditor ID="txtZipCode" runat="server"></ig:WebTextEditor>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>Credentials</div>
                        <ig:WebTextEditor ID="txtCredentials" runat="server"></ig:WebTextEditor>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>FEIN Number</div>
                        <ig:WebTextEditor ID="txtFEIN" runat="server"></ig:WebTextEditor>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>Years of Experience</div>
                        <ig:WebTextEditor ID="txtYearExperience" runat="server"></ig:WebTextEditor>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>W9</div>
                        <asp:DropDownList ID="ddlW9" runat="server" DisplayMode="DropDownList">
                            <Items>
                                <asp:ListItem Text="--- Select ---" Value="0" />
                                <asp:ListItem Text="W9" Value="1" />
                                <asp:ListItem Text="Non-W9" Value="2" />
                            </Items>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>Status</div>
                        <asp:DropDownList ID="ddlStatus" runat="server" DisplayMode="DropDownList">
                            <Items>
                                <asp:ListItem Text="--- Select ---" Value="0" />
                                <asp:ListItem Text="Active" Value="1" />
                                <asp:ListItem Text="Inactive" Value="2" />
                            </Items>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>Geographical Area of Service</div>
                        <ig:WebTextEditor ID="txtServiceArea" runat="server"></ig:WebTextEditor>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>State(s) of Service & Licensure per State</div>
                        <asp:DropDownList ID="ddlServiceStateLicense" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>Types of Claims Handled</div>
                        <asp:DropDownList ID="ddlTypeClaimHandled" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
        <td class="left top" style="width:50%;">
            <asp:GridView ID="gvAdjuster" CssClass="gridView" runat="server"
                AutoGenerateColumns="False"
                AllowSorting="true"
                AllowPaging="true"
                HorizontalAlign="Left"
                OnRowCommand="gvAdjuster_RowCommand"
                OnRowDataBound="gvAdjuster_RowDataBound"
                OnPageIndexChanging="gvAdjuster_PageIndexChanging"
                CellPadding="4"
                AlternatingRowStyle-BackColor="#e8f2ff"
                PageSize="20"
                RowStyle-HorizontalAlign="Center"
                PagerSettings-PageButtonCount="10"
                OnSorting="gv_onSorting"
                Width="100%">
                <PagerStyle CssClass="pager" />
                <EmptyDataTemplate>
                    No adjusters available.
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="32px" />
                        <ItemTemplate>
                            <asp:Image runat="server" ID="adjusterPhoto" Width="32px" Height="32px" ImageAlign="Middle" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Last Name" SortExpression="LastName"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LastName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="First Name" SortExpression="FirstName"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("FirstName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Full Name" SortExpression="adjusterName"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("adjusterName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone #"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("PhoneNumber") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("email") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company Name" SortExpression="CompanyName"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("CompanyName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="State(s) of Service/License per State"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Repeater ID="repeaterStateLicense" runat="server">
                                <ItemTemplate>
                                    <div>
                                        <%# Eval("StateMaster.StateName") %>&nbsp;&nbsp;<%# Eval("LicenseNumber") %>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Types of Claims Handled"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Repeater ID="repeaterTypeClaimHandle" runat="server">
                                <ItemTemplate>
                                    <div>
                                        <%# Eval("LeadPolicyType.Description") %>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="E-Mail Notification"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("isEmailNotification")) ? "Yes" : "No"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" SortExpression="Status"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("Status")) ? "Active" : "Inactive" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server"
                                CommandName="DoEdit"
                                CommandArgument='<%#Eval("AdjusterId") %>'
                                ToolTip="Edit"
                                ImageUrl="~/Images/edit.png"
                                Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>' />
                            &nbsp;
				            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this adjuster?');"
                                CommandArgument='<%#Eval("AdjusterId") %>'
                                ToolTip="Delete"
                                ImageUrl="~/Images/delete_icon.png"
                                Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>' />

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>



<asp:HiddenField ID="hdId" runat="server" Value="0" />
