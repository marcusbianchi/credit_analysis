using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using credit_analysis_consumer.Services;

namespace credit_analysis_consumer_test
{
    public class CommitmentServiceTest
    {
        [Fact]
        public void ShouldReturnDoubleForAnyCPF()
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var commitmentService = new CommitmentService(httpClientFactory);
            var result = commitmentService.GetCommitment("123131313121313").Result;
            Assert.NotNull(result);
        }
    }
}
