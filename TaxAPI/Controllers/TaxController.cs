using Microsoft.AspNetCore.Mvc;
using TaxAPI.Logic;
using TaxAPI.Models;

namespace TaxAPI.Controllers
{
    [ApiController]
    [Route("api/tax")]
    [Consumes("application/json")]
    public class TaxController : ControllerBase
    {
        private readonly ICongestionTaxCalculator congestionTaxCalculator;
        public TaxController(ICongestionTaxCalculator congestionTaxCalculator)
        {
            this.congestionTaxCalculator = congestionTaxCalculator;
        }

        [HttpPost("vehicle/calculateTax")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CalculateTax([FromBody]VehicleTaxInput vehicleTaxInput)
        {

            int totalTax = congestionTaxCalculator.CalculateTax(vehicleTaxInput.VehicleType, vehicleTaxInput.TaxationDates);

            var taxReponse = new VehicleTaxResponse
            {
                TotalTax = totalTax
            };

            return Ok(taxReponse);
        }
    }
}
