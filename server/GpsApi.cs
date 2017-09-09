using System;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public class GpsApi : AuthenticatedApi<LoginTokenContainer, GpsData> {
        protected override bool IsControllerApi => false;

        protected override string EndPoint => "/gps";

        protected override GpsData Handle(LoginTokenContainer req, PlayerInfo player) {
            if (player.Tank.GPS == 0) {
                throw new NotSupportedException("The tank does not have a GPS");
            }
            return new GpsData {
                X = player.X,
                Y = player.Y,
                Rotation = player.Rotation
            };
        }
    }
}
