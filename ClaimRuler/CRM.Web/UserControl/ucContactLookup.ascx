<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContactLookup.ascx.cs" Inherits="CRM.Web.UserControl.ucContactLookup" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

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

            <ig:Selection RowSelectType="Single" Enabled="True" CellClickAction="Row">
                <SelectionClientEvents RowSelectionChanged="gvContacts_rowsSelected" />
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
<script type="text/javascript">
    function showContactDialog() {
        // clear previous selection
        var grid = $find("<%= gvContacts.ClientID %>");
        var selection = grid.get_behaviors().get_selection();
        var rows = selection.get_selectedRows();
        rows.remove(rows.getItem(0));

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
                    $(this).dialog('close');

                }
            }
        });
    }    
</script>
