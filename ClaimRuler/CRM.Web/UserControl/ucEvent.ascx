<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEvent.ascx.cs" Inherits="CRM.Web.UserControl.ucEvent" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div class="paneContentInner">
    
    <table style="margin: auto;" border="0">
        <tr>
            <td class="top" style="width:60%;">
                <div class="boxContainer" style="width: 100%;">
                    <div class="section-title">Event Details</div>
                    <div class="paneContentInner">
                        <table style="width: 100%; margin: auto;" class="editForm no_min_width">
                            <tr>
                                <td class="right" style="width: 30%;">Subject</td>
                                <td class="redstar">*</td>
                                <td>
                                    <ig:WebTextEditor ID="txtSubject" runat="server" MaxLength="250" Width="300px"></ig:WebTextEditor>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvsubejct" runat="server" ControlToValidate="txtSubject" ValidationGroup="task"
                                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter subject." CssClass="validation1" />

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Description</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtDescription" runat="server" MaxLength="500" Width="305px" TextMode="MultiLine" MultiLine-Rows="3"></ig:WebTextEditor>
                                </td>
                            </tr>

                            <tr>
                                <td class="right">Start Date/Time</td>
                                <td class="redstar">*</td>
                                <td>
                                    <ig:WebDatePicker ID="txtEventDateTimeStart" runat="server" Width="135px" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                        <Buttons>
                                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                        </Buttons>
                                    </ig:WebDatePicker>

                                </td>
                            </tr>
                            <tr>
                                <td class="right">End Date/Time</td>
                                <td class="redstar">*</td>
                                <td>
                                    <ig:WebDatePicker ID="txtEventDateTimeEnd" runat="server" Width="135px" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                        <Buttons>
                                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                        </Buttons>
                                    </ig:WebDatePicker>

                                </td>
                            </tr>

                            <tr>
                                <td class="right">Owner</td>
                                <td class="redstar">*</td>
                                <td>
                                    <asp:DropDownList ID="ddlOwner" runat="server" Width="200px" />
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlOwner" ValidationGroup="task"
                                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please select owner." CssClass="validation1" InitialValue="0" />

                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <h3 style="display:none;">Reminder</h3>
                    <div class="paneContentInner" style="display:none;">
                        <table style="width: 100%;" class="editForm no_min_width">
                            <tr>
                                <td class="right" style="width: 30%">When</td>
                                <td class="redstar"></td>
                                <td style="width: 20%;">
                                    <asp:DropDownList ID="ddlReminderWhen" runat="server" Width="135px">
                                        <asp:ListItem Text="--- Select ---" Value="0" />
                                        <asp:ListItem Text="On Time" Value="0" />
                                        <asp:ListItem Text="15 mins" Value="15" />
                                        <asp:ListItem Text="30 mins" Value="30" />
                                        <asp:ListItem Text="1 hour" Value="60" />
                                        <asp:ListItem Text="2 hours" Value="120" />
                                        <asp:ListItem Text="6 hours" Value="360" />
                                        <asp:ListItem Text="12 hours" Value="720" />
                                        <asp:ListItem Text="1 day" Value="1440" />
                                        <asp:ListItem Text="2 days" Value="2880" />
                                        <asp:ListItem Text="3 days" Value="4320" />
                                        <asp:ListItem Text="4 days" Value="5760" />
                                        <asp:ListItem Text="5 days" Value="7200" />
                                        <asp:ListItem Text="6 days" Value="8640" />
                                        <asp:ListItem Text="1 week" Value="10080" />
                                        <asp:ListItem Text="2 weeks" Value="20160" />
                                        <asp:ListItem Text="3 weeks" Value="30240" />
                                    </asp:DropDownList>
                                </td>
                                <td class="left">
                                    <asp:LinkButton ID="lbtnClearReminder" runat="server" Text="Clear" OnClick="lbtnClearReminder_Click" CssClass="link" /></td>
                            </tr>
                            <tr>
                                <td class="right">Repeat</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlReminderRepeat" runat="server" Width="135px">
                                        <asp:ListItem Text="None" Value="0" />
                                        <asp:ListItem Text="Daily" Value="1" />
                                        <asp:ListItem Text="Weekly" Value="2" />
                                        <asp:ListItem Text="Monthly" Value="3" />
                                        <asp:ListItem Text="Yearly" Value="4" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Alert</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlReminderAlert" runat="server" Width="135px">
                                        <asp:ListItem Text="Email" Value="1" />
                                        <asp:ListItem Text="Pop-up" Value="2" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <h3 style="display:none;">Event Recurrence</h3>
                    <div class="paneContentInner" style="display:none;">
                        <table style="width: 100%;" class="editForm no_min_width" border="0">
                            <tr>
                                <td class="right" style="width: 30%;">Start Date</td>
                                <td class="redstar"></td>
                                <td class="top nowrap" style="width: 20%;">

                                    <ig:WebDatePicker ID="txtRecurrenceStartDate" runat="server" Width="135px">
                                        <Buttons>
                                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                        </Buttons>
                                    </ig:WebDatePicker>
                                </td>
                                <td class="left">
                                    <asp:LinkButton ID="lbtnClearRecurrence" runat="server" Text="Clear" OnClick="lbtnClearRecurrence_Click" CssClass="link" /></td>
                            </tr>
                            <tr>
                                <td class="right">End Date</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebDatePicker ID="txtRecurrenceEndDate" runat="server" Width="135px">
                                        <Buttons>
                                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                        </Buttons>
                                    </ig:WebDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Repeat</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlRecurringRepeatFrequency" runat="server" Width="135px">
                                        <asp:ListItem Text="None" Value="0" />
                                        <asp:ListItem Text="Daily" Value="1" />
                                        <asp:ListItem Text="Weekly" Value="2" />
                                        <asp:ListItem Text="Monthly" Value="3" />
                                        <asp:ListItem Text="Yearly" Value="4" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
            <td class="top left">
                <asp:Panel ID="pnlInvitees" runat="server" Visible="false">
                    <div class="boxContainer" style="width: 100%;">
                        <div class="section-title">Invitees</div>
                        <div class="paneContentInner">
                            <div style="margin-bottom: 5px;">
                                <a class="link" href="javascript: showUserDialog();">Add User</a>
                                &nbsp;
                                                <a class="link" href="javascript: showContactDialog();">Add Contact</a>
                                &nbsp;
                                                <a class="link" href="javascript: showInsuredDialog();">Add Insured</a>
                                &nbsp;
                                                 <asp:LinkButton ID="lbtnUninviteeAll" runat="server" Text="Clear"
                                                     OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete all invitee?');"
                                                     CssClass="link" OnClick="lbtnUninviteeAll_Click" />
                            </div>
                            <asp:GridView ID="gvInvitees" runat="server" CssClass="gridView"
                                AutoGenerateColumns="false"
                                CellPadding="2"
                                HorizontalAlign="Center"
                                OnRowCommand="gvInvitees_RowCommand"
                                ShowFooter="false"
                                Width="100%">
                                <RowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Width="20px" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server"
                                                ImageAlign="Middle"
                                                ImageUrl="~/Images/delete_icon.png"
                                                ToolTip="Delete"
                                                CommandName="DoDelete"
                                                CommandArgument='<%#Eval("inviteeID") %>'
                                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this invitee?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="inviteeName" HeaderText="Invitee Name" />
                                </Columns>
                            </asp:GridView>
                        </div>



                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>



