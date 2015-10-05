<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="EventEdit.aspx.cs" Inherits="CRM.Web.Protected.EventEdit" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<%@ Register Src="~/UserControl/ucEvent.ascx" TagName="ucevent" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            <asp:Label ID="lblTitle" runat="server"></asp:Label>
        </div>

        <asp:UpdatePanel ID="updatepanel" runat="server">
            <ContentTemplate>
                <div class="toolbar toolbar-body">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnBackToActivities" runat="server" CssClass="toolbar-item" PostBackUrl="~/Protected/tasks.aspx">
					                <span class="toolbar-img-and-text" style="background-image: url(../images/back.png)">Activities</span>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="message_area">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
                <div class="center" style="margin: 10px">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="task" />
                    &nbsp;
                    <asp:Button ID="btnSaveNew" runat="server" Text="Save & New " CssClass="mysubmit" OnClick="btnSaveNew_Click" CausesValidation="true" ValidationGroup="task" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" CssClass="mysubmit" />
                </div>
                <uc1:ucevent ID="ucevent" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
