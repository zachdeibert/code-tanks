using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public abstract class CannonApi : IEndPoint {
        public GameModel Model {
            get;
            set;
        }

        protected abstract string EndPoint {
            get;
        }

        bool TryParseUri(string uri, out int cannon) {
            if (!uri.StartsWith("/cannon/") || !uri.EndsWith(EndPoint)) {
                cannon = -1;
                return false;
            }
            string middle = uri.Substring("/cannon/".Length, uri.Length - EndPoint.Length - "/cannon/".Length);
            return int.TryParse(middle, out cannon);
        }

        public bool CanHandle(string method, Uri uri) {
            if (method != "POST") {
                return false;
            }
            int cannon;
            return TryParseUri(uri.LocalPath, out cannon);
        }

        protected abstract void Handle(PlayerInfo player, int cannon);

        public void Handle(HttpListenerRequestEventArgs e) {
            Task<string> task = e.Request.ReadContentAsStringAsync();
            task.Wait();
            LoginTokenContainer loginToken = JsonConvert.DeserializeObject<LoginTokenContainer>(task.Result);
            PlayerInfo player = Model.Players.Players.FirstOrDefault(p => p.LoginToken == loginToken.LoginToken);
            if (player == null) {
                throw new UnauthorizedAccessException("Login token is incorrect");
            }
            int cannon;
            TryParseUri(e.Request.Url.LocalPath, out cannon);
            if (cannon < 0 || cannon > player.Tank.Cannons) {
                throw new ArgumentOutOfRangeException("Cannon number is invalid");
            }
            Handle(player, cannon);
            e.Response.WriteContentAsync(JsonConvert.SerializeObject(new SuccessfulResponse()));
        }
    }

    public sealed class ChargeCannonApi : CannonApi {
        protected override string EndPoint => "/charge";

        protected override void Handle(PlayerInfo player, int cannon) {
            if (player.Tank.CannonCharges[cannon] != null) {
                throw new InvalidOperationException("Cannon is already charging");
            }
            player.Tank.CannonCharges[cannon] = DateTime.Now;
        }
    }

    public sealed class FireCannonApi : CannonApi {
        protected override string EndPoint => "/fire";

        protected override void Handle(PlayerInfo player, int cannon) {
            if (player.Tank.CannonCharges[cannon] == null) {
                throw new InvalidOperationException("Cannon is not charged");
            }
            player.Tank.FireCannon(cannon);
            player.Tank.CannonCharges[cannon] = null;
        }
    }
}
