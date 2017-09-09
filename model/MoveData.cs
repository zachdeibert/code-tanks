using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class MoveData : LoginTokenContainer {
        public enum ForwardDirection {
            [JsonProperty("forward")]
            Forwards,
            [JsonProperty("backward")]
            Backwards
        }

        public enum StrafeDirection {
            [JsonProperty("left")]
            Left,
            [JsonProperty("right")]
            Right
        }

        public enum TurnDirection {
            [JsonProperty("clockwise")]
            ClockWise,
            [JsonProperty("counterclockwise")]
            CounterClockWise
        }

        public sealed class MoveInfo<T> {
            [JsonProperty("direction")]
            public T Direction;
            [JsonProperty("rate")]
            public double Rate;
        }

        [JsonProperty("forward")]
        public MoveInfo<ForwardDirection> Forward;
        [JsonProperty("strafe")]
        public MoveInfo<StrafeDirection> Strafe;
        [JsonProperty("turn")]
        public MoveInfo<TurnDirection> Turn;
    }
}
