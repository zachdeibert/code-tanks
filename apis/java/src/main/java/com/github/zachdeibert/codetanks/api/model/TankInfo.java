package com.github.zachdeibert.codetanks.api.model;

public final class TankInfo implements Cloneable {
	public String name;
	public int motor;
	public int battery;
	public int cannon;
	public int radar;
	public int gps;
	public int explosives;

	@Override
	public TankInfo clone() {
		return new TankInfo(name, motor, battery, cannon, radar, gps, explosives);
	}

	public TankInfo() {
	}

	private TankInfo(String name, int motor, int battery, int cannon, int radar, int gps, int explosives) {
		this.name = name;
		this.motor = motor;
		this.battery = battery;
		this.cannon = cannon;
		this.radar = radar;
		this.gps = gps;
		this.explosives = explosives;
	}
}
