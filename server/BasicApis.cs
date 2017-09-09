using System;
using System.Linq;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public sealed class ConfigApi : UnauthenticatedApi<ConfigData> {
        protected override string EndPoint => "/config";

        protected override ConfigData Response => Model.Configuration;
    }

    public sealed class InfoApi : UnauthenticatedApi<InfoData> {
        protected override string EndPoint => "/info";

        protected override InfoData Response => Model.Info;
    }

    public sealed class UnauthenticatedPlayersApi : UnauthenticatedApi<PlayerData> {
        protected override string EndPoint => "/players";

        protected override PlayerData Response => new PlayerData {
            Players = Model.Players.Players.Select(p => new PlayerInfo {
                Name = p.Name,
                Alive = p.Alive,
                Score = p.Score
            }).ToList()
        };
    }

    public sealed class FieldApi : UnauthenticatedApi<WallData> {
        protected override string EndPoint => "/field";

        protected override WallData Response => Model.Walls;
    }
}
