using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public sealed class WebServer : IDisposable {
        readonly HttpListener Listener;
        readonly IEndPoint[] EndPoints = {
            new ConfigApi(),
            new InfoApi(),
            new UnauthenticatedPlayersApi(),
            new FieldApi(),
            new StartApi(),
            new AuthenticatedPlayersApi(),
            new CreateTankApi(),
            new ChargeCannonApi(),
            new FireCannonApi(),
            new MoveApi(),
            new RadarApi(),
            new GpsApi(),
            new DefaultEndPoint()
        };

        void HandleRequest(object sender, HttpListenerRequestEventArgs e) {
            using (e.Response) {
                foreach (IEndPoint endPoint in EndPoints) {
                    if (endPoint.CanHandle(e.Request.HttpMethod, e.Request.Url)) {
                        endPoint.Handle(e);
                        break;
                    }
                }
            }
        }

        public void Dispose() {
            Listener.Dispose();
        }

        public WebServer(GameModel model, int port) {
            Listener = new HttpListener(IPAddress.Any, port);
            Listener.Request += HandleRequest;
            foreach (IEndPoint endPoint in EndPoints) {
                endPoint.Model = model;
            }
            Listener.Start();
        }
    }
}
