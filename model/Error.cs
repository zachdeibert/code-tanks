using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class Error {
        [JsonProperty("error")]
        public bool IsError = true;
        [JsonProperty("type")]
        public string Type;
        [JsonProperty("message")]
        public string Message;

        public Error() {
        }

        public Error(Exception ex) {
            Type = ex.GetType().Name;
            Message = ex.Message;
        }
    }
}
