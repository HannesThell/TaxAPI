using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TaxAPI.Models
{
    public class VehicleTaxInput
    {
        [Required]
        public string VehicleType { get; set; }
        [Required]
        public List<DateTime> TaxationDates { get; set; }
    }
}
