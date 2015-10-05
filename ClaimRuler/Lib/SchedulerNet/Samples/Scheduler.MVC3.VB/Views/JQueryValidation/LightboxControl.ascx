<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl( Of DHXSchedulerSamples.ValidEvent)" %>

<html>
<body>

<% 
    Html.EnableClientValidation()
    Html.EnableUnobtrusiveJavaScript()
%>

<html>
<head>
    <script src="/Scripts/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/jQuery.Validate.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
</head>
<body>
    <% Using (Html.BeginForm("Save", "JQueryValidation", FormMethod.Post, New With {.id = "schedulerForm"}))
        %>   
        <div class="values">  
            <%= Html.LabelFor(Function(m) Model.text)%> : <%= Html.EditorFor(Function(m) Model.text)%> <%= Html.ValidationMessageFor(Function(m) Model.text)%> <br />
            <%= Html.LabelFor(Function(m) Model.email)%> : <%= Html.EditorFor(Function(m) Model.email)%> <%= Html.ValidationMessageFor(Function(m) Model.email)%> <br />
               
            From
            <%= Html.TextBox("start_date", Model.start_date, New With {.[readonly] = "readonly"})%>
            To
            <%= Html.TextBox("end_date", Model.end_date, New With {.[readonly] = "readonly"})%>
            <%= Html.Hidden("id", Model.id)%> 
        </div>
        <div style="text-align:center">
                <input type="submit" name="actionType"  value="Save" />
                <input type="button" onclick="lightbox.close()/* helper-method, only available in custom lightbox */" value="Close" />
                <input type="submit" name="actionType" value="Delete" />
        </div>
    <% End Using%>
    </body>
</html>
