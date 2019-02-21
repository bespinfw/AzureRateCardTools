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
        public string Id { get { return MeterId; } }
        public DateTimeOffset EffectiveDate { get; set; }
        public double IncludedQuantity { get; set; }
        public string MeterCategory { get; set; }
        public string MeterName { get; set; }
        public string MeterId { get; set; }
        public Dictionary<string, double> MeterRates { get; set; }
        public string MeterRegion { get; set; }
        public string MeterSubCategory { get; set; }
        public List<string> MeterTags { get; set; }
        public string Unit { get; set; }
    }

    public class OfferTermsT
    {
        public double Credit { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public List<Guid> ExcludedMeterIds { get; set; }
        public string Name { get; set; }
    }

    public class AzureRIRateCard
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "offers")]
        public Dictionary<string, RIOffers> Offers { get; set; }
    }

    public class RIOffers
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "cores")]
        public int Cores { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "diskSize")]
        public int DiskSize { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "isVcpu")]
        public bool IsVcpu { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "prices")]
        public Dictionary<string, RIPrice> Prices { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "pricingTypes")]
        public string PricingTypes { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ram")]
        public float Ram { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "series")]
        public string Series { get; set; }
    }

    public class RIPrice
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "value")]
        public double Value { get; set; }
    }

    public class RIRateCard : RIOffers
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get { return Name; } }

        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public RIRateCard(string name, RIOffers offer)
        {
            this.Name = name;
            this.IsVcpu = offer.IsVcpu;
            this.Cores = offer.Cores;
            this.DiskSize = offer.DiskSize;
            this.Prices = offer.Prices;
            this.PricingTypes = offer.PricingTypes;
            this.Ram = offer.Ram;
            this.Series = offer.Series;
        }
    }
}
