<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="CRM.Web.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Css/ClaimRuler.css" rel="stylesheet" />
    <style>
        .black_overlay {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }

        .white_content {
            display: none;
            position: absolute;
            top: 25%;
            left: 25%;
            width: 50%;
            height: 50%;
            /*padding: 16px;*/
            border: 1px solid black;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }

        .ui-widget-header2 {
    background: none repeat scroll 0 0 #888888;
    border-bottom: 1px solid #777777;
    color: #FFFFFF;
    font-weight: normal;
      height: 25px;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <p>This is the main content. To display a lightbox click <a href="javascript:void(0)" onclick="document.getElementById('light').style.display='block';document.getElementById('fade').style.display='block'">here</a></p>
       <%-- <div id="light" class="white_content">
            <div style="width: 90%; float: left">This is the lightbox content.</div>
            <div style="width: 10%; float: right">
                <a href="javascript:void(0)" onclick="document.getElementById('light').style.display='none';document.getElementById('fade').style.display='none'">Close</a>
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:Button ID="Button1" runat="server" Text="save" OnClick="Button1_Click"/>
            </div>
        </div>--%>

        <div id="light" style="display: none; width: 40%;" title="Add Auto Invoice" class="white_content">
            <div class="ui-widget-header2">
                Sudeep kumar
            </div>
        <div class="boxContainer" style="margin:6px 13px 7px 13px; ">
        <div class="section-title">
            Add Auto Invoice
        </div>

        Sudeep kumar
        </div>
        </div>


        <div id="fade" class="black_overlay2"></div>
    </form>
</body>
</html>
