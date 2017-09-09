using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.CodeTanks.Model {
    public sealed class Formula {
        public enum Type {
            [JsonProperty("polynomial")]
            Polynomial,
            [JsonProperty("exponential")]
            Exponential
        }

        [JsonProperty("min")]
        public double? Minimum;
        [JsonProperty("max")]
        public double? Maximum;
        [JsonProperty("formula")]
        public Type EquationType;
        [JsonProperty("coefficients")]
        public double[] Coefficients;
        [JsonProperty("coefficient")]
        public Formula Coefficient;
        [JsonProperty("base")]
        public Formula Base;
        [JsonProperty("exponent")]
        public Formula Exponent;

        public double Calculate(double x) {
            if ((Minimum != null && x < Minimum) || (Maximum != null && x > Maximum)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }
            switch (EquationType) {
                case Type.Polynomial:
                    double value = 0;
                    double xPow = 1;
                    for (int i = Coefficients.Length; i >= 0; --i) {
                        value += Coefficients[i] * xPow;
                        xPow *= x;
                    }
                    return value;
                case Type.Exponential:
                    double coefficient = Coefficient?.Calculate(x) ?? 1;
                    double _base = Base?.Calculate(x) ?? 1;
                    double exponent = Exponent?.Calculate(x) ?? 1;
                    return coefficient * Math.Pow(_base, exponent);
                default:
                    throw new NotSupportedException("Invalid equation type");
            }
        }
    }
}
