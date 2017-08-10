using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StatePensionAgeCalculatorApi.Models;

namespace StatePensionAgeCalculatorApi.Controllers
{
    [Route("api/calculation")]
    public class StatePensionAgeCalculationController : Controller
    {
        private readonly string pathToFile;

        public StatePensionAgeCalculationController(IHostingEnvironment env)
        {
            pathToFile = env.ContentRootPath + Path.DirectorySeparatorChar.ToString() + "Data" + Path.DirectorySeparatorChar.ToString() + "rules.json";
        }

        // GET api/calculation?gender=M&dateOfBirth=2012-03-02
        [HttpGet()]
        public async Task<IActionResult> GetAsync([FromQuery]string gender, [FromQuery]string dateOfBirth)
        {
            // The following validation checks could be encapsulated and executed as an Action Filter and called by an attribute decorator BUT this is only actually useful required on multiple/routes/actions where the code can be reused. Potential weakness here, if for some reason there was a large number of query params - this endpoint could be spammed since iteration over query collection is running sync could be DDOSed.
            if (!containsRequiredParams(HttpContext.Request.Query))
            {
                return BadRequest("Unable to process request due to missing query parameters. \n\nRequired query keys: [gender] & [dateOfbirth]");
            }

            if (!genderIsRecognised(gender))
            {
                return BadRequest($"Unable to process request due to unrecognized [gender] value. \n\nAccepted query values: M/m, F/f \nActual [gender] value: {gender} ");
            }

            if (!dateOfBirthIsValid(dateOfBirth))
            {
                return BadRequest($"Unable to process request due to an invalid [dateOfBirth] value. \n\nRequied query value format: Standard Date and Time Format \nActual [dateOfBirth] value: {dateOfBirth}");
            }

            // This query should really be done upon app startup, as data shouldn't change in between requests. i.e. only one disk read. Or stored in a db/cached
            var rawData = await GetRulesAsync();

            // The data is sorted by date time intervals so we filter down to gender specific first to hopefully reduce most elements and quicken further operations - "u" assumed to be either gender.
            var genderFilteredRuleCollection = rawData.Rules.Where(rule => (rule.Gender.ToLower() == gender.ToLower()) || rule.Gender.ToLower() == "u").ToList();

            var parsedDateOfBirth = DateTime.Parse(dateOfBirth);

            // normalize data strcture so we can query uniform schema
            var normalizedRuleCollection = genderFilteredRuleCollection.Select(rule => rule.CreateNormalizedVersion(parsedDateOfBirth)).ToList();

            // we've now got a date-time sorted fully relevent list - optimal algorithm here would be to do binary search on the collection's indices set to find appropriate time boundary but it's getting late...
            var queryResult = normalizedRuleCollection.Where(rule => rule.From <= parsedDateOfBirth && rule.To >= parsedDateOfBirth).ToList().Single();

            if (queryResult == null)
            {
                // Should never happen if the data is legit
                return BadRequest("Unable to caclulate state pension age");
            }

            var response = new CalculationGetResponse()
            {
                StatePensionAge = queryResult.Spa
            };

            return Ok(response);
        }

        private async Task<RuleCollection> GetRulesAsync()
        {
            string rulesDataJson = "";

            using (StreamReader dateSourceReader = System.IO.File.OpenText(pathToFile))
            {
                rulesDataJson = await dateSourceReader.ReadToEndAsync();
            }

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<RuleCollection>(rulesDataJson));
        }

        private bool dateOfBirthIsValid(string dateOfBirth)
        {
            DateTime parsedDateOfBirth;

            if (DateTime.TryParse(dateOfBirth, out parsedDateOfBirth))
            {
                return true;
            }

            return false;
        }

        private bool genderIsRecognised(string gender)
        {
            var acceptedGenderIdentifiers = new List<string>
            {
                "m",
                "f"
            };

            return acceptedGenderIdentifiers.Any(genderId => genderId == gender.ToLower());
        }

        private static bool containsRequiredParams(Microsoft.AspNetCore.Http.IQueryCollection requestQuery)
        {
            return requestQuery.ContainsKey("dateOfBirth") && requestQuery.ContainsKey("gender");
        }
    }
}
