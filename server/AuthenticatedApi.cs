using System;
using System.Linq;
using System.Net.Http;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public abstract class AuthenticatedApi<TReq, TRes> : PostApi<TReq, TRes> where TReq : LoginTokenContainer {
        protected abstract bool IsControllerApi {
            get;
        }

        protected abstract TRes Handle(TReq req, PlayerInfo player);

        protected override TRes Handle(TReq req) {
            PlayerInfo player;
            if (IsControllerApi) {
                if (Model.ControllerId != req.LoginToken) {
                    throw new UnauthorizedAccessException("Login token is incorrect");
                }
                player = null;
            } else {
                player = Model.Players.Players.FirstOrDefault(p => p.LoginToken == req.LoginToken);
                if (player == null) {
                    throw new UnauthorizedAccessException("Login token is incorrect");
                } else if (!player.Alive) {
                    throw new NotSupportedException("The tank is dead");
                }
            }
            return Handle(req, player);
        }
    }
}
