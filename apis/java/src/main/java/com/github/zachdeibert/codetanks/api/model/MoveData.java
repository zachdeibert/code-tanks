package com.github.zachdeibert.codetanks.api.model;

public final class MoveData extends LoginTokenContainer {
	public enum ForwardDirection {
		forward, backward
	}

	public enum StrafeDirection {
		left, right
	}

	public enum TurnDirection {
		clockwise, counterclockwise
	}

	public static final class MoveInfo<T> {
		public T direction;
		public double rate;
		
		public MoveInfo(T direction, double rate) {
			this.direction = direction;
			this.rate = rate;
		}
	}

	public MoveInfo<ForwardDirection> forward;
	public MoveInfo<StrafeDirection> strafe;
	public MoveInfo<TurnDirection> turn;
}
