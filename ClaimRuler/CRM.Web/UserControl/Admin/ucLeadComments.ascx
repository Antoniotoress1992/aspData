<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLeadComments.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucLeadComments" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<style>
    .ajax__html_editor_extender_texteditor
    {
        background-color: White;
        font-weight: normal;
        color: black;
    }
</style>
<script type="text/javascript" src="../../js/jquery-1.7.2.js"></script>

<link rel="stylesheet" href="../../css/default.css">
<div id="mainbox" style="font-size: small;">
    <%--<h2>
        <asp:Label ID="lblheading" runat="server" Text="Upload Document" /></h2>--%>
    <h2>
        <asp:Label ID="lblHead" runat="server" Text="Claim Log" />
        <asp:HiddenField ID="hfLeadsId" runat="server" Value="0" />
        <asp:HiddenField ID="hfView" runat="server" Value="0" />
	    <asp:HiddenField ID="hfCommentId" runat="server"  />
    </h2>
    <div style="height: 2px;">
    </div>
    <div align="center" style="border: 1px solid #e8f2ff">
        <div align="center" style="display: inline-block; width: 100%;" id="divActionButtons" runat="server" >
            <%--<asp:Button ID="btnDocument" runat="server" Text="Document" class="mysubmit" Visible="True"
                OnClick="btnDocument_Click" />
            &nbsp;&nbsp; &nbsp;&nbsp;--%>
            <%--<asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" class="mysubmit"
                OnClick="btnGenerateReport_Click" Visible="false" />--%>
            <%--&nbsp;&nbsp; &nbsp;&nbsp;--%>
            <%--<asp:Button ID="btnGenerateRepLetter" runat="server" Text="Generate Rep Letter" class="mysubmit"
                Visible="True" OnClick="btnGenerateRepLetter_Click" />--%>
            <asp:Button ID="btnDocument" runat="server" Text="Documents" class="mysubmit"
                Visible="True" OnClick="btnDocument_Click" />&nbsp;&nbsp; &nbsp;&nbsp;
            <asp:Button ID="btn" runat="server" Text="Photos" class="mysubmit" Visible="True"
                OnClick="btnUploadPhoto_Click" />&nbsp;&nbsp; &nbsp;&nbsp;
            <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" class="mysubmit"
                OnClick="btnGenerateReport_Click" Visible="false" />
        </div>
        <div style="padding-top: 10px; padding-bottom: 10px;">
            <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
            <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
            <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
        </div>
        <div align="left" style="padding-bottom: 10px;" id="divHeader" runat="server">
            <table>
                <tr>
                    <td style="font-weight: bold; font-size: 15px;">
                        Policyholder Name:
                    </td>
                    <td>
                        <asp:Label ID="lblClaimantName" runat="server" Text="SDFSDFSDFSDFSDFSFSFSF"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold; font-size: 15px;">
                        Business Name:
                    </td>
                    <td>
                        <asp:Label ID="lblBusinessName" runat="server" Text="SDFSDFSDFSDFSDFSFSFSF"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold; font-size: 15px;">
                        Policyholder Address:
                    </td>
                    <td>
                        <asp:Label ID="lblClaimantAdd" runat="server" Text="SDFSDFSDFSDFSDFSFSFSF"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div id="dvEdit" runat="server" visible="false">
            <%--<br />--%>
            <div id="dvDocument" style="display: block;">
                <table border="0" cellspacing="0" cellpadding="0" width="800px" class="new_user"
                    align="center">
                    <%--<tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="right" style="vertical-align: top; padding-top: 15px;">
                            <%-- style="width: 30%;" vertical-align:top;--%>
                            Comment&nbsp;&nbsp;
                        </td>
                        <td align="left" valign="top" id="tr_comment">
                            &nbsp;&nbsp;<asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Columns="85"
                                Rows="10"></asp:TextBox><%-- Columns="46" Rows="10"Height="65px" Width="350px"--%>
                            <ajaxToolkit:HtmlEditorExtender ID="txtTemContent_HtmlEditorExtender" runat="server"
                                Enabled="True" TargetControlID="txtComment">
                            </ajaxToolkit:HtmlEditorExtender>
                            <%--<br />
                            &nbsp;&nbsp; <span style="color: Red; font-size: 10.5px;">(Please be as descriptive
                                as possible.)</span>--%>
                        </td>
                    </tr>
                    <%--<tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>--%>
                    <tr>
                        <td valign="top" colspan="2" align="center" style="padding-top: 25px;">
                            <asp:Button ID="btnSaveContinue" runat="server" ValidationGroup="DocUpload" class="mysubmit"
                                Text="Save and Continue" OnClick="btnSaveContinue_Click" />
                            &nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="false" class="mysubmit"
                                Text="Cancel" OnClientClick="javascript:return canceldoc();" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                        </td>
                    </tr>
                </table>
            </div>
            <%--<br />--%>
        </div>
        <br />
    </div>
    <div align="left" style="border: 1px solid #e8f2ff;">
        <table border="0" cellspacing="0" cellpadding="0" width="100%%" class="new_user">
            <tr>
                <td colspan="2" align="center">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="padding-bottom: 10px;">
                    <asp:Panel ID="Panel1" runat="server" Style="">
                        <%--<div style="height: 25px; font-weight: bold; text-align: left; font-size: 20px;">
                            <h2>
                                Comments</h2>
                        </div>--%>
                        <div style="height: 15px; padding-top: 10px;">
                        </div>
                        <asp:GridView Width="99%" ID="gvComments" BorderColor="silver" BorderStyle="Solid" runat="server" AutoGenerateColumns="False"
                            ShowFooter="false" EmptyDataText="No Record Found !!!" CellPadding="4" AllowPaging="true"
                            HeaderStyle-Font-Size="11px" PageSize="20" PagerSettings-PageButtonCount="5" PagerStyle-Font-Bold="true"
                            PagerStyle-Font-Size="9pt" OnPageIndexChanging="gvComments_PageIndexChanging"
					   OnRowCommand="gvComments_RowCommand" DataKeyNames="CommentId"
					   OnRowDataBound="gvComments_RowBound"
                            HeaderStyle-BackColor="#e8f2ff"><%-- BorderColor="#e8f2ff"--%>
					   
                            <PagerStyle CssClass="pager" />
                           
                            <Columns>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="120px">
                                    <ItemTemplate>                                       
                                        <%# Eval("InsertDate") == null ? "" : Convert.ToDateTime(Eval("InsertDate")).ToString("g")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Name" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfLeadId" runat="server" Value='<%#Eval("LeadId") %>' />
                                        <asp:HiddenField ID="hfUserId" runat="server" Value='<%#Eval("UserId") %>' />
                                        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("SecUser.UserName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comments">
                                    <ItemTemplate>
                                       <asp:Label ID="lblComments" runat="server" 
										 Text='<%#Eval("CommentText") %>' />

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="btnDelete" Text="Delete" CommandName="DoDelete"
                                            ImageUrl="../../Images/DeleteRed.png" CommandArgument='<%# Eval("CommentId") %>'
                                            OnClientClick="javascript:return confirm('Do you want to delete this record ?')"
                                            ToolTip="Delete" />								
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnNewComment" runat="server" Text="New Comment" class="mysubmit"
                        Visible="True" OnClick="btnNewComment_Click" />
                    &nbsp;&nbsp; &nbsp;&nbsp;
                    <asp:Button ID="btnCancelNew" runat="server" Text="Cancel" class="mysubmit" Visible="True"
                        OnClick="btnCancelNew_Click" />
                </td>
            </tr>
        </table>
    </div>
</div>

