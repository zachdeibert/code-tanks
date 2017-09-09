using System;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public sealed class ExplodeApi : AuthenticatedApi<LoginTokenContainer, SuccessfulResponse> {
        protected override bool IsControllerApi => false;

        protected override string EndPoint => "/explode";

        protected override SuccessfulResponse Handle(LoginTokenContainer req, PlayerInfo player) {
            player.Tank.Explode();
            return new SuccessfulResponse();
        }
    }
}