</div>
<div id="div_insuredGrid" style="display: none;" title="Contacts">
    <ig:WebDataGrid ID="gvLeads" runat="server" CssClass="gridView smallheader"
        AutoGenerateColumns="false" Height="400px" DataSourceID="edsLeads"
        Width="100%">
        <Columns>
            <ig:BoundDataField DataFieldName="LeadId" Key="LeadId" Header-Text="LeadId" Hidden="true" />
            <ig:BoundDataField DataFieldName="InsuredName" Key="InsuredName" Header-Text="Insured Name" />
            <ig:BoundDataField DataFieldName="EmailAddress" Key="EmailAddress" Header-Text="E-mail Address" />
        </Columns>
        <Behaviors>
            <ig:Filtering Alignment="Top" Visibility="Visible" Enabled="true" AnimationEnabled="true" />
            <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                ThresholdFactor="0.5" Enabled="true" />

            <ig:Selection RowSelectType="Multiple" Enabled="True" CellClickAction="Row">
            </ig:Selection>
        </Behaviors>
    </ig:WebDataGrid>

    <asp:EntityDataSource ID="edsLeads" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
        EnableFlattening="False" EntitySetName="Leads"
        Where="it.ClientId = @ClientID"
        Select="it.LeadId, it.InsuredName, it.EmailAddress, it.ClientId"
        OrderBy="it.InsuredName Asc">
        <WhereParameters>
            <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
