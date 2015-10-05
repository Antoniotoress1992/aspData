<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFooter.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucFooter" %>
<div class="myfooter" style="text-align: center;">
	©
	<script type="text/javascript">

		now = new Date
		theYear = now.getYear()
		if (theYear < 1900)
			theYear = theYear + 1900
		document.write(theYear)
	</script>
	<a href="http://www.itstrategiesgroup.com" target="_blank">IT Strategies Group</a>
	· <a href="../../terms.pdf" target="_blank">Terms of Use</a> · <a href="http://www.itstrategiesgroup.com/contact"
		target="_blank">Contact Us</a>
</div> 