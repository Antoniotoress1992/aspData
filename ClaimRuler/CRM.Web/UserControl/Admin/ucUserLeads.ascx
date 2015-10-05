<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUserLeads.ascx.cs"
	Inherits="CRM.Web.UserControl.Admin.ucUserLeads" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Src="ucGeneratePDF.ascx" TagName="ucGeneratePDF" TagPrefix="uc1" %>--%>
<head id="Head1" runat="server" />
<div class="all_box">
	<div class="box1">
		<div class="mainbox">
			<h2>
				Tasks</h2>
			<asp:GridView ID="gvTasks" Width="99%" runat="server" AutoGenerateColumns="False"
				CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff" HeaderStyle-Font-Size="10px"
				RowStyle-HorizontalAlign="Center">
				<EmptyDataTemplate>
					No tasks available.
				</EmptyDataTemplate>
				<Columns>
					<asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# Eval("start_date", "{0:g}") %>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Event" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# Eval("text") %>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Details" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# Eval("details")%>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="User Name" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# Eval("owner_name") %>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Lead/Claim" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# Eval("lead_name") %>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Status" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<%# Eval("statusName") %>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
			<br />
			<h2>
				Lead/Claim Detail</h2>
			<div class="warrape">
				<div align="left" class="search_part" style="margin-bottom: 15px; width: 85%;">
					<table width="95%" border="0" cellspacing="0" cellpadding="00" class="new_user" align="center">
						<%--<tr>
                            <td align="right">
                                User Name&nbsp;
                            </td>
                            <td colspan="1">
                                <asp:TextBox ID="txtSearch" Width="85%" runat="server"></asp:TextBox>
                            </td>
                            <td align="right" valign="top">
                                Policyholder Name&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtClaimantName" runat="server" Width="85%"></asp:TextBox>
                            </td>
                            <td align="center">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Claimant Address&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtClaimantAddress" Width="85%" runat="server"></asp:TextBox>
                            </td>
                            <td align="right" valign="top">
                                Policy Number&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtInsurancePolicyNumber" runat="server" Width="85%"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="ClientPayments"
                                    CssClass="mysubmit" OnClick="btnSearch_Click" />
                            </td>
                        </tr>--%>
						<tr>
							<td colspan="4">
								&nbsp;
							</td>
						</tr>
						<tr>
							<td width="100" align="right" valign="top" style="padding-top: 5px;">
								<%-- width="150"--%>
								Date From&nbsp;
							</td>
							<td width="188" align="left" valign="top">
								<div style="width: 290px;">
									<asp:TextBox ID="txtDateFrom" onkeypress="javascript:return false;" Width="150px"
										runat="server" CssClass="myinput" oncopy="return false" onpaste="return false"
										oncut="return false"></asp:TextBox>&nbsp;&nbsp;
									<button id="f_rangeEnd_trigger" tabindex="29" style="margin: 0; padding: 0; border: none;
										background: none; font-size: 1em;">
										<img src="../../Images/calendar.jpg" width="14" height="13" /></button>
									<script type="text/javascript">
										RANGE_CAL_2 = new Calendar({
											inputField: "<%=txtDateFrom.ClientID %>",
											dateFormat: "%d/%m/%Y",
											trigger: "f_rangeEnd_trigger",
											bottomBar: true,
											showTime: true,
											onSelect: function () {
												this.hide();
												if (document.getElementById("<%=txtDateTo.ClientID %>").value != '') {


													var dateF = document.getElementById("<%=txtDateFrom.ClientID %>").value;
													var parts = dateF.split("/");
													var dtf = new Date(parts[2], parts[1] - 1, parts[0]);


													var dateT = document.getElementById("<%=txtDateTo.ClientID %>").value;
													var parts1 = dateT.split("/");
													var dtT = new Date(parts1[2], parts1[1] - 1, parts1[0]);

													if (dtf > dtT) {
														document.getElementById("<%=txtDateFrom.ClientID %>").value = '';
														alert("'Date-From' can't be after 'Date-To'...");
													}

												}
											}
										});
									</script>
								</div>
							</td>
							<td width="100" align="right" valign="top" style="padding-top: 5px;">
								Date To&nbsp;
							</td>
							<td width="10%">
								<div style="width: 290px;">
									<asp:TextBox ID="txtDateTo" onkeypress="javascript:return false;" Width="150px" runat="server"
										CssClass="myinput" oncopy="return false" onpaste="return false" oncut="return false"></asp:TextBox>&nbsp;&nbsp;
									<button id="f_rangeEnd_trigger1" tabindex="29" style="margin: 0; padding: 0; border: none;
										background: none; font-size: 1em;">
										<img src="../../Images/calendar.jpg" width="14" height="13" /></button>
									<script type="text/javascript">
										RANGE_CAL_2 = new Calendar({
											inputField: "<%=txtDateTo.ClientID %>",
											dateFormat: "%d/%m/%Y",
											trigger: "f_rangeEnd_trigger1",
											bottomBar: true,
											showTime: true,
											onSelect: function () {
												this.hide();
												if (document.getElementById("<%=txtDateFrom.ClientID %>").value != '') {

													var dateF = document.getElementById("<%=txtDateFrom.ClientID %>").value;
													var parts = dateF.split("/");
													var dtf = new Date(parts[2], parts[1] - 1, parts[0]);


													var dateT = document.getElementById("<%=txtDateTo.ClientID %>").value;
													var parts1 = dateT.split("/");
													var dtT = new Date(parts1[2], parts1[1] - 1, parts1[0]);

													if (dtf > dtT) {
														document.getElementById("<%=txtDateTo.ClientID %>").value = '';
														alert("'Date-To' can't be before 'Date-From'...");
													}

												}
											}
										});
									</script>
								</div>
								&nbsp;
							</td>
							<%--<td align="center">
                                <asp:Button ID="btnReset" Text="Reset" runat="server" CausesValidation="false" CssClass="mysubmit"
                                    Width="71px" OnClick="btnReset_Click" />
                            </td>--%>
						</tr>
						<tr>
							<td colspan="4" align="right">
								<asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="ClientPayments"
									CssClass="mysubmit" OnClick="btnSearch_Click" />&nbsp;&nbsp;&nbsp;
								<asp:Button ID="btnReset" Text="Reset" runat="server" CausesValidation="false" CssClass="mysubmit"
									Width="71px" OnClick="btnReset_Click" />
							</td>
						</tr>
					</table>
				</div>
			</div>
		</div>
	</div>
