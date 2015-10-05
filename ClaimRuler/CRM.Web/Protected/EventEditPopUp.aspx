<%@ Page Title="" Language="C#" MasterPageFile="~/SitePopUp.Master" AutoEventWireup="true" CodeBehind="EventEditPopUp.aspx.cs" Inherits="CRM.Web.Protected.EventEditPopUp" %>

<%@ Register Src="~/UserControl/ucEvent.ascx" TagName="ucevent" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        html {
            /* no background image for popup */
            background: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">

    <div class="paneContent">
        <div class="page-title">
            <asp:Label ID="lblTitle" runat="server"></asp:Label>
        </div>

      
                <div class="message_area">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
                <div class="center" style="margin: 10px">
                    <button id="btnSave" class="mysubmit" type="button" onclick="saveEvent()">Save</button>
                    &nbsp;
                    <button id="btnClose" class="mysubmit" type="button" onclick="self.close()">Cancel</button>
                </div>
                <uc1:ucevent ID="uc_event" runat="server" />
           
    </div>
    <asp:Button ID="btnSave_hidden" runat="server" OnClick="btnSave_Click" style="display:none;" />
    <script type="text/javascript">
        function saveEvent() {
            $("#<%= btnSave_hidden.ClientID %>").click();

            window.opener.document.forms[0].submit();         
        }
    </script>
</asp:Content>
