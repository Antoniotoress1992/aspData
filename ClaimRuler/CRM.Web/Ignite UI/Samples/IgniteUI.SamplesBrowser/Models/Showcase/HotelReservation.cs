using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgniteUI.SamplesBrowser.Models.Showcase
{
    public class HotelReservation
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public HotelReservation() { }

        public void Save() 
        {
            // For sample purposes, this method was created to demonstrate when values are persisted. 
            // The implementation details vary depending on the persistence medium.
        }

        private bool isReserved()
        {
            return (this.EndDate.HasValue == true);
        }

        public string GetStatus()
        {
            if (this.isReserved())
            {
                return "Room is successfully reserved!";
            }
            else
            {
                return "Room is not reserved!";
            }
        }
    }
}