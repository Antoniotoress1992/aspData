<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContactList.ascx.cs" Inherits="CRM.Web.UserControl.ucContactList" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<ig:WebDataGrid ID="wdgContacts" runat="server" Height="350px" Width="100%"
	AutoGenerateColumns="false"
	CssClass="gridView"
	DataSourceID="edsContacts">
	<Columns>
		<ig:BoundDataField DataFieldName="FirstName" Key="FirstName" Header-Text="First Name" />
		<ig:BoundDataField DataFieldName="LastName" Key="LastName" Header-Text="Last Name" />
		<ig:BoundDataField DataFieldName="CompanyName" Key="CompanyName" Header-Text="Company Name" />
		<ig:BoundDataField DataFieldName="DepartmentName" Key="DepartmentName" Header-Text="Department" />
		<ig:BoundDataField DataFieldName="ContactTitle" Key="ContactTitle" Header-Text="Title" />
		<ig:TemplateDataField Key="LeadContactType" Header-Text="Type">
			<ItemTemplate>
				<%# Eval("LeadContactType.Description") %>
			</ItemTemplate>
		</ig:TemplateDataField>
		<ig:BoundDataField DataFieldName="Email" Key="Email" Header-Text="Email" />
		<ig:BoundDataField DataFieldName="Phone" Key="Phone" Header-Text="Phone" />
		<ig:BoundDataField DataFieldName="Mobile" Key="Mobile" Header-Text="Mobile" />
		<ig:BoundDataField DataFieldName="Fax" Key="Fax" Header-Text="Fax" />
	</Columns>
	<Behaviors>
		<ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="20"
			ThresholdFactor="0.5" Enabled="true" />		
		<ig:Sorting Enabled="true" SortingMode="Single" />
	</Behaviors>
</ig:WebDataGrid>
<asp:EntityDataSource ID="edsContacts" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
	EnableFlattening="False" EntitySetName="Contacts" Include="LeadContactType"
	Where="it.ClientId = @ClientID"
	OrderBy="it.FirstName, it.LastName Asc">
	<WhereParameters>
		<asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
	</WhereParameters>
</asp:EntityDataSource>
