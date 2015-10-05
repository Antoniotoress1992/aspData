using IgniteUI.SamplesBrowser.Models.Showcase;
using System;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class DatePickerController : Controller
    {
        //
        // GET: /DatePicker/

        [ActionName("hotel-reservation")]
        public ActionResult HotelReservation()
        {
            HotelReservation model = new HotelReservation();
            model.StartDate = DateTime.Now;
            ViewData["message"] = String.Empty;

            return View("hotel-reservation", model);
        }

        [HttpPost]
        [ActionName("hotel-reservation")]
        public ActionResult HotelReservation(HotelReservation model)
        {
            model.Save();
            ViewData["message"] = "Your room has been booked successfully. Thank you!";
            return View("hotel-reservation", model);
        }


    }
}
