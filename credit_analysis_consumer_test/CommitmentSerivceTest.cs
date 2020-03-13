using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using credit_analysis_consumer.Services;

namespace credit_analysis_consumer_test
{
    public class CommitmentSerivceTest
    {
        [Fact]
        public void ShouldReturnIntForAnyCPF()
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var commitmentSerivce = new CommitmentService(httpClientFactory);
            var result = commitmentSerivce.GetCommitment("123131313121313");
            Assert.NotNull(result);
        }
    }
}
