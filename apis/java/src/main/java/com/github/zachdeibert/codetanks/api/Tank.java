package com.github.zachdeibert.codetanks.api;

import java.io.IOException;

import com.github.zachdeibert.codetanks.api.model.GpsData;
import com.github.zachdeibert.codetanks.api.model.InfoData.State;
import com.github.zachdeibert.codetanks.api.model.MoveData;
import com.github.zachdeibert.codetanks.api.model.MoveData.ForwardDirection;
import com.github.zachdeibert.codetanks.api.model.MoveData.MoveInfo;
import com.github.zachdeibert.codetanks.api.model.MoveData.StrafeDirection;
import com.github.zachdeibert.codetanks.api.model.MoveData.TurnDirection;
import com.github.zachdeibert.codetanks.api.model.TankInfo;

public final class Tank implements AutoCloseable {
	final TankNetwork network;
	final TankInfo info;
	final MoveData movement;
	public final TankCannon[] cannons;

	public void setForwardMovement(ForwardDirection direction, double rate) throws IOException {
		if (info.motor < 1) {
			throw new IllegalStateException("Need at least one motor to move");
		}
		if (rate < 0 || rate > 1) {
			throw new IllegalArgumentException("rate must be between 0 and 1");
		}
		movement.forward = new MoveInfo<>(direction, rate);
		network.setMovement(movement);
	}

	public void setTurnMovement(TurnDirection direction, double rate) throws IOException {
		if (info.motor < 2) {
			throw new IllegalStateException("Need at least two motors to turn");
		}
		if (rate < 0 || rate > 1) {
			throw new IllegalArgumentException("rate must be between 0 and 1");
		}
		movement.turn = new MoveInfo<>(direction, rate);
		network.setMovement(movement);
	}

	public void setStrafeMovement(StrafeDirection direction, double rate) throws IOException {
		if (info.motor < 4) {
			throw new IllegalStateException("Need at least four motors to strafe");
		}
		if (rate < 0 || rate > 1) {
			throw new IllegalArgumentException("rate must be between 0 and 1");
		}
		movement.strafe = new MoveInfo<>(direction, rate);
		network.setMovement(movement);
	}

	public double readRadar() throws IOException {
		return network.readRadar().distance;
	}

	public GpsData readGps() throws IOException {
		return network.readGps();
	}

	public void explode() throws IOException {
		network.explode();
	}
	
	public boolean hasStarted() throws IOException {
		return network.getInfo().state != State.ready;
	}

	@Override
	public void close() throws IOException {
		network.close();
	}

	public Tank(TankNetwork network, TankInfo info) {
		this.network = network;
		this.info = info;
		movement = new MoveData();
		cannons = new TankCannon[info.cannon];
		for (int i = 0; i < cannons.length; ++i) {
			cannons[i] = new TankCannon(network, i);
		}
	}
}
