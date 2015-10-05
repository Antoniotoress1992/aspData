<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPolicyList.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucPolicyList1" %>
<%@ Register Src="~/UserControl/Admin/ucClaimList.ascx" TagName="ucClaimList" TagPrefix="uc1" %>
<asp:GridView ID="gvPolicyList"
    CssClass="gridView"
    runat="server"
    AutoGenerateColumns="False"
    AlternatingRowStyle-BackColor="#e8f2ff"
    AllowPaging="true"
    CellPadding="4"
    DataSourceID="edsPolicy"
    OnRowCommand="gvPolicyList_RowCommand"
    OnRowDataBound="gvPolicyList_RowDataBound"
    Width="100%">

    <RowStyle HorizontalAlign="Center" />
    <HeaderStyle HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White"></SelectedRowStyle>
    <Columns>
        <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
            <ItemTemplate>
                <asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" 
                    CommandArgument='<%#Eval("Id") %>'
                    ToolTip="Edit" 
                    ImageUrl="~/Images/edit_icon.png" 
                    Visible='<%# CRM.Core.PermissionHelper.checkEditPermission("UsersLeads.aspx") %>'
                    />
                &nbsp;
                 <asp:ImageButton ID="btnView" runat="server" CommandName="DoView" 
                    CommandArgument='<%#Eval("Id") %>'
                    ToolTip="View" 
                    ImageUrl="~/Images/view_icon.png" 
                    Visible='<%# CRM.Core.PermissionHelper.checkViewPermission("UsersLeads.aspx") %>'
                    />
                &nbsp;
				<asp:ImageButton ID="btnDelete" runat="server" 
                    CommandName="DoDelete"
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this policy?');"
                    CommandArgument='<%#Eval("Id") %>' 
                    ToolTip="Delete" 
                    ImageUrl="~/Images/delete_icon.png" 
                    Visible='<%# CRM.Core.PermissionHelper.checkDeletePermission("UsersLeads.aspx")  %>'
                    />
                &nbsp;
                
                    <a href="javascript:ExpandCollapse('div<%# Eval("Id") %>');" style="padding-left: 2px;" visible='<%# Eval("Claim") != null %>' >
                        <img id="imgdiv<%# Eval("Id") %>" alt="" style="width: 9px;" border="0" src="../images/plus.gif" title="Show/Hide Claims" />
                    </a>
               
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Policy Type" SortExpression="PolicyType.TypeDescription">
            <ItemTemplate>
                <%#Eval("LeadPolicyType.Description")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Carrier Name" SortExpression="Carrier.CarrierName">
            <ItemTemplate>
                <%#Eval("Carrier.CarrierName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Policy Number" SortExpression="PolicyNumber">
            <ItemTemplate>
                <%#Eval("PolicyNumber")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Effective Date">
            <ItemTemplate>
                <%#Eval("EffectiveDate")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Expiry Date">
            <ItemTemplate>
                <%#Eval("ExpirationDate")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle CssClass="InvisibleCol" />
            <ItemStyle CssClass="InvisibleCol" />
            <ItemTemplate>
                </td></tr>
                <tr>
                    <td colspan="6">
                        <div id="div<%# Eval("Id") %>" style="display: none; position: relative; left: 10px; overflow: auto; width: 99%;">
                            <uc1:ucClaimList ID="claimList" runat="server" />
                        </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>

</asp:GridView>
<asp:EntityDataSource ID="edsPolicy" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
    EnableFlattening="False" EntitySetName="LeadPolicy"
    Include="LeadPolicyType, Carrier"
    Where="it.LeadId = @LeadID && it.IsActive = true">
    <WhereParameters>
        <asp:SessionParameter Name="LeadID" SessionField="LeadIds" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<script type="text/javascript">
    function ExpandCollapse(obj) {
        var div = document.getElementById(obj);
        var img = document.getElementById('img' + obj);

        if (div) {
            if (div.style.display == "none") {
                div.style.display = "block";
                img.src = "../images/minus.gif";

            }
            else {
                div.style.display = "none";
                img.src = "../images/plus.gif";
            }
        }
    }

</script>
