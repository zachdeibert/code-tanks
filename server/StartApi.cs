using System;
using Com.GitHub.ZachDeibert.CodeTanks.Model;

namespace Com.GitHub.ZachDeibert.CodeTanks.Server {
    public sealed class StartApi : UnauthenticatedApi<LoginTokenContainer> {
        protected override string EndPoint => "/start";

        protected override LoginTokenContainer Response {
            get {
                if (Model.Info.CurrentState == InfoData.State.Ready) {
                    Model.Info.CurrentState = InfoData.State.Simulating;
                    Model.Start();
                    return new LoginTokenContainer {
                        LoginToken = Model.ControllerId
                    };
                } else {
                    throw new InvalidOperationException("The simulator has already been started");
                }
            }
        }
    }
}