</div>
<div id="div_contactGrid" style="display: none;" title="Contacts">
    <ig:WebDataGrid ID="gvContacts" runat="server" CssClass="gridView smallheader"
        AutoGenerateColumns="false" Height="400px" DataSourceID="edsContacts"
        Width="100%">
        <Columns>
            <ig:BoundDataField DataFieldName="ContactID" Key="ContactID" Header-Text="ID" Hidden="true" />
            <ig:BoundDataField DataFieldName="ContactName" Key="ContactName" Header-Text="Contact Name" />
            <ig:BoundDataField DataFieldName="Email" Key="Email" Header-Text="E-mail Address" />
        </Columns>
        <Behaviors>
            <ig:Filtering Alignment="Top" Visibility="Visible" Enabled="true" AnimationEnabled="true" />
            <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                ThresholdFactor="0.5" Enabled="true" />

            <ig:Selection RowSelectType="Multiple" Enabled="True" CellClickAction="Row">
            </ig:Selection>
        </Behaviors>
    </ig:WebDataGrid>

    <asp:EntityDataSource ID="edsContacts" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
        EnableFlattening="False" EntitySetName="vw_Contact"
        Where="it.ClientId = @ClientID"
        OrderBy="it.ContactName Asc">
        <WhereParameters>
            <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
</div>
<div id="div_userGrid" style="display: none;" title="Users">
    <ig:WebDataGrid ID="gvUsers" runat="server" CssClass="gridView smallheader"
        AutoGenerateColumns="false" Height="400px" Width="100%" DataSourceID="edsUsers" BorderStyle="None">
        <Columns>
            <ig:BoundDataField DataFieldName="UserId" Key="UserId" Header-Text="UserId" Hidden="true" />
            <ig:BoundDataField DataFieldName="FirstName" Key="FirstName" Header-Text="First Name" />
            <ig:BoundDataField DataFieldName="LastName" Key="LastName" Header-Text="Last Name" />
        </Columns>
        <Behaviors>
            <ig:Selection RowSelectType="Multiple" Enabled="True" CellClickAction="Row">
            </ig:Selection>
            <ig:Filtering Alignment="Top" Visibility="Visible" Enabled="true" AnimationEnabled="true" />
            <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                ThresholdFactor="0.5" Enabled="true" />
        </Behaviors>
    </ig:WebDataGrid>

    <asp:EntityDataSource ID="edsUsers" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
        EnableFlattening="False" EntitySetName="SecUser"
        Select="it.UserId, it.FirstName, it.LastName, it.Status, it.ClientID"
        Where="it.ClientID = @ClientID && it.Status = true"
        OrderBy="it.FirstName Asc, it.LastName Asc">
        <WhereParameters>
            <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
