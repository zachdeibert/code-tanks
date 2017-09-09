using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public class PlayerInfo : GpsData {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("alive")]
        public bool Alive;
        [JsonProperty("score")]
        public double Score;
        [JsonIgnore]
        public Guid LoginToken;
        [JsonIgnore]
        public TankInfo Tank;
        [JsonIgnore]
        public MoveData Movement = new MoveData {
            Forward = new MoveData.MoveInfo<MoveData.ForwardDirection> {
                Direction = MoveData.ForwardDirection.Forwards,
                Rate = 0
            },
            Strafe = new MoveData.MoveInfo<MoveData.StrafeDirection> {
                Direction = MoveData.StrafeDirection.Left,
                Rate = 0
            },
            Turn = new MoveData.MoveInfo<MoveData.TurnDirection> {
                Direction = MoveData.TurnDirection.ClockWise,
                Rate = 0
            }
        };
    }
}
