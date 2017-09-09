using System;
using System.Threading;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Simulator {
    public sealed class GameSimulator : IDisposable {
        readonly GameModel Model;
        readonly Thread Thread;
        bool Running = true;

        void PlayerAdded(PlayerInfo player) {
            
        }

        void Run() {
            Model.Players.PlayerAdded += PlayerAdded;
            DateTime lastFrame = DateTime.Now;
            while (Running) {
                DateTime thisFrame = DateTime.Now;
                long dt = (lastFrame - thisFrame).Ticks / 10000;
                if (dt == 0) {
                    Thread.Sleep(1);
                } else {
                    foreach (PlayerInfo player in Model.Players.Players) {
                        double speed = Model.Configuration.Speeds.TankDriveSpeed.Calculate(player.Tank.Batteries);
                        double turnSpeed = Model.Configuration.Speeds.TankTurnSpeed.Calculate(player.Tank.Batteries);
                        double df = (player.Movement.Forward.Direction == MoveData.ForwardDirection.Forwards ? 1 : -1) * player.Movement.Forward.Rate * speed;
                        double ds = (player.Movement.Strafe.Direction == MoveData.StrafeDirection.Right ? 1 : -1) * player.Movement.Strafe.Rate * speed;
                        double dr = (player.Movement.Turn.Direction == MoveData.TurnDirection.CounterClockWise ? 1 : -1) * player.Movement.Turn.Rate * turnSpeed;
                        double dx = df * Math.Sin(player.Rotation) + ds * Math.Cos(player.Rotation);
                        double dy = df * Math.Cos(player.Rotation) + ds * Math.Sin(player.Rotation);
                        player.X += dx;
                        player.Y += dy;
                        player.Rotation += dr;
                    }
                }
            }
        }

        public void Dispose() {
            Running = false;
            Thread.Join();
        }

        public GameSimulator(GameModel model) {
            Thread = new Thread(Run);
            Model = model;
            Model.OnStart += Thread.Start;
        }
    }
}
