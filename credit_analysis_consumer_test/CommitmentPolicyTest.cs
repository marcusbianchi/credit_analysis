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
    public class CommitmentPolicyTest
    {
        [Fact]
        public void ShouldApproveTheRequestButWith12Terms()
        {
            var mockDependency = new Mock<ICommitmentService>();

            // set up mock version's method
            mockDependency.Setup(x => x.GetCommitment(It.IsAny<string>()))
                          .ReturnsAsync(0.8);
            var loan = new Loan
            {
                cpf = "abc",
                amount = 2500,
                terms = 6,
                score = 700,
                income = 1500

            };
            var commitmentPolicy = new CommitmentPolicy(mockDependency.Object);
            var result = commitmentPolicy.ProcessCommitmentPolicy(loan).Result;
            Assert.True(result.commitment_policy_result);
            Assert.Equal(12, result.commitment_terms_result);
            Assert.NotEqual(0, result.commitment_terms_value);

        }


        [Fact]
        public void ShouldApproveTheRequestButWithTheRequiredTerms()
        {
            var mockDependency = new Mock<ICommitmentService>();

            // set up mock version's method
            mockDependency.Setup(x => x.GetCommitment(It.IsAny<string>()))
                          .ReturnsAsync(0.8);
            var loan = new Loan
            {
                cpf = "abc",
                amount = 2500,
                terms = 6,
                score = 700,
                income = 5500

            };
            var commitmentPolicy = new CommitmentPolicy(mockDependency.Object);
            var result = commitmentPolicy.ProcessCommitmentPolicy(loan).Result;
            Assert.True(result.commitment_policy_result);
            Assert.Equal(6, result.commitment_terms_result);
            Assert.NotEqual(0, result.commitment_terms_value);
        }

        [Fact]
        public void ShouldReproveTheRequest()
        {
            var mockDependency = new Mock<ICommitmentService>();

            // set up mock version's method
            mockDependency.Setup(x => x.GetCommitment(It.IsAny<string>()))
                          .ReturnsAsync(0.8);
            var loan = new Loan
            {
                cpf = "abc",
                amount = 2500,
                terms = 6,
                score = 700,
                income = 500

            };
            var commitmentPolicy = new CommitmentPolicy(mockDependency.Object);
            var result = commitmentPolicy.ProcessCommitmentPolicy(loan).Result;
            Assert.False(result.commitment_policy_result);
            Assert.Null(result.commitment_terms_result);
        }
    }
}
