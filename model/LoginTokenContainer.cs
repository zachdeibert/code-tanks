using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public class LoginTokenContainer {
        [JsonProperty("id")]
        public Guid LoginToken;
    }
}
