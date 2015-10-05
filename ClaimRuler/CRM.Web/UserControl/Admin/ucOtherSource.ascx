<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOtherSource.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucOtherSource" %>

<div id="mainboxs">
    <div class="all_box">
        <div class="box1">
            <div class="mainbox">
                <h2>
                    Other Source </h2>
                <div class="warrape">
                    <div align="center" style="padding-top:5px;">
                        <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                        <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                        <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
                    </div>
                    <div align="center" class="search_part" style="margin-bottom: 15px;margin-top:15px;">
                        <table width="80%" border="0" cellspacing="0" cellpadding="00" class="new_user">
                            <tr>
                                <td style="width:150px;">
                                   Other Source
                                </td>
                                <td style="vertical-align: top;width:350px;" align="left"">
                                    &nbsp;<asp:TextBox ID="txtOtherSource" MaxLength="250" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtOtherSource"
                                        ErrorMessage="Please Enter Other Source" ValidationGroup="OtherSource" Display="Dynamic"
                                        CssClass="validation1" />
                                </td>
                                <td align="left" style="vertical-align: top;width:300px;">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click"
                                        ValidationGroup="OtherSource" />
                                    &nbsp;
                                    <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
                                        CssClass="mysubmit" OnClick="btnCancel_Click" />
							&nbsp;
								<asp:Button ID="btnReturnToClaim" Text="Rerturn to Claim" CausesValidation="false" runat="server"
										CssClass="mysubmit" OnClick="btnReturnToClaim_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="vendor_list" id="dvData" runat="server">
                        <asp:ListView runat="server" ID="lvData" ItemPlaceholderID="itemPlaceHolder1" OnItemCommand="lvData_ItemCommand"
                            DataKeyNames="OtherSourceId">
                            <LayoutTemplate>
                                <table width="100%" align='center' border="0" cellspacing="0" cellpadding="0" class="mytable hurr">
                                    <tr style="background: #c9dffb">
                                        <td width="10%" class='hl' style='text-align: center;'>
                                            <strong>S.No.</strong>
                                        </td>
                                        <td width="70%" class='hl' style='text-align: center;'>
                                            <strong>Other Source</strong>
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
                                       
                                        <asp:Label ID="lblOtherSource" runat="server" Text='<%#Eval("OtherSource")%>'></asp:Label>&nbsp;&nbsp;
                                    </td>
                                   
                                    <td style=" " align="center">
                                        <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("OtherSourceId") %>'
                                            ToolTip="Edit" ImageUrl="../../Images/edit_icon.png" Enabled='<%# Convert.ToBoolean(Convert.ToInt32(this.hfEditPermission.Value) == 0) ? false : true %>' />&nbsp;&nbsp;
                                    </td>
                                    <td style=" " align="center">
                                        <asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
                                            CommandName="DoDelete" CommandArgument='<%#Eval("OtherSourceId") %>' ToolTip="Delete"
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
                                       
                                        <asp:Label ID="lblOtherSource" runat="server" Text='<%#Eval("OtherSource")%>'></asp:Label>&nbsp;&nbsp;
                                    </td>
                                    
                                    <td style=" " align="center">
                                        <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("OtherSourceId") %>'
                                            ToolTip="Edit" ImageUrl="../../Images/edit_icon.png" Enabled='<%# Convert.ToBoolean(Convert.ToInt32(this.hfEditPermission.Value) == 0) ? false : true %>' />&nbsp;&nbsp;
                                    </td>
                                    <td style=" " align="center">
                                        <asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
                                            CommandName="DoDelete" CommandArgument='<%#Eval("OtherSourceId") %>' ToolTip="Delete"
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