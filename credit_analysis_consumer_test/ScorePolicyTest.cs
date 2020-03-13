using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using credit_analysis_consumer.Services;
using Moq;
using credit_analysis_consumer.Interfaces;
using credit_analysis_consumer;

namespace credit_analysis_consumer_test
{
    public class ScorePolicyTest
    {
        [Fact]
        public void ShouldReturnTrueForGreaterThan600Score()
        {
            var mockDependency = new Mock<IScoreService>();

            // set up mock version's method
            mockDependency.Setup(x => x.GetScore(It.IsAny<string>()))
                          .ReturnsAsync(900);
            var loan = new Loan
            {
                cpf = "abc"
            };
            var scorePolicy = new ScorePolicy(mockDependency.Object);
            var result = scorePolicy.ProcessScorePolicy(loan).Result;
            Assert.NotEqual(0, result.score);
            Assert.True(result.score_policy_result);
        }

        [Fact]
        public void ShouldReturnFalseForLeasserThan600Score()
        {
            var mockDependency = new Mock<IScoreService>();

            // set up mock version's method
            mockDependency.Setup(x => x.GetScore(It.IsAny<string>()))
                          .ReturnsAsync(100);
            var loan = new Loan
            {
                cpf = "abc"
            };
            var scorePolicy = new ScorePolicy(mockDependency.Object);
            var result = scorePolicy.ProcessScorePolicy(loan).Result;
            Assert.NotEqual(0, result.score);
            Assert.False(result.score_policy_result);
        }
    }
}
