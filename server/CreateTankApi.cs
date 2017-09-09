using System;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public class CreateTankApi : PostApi<TankInfo, LoginTokenContainer> {
        protected override string EndPoint => "/create";

        protected override LoginTokenContainer Handle(TankInfo req) {
            double totalPrice = 0;
            totalPrice += Model.Configuration.Costs.MotorCost.Calculate(req.Motors);
            totalPrice += Model.Configuration.Costs.BatteryCost.Calculate(req.Batteries);
            totalPrice += Model.Configuration.Costs.CannonCost.Calculate(req.Cannons);
            totalPrice += Model.Configuration.Costs.RadarCost.Calculate(req.Radars);
            totalPrice += Model.Configuration.Costs.GPSCost.Calculate(req.GPS);
            totalPrice += Model.Configuration.Costs.ExplosivesCost.Calculate(req.Explosives);
            if (totalPrice > Model.Configuration.TotalFunds) {
                throw new InvalidOperationException("The tank costs too much to build");
            }
            PlayerInfo player = new PlayerInfo {
                Name = req.Name,
                Alive = true,
                Score = 0,
                LoginToken = Guid.NewGuid(),
                Tank = req
            };
            player.Tank.CannonCharges = new DateTime?[req.Cannons];
            Model.Players.AddPlayer(player);
            return new LoginTokenContainer {
                LoginToken = player.LoginToken
            };
        }
    }
}
