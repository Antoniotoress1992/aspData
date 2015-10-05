<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="CRM.Web.test" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>  
    <%--<link type="text/css" href="/SamplesCommon/jQuery/igRating/Common/style.css" rel="stylesheet" />

    <link type="text/css" href="/SamplesBrowser/SamplesCommon/aspnet/Common/Styles/jquery/css/themes/infragistics2012/infragistics.theme.css" rel="stylesheet" />
    <link type="text/css" href="/SamplesBrowser/SamplesCommon/aspnet/Common/Styles/jquery/css/structure/infragistics.css" rel="stylesheet" />

    <script type="text/javascript" src="/SamplesBrowser/Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/SamplesBrowser/Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/SamplesBrowser/SamplesCommon/aspnet/Common/Scripts/jquery/js/infragistics.js"></script>--%>

    <script src="http://igniteui.com/js/modernizr.min.js"></script>
<script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.min.js"></script>
<script src="http://cdn-na.infragistics.com/igniteui/latest/js/infragistics.core.js"></script>
<script src="http://cdn-na.infragistics.com/igniteui/latest/js/infragistics.lob.js"></script>
<link href="http://cdn-na.infragistics.com/igniteui/latest/css/themes/infragistics/infragistics.theme.css" rel="stylesheet"/>
<link href="http://cdn-na.infragistics.com/igniteui/latest/css/structure/infragistics.css" rel="stylesheet"/>

    <script type="text/javascript" language="javascript">
        $(window).load(function () {
            $("#igUpload1").igUpload({
                mode: 'multiple',
                maxUploadedFiles: 5,
                maxSimultaneousFilesUploads: 2,
                progressUrl: "/samplesbrowser/IGUploadStatusHandler.ashx",
                onError: function (e, args) {
                    showAlert(args);
                }
            });
        });
        function showAlert(args) {
            $("#error-message").html(args.errorMessage).stop(true, true).fadeIn(500).delay(3000).fadeOut(500);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
       <div>
           <div class="sampleContents">
        <div class="sample-container">
            <p>Multiple Upload</p>
            <div id="igUpload1"></div>
            <div id="error-message" style="color: #FF0000; font-weight: bold;"></div>
        </div>
    </div>
       </div>
    </form>
</body>
</html>
