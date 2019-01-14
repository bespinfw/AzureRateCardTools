using System;
using System.Collections.Generic;
using System.Text;

namespace AzureRateCardUtils
{
    public class MicrosoftLoginOAuthToken
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "access_token")]
        public string Access_token { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "expires_in")]
        public int Expires_in { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "expires_on")]
        public long Expires_on { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ext_expires_in")]
        public int Ext_expires_in { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "not_before")]
        public long Not_before { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "resource")]
        public string Resource { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "token_type")]
        public string Token_type { get; set; }
    }
}
