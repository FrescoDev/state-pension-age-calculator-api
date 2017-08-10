using System;
using Newtonsoft.Json;

namespace StatePensionAgeCalculatorApi.Models
{
    public class GenericRule
    {
        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "spa")]
        public string Spa { get; set; }

        [JsonProperty(PropertyName = "spa_years")]
        public int? SpaYears { get; set; }

        [JsonProperty(PropertyName = "spa_months")]
        public int? SpaMonths { get; set; }

        public NormalizedRule CreateNormalizedVersion(DateTime dateOfBirth)
        {
            if (Spa == null && SpaYears != null)
            {
                return new NormalizedRule()
                {
                    From = DateTime.Parse(this.From),
                    To = DateTime.Parse(this.To),
                    Gender = this.Gender,
                    Spa = SpaYears
                };
            }
            else
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                var dateOfRetirement = DateTime.Parse(Spa);
                var ageTimeSpan = dateOfRetirement - dateOfBirth;

                // calendar, we must subtract a year here.
                int ageAtRetirement = (zeroTime + ageTimeSpan).Year - 1;

                return new NormalizedRule()
                {
                    From = DateTime.Parse(this.From),
                    To = DateTime.Parse(this.To),
                    Gender = this.Gender,
                    Spa = ageAtRetirement
                };
            }
        }
    }
}