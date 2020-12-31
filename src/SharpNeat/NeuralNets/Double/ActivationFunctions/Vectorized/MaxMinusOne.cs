/* ***************************************************************************
 * This file is part of SharpNEAT - Evolution of Neural Networks.
 *
 * Copyright 2004-2020 Colin Green (sharpneat@gmail.com)
 *
 * SharpNEAT is free software; you can redistribute it and/or modify
 * it under the terms of The MIT License (MIT).
 *
 * You should have received a copy of the MIT License
 * along with SharpNEAT; if not, see https://opensource.org/licenses/MIT.
 */
using System;
using System.Numerics;

namespace SharpNeat.NeuralNets.Double.ActivationFunctions.Vectorized
{
    /// <summary>
    /// max(-1, x,) function.
    /// </summary>
    public sealed class MaxMinusOne : IActivationFunction<double>
    {
        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public double Fn(double x)
        {
            double y;
            if (x > -1) {
                y = x;
            } else {
                y = -1;
            }
            return y;
        }

        public void Fn(Span<double> v)
        {
            Fn(v, v);
        }

        public void Fn(Span<double> v, Span<double> w)
        {
            // Init constants.
            var minusOneVec = new Vector<double>(-1.0);
            int width = Vector<double>.Count;

            int i=0;
            for(; i <= v.Length - width; i += width)
            {
                // Load values into a vector.
                var vec = new Vector<double>(v[i..]);

                // Apply max(val, 0) to each element in the vector.
                var vecResult = Vector.Max(vec, minusOneVec);

                // Copy the result back into arr.
                vecResult.CopyTo(w[i..]);
            }

            // Handle vectors with lengths not an exact multiple of vector width.
            for(; i < v.Length; i++) {
                w[i] = Fn(v[i]);
            }
        }
    }
}
