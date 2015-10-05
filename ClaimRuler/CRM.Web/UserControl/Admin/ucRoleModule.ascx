<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRoleModule.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucRoleModule" %>
<script type="text/javascript">
    checked = false;
    function checkedAll1() {
        if (checked == false) { checked = true } else { checked = false }
        for (var i = 0; i < document.forms[0].elements.length; i++) {
            document.forms[0].elements[i].checked = checked;
        }
    }
    function checkedAll(obj) {
        var objId = obj.id;
        var grd = document.getElementById("<%= grdModule.ClientID %>");
        if (checked == false) { checked = true } else { checked = false }
        var chk = grd.getElementsByTagName("input");
        var subgrd = grd.getElementsByTagName("gvSubModule");
        //alert(chkArray.length);
        alert(subgrd.id);
        for (i = 0; i <= chk.length - 1; i++) {
            if (chk[i].type == 'checkbox') {
                if (chk[i].id != objId) {
                    chk[i].checked = checked;
                }
            }
        }
    }
    function SelectAllAdd(grd, chkbox, rows, rowIndex) {
        var ddlvalue = document.getElementById("<%= grdModule.ClientID %>").rows[rowIndex].cells[0].getElementsByTagName('INPUT')[0];

        var chkAdd = chkbox.checked;

        var chkID = chkbox.id;

        var chk = grd.getElementsByTagName('input');

        if (Number(rows) > 0)
            for (var i = 0; i < rows; i++) {
                var inputAdd = document.getElementById(grd.id + '_chkAdd_' + String(i));
                if (inputAdd.disabled != true) {
                    inputAdd.checked = chkAdd;
                }

            }

        if (Number(rows) > 1) {
            var gridview = document.getElementById('content2_ContentPlaceHolderMiddArea_ucRoleModule1_grdModule_gvSubModule_3_gvSubModule1_8');
            if (gridview != null) {
                var rowcount = gridview.rows.length;
                if (Number(rowcount) > 0)
                    for (var i = 0; i < rowcount; i++) {
                        var View = document.getElementById(gridview.id + '_chkAdd_' + String(i));
                        View.checked = chkAdd;
                    }

            }
        }
    }
    function SelectAllEdit(grd, chkbox, rows) {
        var chkEdit = chkbox.checked;
        //alert(test);
        if (Number(rows) > 0)
            for (var i = 0; i < rows; i++) {
                var inputEdit = document.getElementById(grd.id + '_chkEdit_' + String(i));
                if (inputEdit.disabled != true) {
                    inputEdit.checked = chkEdit;
                }
            }
        if (Number(rows) > 1) {
            var gridview = document.getElementById('content2_ContentPlaceHolderMiddArea_ucRoleModule1_grdModule_gvSubModule_3_gvSubModule1_8');
            if (gridview != null) {
                var rowcount = gridview.rows.length;
                if (Number(rowcount) > 0)
                    for (var i = 0; i < rowcount; i++) {
                        var View = document.getElementById(gridview.id + '_chkEdit_' + String(i));
                        View.checked = chkEdit;
                    }

            }
        }
    }
    function SelectAllDelete(grd, chkbox, rows) {
        var chkDel = chkbox.checked;
        if (Number(rows) > 0)
            for (var i = 0; i < rows; i++) {
                var inputDel = document.getElementById(grd.id + '_chkDel_' + String(i));
                if (inputDel.disabled != true) {
                    inputDel.checked = chkDel;
                }
            }
        if (Number(rows) > 1) {
            var gv = document.getElementById('content2_ContentPlaceHolderMiddArea_ucRoleModule1_grdModule_gvSubModule_3_gvSubModule1_8');
            if (gv != null) {
                var rowcnt = gv.rows.length;
                if (Number(rowcnt) > 0)
                    for (var i = 0; i < rowcnt; i++) {
                        var View = document.getElementById(gv.id + '_chkDel_' + String(i));
                        View.checked = chkDel;
                    }

            }
        }
    }
    function SelectAllView(grd, chkbox, rows) {

        var chkView = chkbox.checked;
        if (Number(rows) > 0)
            for (var i = 0; i < rows; i++) {
                var inputView = document.getElementById(grd.id + '_chkView_' + String(i));
                inputView.checked = chkView;
            }
        if (Number(rows) > 1) {
            var gridview = document.getElementById('content2_ContentPlaceHolderMiddArea_ucRoleModule1_grdModule_gvSubModule_3_gvSubModule1_8');
            if (gridview != null) {
                var rowcount = gridview.rows.length;
                if (Number(rowcount) > 0)
                    for (var i = 0; i < rowcount; i++) {
                        var View = document.getElementById(gridview.id + '_chkView_' + String(i));
                        View.checked = chkView;
                    }

            }
        }
    }



</script>
<style type="text/css">
    .hdchk {
        float: right;
        display: inline-table;
        table-layout: fixed;
    }

    .rmchk, .hdlstchk, .rmlstchk {
        display: table-cell;
        width: 70px;
        text-align: center;
    }

    .hdlstchk {
        padding-right: -1px;
    }

    .rmlstchk {
        padding-right: 20px;
    }
</style>


