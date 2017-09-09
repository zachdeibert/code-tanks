using System;
using System.Net.Http;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public class DefaultEndPoint : IEndPoint {
        public GameModel Model {
            get;
            set;
        }

        public bool CanHandle(string method, Uri uri) {
            return true;
        }

        public void Handle(HttpListenerRequestEventArgs e) {
            e.Response.NotFound();
        }
    }
}
