using System;
using System.Collections.Generic;
using System.Text;

namespace AzureRateCardUtils
{
    public class AzureRateCard
    {
        public List<OfferTermsT> OfferTerms { get; set; }
        public List<MetersT> Meters { get; set; }
        public string Currency { get; set; }
        public string Locale { get; set; }
        public Boolean IsTaxIncluded { get; set; }
    }

    public class MetersT
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get { return MeterId; }   }
        public string EffectiveDate { get; set; }
        public int IncludedQuantity { get; set; }
        public string MeterCategory { get; set; }
        public string MeterName { get; set; }
        public string MeterId { get; set; }
        public Dictionary<string, string> MeterRates { get; set; } 
        public string MeterRegion { get; set; }
        public string MeterSubCategory { get; set; }
        public List<string> MeterTags { get; set; }
        public string Unit { get; set; }
    }

    public class MeterRatesT
    {

    }

    public class OfferTermsT
    {

    }
}
