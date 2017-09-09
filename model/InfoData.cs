using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class InfoData {
        public enum State {
            [JsonProperty("ready")]
            Ready,
            [JsonProperty("simulating")]
            Simulating,
            [JsonProperty("finished")]
            Finished
        }

        [JsonProperty("version")]
        public int[] Version = {
            1,
            0
        };
        [JsonProperty("state")]
        public State CurrentState = State.Ready;
    }
}
