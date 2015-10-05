<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimCasualtyLimits.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimCasualtyLimits" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:GridView ID="gvLimits"
    CssClass="gridView no_min_width"
    runat="server"
    AutoGenerateColumns="False"
    CellPadding="2"
    DataKeyNames="ClaimLimitID, LimitID"
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
            <FooterTemplate>
                <b>Total</b>
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Loss Amount">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtLossAmount" runat="server" Width="70px" MinDecimalPlaces="2" Value='<%# Bind("LossAmountACV") %>'>
                      <ClientEvents ValueChanged="calculateCasualtyLimitsTotal" />
                </ig:WebNumericEditor>
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebNumericEditor ID="lblCasualtyTotalLossAmount" runat="server" Enabled="false" CssClass="total" MinDecimalPlaces="2" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Limit">
            <ItemStyle Width="70px" />
            <ItemTemplate>
                 <ig:WebTextEditor ID="lblCasualtyLimitAmount" runat="server" Value='<%# Eval("LimitAmount", "{0:N2}") %>' CssClass="locked" />
            </ItemTemplate>
             <FooterTemplate>
                <ig:WebNumericEditor ID="lblCasualtyTotalLimitAmount" runat="server" Enabled="false" CssClass="total" MinDecimalPlaces="2"/>
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:GridView ID="gvLimits2"
    CssClass="gridView no_min_width"
    runat="server"
    AutoGenerateColumns="False"
    CellPadding="2"
    DataKeyNames="ClaimLimitID, LimitID"
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
            <FooterTemplate>
                <b>Total</b>
            </FooterTemplate>
            <FooterStyle HorizontalAlign="Center" />
        </asp:TemplateField>
       <asp:TemplateField HeaderText="Policy Limit">
            <ItemTemplate>
                <%# Eval("LimitAmount") %>
            </ItemTemplate>           
        </asp:TemplateField>
        
    </Columns>    
</asp:GridView>
<asp:Label ID="lblNoRecordFound" runat="server" Text="No Records Found" ForeColor="Red"></asp:Label>

<script type="text/javascript">

    function calculateCasualtyLimitsTotal(sender, arg) {
        totalGridColumn('txtLossAmount', 'lblCasualtyTotalLossAmount');
        totalGridColumn('lblCasualtyLimitAmount', 'lblCasualtyTotalLimitAmount');
    }

</script>
