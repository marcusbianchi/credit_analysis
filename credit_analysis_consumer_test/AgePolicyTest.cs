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
    public class AgePolicyTest
    {
        [Fact]
        public void ShouldReturnTrueIfAgeOverOrEqual18()
        {

            var loan = new Loan
            {
                birthdate = Convert.ToDateTime("01/01/2000")
            };

            var agePolicy = new AgePolicy();
            var result = agePolicy.ProcessAgePolicy(loan);

            Assert.True(result.age_policy_result);
        }

        [Fact]
        public void ShouldReturnTrueIfAgeUnderl18()
        {
            var loan = new Loan
            {
                birthdate = Convert.ToDateTime("01/01/2018")
            };

            var agePolicy = new AgePolicy();
            var result = agePolicy.ProcessAgePolicy(loan);

            Assert.False(result.age_policy_result);
        }
    }
}
