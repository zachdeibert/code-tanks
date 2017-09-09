package com.github.zachdeibert.codetanks.api.model;

public final class Formula {
	public enum Type {
		polynomial, exponential
	}

	public Double minimum;
	public Double maximum;
	public Type formula;
	public double[] coefficients;
	public Formula coefficient;
	public Formula base;
	public Formula exponent;

	public double calculate(double x) {
		if ((minimum != null && x < minimum) || (maximum != null && x > maximum)) {
			throw new IllegalArgumentException("x");
		}
		switch (formula) {
			case polynomial:
				double value = 0;
				double xPow = 1;
				for (int i = coefficients.length; i >= 0; --i) {
					value += coefficients[i] * xPow;
					xPow *= x;
				}
				return value;
			case exponential:
				double coefficient = this.coefficient == null ? 1 : this.coefficient.calculate(x);
				double base = this.base == null ? 1 : this.base.calculate(x);
				double exponent = this.exponent == null ? 1 : this.exponent.calculate(x);
				return coefficient * Math.pow(base, exponent);
			default:
				throw new IllegalStateException("Invalid equation type");
		}
	}
}
