<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucModule.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucModule" %>
<script type="text/javascript" language="javascript">
    function validateURL(event, args) {
        var textbox = document.getElementById('<%= txtURL.ClientID %>').value;
        if (args.Value != '0')
            args.IsValid = (textbox != '');
        else
            args.IsValid = true;
    }
</script>
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
<asp:HiddenField ID="hfNewPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
<asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
<asp:UpdatePanel runat="server" ID="updatePanel2">
    <ContentTemplate>
        <div id="mainboxss">
            <div class="all_box">
                <div class="box1">
                    <div class="mainbox">
                        <h2>
                            Module</h2>
                        <div class="warrape">
                            <asp:Panel ID="pnlEdit" runat="server" Enabled="false" Visible="false">
                                <asp:HiddenField runat="server" ID="hdId" />
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="tbl" border="0" cellspacing="0" cellpadding="00" class="new_user">
                                            <tr>
                                                <td style="width: 200px; text-align: right;">
                                                    <label>
                                                        Status
                                                    </label>
                                                </td>
                                                <td class="redstar">
                                                    *
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlStatus" CssClass="DDLStyles">
                                                        <asp:ListItem Text="Active" Value="1" />
                                                        <asp:ListItem Text="In-Active" Value="0" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td style="width: 200px; text-align: right;">
                                                    <label>
                                                        Module Name
                                                    </label>
                                                </td>
                                                <td class="redstar">
                                                    *
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtModuleName" runat="server" class="login_st" MaxLength="50" />
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="r2" runat="server" ErrorMessage="*Please Enter Module Name"
                                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="Module" ControlToValidate="txtModuleName"
                                                        CssClass="validation1" SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px; text-align: right;">
                                                    <label>
                                                        Module Description
                                                    </label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtModuleDescription" runat="server" class="login_st" MaxLength="100" />
                                                    <br />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td style="width: 200px; text-align: right;">
                                                    <label>
                                                        Parent
                                                    </label>
                                                </td>
                                                <td class="redstar">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlParentID" runat="server" CssClass="DDLStyles" />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px; text-align: right;">
                                                    <label>
                                                        URL
                                                    </label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtURL" runat="server" class="login_st" MaxLength="100" />
                                                    <br />
                                                    <span class="validationSpan">
                                                        <asp:CustomValidator ID="cvParentID" runat="server" ControlToValidate="ddlParentID"
                                                            CssClass="validation" ValidateEmptyText="true" Display="Dynamic" ClientValidationFunction="validateURL"
                                                            ErrorMessage="*Please Enter URL" SetFocusOnError="true" ValidationGroup="Module"
                                                            OnServerValidate="ValidateURL" /></span>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td style="width: 200px; text-align: right;">
                                                </td>
                                                <td class="redstar">
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button1" runat="server" Text="Save" CssClass="mysubmit" ValidationGroup="Module"
                                                        OnClick="btnSave_Click" />
                                                    <asp:Button ID="Button2" CausesValidation="false" Text="Cancel" runat="server" CssClass="mysubmit"
                                                        OnClick="btnCancel_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="Button1" />
                                        <asp:AsyncPostBackTrigger ControlID="Button2" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlList" runat="server" Style="display: inline-table; table-layout: fixed;
                                width: 100%;">
                                <div align="center" style="display: table-row">
                                    <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                                    <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                                    <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false" />
                                </div>
                                <div style="display: table-row" align="right">
                                    <asp:LinkButton runat="server" ID="lbnNew" Text="+ New" Font-Bold="true" OnClick="lbnNew_Click" /></div>
                                <div class="vendor_list" style="display: table-row">
                                    <asp:GridView runat="server" ID="gvData" DataKeyNames="ModuleId" Width="99%" AutoGenerateColumns="false"
                                        AllowPaging="true" PageSize="15" OnRowCommand="gvData_RowCommand" AlternatingRowStyle-BackColor="#e8f2ff"
                                        HeaderStyle-HorizontalAlign="Left" CssClass="Tables" OnPageIndexChanging="gvData_PageIndexChanging"
                                        PagerSettings-PageButtonCount="4" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="9pt">
                                        <PagerStyle CssClass="pager" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No." HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSrNo" Text='<%# Container.DataItemIndex+1 %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Module Name" DataField="ModuleName" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Module Description" HeaderStyle-BackColor="#e8f2ff" DataField="ModuleDesc"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="Parent" DataField="ParentModuleName" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="URL" DataField="Url" HeaderStyle-BackColor="#e8f2ff"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-BackColor="#ffffff" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litStatus" Text='<%#Convert.ToString(Eval("Status"))=="True"?"Active":"In-Active" %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-BackColor="#e8f2ff">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("ModuleId") %>'
                                                        ImageUrl="../../Images/edit_icon.png" Enabled='<%# Convert.ToBoolean(Convert.ToInt32(this.hfEditPermission.Value) == 0) ? false : true %>' />&nbsp;&nbsp;
                                                    <asp:ImageButton runat="server" ID="imgDelete" CommandName="DoDelete" CommandArgument='<%#Eval("ModuleId") %>'
                                                        OnClientClick="javascript:return confirm('Do you want to delete this record ?')"
                                                        ImageUrl="../../Images/delete_icon.png" Enabled='<%# (this.hfDeletePermission.Value == "0" || Convert.ToBoolean(Eval("Status")) == false) ? false : true %>' />
                                                    <asp:HiddenField ID="hfParentId" runat="server" Value='<%#Eval("ParentId") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
