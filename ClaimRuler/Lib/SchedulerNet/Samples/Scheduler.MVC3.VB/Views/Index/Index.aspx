﻿<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
DHTMLX
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
<link href="<%= VirtualPathUtility.ToAbsolute("~/Content/index.css") %>" rel="stylesheet" type="text/css" />
<span style="padding: 27px 0 0 51px;font-family: arial;font-size: 16px;color: white;float: left;">DHTMLX documentation</span>
<span style="display:inline-block;float:right;background:url('<%= VirtualPathUtility.ToAbsolute("~/Content/mvc.png") %>');repeat:none;width:298px;height:78px;"></span>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">

   <ul class="samples_list">
    <li class="category_item"><div><div class="category_icon" style="background-image:url('/Content/Configuration.png')"></div><span>Configuration</span></div>
        <ul>
            <li><div class='sample_link'><%= Html.ActionLink("Basic initialization", "Index", "BasicScheduler", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Basic initialization of the scheduler. Default modes and default views. Basic loading and saving. 
                </div>
            </li>
            <li><div class='sample_link'><%= Html.ActionLink("Skins and languages", "Index", "SkinLocale", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Available skins and languages.</div>
            </li>
            
            <li><div class='sample_link'><%= Html.ActionLink("Basic initialization, Razor", "IndexRazor", "BasicScheduler", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Basic initialization of the scheduler. Using Razor view engine</div>
            </li>
            <li><div class='sample_link'><%= Html.ActionLink("Authorization", "Index", "SchedulerAuthorization", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Different access levels for different users.</div>
            </li>

      
            <li><div class='sample_link'><%= Html.ActionLink("Limit time sections", "Limit", "BasicScheduler", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Block time sections.</div>
            </li>

            <li><div class='sample_link'><%= Html.ActionLink("Color time sections", "MarkedTimeSpans", "BasicScheduler", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Custom marked sections.</div>
            </li>

            <li><div class='sample_link'><%= Html.ActionLink("Highlight pointer", "Highlight", "BasicScheduler", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Highligting scheduler area.</div>
            </li>

            <li><div class='sample_link'><%= Html.ActionLink("Create on click", "HighlightClickCreate", "BasicScheduler", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Create events in highlighted slots.</div>
            </li>

            <li><div class='sample_link'><%= Html.ActionLink("Custom event boxes", "CustomEventBox", "BasicScheduler", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Render custom containers as events.</div>
            </li>

            <li><div class='sample_link'><%= Html.ActionLink("Integrating with google map", "GoogleMap", "DifferentModes", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            See how google map can be integrated in scheduler.</div>
            </li>
            <li><div class='sample_link'><%= Html.ActionLink("Year view", "YearNWeek", "DifferentModes", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Scheduler year mode.</div>
            </li>
            <li><div class='sample_link'><%= Html.ActionLink("Recurring events", "Index", "RecurringEvents", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Enabling recurring events support.</div>
            </li>
            <li><div class='sample_link'><%= Html.ActionLink("Multiple Resources", "MultipleResources", "DifferentModes", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Loading data from different resources.</div>
            </li>
            <li><div class='sample_link'><%= Html.ActionLink("Table view", "Index", "GridView", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Grid view can be used to display events as a table.</div>
            </li>
            <li><div class='sample_link'><%= Html.ActionLink("Custom view", "Index", "CustomView", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Extend scheduler by creating new views.</div>
            </li>
			<li><div class='sample_link'><%= Html.ActionLink("Customize time scale", "Index", "CustomScale", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Remove hours and days from time scale.</div>
            </li>
            <li><div class='sample_link'><%= Html.ActionLink("Multiple Schedulers", "Index", "MultiScheduler", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
            Create multiple schedulers on single page.</div>
            </li>
        </ul>
    </li>
    <li class='category_item'><div><div class="category_icon" style="background-image:url('/Content/Data-operations.png')"></div><span>Data operations</span></div>
        <ul>
           <li><div class='sample_link'><%= Html.ActionLink("Add range", "Index", "AddRange", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">
           Adding an Items collection  to lightbox controls.
           </div>
           </li>
           <li><div class='sample_link'><%= Html.ActionLink("Static loading", "Index", "StaticLoading", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Loading all data to the page, without Ajax.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink("Custom Field", "Index", "CustomField", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Updating events from the server-side after saving.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink("Client-side filtering", "Index", "ClientSideFiltering", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Filtering events on the client-side.</div></li>

        </ul>
    </li>    
    <li class='category_item'><div><div class="category_icon" style="background-image:url('/Content/Lightbox.png')"></div><span>Details form customization</span></div>
        <ul>
           <li><div class='sample_link'><%= Html.ActionLink("Custom details form", "Details", "AddRange", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Configure details form.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink("'Wide' form", "Wide", "AddRange", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Additional form render.</div></li>

           <li><div class='sample_link'><%= Html.ActionLink("Custom form in lightbox", "Index", "PartialViewInLightbox", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Using a custom form instead of the native lightbox.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink(".Net form in lightbox", "Index", "MVCFormInLightbox", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Using a .Net form instead of the native lightbox.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink(".Net form, JQuery validation", "Index", "JQueryValidation", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Using unobtrusive validation with custom .Net form.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink("Quick Info", "Index", "QuickInfo", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">The extension allows you to replace standard sidebar buttons with new ones, bigger and handier.</div></li>

        </ul>
    </li>     
    <li class='category_item'><div><div class="category_icon" style="background-image:url('/Content/Integration-w-e-c.png')"></div><span>Integration with external controls</span></div>
        <ul>
           <li><div class='sample_link'><%= Html.ActionLink("Server side filtering", "Index", "ServerSideFiltering", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Using native .Net controls within the scheduler.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink("Server side filtering ajax", "Index", "ServerSideFilteringAjax", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Reloading the scheduler by Ajax as a reaction on some external input.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink("Google Calendar", "Index", "GoogleCalendar", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Using google calendar as a data source.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink("Mixed data", "MixedContent", "GoogleCalendar", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Display data from local database and from Google calendar.</div></li>

        </ul>
    </li>    
    <li class='category_item'><div><div class="category_icon" style="background-image:url('/Content/Export.png')"></div><span>Data export</span></div>
        <ul>
           <li><div class='sample_link'><%= Html.ActionLink("Export to PDF", "Index", "ExportToPDF", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Creating PDF documents from a scheduler view.</div></li>
           <li><div class='sample_link'><%= Html.ActionLink("Export to iCal", "Index", "ExportToICal", New With {.id = ""}, New With {.target = "_blank"})%></div><div class="sample_description">Exporting the scheduler to iCal.</div></li>
        </ul>
    </li>     
        
   </ul>
  
   <div class="footer">
   <div style="*zoom:1;*display:inline;display:inline-block;repeat:none;height:14px;width:852px;background:url('/Content/footer.png')"></div>
   <div style="float:bottom;padding:5px 62px;text-align:left;">
   <p>Scheduler .Net is the library that have released to help .NET developers initialize and configure dhtmlxScheduler on server-side backend without dealing with client-side at all. It's fully written in C# and mostly intended for .NET MVC applications.</p>


    <p>
    <strong>First of all, before you start developing, make sure you read the following important chapters:</strong>
    </p>
    <ul>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/introduction_to_scheduler_.net.html" class="wikilink1" title="introduction_to_scheduler_.net">Introduction to Scheduler .Net</a> - gives an introduction to the library, providing general information.</div>
    </li>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/technical_requirements.html" class="wikilink1" title="technical_requirements">Technical Requirements</a> - acquaints you with the requirements of the library.</div>
    </li>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/downloading_the_package.html" class="wikilink1" title="downloading_the_package">Downloading the package</a> - tells you what files and where you should download to use the library.</div>
    </li>
    </ul>

    <p>
    <strong>Confused where to start? Here is a link to get you on your way:</strong>
    </p>
    <ul>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/simple_.net_application_with_scheduler.html" class="wikilink1" title="simple_.net_application_with_scheduler">Simple .Net application with scheduler</a> - gives essential notes you should know to create the first scheduler-related .NET application. Read this guide before coding your program.</div>
    </li>
    </ul>

    <p>

    <strong>Creating a simple scheduler doesn&#039;t cause any problems and you are ready to complicate and customize the app. Here are links to task-oriented chapters:</strong>
    </p>
    <ul>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/loading_data.html" class="wikilink1" title="loading_data">Loading data</a>  - says how to load events to the scheduler.</div>
    </li>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/managing_crud_operations.html" class="wikilink1" title="managing_crud_operations">Managing CRUD operations</a> - provides information that helps you define CRUD logic. Read this when you&#039;re going to create and delete events, save changes made in them.</div>
    </li>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/fields_and_views.html" class="wikilink1" title="fields_and_views">Views</a> - covers views supported by the library (e.g. Timeline, Day, Month views). Also it contains information that helps you add and customize a view.</div>
    </li>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/lightbox.html" class="wikilink1" title="lightbox">Lightbox</a> - contains information concerning the lightbox (details form): how to add a field, create custom form and etc.</div>
    </li>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/data_collections.html" class="wikilink1" title="data_collections">Data collections</a> - describes data collections in the context of static loading the initial data.</div>
    </li>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/identifying_users._autorization.html" class="wikilink1" title="identifying_users._autorization">Identifying users. Autorization</a> - says how to secure your application and differentiate permission levels for users.</div>
    </li>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/filtration.html" class="wikilink1" title="filtration">Filtration</a> - learns how to filter events you load to the scheduler.</div>
    </li>
    <li class="level1"><div class="li"> <a href="http://scheduler-net.com/docs/data_export.html" class="wikilink1" title="data_export">Data export</a>  - provides general guidelines about exporting data.</div>
    </li>
    </ul>

    <p>
    <strong>Can&#039;t find the desired information? Feel free to send us your questions, suggestions or comments to:</strong>
    </p>
    <ul>
    <li class="level1"><div class="li"> Support forum available at <a href="http://forum.dhtmlx.com" class="urlextern" title="http://forum.dhtmlx.com"  rel="nofollow">http://forum.dhtmlx.com</a> ( if you have a commercial license , there is also a support system at <a href="http://support.dhtmlx.com" class="urlextern" title="http://support.dhtmlx.com"  rel="nofollow">http://support.dhtmlx.com</a> )</div>
    </li>
    <li class="level2"><div class="li"> Email - <em class="u">support@dhtmlx.com</em>.</div>
    </li>
    </ul>


   </div>
</div>
</asp:Content>

