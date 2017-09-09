package com.github.zachdeibert.codetanks.api;

import java.io.IOException;
import java.net.URI;

import com.github.zachdeibert.codetanks.api.model.ConfigData;
import com.github.zachdeibert.codetanks.api.model.TankInfo;

public final class TankBuilder {
	final TankNetwork network;
	final ConfigData config;
	TankInfo info;
	boolean built;

	private void precall() {
		if (built) {
			throw new IllegalStateException("The tank has already been built");
		}
	}

	private void buy(TankInfo info) {
		double totalPrice = 0;
		totalPrice += config.costs.motor.calculate(info.motor);
		totalPrice += config.costs.battery.calculate(info.battery);
		totalPrice += config.costs.cannon.calculate(info.cannon);
		totalPrice += config.costs.radar.calculate(info.radar);
		totalPrice += config.costs.gps.calculate(info.gps);
		totalPrice += config.costs.explosives.calculate(info.explosives);
		if (totalPrice > config.funds) {
			throw new IllegalArgumentException("The tank costs too much to build");
		}
		this.info = info;
	}

	public TankBuilder name(String name) {
		precall();
		info.name = name;
		return this;
	}

	public TankBuilder buyMotors(int num) {
		precall();
		TankInfo info = this.info.clone();
		info.motor += num;
		buy(info);
		return this;
	}

	public TankBuilder buyBatteries(int volts) {
		precall();
		TankInfo info = this.info.clone();
		info.battery += volts;
		buy(info);
		return this;
	}

	public TankBuilder buyCannons(int num) {
		precall();
		TankInfo info = this.info.clone();
		info.cannon += num;
		buy(info);
		return this;
	}

	public TankBuilder buyRadar() {
		precall();
		TankInfo info = this.info.clone();
		info.radar = 1;
		buy(info);
		return this;
	}

	public TankBuilder buyGPS() {
		precall();
		TankInfo info = this.info.clone();
		info.gps = 1;
		buy(info);
		return this;
	}

	public TankBuilder buyExplosives() {
		precall();
		TankInfo info = this.info.clone();
		info.explosives = 1;
		buy(info);
		return this;
	}

	public TankNetwork getNetwork() {
		return network;
	}

	public Tank build() throws IOException {
		precall();
		built = true;
		network.create(info);
		return new Tank(network, info);
	}

	public TankBuilder(URI uri) throws IOException {
		network = new TankNetwork(uri);
		config = network.getConfig();
		info = new TankInfo();
		built = false;
	}
}
