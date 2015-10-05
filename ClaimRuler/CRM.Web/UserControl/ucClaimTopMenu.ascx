<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimTopMenu.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimTopMenu" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<ig:WebDataMenu ID="topMenu" runat="server" 
    ActivateOnHover="true" 
    OnItemClick="WebDataMenu1_ItemClick" 
    StyleSetName="Default"
    Width="100%" 
    >
    <GroupSettings Orientation="Horizontal" ExpandDirection="Down"  />
    <Items>
        <ig:DataMenuItem Text="Email">
            <Items>
                <ig:DataMenuItem Text="Send Email" ImageUrl="~/Images/email_edit.png" NavigateUrl="~/Protected/LeadEmail.aspx">
                </ig:DataMenuItem>
                <ig:DataMenuItem Text="Import Email" ImageUrl="~/Images/email_import.png" NavigateUrl="~/Protected/LeadImportEmail.aspx">
                </ig:DataMenuItem>
            </Items>
        </ig:DataMenuItem>
        <ig:DataMenuItem Text="Photos">
            <Items>
                <ig:DataMenuItem Text="Upload Photos" ImageUrl="~/Images/photo_add.gif" NavigateUrl="~/protected/LeadsImagesUpload.aspx">
                </ig:DataMenuItem>
                <ig:DataMenuItem Text="Print Report" ImageUrl="~/Images/email_import.png"  Value="2">
                </ig:DataMenuItem>
            </Items>
        </ig:DataMenuItem>
        <ig:DataMenuItem Text="Letters" Key="Letters">
            <Items>
                <ig:DataMenuItem Text="Print Letters" ImageUrl="~/Images/letter.png" NavigateUrl="~/Protected/MergeTemplateLetter.aspx">                  
                </ig:DataMenuItem>
                <ig:DataMenuItem IsSeparator="true" />
                <ig:DataMenuItem Text="How to Import Your Letters" ImageUrl="~/Images/help2.png" NavigateUrl="~/Content/ClaimRulerMergeMailInstructions.pdf" Target="blank">
                </ig:DataMenuItem>
            </Items>
        </ig:DataMenuItem>
        <ig:DataMenuItem Text="Accounting">
            <Items>
                <ig:DataMenuItem Text="Prepare Invoice" ImageUrl="~/Images/invoice.png" NavigateUrl="~/Protected/LeadInvoice.aspx">
                </ig:DataMenuItem>
                 <ig:DataMenuItem Text="Invoice Time & Expense" ImageUrl="~/Images/invoice.png" NavigateUrl="~/Protected/InvoiceTimeExpense.aspx">
                </ig:DataMenuItem>                          
            </Items>
        </ig:DataMenuItem>
    </Items>
</ig:WebDataMenu>
