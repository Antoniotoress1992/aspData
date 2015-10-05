using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Common;
using DHXScheduler_MVC4.Models;

namespace SchedulerTest.Controllers
{

    /// <summary>
    /// check available skins and localizations
    /// </summary>
    public class SkinLocaleController : Controller
    {
        //
        // GET: /SkinLocale/
        public class LocaleData
        {
            public DHXScheduler scheduler;
            public string locale;
            public string skin;
            public LocaleData(DHXScheduler sched, string loc, string sk)
            {
                scheduler = sched;
                locale = loc;
                skin = sk;
            }
        }
        public ActionResult Index()
        {
            SchedulerLocalization.Localizations lang;
            var language = this.Request.QueryString["language"];
            var skn = this.Request.QueryString["skin"];
            DHXScheduler.Skins skin;
            #region language
            switch (this.Request.QueryString["language"])
            {
                case "ar":
                    lang = SchedulerLocalization.Localizations.Arabic;
                    break;
                case "be":
                    lang = SchedulerLocalization.Localizations.Belarusian;
                    break;
                case "ca":
                    lang = SchedulerLocalization.Localizations.Catalan;
                    break;
                case "cn":
                    lang = SchedulerLocalization.Localizations.Chinese;
                    break;
                case "cs":
                    lang = SchedulerLocalization.Localizations.Czech;
                    break;
                case "da":
                    lang = SchedulerLocalization.Localizations.Danish;
                    break;
                case "nl":
                    lang = SchedulerLocalization.Localizations.Dutch;
                    break;
                case "fi":
                    lang = SchedulerLocalization.Localizations.Finnish;
                    break;
                case "fr":
                    lang = SchedulerLocalization.Localizations.French;
                    break;
                case "de":
                    lang = SchedulerLocalization.Localizations.German;
                    break;
                case "el":
                    lang = SchedulerLocalization.Localizations.Greek;
                    break;
                case "he":
                    lang = SchedulerLocalization.Localizations.Hebrew;
                    break;
                case "hu":
                    lang = SchedulerLocalization.Localizations.Hungarian;
                    break;
                case "id":
                    lang = SchedulerLocalization.Localizations.Indonesia;
                    break;
                case "it":
                    lang = SchedulerLocalization.Localizations.Italian;
                    break;
                case "jp":
                    lang = SchedulerLocalization.Localizations.Japanese;
                    break;
                case "no":
                    lang = SchedulerLocalization.Localizations.Norwegian;
                    break;
                case "pl":
                    lang = SchedulerLocalization.Localizations.Polish;
                    break;
                case "pt":
                    lang = SchedulerLocalization.Localizations.Portuguese;
                    break;
                case "ro":
                    lang = SchedulerLocalization.Localizations.Romanian;
                    break;
                case "ru":
                    lang = SchedulerLocalization.Localizations.Russian;
                    break;
                case "si":
                    lang = SchedulerLocalization.Localizations.Slovenian;
                    break;
                case "es":
                    lang = SchedulerLocalization.Localizations.Spanish;
                    break;
                case "sv":
                    lang = SchedulerLocalization.Localizations.Swedish;
                    break;
                case "tr":
                    lang = SchedulerLocalization.Localizations.Turkish;
                    break;
                case "ua":
                    lang = SchedulerLocalization.Localizations.Ukrainian;
                    break;
                default:
                    lang = SchedulerLocalization.Localizations.English;
                    language = "en";
                    break;
            }
            #endregion

            #region skin
            switch (this.Request.QueryString["skin"])
            {
                case "glossy":
                    skin = DHXScheduler.Skins.Glossy;
                    break;
                case "terrace":
                    skin = DHXScheduler.Skins.Terrace;
                    break;
                default:
                    skin = DHXScheduler.Skins.Standart;
                    skn = "classic";
                    break;
            }
            #endregion
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Terrace;
            scheduler.InitialDate = new DateTime(2011, 11, 24);

            scheduler.XY.scroll_width = 0;
            scheduler.Config.first_hour = 8;
            scheduler.Config.last_hour = 19;
            scheduler.Config.time_step = 30;
            scheduler.Config.multi_day = true;
            scheduler.Config.limit_time_select = true;
            scheduler.Skin = skin;
            scheduler.Localization.Directory = "locale";
            scheduler.Localization.Set(lang, false);

            var rooms = new DHXSchedulerModelsDataContext().Rooms.ToList();

            var unit = new UnitsView("unit1", "room_id");
            unit.AddOptions(rooms);//parse model objects
            scheduler.Views.Add(unit);

            var timeline = new TimelineView("timeline", "room_id");
            timeline.RenderMode = TimelineView.RenderModes.Bar;
            timeline.FitEvents = false;
            timeline.AddOptions(rooms);
            scheduler.Views.Add(timeline);


            scheduler.EnableDataprocessor = true;
            scheduler.LoadData = true;
            scheduler.InitialDate = new DateTime(2011, 9, 19);
            return View(new LocaleData(scheduler, language, skn));
        }
        public ContentResult Data()
        {

            var data = new SchedulerAjaxData((new DHXSchedulerModelsDataContext()).Events);
    

            return data;
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {

            var action = new DataAction(actionValues);

            DHXSchedulerModelsDataContext data = new DHXSchedulerModelsDataContext();
            var changedEvent = (Event)DHXEventsHelper.Bind(typeof(Event), actionValues);
            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        data.Events.InsertOnSubmit(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = data.Events.SingleOrDefault(ev => ev.id == action.SourceId);
                        data.Events.DeleteOnSubmit(changedEvent);
                        break;
                    default:// "update"                          
                        var eventToUpdate = data.Events.SingleOrDefault(ev => ev.id == action.SourceId);
                        DHXEventsHelper.Update(eventToUpdate, changedEvent, new List<string>() { "id" });
                        break;
                }
                data.SubmitChanges();
                action.TargetId = changedEvent.id;
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
        }
    }
}