</div>
<asp:HiddenField ID="hf_taskID" runat="server" Value="0" />
<asp:Button ID="btnRefreshInvietees" runat="server" OnClick="btnRefreshInvietees_Click" Style="display: none" />
<script type="text/javascript">
    function showInsuredDialog() {
        // clear previous selection
        var grid = $find("<%= gvLeads.ClientID %>");
         var selection = grid.get_behaviors().get_selection();
         var rows = selection.get_selectedRows();
         for (var i = 0; i < rows.get_length() ; i++) {
             rows.remove(rows.getItem(i));
         }

         $("#div_insuredGrid").dialog({
             modal: false,
             width: 600,
             resizable: false,
             close: function (e, ui) {
                 $(this).dialog('destroy');
                 $("#div_insuredGrid").hide();
             },
             buttons:
             {
                 'Done': function () {
                     addInsureds();
                     $(this).dialog('close');
                 }
             }
         });
     }
     function addInsureds(sender, args) {
         var grid = $find("<%= gvLeads.ClientID %>");
            var selection = grid.get_behaviors().get_selection();
            var rows = selection.get_selectedRows();
            var taskID = $("#<%= hf_taskID.ClientID %>").val();

            for (var i = 0; i < rows.get_length() ; i++) {
                var leadID = selection.get_selectedRows(i).getItem(i).get_cell(0).get_text();


                // save id in hidden field            
                PageMethods.inviteeAddContact(taskID, null, null, leadID);
            }
            if (rows.get_length() > 0)
                refreshInviteeGrid();
        }
</script>
<script type="text/javascript">
    function showUserDialog() {
        // clear previous selection
        var grid = $find("<%= gvUsers.ClientID %>");
            var selection = grid.get_behaviors().get_selection();
            var rows = selection.get_selectedRows();
            for (var i = 0; i < rows.get_length() ; i++) {
                rows.remove(rows.getItem(i));
            }

            $("#div_userGrid").dialog({
                modal: false,
                width: 600,
                resizable: false,
                close: function (e, ui) {
                    $(this).dialog('destroy');
                    $("#div_userGrid").hide();
                },
                buttons:
                {
                    'Done': function () {
                        addUsers();
                        $(this).dialog('close');
                    }
                }
            });
        }
        function addUsers(sender, args) {
            var grid = $find("<%= gvUsers.ClientID %>");
            var selection = grid.get_behaviors().get_selection();
            var rows = selection.get_selectedRows();
            var taskID = $("#<%= hf_taskID.ClientID %>").val();

            for (var i = 0; i < rows.get_length() ; i++) {
                var userID = selection.get_selectedRows(i).getItem(i).get_cell(0).get_text();


                // save id in hidden field            
                PageMethods.inviteeAddContact(taskID, null, userID, null);
            }
            if (rows.get_length() > 0)
                refreshInviteeGrid();
        }
</script>
<script type="text/javascript">

    function showContactDialog() {
        // clear previous selection
        var grid = $find("<%= gvContacts.ClientID %>");
            var selection = grid.get_behaviors().get_selection();
            var rows = selection.get_selectedRows();
            for (var i = 0; i < rows.get_length() ; i++) {
                rows.remove(rows.getItem(i));
            }

            $("#div_contactGrid").dialog({
                modal: false,
                width: 600,
                resizable: false,
                close: function (e, ui) {
                    $(this).dialog('destroy');
                    $("#div_contactGrid").hide();
                },
                buttons:
                {
                    'Done': function () {
                        addContacts();
                        $(this).dialog('close');
                    }
                }
            });
        }

        function addContacts(sender, args) {
            var grid = $find("<%= gvContacts.ClientID %>");
            var selection = grid.get_behaviors().get_selection();
            var rows = selection.get_selectedRows();
            var taskID = $("#<%= hf_taskID.ClientID %>").val();

            for (var i = 0; i < rows.get_length() ; i++) {
                var contactID = selection.get_selectedRows(i).getItem(i).get_cell(0).get_text();


                // save id in hidden field            
                PageMethods.inviteeAddContact(taskID, contactID, null, null);
            }
            if (rows.get_length() > 0)
                refreshInviteeGrid();
        }

        function refreshInviteeGrid() {
            $("#<%= btnRefreshInvietees.ClientID %>").click();

        }
</script>
