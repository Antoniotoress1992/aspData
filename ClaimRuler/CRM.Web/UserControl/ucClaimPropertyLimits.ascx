<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimPropertyLimits.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimPropertyLimits" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<div style="width:100%;overflow:auto">
<asp:GridView Width="100%" ID="gvLimits"
    CssClass="gridView no_min_width"
    runat="server"
    AutoGenerateColumns="False"
    CellPadding="2" ShowFooter="true"
    DataKeyNames="PolicyLimitID, LimitID, ClaimLimitID"
    HorizontalAlign="Center"
    OnRowDataBound="gvLimits_RowDataBound">
    <RowStyle HorizontalAlign="Center"  />
    <FooterStyle HorizontalAlign="Center"/>
    <Columns>
        <asp:TemplateField HeaderText="Coverage">
            <ItemTemplate>
                <asp:Label ID="txtLimitLetter" runat="server"  Text='<%# Bind("Limit.LimitLetter") %>' Enabled="false" ></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebTextEditor ID="txtMyCoverage"  runat="server" Width="90%"  ></ig:WebTextEditor>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <asp:Label ID="txtDescription" runat="server" MaxLength="50" Text='<%# Bind("Limit.LimitDescription") %>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate >
                <ig:WebTextEditor ID="txtMyDescription"  runat="server" Width="90%"  ></ig:WebTextEditor>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Limit">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtLimit" runat="server" Width="50px" Value='<%# Bind("LimitAmount") %>' ></ig:WebNumericEditor>
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebNumericEditor ID="txtMyLimit" runat="server"  Width="50px"  ></ig:WebNumericEditor>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Deductible">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtDeductible" runat="server" Width="50px" Value='<%# Bind("LimitDeductible") %>'
                    Visible='<%# Eval("Limit.LimitType").Equals(1) %>' />
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebNumericEditor ID="txtMyDeductible" runat="server"  Width="50px"  ></ig:WebNumericEditor>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Wind/Hail Deductible">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtWHDeductible" runat="server" Width="50px" Value='<%# Bind("WindHailDeductible") %>'
                    Visible='<%# Eval("Limit.LimitType").Equals(1) %>' />
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebNumericEditor ID="txtMyWHDeductible" runat="server"  Width="50px"  ></ig:WebNumericEditor>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="CAT Deductible">
            <ItemTemplate>
                <ig:WebTextEditor ID="txtCATDeductible" runat="server" Width="50px" Value='<%# Bind("CATDeductible") %>'
                    Visible='<%# Eval("Limit.LimitType").Equals(1) %>'>
                </ig:WebTextEditor>
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebTextEditor ID="txtMyCATDeductible" runat="server" Width="50px" Value='<%# Bind("CATDeductible") %>'></ig:WebTextEditor>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Apply To">
            <ItemTemplate>
                <ig:WebDropDown ID="ddlSettlementType" runat="server" Width="100px"  
                    DropDownContainerHeight="100px" Visible='<%# Eval("Limit.LimitType").Equals(1) %>'>
                    <Items>
                        <ig:DropDownItem Text="Select" Value="" />
                        <ig:DropDownItem Value="ACV" Text="ACV" />
                        <ig:DropDownItem Value="RCV" Text="RCV" />
                        <ig:DropDownItem Value="Both" Text="Both" />
                        <ig:DropDownItem Value="None" Text="None" />
                    </Items>
                </ig:WebDropDown>             
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebDropDown ID="ddlMySettlementType" runat="server" Width="100px"  
                    DropDownContainerHeight="100px" >
                    <Items>
                        <ig:DropDownItem Text="Select" Value="" />
                        <ig:DropDownItem Value="ACV" Text="ACV" />
                        <ig:DropDownItem Value="RCV" Text="RCV" />
                        <ig:DropDownItem Value="Both" Text="Both" />
                        <ig:DropDownItem Value="None" Text="None" />
                    </Items>
                </ig:WebDropDown>     
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Coinsurance Limit">
            <ItemTemplate>
                <ig:WebPercentEditor ID="txtCoInsuranceLimit" runat="server" Width="50px" Value='<%# Bind("ConInsuranceLimit") %>'
                    Visible='<%# Eval("Limit.LimitType").Equals(1) %>'>
                </ig:WebPercentEditor>
            </ItemTemplate>
            <FooterTemplate>
                 <ig:WebPercentEditor ID="txtMyCoInsuranceLimit" runat="server" Width="50px" Value='<%# Bind("ConInsuranceLimit") %>'> </ig:WebPercentEditor>
            </FooterTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="ITV">
            <ItemTemplate>
                <ig:WebPercentEditor ID="txtITV" runat="server" Width="50px" Value='<%# Bind("ITV") %>'></ig:WebPercentEditor>
            </ItemTemplate>
             <FooterTemplate>
                  <ig:WebPercentEditor ID="txtMyITV" runat="server" Width="50px" Value='<%# Bind("ITV") %>'></ig:WebPercentEditor>
             </FooterTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Reserve">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtReserve" runat="server" MinDecimalPlaces="2" Width="50px" Value='<%# Bind("Reserve") %>'></ig:WebNumericEditor>
                
            </ItemTemplate>
             <FooterTemplate>
                 <ig:WebPercentEditor ID="txtMyReserve" runat="server" Width="50px" Value='<%# Bind("Reserve") %>'></ig:WebPercentEditor>
             </FooterTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Loss Amount(ACV)">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtLossAmountACV" runat="server" Width="70px" MinDecimalPlaces="2"  Value='<%# Bind("ClaimLimit.LossAmountACV") %>'>
                </ig:WebNumericEditor>
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebNumericEditor ID="txtMyLossAmountACV" Width="70px" runat="server"  MinDecimalPlaces="2"/>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Loss Amount(RCV)">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtLossAmountRCV" runat="server" Width="70px" MinDecimalPlaces="2"  Value='<%# Bind("ClaimLimit.LossAmountRCV") %>'>
                </ig:WebNumericEditor>
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebNumericEditor ID="txtMyLossAmountRCV" Width="70px" runat="server"  MinDecimalPlaces="2"/>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Overage">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtOverage" runat="server" Width="70px" MinDecimalPlaces="2"  Value='<%# Bind("ClaimLimit.OverageAmount") %>'>
                </ig:WebNumericEditor>
            </ItemTemplate>
            <FooterTemplate>
                <ig:WebNumericEditor ID="txtMyOverage" Width="70px" runat="server"  MinDecimalPlaces="2"/>
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
        <asp:TemplateField HeaderText="Type">
            <ItemTemplate>
                <%# Eval("LimitDescription") %>
            </ItemTemplate> 
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Policy Limit">
            <ItemTemplate>
                <%# Eval("LimitAmount") %>
            </ItemTemplate>           
        </asp:TemplateField>
       <%-- <asp:TemplateField HeaderText="Deductible">
            <ItemTemplate>
                <%# Eval("LimitDeductible") %>
            </ItemTemplate>          
        </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Apply To">
            <ItemTemplate>
               <%# Eval("ApplyTo") %>
            </ItemTemplate>           
        </asp:TemplateField>
        <asp:TemplateField HeaderText="ITV">
            <ItemTemplate>
                 <%# Eval("ITV") %>
            </ItemTemplate>           
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Reserve">            
            <ItemTemplate>
                 <%# Eval("Reserve") %>
            </ItemTemplate>           
        </asp:TemplateField> 
         <asp:TemplateField HeaderText="Loss Amount(ACV)">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtLossAmountACV" runat="server" Width="70px" MinDecimalPlaces="2" Value='<%# Bind("LossAmountACV") %>'
                    Visible='<%# !Eval("LimitLetter").Equals("D") %>'>
                    <ClientEvents ValueChanged="calculatePropertyLimitsTotal" />
                </ig:WebNumericEditor>
            </ItemTemplate>
           <%-- <FooterTemplate>
                <ig:WebNumericEditor ID="lblTotalLossAmountACV" runat="server" Enabled="false" CssClass="total" MinDecimalPlaces="2"/>
            </FooterTemplate>--%>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Loss Amount(RCV)">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtLossAmountRCV" runat="server" Width="70px" MinDecimalPlaces="2" Value='<%# Bind("LossAmountRCV") %>'>
                    <ClientEvents ValueChanged="calculatePropertyLimitsTotal" />
                </ig:WebNumericEditor>
            </ItemTemplate>
          <%--  <FooterTemplate>
                <ig:WebNumericEditor ID="lblTotalLossAmountRCV" runat="server" Enabled="false" CssClass="total" MinDecimalPlaces="2"/>
            </FooterTemplate>--%>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Recoverable Depreciation">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtDepreciation" runat="server" Width="70px" MinDecimalPlaces="2" Value='<%# Bind("Depreciation") %>'
                    Visible='<%# !Eval("LimitLetter").Equals("D") %>'>
                    <ClientEvents ValueChanged="calculatePropertyLimitsTotal" />
                </ig:WebNumericEditor>
            </ItemTemplate>
           <%-- <FooterTemplate>
                <ig:WebNumericEditor ID="lblTotalDepreciation" runat="server" Enabled="false" CssClass="total" MinDecimalPlaces="2">                   
                </ig:WebNumericEditor>
            </FooterTemplate>--%>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Non Recoverable Depreciation">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtNonRecoverableDepreciation" runat="server" Width="70px" MinDecimalPlaces="2" Value='<%# Bind("NonRecoverableDepreciation") %>'
                    Visible='<%# !Eval("LimitLetter").Equals("D") %>'>
                    <ClientEvents ValueChanged="calculatePropertyLimitsTotal" />
                </ig:WebNumericEditor>
            </ItemTemplate>
           <%-- <FooterTemplate>
                <ig:WebNumericEditor ID="lblTotalNonRecoverableDepreciation" runat="server" Enabled="false" CssClass="total" MinDecimalPlaces="2">                   
                </ig:WebNumericEditor>
            </FooterTemplate>--%>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Deductible">            
            <ItemTemplate>
                <ig:WebTextEditor ID="lblDeductible" runat="server" Value='<%# Eval("LimitDeductible", "{0:N2}") %>'  CssClass="locked" />
            </ItemTemplate>
           <%-- <FooterTemplate>
                <ig:WebNumericEditor ID="lblTotalDeductible" runat="server" Enabled="false" CssClass="total" MinDecimalPlaces="2"/>
            </FooterTemplate>--%>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="CAT Deductible">
            <ItemTemplate>
                <asp:Label ID="lblCATDeductible" runat="server" Width="70px" Text='<%# Bind("CATDeductible","{0:N2}") %>'  />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Limit">
            <ItemStyle Width="70px" />
            <ItemTemplate>
                <%# Eval("LimitAmount", "{0:N2}") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Overage">
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtOverageAmount" runat="server" Width="70px" Value='<%# Eval("OverageAmount", "{0:N2}") %>' MinDecimalPlaces="2" />
            </ItemTemplate>
        </asp:TemplateField>
       <%-- <asp:TemplateField HeaderText="Type of Settlement">
            <ItemStyle Width="70px" />
            <ItemTemplate>
                <asp:Label ID="txtSettlementType" runat="server" Text='<%# Bind("SettlementType") %>' Visible='<%# Eval("LimitType").Equals(1) %>' />
            </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="CoInsurance Limit">            
            <ItemTemplate>                
                <asp:Label ID="txtCoInsuranceLimit" runat="server" Text='<%# Bind("ConInsuranceLimit", "{0:N2}") %>'
                    Visible='<%# Eval("LimitType").Equals(1) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
    <asp:Label ID="lblNoRecordFound" runat="server" Text="No Records Found" ForeColor="Red"></asp:Label>
</div>

<script type="text/javascript">
   
    function calculatePropertyLimitsTotal(sender, arg) {
        totalGridColumn('txtLossAmountACV', 'lblTotalLossAmountACV');
        totalGridColumn('txtLossAmountRCV', 'lblTotalLossAmountRCV');
        totalGridColumn('txtDepreciation', 'lblTotalDepreciation');
        totalGridColumn('txtNonRecoverableDepreciation', 'lblTotalNonRecoverableDepreciation');
        totalGridColumn('lblDeductible', 'lblTotalDeductible');

        calculateGrossClaimPayable();
    }
    
</script>
