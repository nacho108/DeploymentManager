using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeploymentFlow.Tests
{
    [TestClass]
    public class WorkFlowProviderTests
    {
        private List<FlowStep> _stepList;
        private WorkFlowProvider _workFlowProvider;
        private FlowStep _step1;
        private FlowStep _step2;

        [TestInitialize]
        public void Init()
        {
            _stepList = new List<FlowStep>();
            _step1 = new FlowStep(new NullCommand(), "First step", 0);
            _step2 = new FlowStep(new NullCommand(), "Second step", 1);
            _stepList.Add(_step1);
            _stepList.Add(_step2);
            _workFlowProvider = new WorkFlowProvider(_stepList);
        }

        [TestMethod]
        public void ShouldShowFirststep()
        {
            // Assert
            Assert.AreEqual(_step1, _workFlowProvider.GetCurrentStep());
            Assert.AreEqual(false, _workFlowProvider.WorkFlowFinished);
        }

        [TestMethod]
        public void ShouldreturnAllSteps()
        {

            // act
            var allSteps = _workFlowProvider.GetAllSteps();
            
            // assert
            allSteps.ShouldAllBeEquivalentTo(_stepList);
        }

        [TestMethod]
        async public Task ShouldReturnNextStep()
        {
            // act
            await _workFlowProvider.ExecuteCurrentStep();

            // assert
            Assert.AreEqual(_step2, _workFlowProvider.GetCurrentStep());
            Assert.AreEqual(false, _workFlowProvider.WorkFlowFinished);
        }

        [TestMethod]
        async public Task ShouldReturnLastStepAndSetWorkFlowFinished()
        {
            // act
            await _workFlowProvider.ExecuteCurrentStep();
            await _workFlowProvider.ExecuteCurrentStep();

            // assert
            Assert.AreEqual(_step2, _workFlowProvider.GetCurrentStep());
            Assert.AreEqual(true, _workFlowProvider.WorkFlowFinished);
        }

        [TestMethod]
        async public Task ShouldRemainInTheLastStep()
        {
            // act
            await _workFlowProvider.ExecuteCurrentStep();
            await _workFlowProvider.ExecuteCurrentStep();
            await _workFlowProvider.ExecuteCurrentStep();

            // assert
            Assert.AreEqual(_step2, _workFlowProvider.GetCurrentStep());
            Assert.AreEqual(true, _workFlowProvider.WorkFlowFinished);
        }


    }
}
