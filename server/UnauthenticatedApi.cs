using System;
using System.Net.Http;
using Newtonsoft.Json;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public abstract class UnauthenticatedApi<T> : IEndPoint {
        public GameModel Model {
            get;
            set;
        }

        protected abstract string EndPoint {
            get;
        }

        public bool CanHandle(string method, Uri uri) {
            return method == "GET" && uri.LocalPath == EndPoint;
        }

        protected abstract T Response {
            get;
        }

        public void Handle(HttpListenerRequestEventArgs e) {
            e.Response.WriteContentAsync(JsonConvert.SerializeObject(Response)).Wait();
        }
    }
}
