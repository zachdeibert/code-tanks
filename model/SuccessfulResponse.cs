using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class SuccessfulResponse {
        [JsonProperty("success")]
        public bool Successful = true;
    }
}