<div class="paneContentInner">
    <div class="message_area">
        <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
        <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
    </div>
    <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
        <tr>
            <td class="left" style="width: 20%">Select Role
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="DDLStyles" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                &nbsp;
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" Visible="false"
                    OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>


    <asp:Panel ID="pnlList" runat="server" Visible="false">

        <div class="vendor_list">
            <asp:GridView ID="grdModule" runat="server" AutoGenerateColumns="False" Width="100%"
                EmptyDataText="No Record Found" DataKeyNames="RoleModuleId" ShowHeader="true"
                OnRowDataBound="grdModule_RowDataBound" GridLines="None" HorizontalAlign="Center"
                RowStyle-BackColor="#d6e6fb" RowStyle-VerticalAlign="Top">
                <HeaderStyle HorizontalAlign="left" />
                <Columns>
                    <asp:TemplateField HeaderText="Module">
                        <ItemStyle Width="300px" />
                        <ItemTemplate>
                            <asp:HiddenField ID="hfModuleId" runat="server" Value='<%# Eval("ModuleId") %>' />
                            <asp:HiddenField ID="hfParentId" runat="server" Value='<%# Eval("ParentId") %>' />
                            <asp:HiddenField ID="hfModuleType" runat="server" Value='<%# Eval("ModuleType") %>' />
                            <asp:HiddenField ID="hfRoleModuleId" runat="server" Value='<%# Eval("RoleModuleId") %>' />

                            <asp:Label ID="lblModule" runat="server" Font-Bold="True" Text='<%# Eval("ModuleName") %>' CssClass="ab" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="View">
                         <ItemStyle Width="100px" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkAllView" Checked='<%#Convert.ToBoolean(Eval("ViewPermssion")) == null ? false : Convert.ToBoolean(Eval("ViewPermssion"))%>'
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Has Add">
                        <ItemStyle Width="100px" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkAllAdd" Checked='<%#Convert.ToBoolean(Eval("AddPermssion")) == null ? false : Convert.ToBoolean(Eval("AddPermssion"))%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Has Edit">
                        <ItemStyle Width="100px" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkAllEdit" Checked='<%#Convert.ToBoolean(Eval("EditPermission")) == null ? false : Convert.ToBoolean(Eval("EditPermission"))%>'
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Has Delete">
                        <ItemStyle Width="100px" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkAllDel" Checked='<%#Convert.ToBoolean(Eval("DeletePermission")) == null ? false : Convert.ToBoolean(Eval("DeletePermission"))%>'
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            </td></tr>
                            <tr>
                                <td colspan="5">
                            <asp:GridView ID="gvSubModule" runat="server" AutoGenerateColumns="False" Width="100%"
                                OnRowDataBound="grdSubModule_RowDataBound" ShowHeader="false" GridLines="None"
                                AlternatingRowStyle-BackColor="#eff6ff" HorizontalAlign="Center">
                                  <HeaderStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Module">
                                        <ItemStyle Width="300px" />
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hfModuleIdChild" runat="server" Value='<%# Eval("ModuleId") %>' />
                                            <asp:Label ID="lblURL" runat="server" Text='<%# Eval("ModuleName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemStyle Width="100px" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkView" runat="server" Checked='<%#Convert.ToBoolean(Eval("ViewPermssion")) == null ? false : Convert.ToBoolean(Eval("ViewPermssion"))%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add">
                                        <ItemStyle Width="100px" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAdd" runat="server" Checked='<%#Convert.ToBoolean(Eval("AddPermssion")) == null ? false : Convert.ToBoolean(Eval("AddPermssion"))%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemStyle Width="100px" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkEdit" runat="server" Checked='<%#Convert.ToBoolean(Eval("EditPermission")) == null ? false : Convert.ToBoolean(Eval("EditPermission"))%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemStyle Width="100px" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDel" runat="server" Checked='<%#Convert.ToBoolean(Eval("DeletePermission")) == null ? false : Convert.ToBoolean(Eval("DeletePermission"))%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                               </td></tr>
                                                <tr>
                                                    <td colspan="5">
                                            <asp:GridView ID="gvSubModule1" runat="server" AutoGenerateColumns="False" Width="100%"
                                                ShowHeader="false" GridLines="None" AlternatingRowStyle-BackColor="#eff6ff">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Module">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hfModuleIdChild1" runat="server" Value='<%# Eval("ModuleId") %>' />
                                                            <asp:Label ID="lblURL" runat="server" Text='<%# Eval("ModuleName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkView" runat="server" Checked='<%#Convert.ToBoolean(Eval("ViewPermssion")) == null ? false : Convert.ToBoolean(Eval("ViewPermssion"))%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Add">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkAdd" runat="server" Checked='<%#Convert.ToBoolean(Eval("AddPermssion")) == null ? false : Convert.ToBoolean(Eval("AddPermssion"))%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkEdit" runat="server" Checked='<%#Convert.ToBoolean(Eval("EditPermission")) == null ? false : Convert.ToBoolean(Eval("EditPermission"))%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkDel" runat="server" Checked='<%#Convert.ToBoolean(Eval("DeletePermission")) == null ? false : Convert.ToBoolean(Eval("DeletePermission"))%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>


                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</div>
