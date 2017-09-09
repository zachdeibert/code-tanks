using System;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class GameModel {
        public ConfigData Configuration;
        public InfoData Info = new InfoData();
        public PlayerData Players = new PlayerData();
        public WallData Walls;
        public event Action OnStart;
        public Guid ControllerId = Guid.NewGuid();

        public void Start() {
            OnStart?.Invoke();
        }
    }
}
