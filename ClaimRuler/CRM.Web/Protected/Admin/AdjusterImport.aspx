<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="AdjusterImport.aspx.cs" Inherits="CRM.Web.Protected.Admin.AdjusterImport" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Import Adjusters
        </div>
        <div class="message_area">
            <asp:Label ID="lblMessage" runat="server"  />
        </div>
        <div class="paneContentInner">
            <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0">
                <tr>
                    <td colspan="2" class="left top" style="height: 40px;">
                        <div style="margin: 10px 0px 10px 0px;">
                            <p>
                                Please follow these steps to import your data:<br />
                                <br />
                            </p>

                            <ul>
                                <li>1. <a href="../../Content/New_Import_Template.xls">Download Import Template.</a></li>
                                <li>2. Fill in your data.</li>
                                <li>3. Save spreadsheet as CSV(Comma delimited).</li>
                                <li>4. Choose file saved in Step 3.</li>
                                <li>5. Click Upload button.</li>
                            </ul>

                        </div>

                    </td>
                </tr>
                <tr>

                    <td colspan="2" class="center top">&nbsp;&nbsp;<asp:FileUpload ID="fileUpload" runat="server" ValidationGroup="adjuster" />
                        <div>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="validation1"
                                ID="RequiredFieldValidator2" runat="server" ErrorMessage="You have not uploaded your file"
                                ValidationGroup="ImgUpload" ControlToValidate="fileUpload"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="center top">
                        <br />
                        <br />
                        <asp:Button ID="btnUpload" runat="server" ValidationGroup="adjuster" class="mysubmit" CausesValidation="true"
                            Text="Import" OnClick="btnUpload_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>
