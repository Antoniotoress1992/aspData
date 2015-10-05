<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimComments.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimComments" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div style="margin: 3px;">
     <asp:LinkButton ID="lbtnNewComment" runat="server" Visible="false" Text="Add Notes" CssClass="link" OnClick="lbtnNewComment_Click" />
</div>
<asp:Panel ID="pnlEdit" runat="server" Visible="false">
    <div class="message_area">
        <asp:Label ID="lblMessage" runat="server" />
    </div>
    <div class="center">
        <table style="width:100%">
            <tr>
                <td style="width:150px">
                    <div>Activity</div>
                </td>
                <td style="text-align:left">
                    <%--<asp:TextBox ID="txtActivityTypes" runat="server" TabIndex="1"></asp:TextBox>--%>
                     <asp:DropDownList ID="ddlActivity" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width:150px">
                    <div>Start Date</div>
                </td>
                <td style="text-align:left">
                    <ig:WebDatePicker ID="txtStartDate" runat="server" TabIndex="2" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy" >
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                </td>
            </tr>
            <tr>
                <td style="width:150px">
                    <div>End Date</div>
                </td>
                <td style="text-align:left">
                    <ig:WebDatePicker ID="txtEndDate" runat="server" TabIndex="3" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align:left">
                     <ig:WebTextEditor ID="txtComment" runat="server" MultiLine-Rows="15" TextMode="MultiLine" Width="80%" TabIndex="4"></ig:WebTextEditor>
                </td>
            </tr>
        </table>
       

    </div>
    <div class="center">
        <asp:Button ID="btnCommentSave" runat="server" Text="Save" OnClick="btnCommentSave_Click" CssClass="mysubmit" ValidationGroup="comment" CausesValidation="true" />
        &nbsp;
        <asp:Button ID="btnCommentCancel" runat="server" Text="Cancel" OnClick="btnCommentCancel_Click" CssClass="mysubmit" CausesValidation="false" />
    </div>
</asp:Panel>
<asp:Panel ID="pnlGridPanel" runat="server">
    
    <div class="boxContainer">
        <div class="section-title">
            Current Notes
        </div>
        <asp:GridView Width="100%"
            ID="gvComments"
            runat="server"
            AllowPaging="true"
            AutoGenerateColumns="False"
            CellPadding="4"
            CssClass="gridView"
            HorizontalAlign="Center"
            DataKeyNames="CommentID"
            PageSize="10"
            OnRowCommand="gvComments_RowCommand"
            OnPageIndexChanging="gvComments_PageIndexChanging">
            <PagerStyle CssClass="pager" />
            <Columns>
                <asp:TemplateField>
                    <ItemStyle Width="45px" HorizontalAlign="Center" VerticalAlign="Top" />
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Images/edit.png"
                            ToolTip="Edit" 
                            CommandName="DoEdit" 
                            CommandArgument='<%#Eval("CommentID") %>' 
                            Visible='<%# CRM.Core.PermissionHelper.checkAddPermission("UsersLeads.aspx") %>' />
                        &nbsp;
                <asp:ImageButton runat="server" ID="btnDelete"
                    Text="Delete"
                    CommandName="DoDelete"
                    ImageUrl="~/Images/delete_icon.png"
                    CommandArgument='<%# Eval("CommentID") %>'
                    OnClientClick="javascript:return ConfirmDialog(this, 'Do you want to delete this comment?')"
                    ToolTip="Delete" 
                    Visible='<%# CRM.Core.PermissionHelper.checkDeletePermission("UsersLeads.aspx") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date" ItemStyle-Width="120px">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                    <ItemTemplate>
                        <%# Eval("CommentDate", "{0:g}") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="User Name" ItemStyle-Width="100px">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                    <ItemTemplate>
                        <%#Eval("UserName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Activity" ItemStyle-Width="150px">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                    <ItemTemplate>
                        <%#Eval("ActivityType") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Internal Comments">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                    <ItemTemplate>
                        <%#Eval("InternalComments") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Audit Trail">
                    <ItemTemplate>
                        <asp:Label ID="lblComments" runat="server" Text='<%#Eval("CommentText") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
             <PagerStyle CssClass="pager" />
        </asp:GridView>
    </div>
    <div class="boxContainer">
        <div class="section-title">
            Previous Notes
        </div>
        <asp:GridView Width="100%" ID="gvHistoricalComments"
            CssClass="gridView"
            runat="server"
            AutoGenerateColumns="False"
            CellPadding="2"
            AllowPaging="True"
            PageSize="10"
            OnPageIndexChanging="gvHistoricalComments_PageIndexChanging"
            DataKeyNames="CommentId">
            <PagerStyle CssClass="pager" />
            <Columns>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <%# Eval("InsertDate", "{0:g}") %>
                    </ItemTemplate>
                    <ItemStyle Width="125px" Wrap="False" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="User Name">
                    <ItemTemplate>
                        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("SecUser.UserName") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                </asp:TemplateField>
              <%--  <asp:TemplateField HeaderText="Activity">
                    <ItemTemplate>
                        <asp:Label ID="lblActivity" runat="server" Text='<%#Eval("ActivityType") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Notes">
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblComment" runat="server" Text='<%# Eval("CommentText") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Panel>


