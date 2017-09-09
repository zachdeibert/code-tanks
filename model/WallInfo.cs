using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class WallInfo {
        [JsonProperty("x0")]
        public double X0;
        [JsonProperty("x1")]
        public double X1;
        [JsonProperty("y0")]
        public double Y0;
        [JsonProperty("y1")]
        public double Y1;
    }
}
