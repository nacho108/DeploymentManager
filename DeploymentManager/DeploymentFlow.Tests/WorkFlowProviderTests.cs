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
        [TestMethod]
        public void ShouldShowFirststep()
        {
            // Arange
            var stepList=new List<FlowStep>();
            var step1 = new FlowStep(new NullCommand(), "First step", 0);
            var step2 = new FlowStep(new NullCommand(), "Second step", 1);
            stepList.Add(step1);
            stepList.Add(step2);
            WorkFlowProvider workFlowProvider = new WorkFlowProvider(stepList);


            // Assert
            Assert.AreEqual(step1, workFlowProvider.GetCurrentStep());
            Assert.AreEqual(false, workFlowProvider.WorkFlowFinished);
        }

        [TestMethod]
        public void ShouldreturnAllSteps()
        {
            // Arange
            var stepList = new List<FlowStep>();
            var step1 = new FlowStep(new NullCommand(), "First step", 0);
            var step2 = new FlowStep(new NullCommand(), "Second step", 1);
            stepList.Add(step1);
            stepList.Add(step2);
            WorkFlowProvider workFlowProvider = new WorkFlowProvider(stepList);

            // act
            var allSteps = workFlowProvider.GetAllSteps();
            
            // assert
            allSteps.ShouldAllBeEquivalentTo(stepList);
        }

        [TestMethod]
        async public Task ShouldReturnNextStep()
        {
            // Arange
            var stepList = new List<FlowStep>();
            var step1 = new FlowStep(new NullCommand(), "First step", 0);
            var step2 = new FlowStep(new NullCommand(), "Second step", 1);
            stepList.Add(step1);
            stepList.Add(step2);
            WorkFlowProvider workFlowProvider = new WorkFlowProvider(stepList);

            // act
            await workFlowProvider.ExecuteCurrentStep();

            // assert
            Assert.AreEqual(step2, workFlowProvider.GetCurrentStep());
            Assert.AreEqual(false, workFlowProvider.WorkFlowFinished);
        }

        [TestMethod]
        async public Task ShouldReturnLastStepAndSetWorkFlowFinished()
        {
            // Arange
            var stepList = new List<FlowStep>();
            var step1 = new FlowStep(new NullCommand(), "First step", 0);
            var step2 = new FlowStep(new NullCommand(), "Second step", 1);
            stepList.Add(step1);
            stepList.Add(step2);
            WorkFlowProvider workFlowProvider = new WorkFlowProvider(stepList);

            // act
            await workFlowProvider.ExecuteCurrentStep();
            await workFlowProvider.ExecuteCurrentStep();

            // assert
            Assert.AreEqual(step2, workFlowProvider.GetCurrentStep());
            Assert.AreEqual(true, workFlowProvider.WorkFlowFinished);
        }

    }
}
