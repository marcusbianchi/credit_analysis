using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;
using credit_analysis_consumer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace credit_analysis_consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<IAgePolicy, AgePolicy>();
                    services.AddTransient<ICommitmentService, CommitmentService>();
                    services.AddTransient<IScoreService, ScoreService>();
                    services.AddTransient<IQueueService, QueueService>();
                    services.AddTransient<IScorePolicy, ScorePolicy>();
                    services.AddTransient<ICommitmentPolicy, CommitmentPolicy>();
                    services.AddTransient<IProcessRequestService, ProcessRequestService>();
                    services.AddHostedService<Worker>();
                });
    }
}
