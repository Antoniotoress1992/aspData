<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SchedulerTest.Models.Event>" %>
<html>
<body>
<% Html.BeginForm("Save", "MVCFormInLightbox"); %>

<% var obj = (Model as SchedulerTest.Models.Event); %>
<div>
<%= Html.TextArea("text", obj.text,  5, 42, null)%>
</div>
<%= Html.Hidden("id", obj.id)%>
<div>
From
<%= Html.TextBox("start_date", obj.start_date, new { @readonly="readonly"  })%>
To
<%= Html.TextBox("end_date", obj.end_date, new { @readonly = "readonly" })%>
</div>
<%= Html.Hidden("user_id", obj.user_id)%>
<p>
<input type="submit" name="actionButton" value="Save" />
<input type="button" onclick="lightbox.close()/* helper-method, only available in custom lightbox */" value="Close" />
<input type="submit" name="actionButton" value="Delete" />
</p>
<% Html.EndForm(); %>
</body>
</html>