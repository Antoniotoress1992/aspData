<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
    CodeBehind="LeadSchedule.aspx.cs" Inherits="CRM.Web.Protected.LeadSchedule" %>

<%@ Register Src="~/UserControl/Admin/ucSchedule.ascx" TagName="ucSchedule" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Client/Claim Tasks
        </div>
        <h2>
            <asp:Label ID="lblClaimant" runat="server" />
        </h2>

        <div class="toolbar toolbar-body">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="btnReturnToClaim" runat="server" Text="" CssClass="toolbar-item" OnClick="Master.btnReturnToClaim_Click" CausesValidation="false">
					        <span class="toolbar-img-and-text" style="background-image: url(../images/back.png)">Return to Claim</span>
                        </asp:LinkButton>
                    </td>
                     <td>
                            <asp:LinkButton ID="btnBackToPolicy" runat="server" CssClass="toolbar-item" OnClick="Master.btnReturnToPolicy_Click">
								<span class="toolbar-img-and-text" style="background-image: url(../images/back.png)">Policy</span>
                            </asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="btnReturnToClient" runat="server" CssClass="toolbar-item" OnClick="Master.btnReturnToClient_Click">
								<span class="toolbar-img-and-text" style="background-image: url(../images/back.png)">Insured</span>
                            </asp:LinkButton>
                        </td>
                </tr>

            </table>
        </div>

        <uc1:ucSchedule ID="scheduler" runat="server" showResizeFullButton="false" dataMode="Lead" />

    </div>

    <script type="text/javascript">
        function openAppointmentDialog(scheduleInfo, evnt, dialog, activity) {
            evnt.cancel = true;

            // pass to appointment form
            $("#startDateTime").val(activity.getStartDateTime());


            var id = activity.getDataKey();
            var strID = (id == null ? "0" : id.toString());

            PopupCenter("../Content/AppointmentForm.aspx?o=lead&id=" + strID, "Appointment", 700, 650);
        }
        function openReminderDialog(scheduleInfo, evnt) {
            evnt.cancel = true;

            // pass to appointment form
            $("#startDateTime").val(activity.getStartDateTime());


            var id = activity.getDataKey();
            var strID = (id == null ? "0" : id.toString());

            PopupCenter("../Content/Reminder.aspx?o=lead&id=" + strID, "Reminder", 700, 650);
        }
        //	$(document).ready(function () {
        //		scheduler.attachEvent("onEventAdded", function (id, data, flag) {
        //			top.opener.location.reload(); 
        //		})
        //	});
    </script>
</asp:Content>

