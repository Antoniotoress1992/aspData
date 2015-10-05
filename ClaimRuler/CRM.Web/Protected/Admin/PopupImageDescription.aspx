<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupImageDescription.aspx.cs"
    Inherits="CRM.Web.Protected.Admin.PopupImageDescription" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <link href="../../Css/new.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../Css/ddsmooth.css">
  
     <script type="text/javascript" src="../../pirobox_extended/jquery.min.js"></script>
    <script type="text/javascript" src="../../pirobox_extended/ddpowerzoomer.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function ($) { //fire on DOM ready
            $('#myimage').addpowerzoom({
                magnifiersize: [100, 100] //<--no comma following last option!
            })
        })



  
  
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfLeadsId" runat="server" Value="0" />
    <asp:HiddenField ID="hfLeadImageId" runat="server" Value="0" />
    <asp:HiddenField ID="hfView" runat="server" Value="0" />
    <div>
        <table width="100%">
            <tr>
                <td style="height: 100%;">
                    <div style="width: 100%;">
                        <table border="0" cellspacing="0" cellpadding="0" width="800px" class="new_user"
                            align="center">
                            <tr style="height: 30px">
                                <td align="right" style="width: 35%; vertical-align: top;">
                                    &nbsp;
                                </td>
                                <td align="left" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 35%; vertical-align: top;">
                                    Location of Photo/Damage in Property&nbsp;<span class="redstar">*</span>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtLocation" runat="server" class="myinput1" MaxLength="250" Width="350px"></asp:TextBox>														
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 35%; vertical-align: top; padding-top: 5px;">
                                    Full Description of Photo/Damage&nbsp;<span class="redstar">*</span>
                                </td>
                                <td align="left" valign="top" style="padding-top: 5px;">
                                    <asp:TextBox ID="txtDescription" runat="server" class="myinput1" MaxLength="1000"
                                        TextMode="MultiLine" Height="65px" Width="357px" />
                                    <br />							
                                    &nbsp;&nbsp; <span style="color: Red; font-size: 10.5px;">(Please be as descriptive
                                        as possible)</span>
							<br />							
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Image&nbsp;&nbsp;
                                </td>
                                <td align="left" valign="top">
                                    <div class="example" style="height: 380px; width: 480px; border: 3px solid #184687;
                                        border-radius: 5px;">
                                        <asp:Image ID="myimage" class="imgzoom" runat="server" Width="480px" Height="380px" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnUpload" runat="server" class="mysubmit" Text="Save and Continue"
                                        OnClick="btnUpload_Click" />
                                    &nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" class="mysubmit" Text="Cancel"
                                        OnClick="btnCancel_Click" />
                                       &nbsp;&nbsp; <asp:Button ID="btnRotate" runat="server"  class="mysubmit" Text="Rotate Image" 
                        onclick="btnRotate_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
           <%-- <tr>
                <td>
                    &nbsp;&nbsp;<input id="b1" type="button" value="+ angle" class="plus" />
                    &nbsp;&nbsp;
                    <input id="b2" type="button" value="- angle" class="minus" />
                    &nbsp;
                </td>
            </tr>--%>
        </table>
    </div>
    </form>
</body>
</html>
