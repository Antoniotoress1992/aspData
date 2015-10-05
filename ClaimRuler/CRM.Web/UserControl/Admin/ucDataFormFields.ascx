<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDataFormFields.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucDataFormFields" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
  <ig:WebDataGrid ID="wdgFields" runat="server" CssClass="gridView smallheader" 
            AutoGenerateColumns="false" Height="300px"
            Width="100%">
            <Columns>
                 <ig:TemplateDataField Key="IsChecked" Header-Text="">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbxSelected" runat="server" Checked='<%# Eval("IsSelected") %>' />
                    </ItemTemplate>
                </ig:TemplateDataField> 
                <ig:BoundDataField DataFieldName="FieldPrompt" Key="FieldPrompt" Header-Text="Field" />                
            </Columns>          
        </ig:WebDataGrid>
