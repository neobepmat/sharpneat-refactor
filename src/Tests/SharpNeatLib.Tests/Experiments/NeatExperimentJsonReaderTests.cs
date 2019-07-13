﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using SharpNeat.Experiments;
using Moq;
using SharpNeat.Evaluation;

namespace SharpNeatLib.Tests.Experiments
{
    [TestClass]
    public class NeatExperimentJsonReaderTests
    {
        [TestMethod]
        public void Read()
        {
            JObject jobj = JObject.Parse(
@"{
    'description':'bar description',
    'isAcyclic':false,
    'cyclesPerActivation':111,
    'activationFnName':'bar-activation-fn',

    'neatEvolutionAlgorithmSettings':
    {
        'speciesCount':1111,
        'elitismProportion':0.11,
        'selectionProportion':0.22,
        'offspringAsexualProportion':0.33,
        'offspringSexualProportion':0.44,
        'interspeciesMatingProportion':0.55,
        'statisticsMovingAverageHistoryLength':2222
    },
    'reproductionAsexualSettings':
    {
        'connectionWeightMutationProbability':0.11,
        'addNodeMutationProbability':0.22,
        'addConnectionMutationProbability':0.33,
        'deleteConnectionMutationProbability':0.44
    },
    'reproductionSexualSettings':
    {
        'secondaryParentGeneProbability':0.11,
        'disjointExcessGenesRecombinedProbability':0.22
    },

    'populationSize':222,
    'initialInterconnectionsProportion':0.33,
    'connectionWeightScale':4.44,
    'suppressHardwareAcceleration':true,
    'degreeOfParallelism':6
}");

            // Create a mock evaluation scheme.
            var evalScheme = new Mock<IBlackBoxEvaluationScheme<double>>();

            // Init a default settings object.
            var experiment = NeatExperiment<double>.CreateAcyclic(
                "foo-experiment",
                evalScheme.Object,
                "foo-activation-fn");

            // Read json properties into the experiment object.
            NeatExperimentJsonReader<double>.Read(experiment, jobj);

            // Assert the expected values.
            Assert.AreEqual("bar description", experiment.Description);
            Assert.AreEqual(false, experiment.IsAcyclic);
            Assert.AreEqual(111, experiment.CyclesPerActivation);
            Assert.AreEqual("bar-activation-fn", experiment.ActivationFnName);

            var eaSettings = experiment.NeatEvolutionAlgorithmSettings;
            Assert.AreEqual(1111, eaSettings.SpeciesCount);
            Assert.AreEqual(0.11, eaSettings.ElitismProportion);
            Assert.AreEqual(0.22, eaSettings.SelectionProportion);
            Assert.AreEqual(0.33, eaSettings.OffspringAsexualProportion);
            Assert.AreEqual(0.44, eaSettings.OffspringSexualProportion);
            Assert.AreEqual(0.55, eaSettings.InterspeciesMatingProportion);
            Assert.AreEqual(2222, eaSettings.StatisticsMovingAverageHistoryLength);

            var asexualSettings = experiment.ReproductionAsexualSettings;
            Assert.AreEqual(0.11, asexualSettings.ConnectionWeightMutationProbability);
            Assert.AreEqual(0.22, asexualSettings.AddNodeMutationProbability);
            Assert.AreEqual(0.33, asexualSettings.AddConnectionMutationProbability);
            Assert.AreEqual(0.44, asexualSettings.DeleteConnectionMutationProbability);

            var sexualSettings = experiment.ReproductionSexualSettings;
            Assert.AreEqual(0.11, sexualSettings.SecondaryParentGeneProbability);
            Assert.AreEqual(0.22, sexualSettings.DisjointExcessGenesRecombinedProbability);

            Assert.AreEqual(222, experiment.PopulationSize);
            Assert.AreEqual(0.33, experiment.InitialInterconnectionsProportion);
            Assert.AreEqual(4.44, experiment.ConnectionWeightScale);
            Assert.AreEqual(6, experiment.DegreeOfParallelism);
            Assert.AreEqual(true, experiment.SuppressHardwareAcceleration);
        }
    }
}