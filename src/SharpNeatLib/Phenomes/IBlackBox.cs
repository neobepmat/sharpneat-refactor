/* ***************************************************************************
 * This file is part of SharpNEAT - Evolution of Neural Networks.
 * 
 * Copyright 2004-2016 Colin Green (sharpneat@gmail.com)
 *
 * SharpNEAT is free software; you can redistribute it and/or modify
 * it under the terms of The MIT License (MIT).
 *
 * You should have received a copy of the MIT License
 * along with SharpNEAT; if not, see https://opensource.org/licenses/MIT.
 */
namespace SharpNeat.Phenomes
{
    /// <summary>
    /// IBlackBox represents an abstract device, system or function which has inputs and outputs. The internal
    /// workings and state of the box are not relevant to any method or class that accepts an IBlackBox - only that it
    /// has inputs and outputs and a means of activation. In NEAT the neural network implementations generally fit this
    /// pattern, that is:
    /// 
    ///  - inputs are fed to a network.
    ///  - The network is activated (e.g. for some fixed number of timesteps).
    ///  - The network outputs are read and fed into the evaluation/scoring/fitness scheme.
    /// 
    /// From wikipedia:
    /// Black box is a technical term for a device or system or object when it is viewed primarily in terms 
    /// of its input and output characteristics. Almost anything might occasionally be referred to as a black box -
    /// a transistor, an algorithm, humans, the Internet.
    /// </summary>
    public interface IBlackBox<T> where T : struct
    {
        /// <summary>
        /// Gets the number of inputs to the blackbox. This is assumed to be fixed for the lifetime of the IBlackBox.
        /// </summary>
        int InputCount { get; }

        /// <summary>
        /// Gets the number of outputs from the blackbox. This is assumed to be fixed for the lifetime of the IBlackBox.
        /// </summary>
        int OutputCount { get; }

        /// <summary>
        /// Gets an array of input values that feed into the black box. 
        /// </summary>
        IVector<T> InputSignalVector { get; }

        /// <summary>
        /// Gets an array of output values that feed out from the black box. 
        /// </summary>
        IVector<T> OutputSignalVector { get; }

        /// <summary>
        /// Activate the black box. This is a request for the box to accept its inputs and produce output signals
        /// ready for reading from OutputSignalArray.
        /// </summary>
        void Activate();

        /// <summary>
        /// Reset any internal state.
        /// </summary>
        void ResetState();
    }
}
