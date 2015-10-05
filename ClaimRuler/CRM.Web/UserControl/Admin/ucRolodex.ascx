<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRolodex.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucRolodex" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<div class="paneContent">
    <div class="page-title">
        Address Book 
    </div>

    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnNewContact" runat="server" CssClass="toolbar-item" OnClick="btnNewContact_Click">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Contact</span>
                    </asp:LinkButton>
                </td>
                <td></td>
            </tr>
        </table>
    </div>
    <div class="paneContentInner">
        <asp:Panel ID="pnlContact" runat="server" Style="margin-top: 10px;">


            <asp:GridView ID="gvContact" CssClass="gridView" Width="100%" runat="server"
                AllowPaging="true"
                AllowSorting="true"
                OnRowCommand="gvContact_RowCommand"
                OnSorting="gvContact_Sorting"
                OnPageIndexChanging="gvContact_PageIndexChanging"
                AutoGenerateColumns="False"
                CellPadding="4"
                AlternatingRowStyle-BackColor="#e8f2ff"
                PageSize="20"
                HorizontalAlign="Center"
                PagerSettings-PageButtonCount="10"
                PagerStyle-Font-Bold="true"
                PagerStyle-Font-Size="9pt">
                <PagerStyle CssClass="pager" />
                <RowStyle HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    No contacts available.
                </EmptyDataTemplate>
                <Columns>
                      <asp:TemplateField HeaderText="Contact Name" SortExpression="ContactName">
                        <ItemTemplate>
                            <%# Eval("ContactName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Last Name" SortExpression="LastName">
                        <ItemTemplate>
                            <%# Eval("LastName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="First Name" SortExpression="FirstName">
                        <ItemTemplate>
                            <%# Eval("FirstName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company Name" SortExpression="CompanyName">
                        <ItemTemplate>
                            <%# Eval("CompanyName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Type" SortExpression="LeadContactType.Description">
                        <ItemTemplate>
                            <%# Eval("LeadContactType.Description") %>
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
                    <asp:TemplateField HeaderText="City" SortExpression="CityMaster.CityName">
                        <ItemTemplate>
                            <%# Eval("CityMaster.CityName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("ContactID") %>'
                                ToolTip="Edit" 
                                Visible='<%# masterPage.hasEditPermission %>'
                                ImageUrl="~/Images/edit.png"></asp:ImageButton>
                            &nbsp;
					<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete" 
                            OnClientClick="javascript:return ConfirmDialog(this,'Are you sure you want to delete this contact?');"
                            CommandArgument='<%#Eval("ContactID") %>' 
                            ToolTip="Delete" 
                            Visible='<%# masterPage.hasDeletePermission %>'
                            ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div>
                <ul class="tablist">
                    <li><a href="?">All</a></li>
                    <li><a href="?q=a">A</a></li>
                    <li><a href="?q=b">B</a></li>
                    <li><a href="?q=c">C</a></li>
                    <li><a href="?q=d">D</a></li>
                    <li><a href="?q=e">E</a></li>
                    <li><a href="?q=f">F</a></li>
                    <li><a href="?q=g">G</a></li>
                    <li><a href="?q=h">H</a></li>
                    <li><a href="?q=i">I</a></li>
                    <li><a href="?q=j">J</a></li>
                    <li><a href="?q=k">K</a></li>
                    <li><a href="?q=l">L</a></li>
                    <li><a href="?q=m">M</a></li>
                    <li><a href="?q=n">N</a></li>
                    <li><a href="?q=o">O</a></li>
                    <li><a href="?q=p">P</a></li>
                    <li><a href="?q=q">Q</a></li>
                    <li><a href="?q=r">R</a></li>
                    <li><a href="?q=s">S</a></li>
                    <li><a href="?q=t">T</a></li>
                    <li><a href="?q=u">U</a></li>
                    <li><a href="?q=v">V</a></li>
                    <li><a href="?q=w">W</a></li>
                    <li><a href="?q=x">X</a></li>
                    <li><a href="?q=y">Y</a></li>
                    <li><a href="?q=z">Z</a></li>
                </ul>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlContactDetail" runat="server" Visible="false">
            <igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="700px" TitleAlignment="Left" Text="Contact Details" CssClass="canvas">
                <Template>
                    <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                        <tr>
                            <td class="right">Type
                            </td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddlContactType" runat="server" />                               
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Last Name</td>
                            <td style="width: 5px;" class="redstar">*</td>
                            <td>

                                <ig:WebTextEditor ID="txtLastName" runat="server" MaxLength="50" Width="200px" />
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtLastName"
                                        ErrorMessage="Please enter last name." Display="Dynamic" CssClass="validation1" ValidationGroup="contact" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">First Name
                            </td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtFirstName" runat="server" MaxLength="50" Width="200px" />
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtFirstName"
                                        ErrorMessage="Please enter first name." Display="Dynamic" CssClass="validation1" ValidationGroup="contact" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Company Name
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtCompanyName" runat="server" MaxLength="100" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Title</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtContactTile" MaxLength="50" runat="server"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Department Name</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtDepartmentName" MaxLength="50" runat="server"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Phone Number
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtPhone" runat="server" MaxLength="20" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Mobile Phone
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtMobile" runat="server" MaxLength="20" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Fax</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtContactFax" MaxLength="20" runat="server"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Email
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtEmail" runat="server" MaxLength="100" Width="200px" TextMode="Email" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Address
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtAddress1" runat="server" MaxLength="100" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right"></td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtAddress2" runat="server" MaxLength="100" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">State
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">City
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="dllCity_SelectedIndexChanged">
                                    <asp:ListItem Text="--- Select ---" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Zip Code
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlLossZip" runat="server">
                                    <asp:ListItem Text="--- Select ---" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">County
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtCounty" runat="server" Width="200px" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Balance
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebNumericEditor ID="txtBalance" runat="server" MinDecimalPlaces="2" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center;">
                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click" CausesValidation="true"
                                    ValidationGroup="contact" />
                                &nbsp;
								<asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
                                    CssClass="mysubmit" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </Template>
            </igmisc:WebGroupBox>
        </asp:Panel>
    </div>
</div>
<asp:HiddenField ID="hfNewPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
<asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
<asp:HiddenField ID="hfStatus" runat="server" Value="0" />
<asp:HiddenField ID="hdId" runat="server" Value="0" />
