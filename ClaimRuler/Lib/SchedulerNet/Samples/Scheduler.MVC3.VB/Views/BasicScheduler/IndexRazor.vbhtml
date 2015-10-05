@Code
    ViewData("Title") = "IndexRazor"
    Layout = "~/Views/Shared/_SchedulerLayout.vbhtml"
End Code


@Section SampleTitle
    Basic initialization
End Section

@Section TitleContent
    Basic initialization
End Section

@Section ShortDescription
    See how scheduler can be initialized using Razor engine
End Section

@Section LongDescription
    Drag or double-click to create new event
    Double-click on existing events to edit them, or drag them to change their time. 
End Section

@Html.Raw(Model.Render())

