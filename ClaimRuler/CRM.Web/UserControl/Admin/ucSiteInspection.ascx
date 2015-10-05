<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSiteInspection.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucSiteInspection" %>

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
<div id="mainboxs">
    <div class="all_box">
        <div class="box1">
            <div class="mainbox">
                <h2>
                    Site Inspection Complete</h2>
                <div class="warrape">
                    <div align="center" style="padding-top:5px;">
                        <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                        <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                        <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
                    </div>
                    <div align="center" class="search_part" style="margin-bottom: 15px;margin-top:15px;">
                        <table width="70%" border="0" cellspacing="0" cellpadding="00" class="new_user">
                            <tr>
                                <td style="width:190px;">
                                    Site Inspection Complete
                                </td>
                                <td style="vertical-align: top;width:300px;">
                                    <asp:TextBox ID="txtName" MaxLength="100" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="req1" ControlToValidate="txtName"
                                        ErrorMessage="Please Enter Site Inspection Complete" ValidationGroup="siteInspection" Display="Dynamic"
                                        CssClass="validation1" />
                                </td>
                                <td align="left" style="vertical-align: top;width:300px;">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click"
                                        ValidationGroup="siteInspection" />
                                    &nbsp;
                                    <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
                                        CssClass="mysubmit" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="vendor_list" id="dvData" runat="server">
                        <asp:ListView runat="server" ID="lvData" ItemPlaceholderID="itemPlaceHolder1" OnItemCommand="lvData_ItemCommand"
                            DataKeyNames="SiteInspectionCompleteId">
                            <LayoutTemplate>
                                <table width="100%" align='center' border="0" cellspacing="0" cellpadding="0" class="mytable hurr">
                                    <tr style="background: #c9dffb">
                                        <td width="10%" class='hl' style='text-align: center;'>
                                            <strong>S.No.</strong>
                                        </td>
                                        <td width="70%" class='hl' style='text-align: center;'>
                                            <strong>Site Inspection Complete</strong>
                                        </td>
                                       
                                        <td width="10%" class='hl' style='text-align: center;'>
                                            &nbsp;
                                        </td>
                                        <td width="10%" class='hl' style='text-align: center;'>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr style='color: black; background: #FFFFFF'>
                                    <td style=" " align="center">
                                        <%# Container.DataItemIndex+1%>
                                    </td>
                                    <td style=" ">
                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("SiteInspectionCompleteName")%>'></asp:Label>&nbsp;&nbsp;
                                    </td>
                                  
                                    <td style=" " align="center">
                                        <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("SiteInspectionCompleteId") %>'
                                            ToolTip="Edit" ImageUrl="../../Images/edit_icon.png" Enabled='<%# Convert.ToBoolean(Convert.ToInt32(this.hfEditPermission.Value) == 0) ? false : true %>' />&nbsp;&nbsp;
                                    </td>
                                    <td style=" " align="center">
                                        <asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
                                            CommandName="DoDelete" CommandArgument='<%#Eval("SiteInspectionCompleteId") %>' ToolTip="Delete"
                                            ImageUrl="../../Images/delete_icon.png"
                                            Enabled='<%# (this.hfDeletePermission.Value == "0" || Convert.ToString(Eval("Status"))=="False") ? false : true %>'
                                            OnClientClick="javascript:return ConfirmDialog(this, 'Confirmation', 'Do you want to delete this record ?');" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr style='color: black; background: #e8f2ff'>
                                    <td style=" " align="center">
                                        <%# Container.DataItemIndex+1%>
                                    </td>
                                    <td style=" ">
                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("SiteInspectionCompleteName")%>'></asp:Label>&nbsp;&nbsp;
                                    </td>
                                   
                                    <td style=" " align="center">
                                        <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("SiteInspectionCompleteId") %>'
                                            ToolTip="Edit" ImageUrl="../../Images/edit_icon.png" Enabled='<%# Convert.ToBoolean(Convert.ToInt32(this.hfEditPermission.Value) == 0) ? false : true %>' />&nbsp;&nbsp;
                                    </td>
                                    <td style=" " align="center">
                                        <asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
                                            CommandName="DoDelete" CommandArgument='<%#Eval("SiteInspectionCompleteId") %>' ToolTip="Delete"
                                            ImageUrl="../../Images/delete_icon.png"
                                            Enabled='<%# (this.hfDeletePermission.Value == "0" || Convert.ToString(Eval("Status"))=="False") ? false : true %>'
                                            OnClientClick="javascript:return ConfirmDialog(this, 'Confirmation', 'Do you want to delete this record ?');" />
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
                    </div>
                    <div class="pagination">
                        <asp:DataPager ID="PagerRow" Visible="true" PageSize="15" runat="server" PagedControlID="lvData"
                            OnPreRender="lvData_PreRender">
                            <Fields>
                                <asp:NumericPagerField ButtonCount="5" NextPageText=">>" PreviousPageText="<<" CurrentPageLabelCssClass="PagerCurrent"
                                    NextPreviousButtonCssClass="PagerNormal" NumericButtonCssClass="PagerNormal" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<asp:HiddenField ID="hfNewPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
<asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
<asp:HiddenField ID="hfStatus" runat="server" Value="0" />
<asp:HiddenField ID="hdId" runat="server" Value="0" />