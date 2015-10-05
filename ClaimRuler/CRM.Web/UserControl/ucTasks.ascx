<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTasks.ascx.cs" Inherits="CRM.Web.UserControl.ucTasks" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<ig:WebDataGrid ID="gvTasks" runat="server"
    AutoGenerateColumns="false"
    CssClass="gridView smallheader"
    DataSourceID="edsTasks"
    Height="650px"
    OnInitializeRow="gvTasks_InitializeRow"
    OnItemCommand="gvTasks_ItemCommand"
    Width="100%">
    <EmptyRowsTemplate>
        No Activities found
    </EmptyRowsTemplate>
    <Columns>
        <ig:TemplateDataField CssClass="center" Key="commands" Width="40px">
            <ItemTemplate>
                <asp:ImageButton ID="btnEdit" runat="server"
                    ImageAlign="Middle"
                    ImageUrl="~/Images/edit.png"
                    ToolTip="Edit"
                    CommandName="DoEdit"
                    Visible='<%# masterPage.hasEditPermission %>'
                    CommandArgument='<%# string.Format("{0}|{1}", Eval("TaskID"),Eval("Activity")) %>' />

                <asp:ImageButton ID="btnDelete" runat="server"
                    ImageAlign="Middle"
                    ImageUrl="~/Images/delete_icon.png"
                    ToolTip="Delete"
                    CommandName="DoRemove"
                    CommandArgument='<%#Eval("TaskID") %>'
                    Visible='<%# masterPage.hasDeletePermission %>'
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this activity?');" />
            </ItemTemplate>
        </ig:TemplateDataField>
        <ig:TemplateDataField Header-Text="Date Due" Key="DateDue" Width="65px">
            <ItemTemplate>
                <div class="center">
                    <%# Eval("DateDue", "{0:MM/dd/yyyy}") %>
                </div>
            </ItemTemplate>
        </ig:TemplateDataField>
        <ig:TemplateDataField Header-Text="Status" Key="TaskStatusName" Width="60px">
            <ItemTemplate>
                <div class="center">
                    <%# Eval("TaskStatusName") %>
                </div>
            </ItemTemplate>
        </ig:TemplateDataField>
        <ig:TemplateDataField Header-Text="Priority" Key="PriorityName" Width="60px">
            <ItemTemplate>
                <div class="center">
                    <%# Eval("PriorityName") %>
                </div>
            </ItemTemplate>
        </ig:TemplateDataField>
        <ig:TemplateDataField Header-Text="Subject / Description" Key="Subject">
            <ItemTemplate>
                <div class="center">
                    <%# Eval("Subject") %>
                </div>
                <div class="center">
                    <asp:HyperLink ID="hlnkLead" runat="server" />
                </div>
                <div class="center">
                    <%# Eval("Description") %>
                </div>
            </ItemTemplate>
        </ig:TemplateDataField>
        <ig:TemplateDataField Header-Text="User Name" Key="UserName" Width="100px">
            <ItemTemplate>
                <div class="center">
                    <%# Eval("UserName") %>
                </div>
            </ItemTemplate>
        </ig:TemplateDataField>
    </Columns>
    <EditorProviders>
        <ig:TextBoxProvider ID="TextBoxProvider" />
    </EditorProviders>

    <Behaviors>

        <ig:Selection RowSelectType="Single" CellClickAction="Row" Enabled="true">
        </ig:Selection>
        <ig:Sorting Enabled="true" SortingMode="Single">
            <ColumnSettings>
                <ig:SortingColumnSetting ColumnKey="commands" Sortable="false" />
                <ig:SortingColumnSetting ColumnKey="Description" Sortable="false" />
            </ColumnSettings>
        </ig:Sorting>
        <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
            ThresholdFactor="0.5" Enabled="true" />

    </Behaviors>
</ig:WebDataGrid>



