<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SchedulerTest.Models.ValidEvent>" %>
<html>
<body>

<% 
    Html.EnableClientValidation();
    Html.EnableUnobtrusiveJavaScript(); 
%>

<html>
<head>
    <script src="/Scripts/jquery-1.8.0.min.js"></script>
    <script src="/Scripts/jQuery.Validate.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
</head>
<body>
    <% using (Html.BeginForm("Save", "JQueryValidation", FormMethod.Post, new { id = "schedulerForm" }))
       { %>   
        <div class="values">  
            <%= Html.LabelFor(m => Model.text) %> : <%= Html.EditorFor(m => Model.text) %> <%= Html.ValidationMessageFor(m => Model.text) %> <br />
            <%= Html.LabelFor(m => Model.email) %> : <%= Html.EditorFor(m => Model.email)%> <%= Html.ValidationMessageFor(m => Model.email)%> <br />
               
            From
            <%= Html.TextBox("start_date", Model.start_date, new { @readonly = "readonly" })%>
            To
            <%= Html.TextBox("end_date", Model.end_date, new { @readonly = "readonly" })%>
            <%= Html.Hidden("id", Model.id)%> 
        </div>
        <div style="text-align:center">
                <input type="submit" name="actionType"  value="Save" />
                <input type="button" onclick="lightbox.close()/* helper-method, only available in custom lightbox */" value="Close" />
                <input type="submit" name="actionType" value="Delete" />
        </div>
    <% } %>
    </body>
</html>
