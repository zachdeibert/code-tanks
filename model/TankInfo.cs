using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class TankInfo {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("motor")]
        public int Motors;
        [JsonProperty("battery")]
        public int Batteries;
        [JsonProperty("cannon")]
        public int Cannons;
        [JsonProperty("radar")]
        public int Radars;
        [JsonProperty("gps")]
        public int GPS;
        [JsonProperty("explosives")]
        public int Explosives;
        
        [JsonIgnore]
        public DateTime?[] CannonCharges;
        public event Action<int> CannonFire;
        public event Action Exploded;

        public void FireCannon(int i) {
            CannonFire?.Invoke(i);
        }

        public void Explode() {
            Exploded?.Invoke();
        }
    }
}
