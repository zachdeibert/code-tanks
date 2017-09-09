using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class ConfigData {
        public sealed class CostData {
            [JsonProperty("motor")]
            public Formula MotorCost;
            [JsonProperty("battery")]
            public Formula BatteryCost;
            [JsonProperty("cannon")]
            public Formula CannonCost;
            [JsonProperty("radar")]
            public Formula RadarCost;
            [JsonProperty("gps")]
            public Formula GPSCost;
            [JsonProperty("explosives")]
            public Formula ExplosivesCost;
        }

        public sealed class SpeedData {
            [JsonProperty("bullet")]
            public Formula BulletSpeed;
            [JsonProperty("tankDrive")]
            public Formula TankDriveSpeed;
            [JsonProperty("tankTurn")]
            public Formula TankTurnSpeed;
        }

        public sealed class ExplosionData {
            [JsonProperty("size")]
            public Formula Size;
            [JsonProperty("duration")]
            public int Duration;
        }

        [JsonProperty("funds")]
        public double TotalFunds;
        [JsonProperty("costs")]
        public CostData Costs;
        [JsonProperty("speeds")]
        public SpeedData Speeds;
        [JsonProperty("explosions")]
        public ExplosionData Explosions;
        [JsonProperty("tankSize")]
        public double TankSize;
    }
}
