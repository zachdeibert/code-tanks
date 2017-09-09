using System;
using System.Net.Http;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public interface IEndPoint {
        GameModel Model {
            get;
            set;
        }

        bool CanHandle(string method, Uri uri);

        void Handle(HttpListenerRequestEventArgs e);
    }
}
