<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSecondaryProducer.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucSecondaryProducer" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div class="toolbar toolbar-body">
	<table>
		<tr>
			<td>
				<asp:LinkButton ID="btnReturnToClaim" runat="server" Text="" CssClass="toolbar-item" OnClick="btnReturnToClaim_Click">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">Return to Claim</span>
				</asp:LinkButton>
			</td>
		</tr>
	</table>
</div>
<div class="paneContentInner">
    
	<div class="message_area">
		<asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
		<asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
		<asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
	</div>

	<table style="border-collapse: separate; border-spacing: 7px; padding: 2px;" border="0" class="editForm">
		<tr>			
			<td class="right">Secondary Producer</td>
			<td style="vertical-align: middle;">
				<ig:WebTextEditor ID="txtName" Width="300px" MaxLength="100" runat="server"/>
			</td>
			<td style="vertical-align: middle;">
				<asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click"
					ValidationGroup="producer"  CausesValidation="true"/>
				&nbsp;
				<asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
					CssClass="mysubmit" OnClick="btnCancel_Click" />

			</td>
		</tr>
		<tr>
			<td>
				<asp:RequiredFieldValidator runat="server" ID="req1" ControlToValidate="txtName"
						ErrorMessage="Please Enter Secondary Producer Name" ValidationGroup="producer" Display="Dynamic"
						CssClass="validation1" />
			</td>
		</tr>
	</table>

	<div class="gridView" id="dvData" runat="server">
		<asp:ListView runat="server" ID="lvData" ItemPlaceholderID="itemPlaceHolder1" OnItemCommand="lvData_ItemCommand"
			DataKeyNames="SecondaryProduceId">
			<LayoutTemplate>
				<table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px;" border="0" class="editForm">
					<tr style="background: #c9dffb">
						<td width="10%" class='hl' style='text-align: center;'>
							<strong>S.No.</strong>
						</td>
						<td width="70%" class='hl' style='text-align: center;'>
							<strong>Lead Source</strong>
						</td>

						<td width="10%" class='hl' style='text-align: center;'>&nbsp;
						</td>
					
					</tr>
					<tr>
						<asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
					</tr>
				</table>
			</LayoutTemplate>
			<ItemTemplate>
				<tr style='color: black; background: #FFFFFF'>
					<td style="" class="center">
						<%# Container.DataItemIndex+1%>
					</td>
					<td style="">

						<asp:Label ID="lblName" runat="server" Text='<%#Eval("SecondaryProduceName")%>'></asp:Label>&nbsp;&nbsp;
					</td>

					<td style="" class="center">
						<asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("SecondaryProduceId") %>'
							ToolTip="Edit" 
                            ImageUrl="../../Images/edit_icon.png" 
                            Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>' />
						&nbsp;
						<asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
							CommandName="DoDelete" CommandArgument='<%#Eval("SecondaryProduceId") %>' 
                            ToolTip="Delete"
							ImageUrl="../../Images/delete_icon.png"
							Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>'
							OnClientClick="javascript:return ConfirmDialog(this, 'Confirmation', 'Do you want to delete this record ?');" />
					</td>
					
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr style='color: black; background: #e8f2ff'>
					<td style="" class="center">
						<%# Container.DataItemIndex+1%>
					</td>
					<td style="">

						<asp:Label ID="lblName" runat="server" Text='<%#Eval("SecondaryProduceName")%>'></asp:Label>&nbsp;&nbsp;
					</td>

					<td style="" class="center">
						<asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("SecondaryProduceId") %>'
							ToolTip="Edit" 
                            ImageUrl="../../Images/edit_icon.png" 
                            Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>'  />
						&nbsp;					
						<asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
							CommandName="DoDelete" CommandArgument='<%#Eval("SecondaryProduceId") %>' 
                            ToolTip="Delete"
							ImageUrl="../../Images/delete_icon.png"
							Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>' 
							OnClientClick="javascript:return ConfirmDialog(this, 'Confirmation', 'Do you want to delete this record ?');" />
					</td>
				</tr>
			</AlternatingItemTemplate>
			<EmptyDataTemplate>
				<tr>
					<td colspan="5">
						<div style="padding-top: 10px; padding-bottom: 10px;">
							<asp:Label ID="lblRecordNotFound" runat="server" CssClass="info" Text="Records Not Found !!!" />
						</div>
					</td>
				</tr>
			</EmptyDataTemplate>
		</asp:ListView>

	</div>

	<div class="pagination">
		<asp:DataPager ID="PagerRow" Visible="true" PageSize="15" runat="server" PagedControlID="lvData"
			OnPreRender="lvData_PreRender">
			<Fields>
				<asp:NumericPagerField ButtonCount="5" NextPageText=">>" PreviousPageText="<<" CurrentPageLabelCssClass="PagerCurrent"
					NextPreviousButtonCssClass="PagerNormal" NumericButtonCssClass="PagerNormal" />
			</Fields>
		</asp:DataPager>
	</div>

</div>

<asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
<asp:HiddenField ID="hfStatus" runat="server" Value="0" />
<asp:HiddenField ID="hdId" runat="server" Value="0" />