</div>
<div align="center" style="padding-top: 5px;">
	<asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
	<asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
	<asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
</div>
<div style="padding-top: 5px;">
</div>
<div style="width: 980px;">
	<asp:Panel ID="pnlUserLeads" runat="server" Width="980px">
		<asp:GridView ID="gvUserLeads" CssClass="Tables" OnRowCommand="gvUserLeads_RowCommand"
			ShowFooter="true" Width="100%" runat="server" AutoGenerateColumns="False" CellPadding="4"
			AlternatingRowStyle-BackColor="#e8f2ff" HeaderStyle-Font-Size="11px" AllowPaging="true"
			RowStyle-HorizontalAlign="Center" PageSize="10" OnPageIndexChanging="gvUserLeads_PageIndexChanging"
			OnSelectedIndexChanging="gvUserLeads_SelectedIndexChanging" PagerSettings-PageButtonCount="5"
			PagerStyle-Font-Bold="true" PagerStyle-Font-Size="9pt" OnSorting="gvUserLeads_Sorting"
			AllowSorting="true">
			<%-- AllowSorting="true"--%>
			<PagerStyle CssClass="pager" />
			<Columns>
				<%--   <asp:TemplateField HeaderText="User Name" HeaderStyle-BackColor="#e8f2ff" SortExpression="">
                    <ItemTemplate>
                        <%# Eval("SecUser.UserName") %>
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="Original Lead Date">
                    <ItemTemplate>
                       
                        <%# Eval("OriginalLeadDate") == null ? "" : Convert.ToDateTime(Eval("OriginalLeadDate")).ToString("dd-MMM-yy") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claims Number" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%#Eval("ClaimsNumber")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        
                        <%# Eval("StatusMaster.StatusName") == null ? "" : Eval("StatusMaster.StatusName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Status" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                       
                        <%# Eval("SubStatusMaster.SubStatusName") == null ? "" : Eval("SubStatusMaster.SubStatusName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date of Site Survey">
                    <ItemTemplate>
                        <%# Eval("SiteSurveyDate") == null ? "" : Convert.ToDateTime(Eval("SiteSurveyDate")).ToString("dd-MMM-yy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="All Documents on File?" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                        <%# Eval("AllDocumentsOnFile") == null ? "No" :  Convert.ToBoolean(Eval("AllDocumentsOnFile")) == true ? "Yes" : "No" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant First Name">
                    <ItemTemplate>
                        <%# Eval("ClaimantFirstName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant Last Name" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                        <%# Eval("ClaimantLastName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant City">
                    <ItemTemplate>
                        <%#Eval("CityMaster_1.CityName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant State" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                       
                         <%#Eval("StateMaster1.StateCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant Zip">
                    <ItemTemplate>
                        <%# Eval("Zip") %>
                    </ItemTemplate>
                </asp:TemplateField>
             
                <asp:TemplateField HeaderText="Lead Source">
                    <ItemTemplate>
                       
                        <%#Eval("LeadSourceMaster.LeadSourceName") == null ? "" : Eval("LeadSourceMaster.LeadSourceName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type of Damage" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                       
                        <%#Eval("TypeOfDamageText") == null ? "" : Eval("TypeOfDamageText")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type Of Property">
                    <ItemTemplate>
                             <%# Eval("TypeOfPropertyMaster.TypeOfProperty") == null ? "" : Eval("TypeOfPropertyMaster.TypeOfProperty")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
				<asp:TemplateField HeaderText="User Name" HeaderStyle-BackColor="#e8f2ff" SortExpression="SecUser.UserName">
					<%----%>
					<ItemTemplate>
						<%# Eval("SecUser.UserName") %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Original Lead Date" SortExpression="OriginalLeadDate">
					<ItemTemplate>
						<%# Eval("OriginalLeadDate") == null ? "" : Convert.ToDateTime(Eval("OriginalLeadDate")).ToString("dd-MMM-yy") %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Claims Number" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Right"
					SortExpression="ClaimsNumber">
					<ItemTemplate>
						<%#Eval("ClaimsNumber")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Status" SortExpression="StatusMaster.StatusName">
					<ItemTemplate>
						<%# Eval("StatusMaster.StatusName") == null ? "" : Eval("StatusMaster.StatusName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Sub Status" HeaderStyle-BackColor="#e8f2ff" SortExpression="SubStatusMaster.SubStatusName">
					<ItemTemplate>
						<%# Eval("SubStatusMaster.SubStatusName") == null ? "" : Eval("SubStatusMaster.SubStatusName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Date of Site Survey" SortExpression="SiteSurveyDate">
					<ItemTemplate>
						<%# Eval("SiteSurveyDate") == null ? "" : Convert.ToDateTime(Eval("SiteSurveyDate")).ToString("dd-MMM-yy")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="All Documents on File?" HeaderStyle-BackColor="#e8f2ff"
					SortExpression="AllDocumentsOnFile">
					<ItemTemplate>
						<%# Eval("AllDocumentsOnFile") == null ? "No" :  Convert.ToBoolean(Eval("AllDocumentsOnFile")) == true ? "Yes" : "No" %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="First Name" SortExpression="ClaimantFirstName">
					<ItemTemplate>
						<%# Eval("ClaimantFirstName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Last Name" HeaderStyle-BackColor="#e8f2ff"
					SortExpression="ClaimantLastName">
					<ItemTemplate>
						<%# Eval("ClaimantLastName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="City" SortExpression="CityMaster_1.CityName">
					<ItemTemplate>
						<%#Eval("CityMaster_1.CityName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="State" HeaderStyle-BackColor="#e8f2ff" SortExpression="StateMaster1.StateCode">
					<ItemTemplate>
						<%#Eval("StateMaster1.StateCode")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Zip" SortExpression="Zip">
					<ItemTemplate>
						<%# Eval("Zip") %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Lead Source" SortExpression="LeadSourceMaster.LeadSourceName">
					<ItemTemplate>
						<%#Eval("LeadSourceMaster.LeadSourceName") == null ? "" : Eval("LeadSourceMaster.LeadSourceName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Type of Damage" HeaderStyle-BackColor="#e8f2ff" SortExpression="TypeOfDamageText">
					<ItemTemplate>
						<%#Eval("TypeOfDamageText") == null ? "" : Eval("TypeOfDamageText")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Type Of Property" SortExpression="TypeOfPropertyMaster.TypeOfProperty">
					<ItemTemplate>
						<%# Eval("TypeOfPropertyMaster.TypeOfProperty") == null ? "" : Eval("TypeOfPropertyMaster.TypeOfProperty")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="left" ItemStyle-Width="8%" HeaderStyle-BackColor="#e8f2ff">
					<ItemTemplate>
						<asp:LinkButton ID="lnkEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("LeadId") %>'
							ToolTip="Edit" Text="Edit"></asp:LinkButton>&nbsp;&nbsp;&nbsp;<br />
						<asp:LinkButton ID="lnkView" runat="server" CommandName="DoView" CommandArgument='<%#Eval("LeadId") %>'
							ToolTip="View" Text="View"></asp:LinkButton><br />
						<asp:LinkButton ID="lnkCopy" runat="server" CommandName="DoCopy" CommandArgument='<%#Eval("LeadId") %>'
							ToolTip="Copy" Text="Copy"></asp:LinkButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</asp:Panel>
	<h2>
		For All</h2>
	<asp:Panel ID="pnlgrdAllLead" runat="server" Width="980px">
		<asp:GridView ID="grdAllLead" CssClass="Tables" ShowFooter="true" Width="100%" runat="server"
			AutoGenerateColumns="False" CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff"
			HeaderStyle-Font-Size="12px" AllowPaging="true" PageSize="20" OnPageIndexChanging="grdAllLead_PageIndexChanging"
			OnRowCommand="grdAllLead_RowCommand" RowStyle-HorizontalAlign="Center" PagerSettings-PageButtonCount="5"
			PagerStyle-Font-Bold="true" PagerStyle-Font-Size="9pt" OnSorting="grdAllLead_Sorting"
			AllowSorting="true">
			<PagerStyle CssClass="pager" />
			<Columns>
				<%--   <asp:TemplateField HeaderText="Original Lead Date">
                    <ItemTemplate>
                        <%# Convert.ToDateTime(Eval("OriginalLeadDate")).ToString("dd-MMM-yy") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claims Number" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%#Eval("ClaimsNumber")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                       
                        <%# Eval("StatusMaster.StatusName") == null ? "" : Eval("StatusMaster.StatusName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Status" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                       
                        <%# Eval("SubStatusMaster.SubStatusName") == null ? "" : Eval("SubStatusMaster.SubStatusName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date of Site Survey">
                    <ItemTemplate>
                        <%# Eval("SiteSurveyDate") == null ? "" : Convert.ToDateTime(Eval("SiteSurveyDate")).ToString("dd-MMM-yy")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="All Documents on File?" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                        <%# Eval("AllDocumentsOnFile") == null ? "No" :  Convert.ToBoolean(Eval("AllDocumentsOnFile")) == true ? "Yes" : "No" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant First Name">
                    <ItemTemplate>
                        <%# Eval("ClaimantFirstName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant Last Name" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                        <%# Eval("ClaimantLastName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant City">
                    <ItemTemplate>
                        <%#Eval("CityMaster_1.CityName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant State" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                        
                        <%#Eval("StateMaster1.StateCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Claimant Zip">
                    <ItemTemplate>
                        <%# Eval("Zip") %>
                    </ItemTemplate>
                </asp:TemplateField>
              
                <asp:TemplateField HeaderText="Lead Source">
                    <ItemTemplate>
                       
                        <%#Eval("LeadSourceMaster.LeadSourceName") == null ? "" : Eval("LeadSourceMaster.LeadSourceName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type of Damage" HeaderStyle-BackColor="#e8f2ff">
                    <ItemTemplate>
                      
                        <%# Eval("TypeOfDamageText") == null ? "" : Eval("TypeOfDamageText")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type Of Property">
                    <ItemTemplate>
                         <%# Eval("TypeOfPropertyMaster.TypeOfProperty") == null ? "" : Eval("TypeOfPropertyMaster.TypeOfProperty")%>
                    </ItemTemplate>
                </asp:TemplateField>
				--%>
				<asp:TemplateField HeaderText="Original Lead Date" SortExpression="OriginalLeadDate">
					<ItemTemplate>
						<%# Convert.ToDateTime(Eval("OriginalLeadDate")).ToString("dd-MMM-yy") %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Claims Number" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Right"
					SortExpression="ClaimsNumber">
					<ItemTemplate>
						<%#Eval("ClaimsNumber")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Status" SortExpression="StatusMaster.StatusName">
					<ItemTemplate>
						<%# Eval("StatusMaster.StatusName") == null ? "" : Eval("StatusMaster.StatusName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Sub Status" HeaderStyle-BackColor="#e8f2ff" SortExpression="SubStatusMaster.SubStatusName">
					<ItemTemplate>
						<%# Eval("SubStatusMaster.SubStatusName") == null ? "" : Eval("SubStatusMaster.SubStatusName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Date of Site Survey" SortExpression="SiteSurveyDate">
					<ItemTemplate>
						<%# Eval("SiteSurveyDate") == null ? "" : Convert.ToDateTime(Eval("SiteSurveyDate")).ToString("dd-MMM-yy")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="All Documents on File?" HeaderStyle-BackColor="#e8f2ff"
					SortExpression="AllDocumentsOnFile">
					<ItemTemplate>
						<%# Eval("AllDocumentsOnFile") == null ? "No" :  Convert.ToBoolean(Eval("AllDocumentsOnFile")) == true ? "Yes" : "No" %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="First Name" SortExpression="ClaimantFirstName">
					<ItemTemplate>
						<%# Eval("ClaimantFirstName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Last Name" HeaderStyle-BackColor="#e8f2ff"
					SortExpression="ClaimantLastName">
					<ItemTemplate>
						<%# Eval("ClaimantLastName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="City" SortExpression="CityMaster_1.CityName">
					<ItemTemplate>
						<%#Eval("CityMaster_1.CityName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="State" HeaderStyle-BackColor="#e8f2ff" SortExpression="StateMaster1.StateCode">
					<ItemTemplate>
						<%#Eval("StateMaster1.StateCode")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Zip" SortExpression="Zip">
					<ItemTemplate>
						<%# Eval("Zip") %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Lead Source" SortExpression="LeadSourceMaster.LeadSourceName">
					<ItemTemplate>
						<%#Eval("LeadSourceMaster.LeadSourceName") == null ? "" : Eval("LeadSourceMaster.LeadSourceName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Type of Damage" HeaderStyle-BackColor="#e8f2ff" SortExpression="TypeOfDamageText">
					<ItemTemplate>
						<%# Eval("TypeOfDamageText") == null ? "" : Eval("TypeOfDamageText")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Type Of Property" SortExpression="TypeOfPropertyMaster.TypeOfProperty">
					<ItemTemplate>
						<%# Eval("TypeOfPropertyMaster.TypeOfProperty") == null ? "" : Eval("TypeOfPropertyMaster.TypeOfProperty")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" HeaderStyle-BackColor="#e8f2ff">
					<ItemTemplate>
						<asp:LinkButton ID="lnkEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("LeadId") %>'
							ToolTip="Edit" Text="Edit"></asp:LinkButton>&nbsp;&nbsp;&nbsp;<br />
						<asp:LinkButton ID="lnkView" runat="server" CommandName="DoView" CommandArgument='<%#Eval("LeadId") %>'
							ToolTip="View" Text="View"></asp:LinkButton><br />
						<asp:LinkButton ID="lnkCopy" runat="server" CommandName="DoCopy" CommandArgument='<%#Eval("LeadId") %>'
							ToolTip="Copy" Text="Copy"></asp:LinkButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</asp:Panel>
</div>
<asp:HiddenField ID="hfNewPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
<asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfFromDate" runat="server" Value="" />
<asp:HiddenField ID="hfToDate" runat="server" Value="" />
<asp:HiddenField ID="hfCriteria" runat="server" Value="" />
