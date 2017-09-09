using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public class GpsData {
        [JsonProperty("x")]
        public double X;
        [JsonProperty("y")]
        public double Y;
        [JsonProperty("rot")]
        public double Rotation;
    }
}
