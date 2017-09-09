using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class RadarData {
        [JsonProperty("distance")]
        public double Distance;
    }
}
