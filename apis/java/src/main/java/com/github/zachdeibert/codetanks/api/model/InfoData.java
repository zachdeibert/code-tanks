package com.github.zachdeibert.codetanks.api.model;

public final class InfoData {
	public enum State {
		ready, simulating, finished
	}

	public int[] version;
	public State state;
}
