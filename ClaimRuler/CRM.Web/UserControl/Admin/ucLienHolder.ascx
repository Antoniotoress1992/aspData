<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLienHolder.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucLienHolder" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<div class="paneContent">
    <div class="page-title">
        Mortgagees
    </div>

    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnNew" runat="server" CssClass="toolbar-item" OnClick="btnNew_Click">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Mortgagee</span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hf_id" runat="server" Value="0" />


    <div class="paneContentInner">
        <div class="message_area">
            <asp:Label ID="lblMessage" runat="server" />
        </div>
        <asp:Panel ID="pnlList" runat="server">

            <asp:GridView ID="gvMortgagee" CssClass="gridView" OnRowCommand="gv_RowCommand"
                Width="100%" runat="server" AutoGenerateColumns="False" CellPadding="2" AlternatingRowStyle-BackColor="#e8f2ff"
                AllowPaging="true" PageSize="100" OnPageIndexChanging="gv_PageIndexChanging"
                PagerSettings-PageButtonCount="5" PagerStyle-Font-Bold="true"
                AllowSorting="true"
                OnSorting="gv_onSorting">
                <PagerStyle CssClass="pager" />
                <RowStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    No carriers available.
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server"
                                CommandName="DoEdit"
                                CommandArgument='<%#Eval("MortgageeID") %>'
                                ToolTip="Edit"
                                ImageUrl="~/Images/edit.png"
                                Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>'></asp:ImageButton>

                            &nbsp;
				            <asp:ImageButton ID="btnDelete" runat="server"
                                CommandName="DoDelete"
                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this mortgagee?');"
                                CommandArgument='<%#Eval("MortgageeID") %>'
                                ToolTip="Delete Client"
                                ImageUrl="~/Images/delete_icon.png"
                                Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mortgagee Name" SortExpression="MortageeName">
                        <ItemTemplate>
                            <%#Eval("MortageeName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Name">
                        <ItemTemplate>
                            <%#Eval("ContactName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone" SortExpression="Phone">
                        <ItemTemplate>
                            <%#Eval("Phone")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fax" SortExpression="Fax">
                        <ItemTemplate>
                            <%#Eval("Fax")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <%#Eval("Email")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </asp:Panel>

        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="700px" TitleAlignment="Left" Text="Mortgagee Details" CssClass="canvas">
                <Template>
                    <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                        <tr>
                            <td class="right">Mortgagee Name</td>
                            <td class="redstar" style="width: 5px;">*</td>
                            <td>

                                <ig:WebTextEditor ID="txtName" MaxLength="100" runat="server" Width="250px" />
                                <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtName"
                                    ErrorMessage="Name is required!" ValidationGroup="lienholder" Display="Dynamic"
                                    CssClass="validation1" />
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
                                        ValidationGroup="lienholder" SetFocusOnError="True" />
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
                            <td class="right">State</td>
                            <td class="redstar">*</td>
                            <td>
                                <asp:DropDownList ID="ddlState" CssClass="DDLStyles" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                </asp:DropDownList>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvstate" ControlToValidate="ddlState"
                                        ErrorMessage="Please select state." ValidationGroup="lienholder" Display="Dynamic"
                                        CssClass="validation1" InitialValue="0" SetFocusOnError="True" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">City</td>
                            <td class="redstar">*</td>
                            <td>
                                <asp:DropDownList ID="ddlCity" CssClass="DDLStyles" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="dllCity_SelectedIndexChanged">
                                    <asp:ListItem Text="--- Select ---" Value="0" />
                                </asp:DropDownList>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvcity" ControlToValidate="ddlCity"
                                        ErrorMessage="Please select city" ValidationGroup="lienholder" Display="Dynamic"
                                        CssClass="validation1" InitialValue="0" SetFocusOnError="True" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Zip Code
                            </td>
                            <td class="redstar">*</td>
                            <td>
                                <asp:DropDownList ID="ddlLossZip" CssClass="DDLStyles" runat="server">
                                    <asp:ListItem Text="--- Select ---" Value="0" />
                                </asp:DropDownList>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvzipcode" ControlToValidate="ddlLossZip"
                                        ErrorMessage="Please enter zip code." ValidationGroup="lienholder" Display="Dynamic"
                                        InitialValue="0" CssClass="validation1" SetFocusOnError="True" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Country
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Primary Contact Name
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtContactName" runat="server" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Phone
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtPhone" runat="server" MaxLength="20" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Fax
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtFax" runat="server" MaxLength="20" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">E-mail
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtEmail" runat="server" MaxLength="100" />
                            </td>
                        </tr>
                       <%--  <tr>
                            <td class="right">Loan Number
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtLoanNumber" runat="server" MaxLength="50" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center;">
                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click" CausesValidation="true"
                                    ValidationGroup="lienholder" />
                                &nbsp;
								<asp:Button ID="btnClose" Text="Close" CausesValidation="false" runat="server"
                                    CssClass="mysubmit" OnClick="btnClose_Click" />

                            </td>
                        </tr>
                    </table>

                </Template>
            </igmisc:WebGroupBox>

        </asp:Panel>
    </div>
</div>
