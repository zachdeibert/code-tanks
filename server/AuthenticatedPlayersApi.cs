using System;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public sealed class AuthenticatedPlayersApi : AuthenticatedApi<LoginTokenContainer, PlayerData> {
        protected override bool IsControllerApi => true;

        protected override string EndPoint => "/players";

        protected override PlayerData Handle(LoginTokenContainer req, PlayerInfo player) {
            return Model.Players;
        }
    }
}
