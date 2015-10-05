<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="CarrierEdit.aspx.cs" Inherits="CRM.Web.Protected.Admin.CarrierEdit" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControl/Admin/ucCarrierEdit.ascx" TagName="ucCarrierEdit" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Admin/ucCarrierChannel.ascx" TagName="ucCarrierChannel" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">   
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
           <%-- Insurer/Carrier Detail--%>
            Client Detail
        </div>


        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>

                <uc1:ucCarrierEdit ID="ucCarrierEdit1" runat="server" />
                          
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

 <%-- <script src="../../js/jquery-1.9.1.min.js"></script>--%>
    <script type="text/javascript">

        
        function showUploadDocumentDialog() {
            // clear fields
            $("#txtDocumentDescription").val('');
            
            // build upload control
            $("#webUpload").igUpload({
                mode: 'single',
                //progressUrl: "http://appv3.claimruler.com/IGUploadStatusHandler.ashx",
                //progressUrl: "/samplesbrowser/IGUploadStatusHandler.ashx",
                progressUrl: "http://localhost:6375/IGUploadStatusHandler.ashx",
                onError: function (e, args) {
                    showAlert(e);
                },
                fileUploading: function (e, args) {
                    if (!validateDocumentUploadFields()) {
                        $("#txtDocumentDescription").focus();
                        return false;   // cancel upload
                    }
                },
                fileUploaded: function (e, args) {
                    // alert(args.fileID + " " + args.filePath);            $("[id$='txtTitle']")   
                    // get carrier id

                    var carrierID = parseInt($("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucCarrierEdit1_tabContainer_tabPanelDocuments_carrierDocuments_hf_carrierID").val());
                    var documentDescription = $("#txtDocumentDescription").val();

                    // save document to carrier
                    PageMethods.saveFile(carrierID, args.filePath, documentDescription);

                    $("#documentUpload").dialog('close');

                    // refresh carrier document after upload
                    $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucCarrierEdit1_tabContainer_tabPanelDocuments_carrierDocuments_btnRefresh").click();
            }
        });
           
        // show upload dialog
            $("#documentUpload").dialog({
                modal: false,
                width: 600,
                close: function (e, ui) {
                    $(this).dialog('destroy');
                },
                buttons:
                {
                    'Close': function () {
                        $(this).dialog('close');
                    }
                }
            });
           
        //return false;
    }
    function validateDocumentUploadFields() {
        var isValid = true;
        var description = $("#txtDocumentDescription").val();

        if ($.trim(description) == "")
            isValid = false;

        return isValid;
    }
</script>

  
</asp:Content>
