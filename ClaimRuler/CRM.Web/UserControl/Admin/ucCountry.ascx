<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCountry.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucCountry" %>
<script type="text/javascript" src="../../js/JQueryConfirm/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="../../js/JQueryConfirm/jquery-ui-1.8.2.custom.min.js"></script>
<link type="text/css" rel="Stylesheet" href="../../js/JQueryConfirm/overcast/jquery-ui-1.8.5.custom.css" />
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
                    draggable: false,
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
<div id="right_contant_area">
    <div class="all_box">
        <div class="box1">
            <div class="innerbox1">
                <h1>
                    Country</h1>
                <div class="warrape">
                    <div class="search_part" runat="server" id="divEntry" visible="false" style="margin-bottom: 15px;">
                        <asp:Panel ID="pnlEdit" runat="server">
                            <table width="100%" border="0">
                                <tr>
                                    <tr>
                                        <td>
                                            Status
                                        </td>
                                        <td align="left" style="width: 200px">
                                            <asp:DropDownList runat="server" ID="ddlStatus" class="login_st_search">
                                                <asp:ListItem Text="Active" Value="1" />
                                                <asp:ListItem Text="In-Active" Value="0" />
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right">
                                            &nbsp;&nbsp;&nbsp;&nbsp; Country Name
                                        </td>
                                        <td align="left" style="width: 200px">
                                            <asp:TextBox ID="txtCountryName" runat="server" class="login_st_search" MaxLength="20" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*Please Enter Country"
                                                Display="Dynamic" EnableClientScript="true" ValidationGroup="country" ControlToValidate="txtCountryName"
                                                CssClass="validation1" SetFocusOnError="true" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCountryName"
                                                ErrorMessage="*Please Enter Only Alphabets" ValidationExpression="^[A-Za-z ]{0,20}$"
                                                SetFocusOnError="true" CssClass="validation1" EnableClientScript="true" Display="Dynamic"
                                                ValidationGroup="country"></asp:RegularExpressionValidator>
                                            <br />
                                        </td>
                                    </tr>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/Images/save.jpg" OnClick="btnSave_Click"
                                            ValidationGroup="country" />
                                        <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/Images/Cancel.jpg" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div class="search_part" style="margin-bottom: 15px;">
                        <table width="100%" border="0">
                            <tr>
                                <td>
                                    Keywords
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtKeywords" class="login_st1" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnSearch" ImageUrl="~/Images/search_btn.jpg" runat="server"
                                        OnClick="btnSearch_Click" align="center" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnReset" ImageUrl="~/Images/reset_btn.jpg" runat="server" OnClick="btnReset_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div align="center" style="padding-top: 20px;">
                        <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                        <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                        <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false" />
                    </div>
                    <asp:Panel ID="pnlList" runat="server">
                        <div align="right" id="divNew" runat="server">
                            <asp:LinkButton runat="server" ID="lbnNew" Text="New" OnClick="lbnNew_Click" Font-Bold="true" />
                        </div>
                        <div class="vendor_list" style="margin-bottom: 15px;">
                            <asp:ListView runat="server" ID="lvCountry" ItemPlaceholderID="itemPlaceHolder1"
                                OnItemCommand="lvCountry_ItemCommand" DataKeyNames="CountryID" OnPagePropertiesChanging="lvCountry_PagePropertiesChanging">
                                <LayoutTemplate>
                                    <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td>
                                                <strong>ID</strong>
                                            </td>
                                            <td>
                                                <strong>Country Name</strong>
                                            </td>
                                            <td>
                                                <strong>Status</strong>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td bgcolor="#f4f4f4">
                                            <%#Eval("CountryID")%>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td bgcolor="#f4f4f4">
                                            <%#Eval("CountryName")%>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td bgcolor="#f4f4f4">
                                            <%# Convert.ToBoolean(Eval("Status")) == true ? "Active" : "In-Active"%>
                                        </td>
                                        <td bgcolor="#f4f4f4">
                                            <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("CountryID") %>'
                                                ToolTip="Edit" ImageUrl="~/Images/edit_icon.png" Enabled='<%# Convert.ToBoolean(Convert.ToInt32(this.hfEditPermission.Value) == 0) ? false : true %>' />&nbsp;&nbsp;
                                        </td>
                                        <td bgcolor="#f4f4f4">
                                            <asp:ImageButton runat="server" ID="imgDelete" CommandName="DoDelete" CommandArgument='<%#Eval("CountryID") %>'
                                                ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" Enabled='<%# (this.hfDeletePermission.Value == "0" || Convert.ToString(Eval("Status")) == "False" ? false : true) %>'
                                                OnClientClick="return ConfirmDialog(this, 'Confirmation', 'Do you want to delete this record ?');" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr>
                                        <td>
                                            <%#Eval("CountryID")%>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <%#Eval("CountryName")%>
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <%# Convert.ToBoolean(Eval("Status")) == true ? "Active" : "In-Active"%>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("CountryID") %>'
                                                ToolTip="Edit" ImageUrl="~/Images/edit_icon.png" Enabled='<%# Convert.ToBoolean(Convert.ToInt32(this.hfEditPermission.Value) == 0) ? false : true %>' />&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imgDelete" CommandName="DoDelete" CommandArgument='<%#Eval("CountryID") %>'
                                                ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" Enabled='<%# (this.hfDeletePermission.Value == "0" || Convert.ToString(Eval("Status")) == "True" ? true : false) %>'
                                                OnClientClick="return ConfirmDialog(this, 'Confirmation', 'Do you want to delete this record ?');" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td colspan="5">
                                            <div style="padding-top: 10px; padding-bottom: 10px;">
                                                <asp:Label ID="lblRecordNotFound" runat="server" CssClass="info" Text="Records Not Found !!!" /></div>
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                    </asp:Panel>
                    <div class="pagination">
                        <asp:DataPager ID="PagerRow" PageSize="15" Visible="false" runat="server" PagedControlID="lvCountry">
                            <Fields>
                                <asp:NumericPagerField ButtonCount="4" NextPageText=">>" PreviousPageText="<<" CurrentPageLabelCssClass="PagerCurrent"
                                    NextPreviousButtonCssClass="PagerNormal" NumericButtonCssClass="PagerNormal" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<asp:HiddenField runat="server" ID="hdId" />
<asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
<asp:HiddenField ID="hfNewPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
<asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
