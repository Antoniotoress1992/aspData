<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimSubLimits.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimSubLimits" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:HiddenField ID="hf_SublimitAmounts" runat="server" />
<asp:GridView  ID="gvLimits"
    CssClass="gridView no_min_width"
    runat="server"
    AutoGenerateColumns="False"
    CellPadding="2"
    DataKeyNames="ClaimSubLimitID"
    HorizontalAlign="Center"
    OnRowCommand="gvLimits_RowCommand"
    OnRowDataBound="gvLimits_RowDataBound"
    Width="100%">
    <RowStyle HorizontalAlign="Center" />
    <Columns>       
         <asp:TemplateField HeaderText="Coverage">           
            <ItemTemplate>
               <ig:WebTextEditor ID="txtCoverageLetter" runat="server" Width="50px" CssClass="center" Value='<%# Bind("LimitLetter") %>'>                   
                     <Buttons SpinButtonsDisplay="OnRight" SpinOnArrowKeys="true" ListOfValues="A|B|C|D"
                                            SpinOnReadOnly="true" SpinWrap="true" />
               </ig:WebTextEditor>
            </ItemTemplate>
        </asp:TemplateField>
      <asp:TemplateField HeaderText="Sub-Limit Description">            
            <ItemTemplate>
               <asp:DropDownList ID="ddlSubLimit" runat="server" onchange="setSubLimitAmount(this);" />
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Loss Amount">            
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtSubLimitLossAmount" runat="server" Value='<%# Bind("LossAmount") %>' Width="80px" MinDecimalPlaces="2">
                </ig:WebNumericEditor>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Sub-Limit">            
            <ItemTemplate>
               <ig:WebNumericEditor ID="txtSubLimitAmount" runat="server" MinDecimalPlaces="2"  Width="80px" CssClass="locked"  Value='<%# Eval("PolicySubLimit.Sublimit")%>' />
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="ACV">            
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtACVAmount" runat="server" Value='<%# Bind("ACVAmount") %>' MinDecimalPlaces="2"  Width="80px">
                </ig:WebNumericEditor>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="RCV">            
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtRCVAmount" runat="server" Value='<%# Bind("RCVAmount") %>' MinDecimalPlaces="2"  Width="80px">
                </ig:WebNumericEditor>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Overage">            
            <ItemTemplate>
                <ig:WebNumericEditor ID="txtOverage" runat="server" Value='<%# Bind("OverageAmount") %>' MinDecimalPlaces="2"  Width="80px">
                </ig:WebNumericEditor>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<script type="text/javascript">
    function setSubLimitAmount(ddl) {
        var selectedIndex = ddl.selectedIndex;
        var values = $("#<%= hf_SublimitAmounts.ClientID%>").val().split('|');
        var index = 0;

        if (selectedIndex > 0 && values.length > 0) {
            index = selectedIndex - 1;

            var limitAmount = values[index];
                                  
            var controls = $("input[id$='txtSubLimitAmount']");

            var ctrl_id = controls[index].id;

            $find(ctrl_id).set_value(limitAmount);
        }
    }
</script>