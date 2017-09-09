package com.github.zachdeibert.codetanks.api;

import java.io.IOException;

import com.github.zachdeibert.codetanks.api.model.SuccessfulResponse;

public final class TankCannon {
	final TankNetwork network;
	final String charge;
	final String fire;

	public void charge() throws IOException {
		network.postAuthenticated(charge, SuccessfulResponse.class);
	}

	public TankCannon(final TankNetwork network, final int i) {
		this.network = network;
		this.charge = String.format("/cannon/%d/charge", i);
		this.fire = String.format("/cannon/%d/fire", i);
	}
}
