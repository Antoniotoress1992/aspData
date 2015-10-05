<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="LeadPolicyEdit.aspx.cs" Inherits="CRM.Web.Protected.LeadPolicyEdit" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<%@ Register Src="~/UserControl/Admin/ucLeadPolicy.ascx" TagName="ucLeadPolicy" TagPrefix="uc2" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">


    <div class="page-title">
        Policy Detail 
    </div>
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div class="toolbar toolbar-body">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnReturnToClaim" runat="server" CssClass="toolbar-item" OnClick="Master.btnReturnToClient_Click">
							<span class="toolbar-img-and-text" style="background-image: url(../images/back.png)">Insured</span>
                            </asp:LinkButton>

                        </td>
                        <td>
                            <asp:LinkButton ID="lbtnSave" runat="server" CssClass="toolbar-item" OnClick="btnSave_Click">
								<span class="toolbar-img-and-text" style="background-image: url(../images/toolbar_save.png)">Save</span>
                            </asp:LinkButton>

                        </td>

                    </tr>
                </table>
            </div>

        

            <uc2:ucLeadPolicy ID="policyEditForm" runat="server" />
        </ContentTemplate>

    </asp:UpdatePanel>


    <script type="text/javascript">

    
        function addNewComment(policyType) {
            PopupCenter("../Content/Comment.aspx?t=" + policyType, "New Comment", 700, 600);
        }
        function uploadDocument(policyType) {
            PopupCenter("../Content/DocumentUpload.aspx?t=" + policyType, "Upload Document", 700, 400);
        }
    </script>
</asp:Content>
