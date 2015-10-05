<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUserDetail.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucUserDetail" %>
<div id="right_contant_area">
    <div class="all_box">
        <div class="box1">
            <div class="innerbox1">
                <h1>
                    User Details</h1>
                <div class="form_area_shw">
                    <h3>Basic</h3>
                </div>
                <div class="form_area_shw">
                    <label>User Name :</label>
                    <asp:Label runat="server" ID="lblUserName"/>
                </div>
                <div class="form_area_shw">
                    <label>First Name :</label>
                    <asp:Label runat="server" ID="lblFitstName" />
                </div>
                <div class="form_area_shw">
                    Last Name :
                    <asp:Label runat="server" ID="lblLastName" />
                </div>
                <div class="form_area_shw">
                    Role Name :
                    <asp:Label runat="server" ID="lblRoleName" />
                </div>
                <div class="form_area_shw">
                    Status :
                    <asp:Label runat="server" ID="lblStatus" />
                </div>
                <div class="form_area_shw">
                    Blocked :
                    <asp:Label runat="server" ID="lblBlocked" />
                </div>
             
                </div>
            </div>
        </div>
    </div>
