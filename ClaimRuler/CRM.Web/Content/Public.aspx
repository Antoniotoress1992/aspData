<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Public.aspx.cs" Inherits="CRM.Web.Content.Public" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Claim Ruler - Industrial Strength Property Claim Management Software</title>
    <link type="text/css" rel="stylesheet" runat="server" href="~/Css/ClaimRuler.css" />
    <link type="text/css" rel="Stylesheet" runat="server" href="~/Css/cupertino/jquery-ui-1.9.2.custom.css" />

    <link rel="shortcut icon" href="/favicon.ico" />
    <link rel="icon" href="favicon.ico" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 70%; margin: auto;">
            <div class="paneContentInner">
                <div style="margin: 8px 8px 8px 15px;">
                    <asp:Image ID="logo" runat="server" ImageUrl="~/Images/claim_ruler_logo.png" Width="70" />
                </div>
                <div class="page-title">
                    Shared Documents 
                </div>
                <h2>
                    <asp:Label ID="lblName" runat="server" />
                </h2>
                <div style="height: 600px; overflow: auto;">
                    <asp:GridView ID="gvDocument" runat="server" AutoGenerateColumns="false" CssClass="gridView" ShowHeader="true" RowStyle-BackColor="White" AlternatingRowStyle-BackColor="#e8f2ff"
                        OnRowDataBound="gvDocument_RowDataBound" HorizontalAlign="Center" CellPadding="2" Width="100%" GridLines="Both">
                        <Columns>

                            <asp:TemplateField>
                                <ItemStyle Width="32px" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Image ID="imgDocumentType" runat="server" Width="24px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Document Name">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkDocument" runat="server" Target="_blank" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <%# Eval("Description") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="myfooter" style="text-align: center; margin-top:20px;">
                                ©
	            <script type="text/javascript">

	                now = new Date
	                theYear = now.getYear()
	                if (theYear < 1900)
	                    theYear = theYear + 1900
	                document.write(theYear)
                </script>
                                <a href="http://www.itstrategiesgroup.com" target="_blank">IT Strategies Group</a>
                                · <a href="http://app.claimruler.com/terms.pdf" target="_blank">Terms of Use</a> · <a href="http://www.itstrategiesgroup.com/contact"
                                    target="_blank">Contact Us</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
