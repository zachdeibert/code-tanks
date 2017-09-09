using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class WallData {
        [JsonProperty("walls")]
        public List<WallInfo> Walls = new List<WallInfo>();
    }
}
