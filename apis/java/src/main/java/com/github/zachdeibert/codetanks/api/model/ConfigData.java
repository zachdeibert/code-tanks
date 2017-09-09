package com.github.zachdeibert.codetanks.api.model;

public final class ConfigData {
	public static final class CostData {
		public Formula motor;
		public Formula battery;
		public Formula cannon;
		public Formula radar;
		public Formula gps;
		public Formula explosives;
	}

	public static final class SpeedData {
		public Formula bullet;
		public Formula tankDrive;
		public Formula tankTurn;
	}

	public static final class ExplosionData {
		public Formula size;
		public int duration;
	}

	public double funds;
	public CostData costs;
	public SpeedData speeds;
	public ExplosionData explosions;
	public double tankSize;
}
