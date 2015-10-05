<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPolicyCasualtyLimits.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucPolicyCasualtyLimits" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:GridView Width="100%" ID="gvLimits"
    CssClass="gridView no_min_width"
    runat="server"
    AutoGenerateColumns="False"
    CellPadding="2"
    DataKeyNames="PolicyLimitID, LimitID"
    HorizontalAlign="Center">
    <RowStyle HorizontalAlign="Center" />
    <Columns>
        <asp:TemplateField HeaderText="Coverage">
            <ItemTemplate>
                <asp:Label ID="txtLimitLetter" runat="server" Width="20px" CssClass="center" Text='<%# Bind("Limit.LimitLetter") %>' Enabled="false"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <asp:Label ID="txtDescription" runat="server" MaxLength="50" Text='<%# Bind("Limit.LimitDescription") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Limit">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtLimit" runat="server" Width="50px" Value='<%# Bind("LimitAmount") %>'></ig:WebNumericEditor>
            </ItemTemplate>
        </asp:TemplateField>        
    </Columns>
</asp:GridView>

<asp:GridView ID="gvLimits2"
    CssClass="gridView no_min_width"
    runat="server"
    OnRowCommand="gvLimits2_RowCommand"
    AutoGenerateColumns="False"
    CellPadding="2"    
    HorizontalAlign="Center"
    ShowFooter="true"
    Width="100%">
    <RowStyle HorizontalAlign="Center" />
    <FooterStyle HorizontalAlign="Center" />
    <Columns>
        <asp:TemplateField HeaderText="Coverage">
            <ItemTemplate>
                <%# Eval("Limit.LimitLetter") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <%# Eval("Limit.LimitDescription") %>
            </ItemTemplate>
            <%--<FooterTemplate>
                <b>Total</b>
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Center" />--%>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="Policy Limit">
            <ItemTemplate>
                <%# Eval("LimitAmount") %>
            </ItemTemplate>           
        </asp:TemplateField>
         <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="35px">
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <div style="vertical-align: top;">
                                                                <asp:ImageButton ID="btnEditPropertyEdit" runat="server"  
                                                                    ToolTip="Edit"
                                                                    ImageAlign="Top"
                                                                    ImageUrl="~/Images/edit_icon.png"
                                                                    OnClientClick="return LossDetailsPopUpAddEditProperty(this)"
                                                                />    
                                                                <asp:ImageButton ID="btnDelete" runat="server" 
                                                                    CommandName="DoDelete"
                                                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this coverage?');"
                                                                    CommandArgument='<%#Eval("LimitID") %>' 
                                                                    ToolTip="Delete" 
                                                                    ImageUrl="~/Images/delete_icon.png"                                                                    
                                                                    />                                                            
                                                            </div>
                                                            <asp:HiddenField id="hdnEditPropertyEdit" runat="server" Value='<%#Eval("LimitID") %>'/>
                                                            </ItemTemplate>
             </asp:TemplateField>
        
    </Columns>    
</asp:GridView>

<asp:Label ID="lblNoRecordFound" runat="server" Text="No Records Found" ForeColor="Red"></asp:Label>
<asp:GridView ID="gvLimits3"
    CssClass="gridView no_min_width"
    runat="server"
    OnRowCommand="gvLimits3_RowCommand"
    AutoGenerateColumns="False"
    CellPadding="2"    
    HorizontalAlign="Center"
    ShowFooter="true"
    Width="100%">
    <RowStyle HorizontalAlign="Center" />
    <FooterStyle HorizontalAlign="Center" />
    <Columns>
        <asp:TemplateField HeaderText="Coverage">
            <ItemTemplate>
                <%# Eval("LimitLetter") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <%# Eval("LimitDescription") %>
            </ItemTemplate>
            <%--<FooterTemplate>
                <b>Total</b>
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Center" />--%>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="Policy Limit">
            <ItemTemplate>
                <%# Eval("LimitAmount") %>
            </ItemTemplate>           
        </asp:TemplateField>
         <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="35px">
                                                        <ItemStyle Wrap="false" />
                                                        <ItemTemplate>
                                                            <div style="vertical-align: top;">
                                                                <asp:ImageButton ID="btnEditAddEditProperty" runat="server"  
                                                                    ToolTip="Edit"
                                                                    ImageAlign="Top"
                                                                    ImageUrl="~/Images/edit_icon.png"
                                                                    OnClientClick="return LossDetailsPopUpAddEditProperty(this)"
                                                                />    
                                                                 <asp:ImageButton ID="btnDelete" runat="server" 
                                                                    CommandName="DoDelete"
                                                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this coverage?');"
                                                                    CommandArgument='<%#Eval("LimitID") %>' 
                                                                    ToolTip="Delete" 
                                                                    ImageUrl="~/Images/delete_icon.png"                                                                    
                                                                    />                                                             
                                                            </div>
                                                            <asp:HiddenField id="hdnEditAddEditProperty" runat="server" Value='<%#Eval("LimitID") %>'/>
                                                            </ItemTemplate>
             </asp:TemplateField>
        
    </Columns>    
</asp:GridView>