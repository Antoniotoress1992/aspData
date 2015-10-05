<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
	CodeBehind="Adjuster.aspx.cs" Inherits="CRM.Web.Protected.Admin.Adjuster" %>

<%@ Register Src="~/UserControl/Admin/ucAdjusterList.ascx" TagName="ucAdjusterList" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<div class="paneContent">
		<div class="page-title">
			Adjusters 
		</div>

		
		<asp:UpdatePanel ID="updatePanel" runat="server">
			<ContentTemplate>
                <div style="overflow-x:scroll;width:100%">
				<uc1:ucAdjusterList ID="ucAdjuster1" runat="server" />
                </div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>

</asp:Content>
