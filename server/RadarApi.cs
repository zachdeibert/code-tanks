using System;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public sealed class RadarApi : AuthenticatedApi<LoginTokenContainer, RadarData> {
        protected override bool IsControllerApi => false;

        protected override string EndPoint => "/radar";

        double IntersectLines(double x1, double y1, double m1, double x2, double y2, double m2, double dx2) {
            if (m1 == m2) {
                return double.NaN;
            }
            double x = (m1 * x1 - y1 - m2 * x2 + y2) / (m1 - m2);
            if (Math.Sign(dx2) == Math.Sign(x2 - x)) {
                return double.NaN;
            }
            return x;
        }

        double Distance(double x1, double y1, double m1, double dx1, double x2, double y2) {
            if (m1 == 0) {
                return double.NaN;
            }
            double m2 = -1 / m1;
            double x = IntersectLines(x2, y2, m2, x1, y1, m1, dx1);
            double y = IntersectLines(x2, y2, m1, y1, x1, m2, dx1);
            double dx = x - x2;
            double dy = y - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        
        bool Between(double search, double bound1, double bound2) {
            return search > Math.Min(bound1, bound2) && search < Math.Max(bound1, bound2);
        }

        protected override RadarData Handle(LoginTokenContainer req, PlayerInfo player) {
            double closest = double.MaxValue;
            foreach (PlayerInfo tank in Model.Players.Players) {
                double distance = Distance(player.X, player.Y, Math.Tan(player.Rotation), tank.X, tank.Y, Math.Cos(player.Rotation));
                if (double.IsNaN(distance)) {
                    distance = Distance(player.Y, player.X, -Math.Tan(player.Rotation + Math.PI / 2), tank.Y, tank.X, Math.Sin(player.Rotation));
                }
                closest = Math.Min(closest, distance);
            }
            foreach (WallInfo wall in Model.Walls.Walls) {
                double x = IntersectLines(wall.X0, wall.Y0, (wall.Y1 - wall.Y0) / (wall.X1 - wall.X0), player.X, player.Y, Math.Tan(player.Rotation), Math.Cos(player.Rotation));
                double y = IntersectLines(wall.Y0, wall.X0, (wall.X1 - wall.X0) / (wall.Y1 - wall.Y0), player.Y, player.X, -Math.Tan(player.Rotation + Math.PI / 2), Math.Sin(player.Rotation));
                if (Between(x, wall.X0, wall.X1) && Between(y, wall.Y0, wall.Y1)) {
                    double dx = x - player.X;
                    double dy = y - player.Y;
                    double distance = Math.Sqrt(dx * dx + dy * dy);
                    closest = Math.Min(closest, distance);
                }
            }
            return new RadarData {
                Distance = closest
            };
        }
    }
}
