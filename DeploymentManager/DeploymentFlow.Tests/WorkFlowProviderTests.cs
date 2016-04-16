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
            _step1 = new FlowStep(new NullCommand("FirstStep"), "First step", 0);
            _step2 = new FlowStep(new NullCommand("SecondStep"), "Second step", 1);
            _stepList.Add(_step1);
            _stepList.Add(_step2);
            _workFlowProvider = new WorkFlowProvider(_stepList);
        }

        [TestMethod]
        public void ShouldreturnAllSteps()
        {

            // act
            var allSteps = _workFlowProvider.AllSteps;
            
            // assert
            allSteps.ShouldAllBeEquivalentTo(_stepList);
        }

        [TestMethod]
        public async Task ShouldRunAllStepsAndSetFinished()
        {
            // act
            await _workFlowProvider.StartWorkFlow();

            // assert
            _workFlowProvider.WorkFlowFinished.Should().BeTrue(); 
        }
    }
}
