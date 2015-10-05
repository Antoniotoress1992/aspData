<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimProgressChart.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimProgressChart" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<script type="text/javascript">

    function HandleFilterClick(me)
    {
        var hiddenElement = document.getElementById('ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_claimProgressChart_hiddenSearchClick');
        hiddenElement.value = "1";

        var carrierid = $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_claimProgressChart_ddlCarrier").val();
        var adjusterid = $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_claimProgressChart_ddlAdjuster").val();

        var hiddenCarrier = document.getElementById('ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_claimProgressChart_hdnCarrier');
        hiddenCarrier.value = carrierid;

        var hiddenAdjuster = document.getElementById('ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_claimProgressChart_hdnAdjuster');
        hiddenAdjuster.value = adjusterid;
    }

</script>

<table >
    <tr>
        <td style="width:150px;text-align:right">
        <div>Carrier</div>
        </td>
        <td>
            
                        <asp:DropDownList ID="ddlCarrier" Width="140px" runat="server" DisplayMode="DropDownList">                           
                        </asp:DropDownList>
        </td>
        <td>
             <div>Adjuster</div>
        </td>
        <td>
         
                        <asp:DropDownList ID="ddlAdjuster" Width="140px" runat="server" DisplayMode="DropDownList">                           
                        </asp:DropDownList>
        </td>
       <td  style="width:200px;text-align:right">
                    <asp:LinkButton ID="lbtrnSearchPanel" Width="100px" runat="server" CssClass="toolbar-item" OnClientClick="return HandleFilterClick(this);"  OnClick="lbtrnSearchPanel_Click">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/toolbar_search.png)">Search</span>
                    </asp:LinkButton>
                    <input type="hidden" id="hiddenSearchClick" runat="server" value="0"  />
                           <input type="hidden" id="hdnCarrier" runat="server" value="0"  />
                           <input type="hidden" id="hdnAdjuster" runat="server" value="0"  />
                </td>
       
    </tr>
</table>

<asp:Chart ID="Chart1" runat="server" BorderColor="26, 59, 105"
    BorderWidth="2"
    BackGradientStyle="TopBottom"
    BackSecondaryColor="White"
    EnableViewState="false"
    Palette="Pastel"
    BorderDashStyle="Solid"
    BackColor="WhiteSmoke"
    ImageType="Png"
    ImageLocation="~/Temp/claimstats"
    Width="500px"
    Height="300px">
    <Legends>
        <asp:Legend Enabled="False" Name="Default" BackColor="Transparent">
            <Position Y="85" Height="5" Width="40" X="5"></Position>
        </asp:Legend>
    </Legends>
    <Series>
        <asp:Series ChartArea="ChartArea1" XValueType="Double" Name="Series2" CustomProperties="DrawingStyle=Cylinder" ShadowColor="Transparent" BorderColor="180, 26, 59, 105" Color="65, 140, 240" Legend="Legend2" YValueType="Double" ChartType="Bar" IsValueShownAsLabel="True" Palette="Pastel">
           <EmptyPointStyle IsValueShownAsLabel="false" IsVisibleInLegend="false" />             
        </asp:Series>
       
        
    </Series>
   
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White" BackColor="Transparent" ShadowColor="Transparent">
            <Area3DStyle PointGapDepth="0" Rotation="5" Perspective="10" Inclination="15" IsRightAngleAxes="False" WallWidth="0" IsClustered="False"></Area3DStyle>
            <AxisY LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisY>
            <AxisX LineColor="64, 64, 64, 64" IsLabelAutoFit="False" Interval="1">
                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisX>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>
