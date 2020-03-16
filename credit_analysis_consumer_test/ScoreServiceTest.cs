using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using credit_analysis_consumer.Services;

namespace credit_analysis_consumer_test
{
    public class ScoreServiceTest
    {
        [Fact]
        public void ShouldReturnIntForAnyCPF()
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var scoreService = new ScoreService(httpClientFactory);
            var result = scoreService.GetScore("teste").Result;
            Assert.NotNull(result);
        }
    }
}

