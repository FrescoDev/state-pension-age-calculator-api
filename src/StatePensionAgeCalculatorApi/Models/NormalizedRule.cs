using System;

namespace StatePensionAgeCalculatorApi.Models
{
    public class NormalizedRule
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Gender { get; set; }
        public int? Spa { get; set; }
    }
}