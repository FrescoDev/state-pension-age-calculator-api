using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StatePensionAgeCalculatorApi.Controllers
{
    [Route("/")]
    public class HealthCheckController : Controller
    {
        public IActionResult Get()
        {
            return Ok();
        }
    }
}