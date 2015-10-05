<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage<SchedulerTest.Controllers.SkinLocaleController.LocaleData>" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Skins and languages
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Available skins and languages
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    There are three skins (‘standart’, ‘glossy’ and ‘terrace’) and 25 languages available.<br />
    Select skin and language and press ‘Apply’
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   <form method="get" action="/SkinLocale" style="text-align:right" class="contenttext">

Skin:<select id="skin_select" name="skin" >
        <option value="classic" id="classic">Standart</option>
        <option value="glossy" id="glossy">Glossy</option>
        <option value="terrace" id="terrace">Terrace</option>
     </select>&nbsp;&nbsp;
Language:<select id="language_select" name="language">
        <option value="en" id="en">English</option>
        <option value="ar" id="ar">Arabic</option>
        <option value="be" id="be">Belarusian</option>
        <option value="ca" id="ca">Catalan</option>
        <option value="cn" id="cn">Chinese</option>
        <option value="cs" id="cs">Czech</option>
        <option value="da" id="da">Danish</option>
        <option value="nl" id="nl">Dutch</option>
        <option value="fi" id="fi">Finnish</option>
        <option value="fr" id="fr">French</option>
        <option value="de" id="de">German</option>
        <option value="el" id="el">Greek</option>
        <option value="he" id="he">Hebrew</option>
        <option value="hu" id="hu">Hungarian</option>
        <option value="id" id="id">Indonesia</option>
        <option value="it" id="it">Italian</option>
        <option value="jp" id="jp">Japanese</option>
        <option value="no" id="no">Norwegian</option>
        <option value="pl" id="pl">Polish</option>
        <option value="pt" id="pt">Portuguese</option>
        <option value="ro" id="ro">Romanian</option>
        <option value="ru" id="ru">Russian</option>
        <option value="si" id="si">Slovenian</option>
        <option value="es" id="es">Spanish</option>
        <option value="sv" id="sv">Swedish</option>
        <option value="tr" id="tr">Turkish</option>
        <option value="ua" id="ua">Ukrainian</option>
</select>&nbsp;&nbsp;

<a style="cursor:pointer;color:#1F8192;text-decoration:underline;margin-right:15px;" onclick="this.parentNode.submit()">Apply</a>

</form>
<script type="text/javascript">
    document.getElementById("<%= Model.skin%>").selected = "selected";
    document.getElementById("<%= Model.locale%>").selected = "selected";
</script>

    <%= Model.scheduler.Render() %> 

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Available skins
</asp:Content>
