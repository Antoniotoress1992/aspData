using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Models.DataAnnotations
{
    public class ValidatedOrder
    {
        [Required(ErrorMessage = "Select ship date")]       
        public DateTime OrderShipDate { get; set; }

        [Required]
        public DateTime OrderDueDate { get; set; }


        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "$$(name_length)")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Select ship method")]
        public string ShipMethod { get; set; }

        public List<SelectListItem> ShipMethodList { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string ContactPhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter advanced payment")]
        [Range(0, 100)]
        public double AdvancePaymentAmount { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression("^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$", ErrorMessage = "Please enter a valid email")]
        public string ContactEmail { get; set; }

    }
}