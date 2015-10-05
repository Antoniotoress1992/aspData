<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>
<html>
<body>
<% Html.BeginForm("Save", "MVCFormInLightbox")%>

<% Dim obj = TryCast(Model, DHXSchedulerSamples.Event)%>
<div>
<%= Html.TextArea("text", obj.text, 5, 42, Nothing)%>
</div>
<%= Html.Hidden("id", obj.id)%>
<div>
From
<%= Html.TextBox("start_date", obj.start_date, New With {.[readonly] = "readonly"})%>
To
<%= Html.TextBox("end_date", obj.end_date, New With {.[readonly] = "readonly"})%>
</div>
<%= Html.Hidden("user_id", obj.user_id)%>
<p>
<input type="submit" name="actionButton" value="Save" />
<input type="submit" value="Close" />

<input type="submit" name="actionButton" value="Delete" />
</p>
<% Html.EndForm()%>

</body>
</html>