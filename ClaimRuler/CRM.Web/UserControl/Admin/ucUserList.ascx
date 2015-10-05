<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUserList.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucEditUser" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="ucUploadCSV.ascx" TagName="ucUploadCSV" TagPrefix="uc1" %>




<div class="page-title">
    Users Administration
</div>

<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>

        <div class="toolbar toolbar-body">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="btnSearch" runat="server" CssClass="toolbar-item" OnClick="btnSearch_Click">
								<span class="toolbar-img-and-text" style="background-image: url(../../images/toolbar_search.png)">Search</span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnNew" runat="server" CssClass="toolbar-item" 
                             Visible='<%# masterPage.hasAddPermission %>'
                            PostBackUrl="~/Protected/Admin/UserEdit.aspx">
								<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New</span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnReset" runat="server" CssClass="toolbar-item" OnClick="btnReset_Click">
								<span class="toolbar-img-and-text" style="background-image: url(../../images/refresh.png)">Clear</span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>

        <div class="paneContentInner">
            <div style="text-align: center;">
                <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false" />
            </div>
            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
                <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px;" border="0" class="editForm">
                    <tr>
                        <td class="top left" style="width: 20%;">
                            <div class="boxContainer">
                                <div class="section-title">Search Filters</div>
                                <table style="border-collapse: separate; border-spacing: 5px; padding: 2px;">
                                    <tr>
                                        <td class="right" style="width: 10%;">Keywords</td>
                                        <td>
                                            <ig:WebTextEditor ID="txtSearch" runat="server" Width="300px"></ig:WebTextEditor>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="right">Role</td>
                                        <td>
                                            <asp:DropDownList ID="ddlRole" runat="server" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Status</td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatus" Width="300px" runat="server">
                                                <asp:ListItem Text="--- Select ---" Value="0" />
                                                <asp:ListItem Text="Deleted" Value="2" />
                                                <asp:ListItem Text="Not-Deleted" Value="1" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>

                         <%--  --%>
                        <td class="top left">
                            <asp:GridView runat="server" ID="gvUsers"
                                AllowPaging="true"
                                AllowSorting="true"
                                AutoGenerateColumns="false"
                                CssClass="gridView"
                                DataKeyNames="UserId"
                                OnRowDataBound="gvUsers_RowDataBound"
                                OnRowCommand="gvUsers_RowCommand"
                                OnPageIndexChanging="gvUsers_PageIndexChanging"
                                OnSorting="gv_onSorting"
                                PageSize="20"
                                PagerSettings-PageButtonCount="15"
                                Width="100%">
                                <PagerStyle CssClass="pager" Font-Bold="true" />
                                <RowStyle HorizontalAlign="Center" />
                                <Columns>

                                    <asp:TemplateField>
                                        <ItemStyle Width="32px" />
                                        <ItemTemplate>
                                            <asp:Image runat="server" ID="userPhoto" Width="32px" Height="32px" ImageAlign="Middle" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name" SortExpression="UserName">
                                        <ItemTemplate>
                                            <%# Eval("UserName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Name" SortExpression="LastName">
                                        <ItemTemplate>
                                            <%# Eval("LastName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="First Name" SortExpression="FirstName">
                                        <ItemTemplate>
                                            <%# Eval("FirstName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="E-mail">
                                        <ItemTemplate>
                                            <%# Eval("Email")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role Name" >  <%--SortExpression="SecRole.RoleName"--%>
                                        <ItemTemplate>
                                            <%# Eval("SecRole.RoleName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <%# Convert.ToString(Eval("Status")) == "False" ? "In-Active" : "Active"%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Width="100px" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("UserId") %>'
                                                ToolTip="Edit" ImageUrl="~/Images/edit_icon.png"
                                                Visible='<%# masterPage.hasEditPermission %>' />
                                            &nbsp;
							                <asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
                                                CommandName="DoDelete" CommandArgument='<%#Eval("UserId") %>' ToolTip="Delete"
                                                ImageUrl="~/Images/delete_icon.png"
                                                Visible='<%# masterPage.hasDeletePermission %>'
                                                OnClientClick="javascript:return ConfirmDialog(this, 'Confirm', 'Do you want to delete this user?');" />
                                                            &nbsp;
							                <asp:ImageButton runat="server" ID="btnImpersonate" CommandName="DoImpersonate" 
                                                CommandArgument='<%#Eval("UserId") %>'
                                                ToolTip="Login" 
                                                ImageUrl="~/Images/view_icon.png" 
                                                Visible='<%# CRM.Core.PermissionHelper.isAdmin() %>'/>
                                             &nbsp;
							                <asp:ImageButton ID="btnImpresonateClient" runat="server" 
                                                CommandName="DoImpersonate" ToolTip="Login as user"
                                                CommandArgument='<%#Eval("UserId") %>' 
                                                ImageUrl="~/Images/SearchPng.png" 
                                                ImageAlign="Middle"
                                                Visible='<%# CRM.Core.PermissionHelper.isAdmin() %>'>
							                </asp:ImageButton>
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                </Columns>

                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>



        </div>

    </ContentTemplate>
</asp:UpdatePanel>


<asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
<asp:HiddenField ID="hfRole" runat="server" Value="0" />
<asp:HiddenField ID="hfStatus" runat="server" Value="0" />
<script type="text/javascript">

    String.Format = function () {
        var s = arguments[0];
        for (var i = 0; i < arguments.length - 1; i++) {
            var reg = new RegExp("\\{" + i + "\\}", "gm");
            s = s.replace(reg, arguments[i + 1]);
        }
        return s;
    }

    var dialogConfirmed = false;
    function ConfirmDialog(obj, title, dialogText) {
        if (!dialogConfirmed) {
            $('body').append(String.Format("<div id='dialog' title='{0}'><p>{1}</p></div>",
				title, dialogText));

            $('#dialog').dialog
			 ({
			     height: 150,
			     modal: true,
			     resizable: false,
			     draggable: true,
			     close: function (event, ui) { $('body').find('#dialog').remove(); },
			     buttons:
                 {
                     'Yes': function () {
                         $(this).dialog('close');
                         dialogConfirmed = true;
                         if (obj) obj.click();
                     },
                     'No': function () {
                         $(this).dialog('close');
                     }
                 }
			 });
        }

        return dialogConfirmed;
    }
</script>
