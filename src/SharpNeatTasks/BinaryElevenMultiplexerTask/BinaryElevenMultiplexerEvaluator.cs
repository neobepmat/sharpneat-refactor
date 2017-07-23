﻿using System;
using System.Diagnostics;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace SharpNeatTasks.BinaryElevenMultiplexerTask
{
    /// <summary>
    /// Binary 11-Multiplexer task.
    /// Three inputs supply a binary number between 0 and 7. This number selects one of the
    /// further 8 inputs (eleven inputs in total). The correct response is the selected input's
    /// input signal (0 or 1).
    /// </summary>
    public class BinaryElevenMultiplexerEvaluator : IPhenomeEvaluator<IBlackBox<double>>
    {
        #region instance Fields

        const double __stopFitness = 10000.0;
        bool _stopConditionSatisfied;

        #endregion

        #region Properties

        public bool StopConditionSatisfied => throw new NotImplementedException();

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Evaluate the provided IBlackBox against the Binary 6-Multiplexer problem domain and return
        /// its fitness score.
        /// </summary>
        public double Evaluate(IBlackBox<double> box)
        {
            double fitness = 0.0;
            bool success = true;
            double output;
            IVector<double> inputArr = box.InputSignalVector;
            IVector<double> outputArr = box.OutputSignalVector;
            
            // 2048 test cases.
            for(int i=0; i<2048; i++)
            {
                // Bias input.
                inputArr[0] = 1.0;

                // Apply bitmask to i and shift left to generate the input signals.
                // In addition we scale 0->1 to be 0.1->1.0
                // Note. We /could/ eliminate all the boolean logic by pre-building a table of test 
                // signals and correct responses.
                int tmp = i;
                for(int j=0; j<11; j++) 
                {   
                    inputArr[j+1] = tmp&0x1;
                    tmp >>= 1;
                }
                                
                // Activate the black box.
                box.Activate();

                // Read output signal.
                output = outputArr[0];
                Debug.Assert(output >= 0.0, "Unexpected negative output.");

                // Determine the correct answer by using highly cryptic bit manipulation :)
                // The condition is true if the correct answer is true (1.0).
                if(((1<<(3+(i&0x7)))&i) != 0)
                {   // correct answer = true.
                    // Assign fitness on sliding scale between 0.0 and 1.0 based on squared error.
                    // In tests squared error drove evolution significantly more efficiently in this domain than absolute error.
                    // Note. To base fitness on absolute error use: fitness += output;
                    fitness += 1.0-((1.0-output)*(1.0-output));
                    if(output<0.5) {
                        success=false;
                    }
                }
                else
                {   // correct answer = false.
                    // Assign fitness on sliding scale between 0.0 and 1.0 based on squared error.
                    // In tests squared error drove evolution significantly more efficiently in this domain than absolute error.
                    // Note. To base fitness on absolute error use: fitness += 1.0-output;
                    fitness += 1.0-(output*output);
                    if(output>=0.5) {
                        success=false;
                    }
                }

                // Reset black box state ready for next test case.
                box.ResetState();
            }

            // If the correct answer was given in each case then add a bonus value to the fitness.
            if(success) {
                fitness += 10000.0;
            }

            if(fitness >= __stopFitness) {
                _stopConditionSatisfied = true;
            }

            return fitness;
        }

        #endregion
    }
}
