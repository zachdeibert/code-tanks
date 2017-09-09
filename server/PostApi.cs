using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public abstract class PostApi<TReq, TRes> : IEndPoint {
        public GameModel Model {
            get;
            set;
        }

        protected abstract string EndPoint {
            get;
        }

        public virtual bool CanHandle(string method, Uri uri) {
            return method == "POST" && uri.LocalPath == EndPoint;
        }

        protected abstract TRes Handle(TReq req);

        public virtual void Handle(HttpListenerRequestEventArgs e) {
            Task<string> task = e.Request.ReadContentAsStringAsync();
            task.Wait();
            e.Response.WriteContentAsync(JsonConvert.SerializeObject(Handle(JsonConvert.DeserializeObject<TReq>(task.Result))));
        }
    }
}
