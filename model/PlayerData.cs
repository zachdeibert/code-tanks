using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class PlayerData {
        [JsonProperty("players")]
        public List<PlayerInfo> Players = new List<PlayerInfo>();

        public event Action<PlayerInfo> PlayerAdded;

        public void AddPlayer(PlayerInfo player) {
            Players.Add(player);
            PlayerAdded?.Invoke(player);
        }
    }
}
