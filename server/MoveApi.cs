using System;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public sealed class MoveApi : AuthenticatedApi<MoveData, SuccessfulResponse> {
        protected override bool IsControllerApi => false;

        protected override string EndPoint => "/move";

        protected override SuccessfulResponse Handle(MoveData req, PlayerInfo player) {
            if (req.Forward.Rate != 0 && player.Tank.Motors < 1) {
                throw new NotSupportedException("Need at least one motor to move");
            }
            if (req.Turn.Rate != 0 && player.Tank.Motors < 2) {
                throw new NotSupportedException("Need at least two motors to turn");
            }
            if (req.Strafe.Rate != 0 && player.Tank.Motors < 4) {
                throw new NotSupportedException("Need at least four motors to strafe");
            }
            if (req.Forward.Rate < 0 || req.Forward.Rate > 1 || req.Turn.Rate < 0 || req.Turn.Rate > 1 || req.Strafe.Rate < 0 || req.Strafe.Rate > 1) {
                throw new ArgumentOutOfRangeException("Rates must be between 0 and 1");
            }
            player.Movement = req;
            return new SuccessfulResponse();
        }
    }
}
